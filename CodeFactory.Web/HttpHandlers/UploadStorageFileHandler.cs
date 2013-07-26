using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using CodeFactory.Web.Storage;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace CodeFactory.Web.HttpHandlers
{
    public class UploadStorageFileHandler : IHttpHandler
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
                bool inline = false;

                string id = context.Request.QueryString["guid"];

                if (context.Request.QueryString["inline"] != null)
                    inline = Convert.ToBoolean(context.Request.QueryString["inline"]);

                OnServing(id);

                try
                {
                    UploadedFile file = null;

                    file = UploadedFile.Load(new Guid(id));

                    if (file == null)
                    {
                        // file not found
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.Flush();
                        return;
                    }

                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    // instructs a user agent (i.e. Internet Explorer) to save a file to disk or saving it inline.
                    context.Response.AddHeader("Content-Disposition",
                        string.Format("{0}; filename={1}", inline ? "inline" : "attachment", file.FileName));
                    context.Response.AddHeader("Content-Type", file.ContentType);
                    context.Response.AddHeader("Content-Length", file.ContentLength.ToString());

                    context.Response.BufferOutput = false;

                    long dataToRead = file.InputStream.Length;

                    BinaryReader reader = new BinaryReader(file.InputStream);

                    try
                    {
                        Trace.TraceInformation(string.Format("{0} Sending file {1}", DateTime.Now, id));

                        while (dataToRead > 0)
                        {
                            if (!context.Response.IsClientConnected)
                            {
                                dataToRead = -1;
                                continue;
                            }

                            byte[] fragment = reader.ReadBytes(4096);

                            // Some files are compressed in db, so there could be a subtle difference in length
                            // between file.ContentLength and file.InputStream.Length.
                            if (fragment.Length == 0)
                            {
                                dataToRead = -1;
                                continue;
                            }

                            context.Response.OutputStream.Write(fragment, 0, fragment.Length);
                            context.Response.Flush();

                            dataToRead -= fragment.Length;
                        }
                        Trace.TraceInformation(string.Format("{0} File {1} sent", DateTime.Now, id));

                        OnServed(id);
                    }
                    finally
                    {
                        reader.Close();
                        context.Response.Close();
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(string.Format("{0} ProcessRequest failed. Description: {1}",
                        DateTime.Now, ex.Message));
                    OnBadRequest(id);
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.End();
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
