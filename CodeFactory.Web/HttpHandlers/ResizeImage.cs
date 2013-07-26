using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Mime;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CodeFactory.Web.HttpHandlers
{
    public class ResizeImage : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                float scaleFactor = .5f;

                if (string.IsNullOrEmpty(context.Request.QueryString["imageUrl"]))
                    throw new ArgumentNullException("imageUrl");

                if (!string.IsNullOrEmpty(context.Request.QueryString["scaleFactor"]))
                    scaleFactor = float.Parse(context.Request.QueryString["scaleFactor"]);

                string imageUrl = context.Server.UrlDecode(context.Request.QueryString["imageUrl"]);

                if (!VirtualPathUtility.IsAppRelative(imageUrl))
                    throw new ArgumentException("Image url must be app relative.");

                Uri imageUri = null;

                if (VirtualPathUtility.IsAppRelative(imageUrl))
                    imageUri = new Uri(context.Request.Url.Scheme + "://" + context.Request.Url.Authority + VirtualPathUtility.ToAbsolute(imageUrl));
                else if (VirtualPathUtility.IsAbsolute(imageUrl))
                    imageUri = new Uri(context.Request.Url.Scheme + "://" + context.Request.Url.Authority + imageUri);
                else
                    throw new ArgumentException("Image url must be app relative.");

                WebRequest request = HttpWebRequest.Create(imageUri);

                try
                {
                    WebResponse response = request.GetResponse();

                    if (response.ContentType != MediaTypeNames.Image.Gif && response.ContentType != MediaTypeNames.Image.Jpeg &&
                        response.ContentType != MediaTypeNames.Image.Tiff)
                        throw new ArgumentException("Unsupported type.");

                    Image image = Image.FromStream(response.GetResponseStream());

                    try
                    {
                        Stream input = new MemoryStream();

                        try
                        {
                            int width = (int)(image.Size.Width * scaleFactor);
                            int heigth = (int)(image.Size.Height * scaleFactor);

                            Image thumbnail = new Bitmap(width, heigth, image.PixelFormat);

                            try
                            {
                                Graphics graphic = Graphics.FromImage(thumbnail);

                                graphic.CompositingQuality = CompositingQuality.HighQuality;
                                graphic.SmoothingMode = SmoothingMode.HighQuality;
                                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                                Rectangle rectangle = new Rectangle(0, 0, width, heigth);

                                graphic.DrawImage(image, rectangle);

                                thumbnail.Save(input, image.RawFormat);

                                context.Response.ContentType = response.ContentType;

                                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                input.Position = 0;

                                BinaryReader reader = new BinaryReader(input);

                                context.Response.OutputStream.Write(reader.ReadBytes((int)input.Length), 0, (int)input.Length);
                                context.Response.Flush();

                                OnServed(context.Request.RawUrl);
                            }
                            finally
                            {
                                thumbnail.Dispose();
                            }
                        }
                        finally
                        {
                            input.Dispose();
                        }
                    }
                    finally
                    {
                        image.Dispose();
                    }
                }
                catch (WebException)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                OnBadRequest(context.Request.RawUrl);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.Flush();
            }
        }

        #endregion

        /// <summary>
        /// Occurs when a file is being serving.
        /// </summary>
        public static event EventHandler<EventArgs> Serving;

        private static void OnServing(string rawUrl)
        {
            if (Serving != null)
                Serving(rawUrl, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when a file is served;
        /// </summary>
        public static event EventHandler<EventArgs> Served;

        private static void OnServed(string rawUrl)
        {
            if (Served != null)
                Served(rawUrl, EventArgs.Empty);
        }
        
        /// <summary>
        /// Occurs when the requested file does not exist;
        /// </summary>
        public static event EventHandler<EventArgs> BadRequest;

        private static void OnBadRequest(string rawUrl)
        {
            if (BadRequest != null)
                BadRequest(rawUrl, EventArgs.Empty);
        }
    }
}