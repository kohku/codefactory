using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using CodeFactory.Gallery.Core;
using CodeFactory.Gallery.Core.Providers;
using System.Web.SessionState;
using CodeFactory.Web.Storage;

namespace CodeFactory.Gallery.Core.Web.HttpHandlers
{
    public class GalleryHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// The ImageHanlder serves all xml gallery that is requested 
        /// from a session.
        /// </summary>
        #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that 
        /// implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> 
        /// object that provides references to the intrinsic server objects 
        /// (for example, Request, Response, Session, and Server) used to service HTTP requests.
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            int totalCount = 0;
            bool masterGraphic = true;

            featuredProjects result = new featuredProjects();

            OnServing(result);

            try
            {
                Guid? id = null;
                bool? visible = null;
                string lastUpdatedBy = null;
                string author = null;
                string title = null;
                GalleryStatus? status = null;
                int pageSize = int.MaxValue;
                int pageIndex = 0;

                // Users without administrator role can get only visible projects.
                if (!HttpContext.Current.User.IsInRole("Administrator"))
                    visible = true;

                if (HttpContext.Current.Session["masterGraphic"] != null)
                    masterGraphic = Convert.ToBoolean(HttpContext.Current.Session["masterGraphic"]);

                if (HttpContext.Current.Session["guid"] != null)
                    id = (Guid)HttpContext.Current.Session["guid"];

                List<Guid> entries = GalleryManagementService.GetGalleryList(
                    id, author, visible, lastUpdatedBy, title, status, pageSize, pageIndex, out totalCount);

                if (entries.Count > 0)
                {
                    List<Gallery> galleries = GalleryManagementService.GetGalleries(entries, false, true);

                    foreach (Gallery gallery in galleries)
                    {
                        if (!HttpContext.Current.User.IsInRole("Administrator") && !gallery.Users.Contains(HttpContext.Current.User.Identity.Name))
                            continue;

                        if (gallery.Files.Count <= 0)
                            continue;

                        // Explicity sort
                        gallery.Files.Sort(delegate(UploadedFile x, UploadedFile y)
                        {
                            return x.DateCreated.CompareTo(y.DateCreated);
                        });

                        int index = 0;

                        do
                        {
                            UploadedFile file = gallery.Files[index];

                            featuredProjectsProj proj = new featuredProjectsProj();

                            proj.label = masterGraphic ? gallery.Title : file.FileName;
                            proj.link = masterGraphic ? gallery.RelativeLink : file.RelativeLink;
                            // The following two parameters are not really used. Those were created 
                            // to modify the size of a popup window by the gallery control.
                            proj.width = GalleryManagementService.Settings.Thumbnail.Width;
                            proj.height = GalleryManagementService.Settings.Thumbnail.Heigth;
                            proj.pic = file.ThumbnailLink;

                            result.Add(proj);
                        } while (!masterGraphic && ++index < gallery.Files.Count);
                    }
                }

                XmlSerializer serializer = new XmlSerializer(typeof(featuredProjects));

                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.AddHeader("Content-Type", "text/xml");

                serializer.Serialize(context.Response.Output, result);

                context.Response.Flush();

                OnServed(result);
            }
            catch
            {
                OnBadRequest(result);
            }
        }

        #endregion

        /// <summary>
        /// Occurs when a gallery is being serving.
        /// </summary>
        public static event EventHandler<EventArgs> Serving;

        private static void OnServing(featuredProjects result)
        {
            if (Serving != null)
                Serving(result, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when a gallery is served;
        /// </summary>
        public static event EventHandler<EventArgs> Served;

        private static void OnServed(featuredProjects result)
        {
            if (Served != null)
                Served(result, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when the requested gallery does not exist;
        /// </summary>
        public static event EventHandler<EventArgs> BadRequest;

        private static void OnBadRequest(featuredProjects result)
        {
            if (BadRequest != null)
                BadRequest(result, EventArgs.Empty);
        }
    }
}
