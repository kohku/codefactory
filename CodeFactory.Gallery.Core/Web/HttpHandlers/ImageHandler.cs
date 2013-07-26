using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.Caching;
using CodeFactory.Gallery.Core.Providers;
using CodeFactory.Web.Storage;
using System.Net.Mime;

namespace CodeFactory.Gallery.Core.Web.HttpHandlers
{
    /// <summary>
    /// The ImageHanlder serves all images that is uploaded from
    /// the admin pages. 
    /// </summary>
    /// <remarks>
    /// By using a HttpHandler to serve images, it is very easy
    /// to add the capability to stop bandwidth leeching or
    /// to create a statistics analysis feature upon it.
    /// </remarks>
    public class ImageHandler : IHttpHandler
    {
        #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return false; }
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
            if (!string.IsNullOrEmpty(context.Request.QueryString["guid"]))
            {
                int width = 0;
                int height = 0;
                bool quality = false;

                string id = context.Request.QueryString["guid"];

                if (!string.IsNullOrEmpty(context.Request.QueryString["width"]))
                    width = Convert.ToInt32(context.Request.QueryString["width"]);

                if (!string.IsNullOrEmpty(context.Request.QueryString["height"]))
                    height = Convert.ToInt32(context.Request.QueryString["height"]);

                if (!string.IsNullOrEmpty(context.Request.QueryString["quality"]))
                    quality = Convert.ToBoolean(context.Request.QueryString["quality"]);

                OnServing(id);

                try
                {
                    UploadedFile file = UploadedFile.Load(new Guid(id));

                    if (file == null)
                    {
                        OnBadRequest(id);
                        context.Response.Status = "404 Bad Request";
                        context.Response.End();
                        return;
                    }

                    Stream input = null;

                    if (width > 0 && height > 0)
                    {
                        try
                        {
                            input = new MemoryStream();

                            Image image = Image.FromStream(file.InputStream);

                            if (quality)
                            {
                                Image thumbnail = new Bitmap(width, height, image.PixelFormat);

                                Graphics graphic = Graphics.FromImage(thumbnail);

                                graphic.CompositingQuality = CompositingQuality.HighQuality;
                                graphic.SmoothingMode = SmoothingMode.HighQuality;
                                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                                Rectangle rectangle = new Rectangle(0, 0, width, height);

                                graphic.DrawImage(image, rectangle);

                                thumbnail.Save(input, image.RawFormat);
                            }
                            else
                            {
                                Image thumbnail = image.GetThumbnailImage(width, height, null, IntPtr.Zero);

                                thumbnail.Save(input, image.RawFormat);
                            }
                        }
                        catch (Exception)
                        {
                            input = File.OpenRead(Path.Combine(context.Server.MapPath(
                                context.Request.ApplicationPath), "images/file.jpg"));
                        }
                    }
                    else
                    {
                        input = file.InputStream;
                    }

                    context.Response.ContentType = file.ContentType;

                    string etag = "\"" + file.DateCreated.GetHashCode() + "\"";
                    string incomingEtag = context.Request.Headers["If-None-Match"];

                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
                    context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
                    context.Response.Cache.SetLastModified(file.DateCreated);
                    context.Response.Cache.SetETag(etag);

                    if (String.Compare(incomingEtag, etag) == 0)
                    {
                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                    }
                    else
                    {
                        input.Position = 0;

                        BinaryReader reader = new BinaryReader(input);

                        context.Response.OutputStream.Write(reader.ReadBytes((int)input.Length), 0, (int)input.Length);
                        context.Response.Flush();
                    }

                    OnServed(id);
                }
                catch (Exception)
                {
                    OnBadRequest(id);
                    context.Response.Status = "404 Bad Request";
                }
            }
        }

        #endregion

        /// <summary>
        /// Sets the content type depending on the filename's extension.
        /// </summary>
        private static void SetContentType(HttpContext context, string fileName)
        {
            if (fileName.EndsWith(".pdf"))
                context.Response.AddHeader("Content-Type", "application/pdf");
            else
                context.Response.AddHeader("Content-Type", "application/octet-stream");
        }

        /// <summary>
        /// Occurs when a file is being serving.
        /// </summary>
        public static event EventHandler<EventArgs> Serving;

        private static void OnServing(string file)
        {
            if (Serving != null)
                Serving(file, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when a file is served;
        /// </summary>
        public static event EventHandler<EventArgs> Served;

        private static void OnServed(string file)
        {
            if (Served != null)
                Served(file, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when the requested file does not exist;
        /// </summary>
        public static event EventHandler<EventArgs> BadRequest;

        private static void OnBadRequest(string file)
        {
            if (BadRequest != null)
                BadRequest(file, EventArgs.Empty);
        }
    }
}
