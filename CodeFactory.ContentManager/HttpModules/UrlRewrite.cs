//#define GET_PAGE_ASPX

using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using System.Web;
using CodeFactory.Web;
using System.Web.Security;
using System.IO;

namespace CodeFactory.ContentManager.HttpModules
{
    /// <summary>
    /// Handles pretty URL's and redirects them to the permalinks.
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class UrlRewrite : IHttpModule
    {
        #region IHttpModule Members

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module 
        /// that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> 
        /// that provides access to the methods, properties, and events common to 
        /// all application objects within an ASP.NET application.
        /// </param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        #endregion

        /// <summary>
        /// Handles the begin request of the http aplication context.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            string url = context.Request.RawUrl;

            // Won't process: http handlers, login url, posts
            if (context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
                context.Request.Url.LocalPath.ToLower() == FormsAuthentication.LoginUrl.ToLower() ||
                url.EndsWith("css", StringComparison.OrdinalIgnoreCase) ||
                url.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                url.EndsWith("gif", StringComparison.OrdinalIgnoreCase) ||
                url.ToLower().Contains(".axd"))
                return;

            string directory = VirtualPathUtility.GetDirectory(context.Request.Url.LocalPath).ToLower();
            string filename = VirtualPathUtility.GetFileName(context.Request.Url.LocalPath).ToLower();
            string extension = VirtualPathUtility.GetExtension(context.Request.Url.LocalPath).ToLower();
            string sections = directory.Contains(context.Request.ApplicationPath.ToLower()) ?
                directory.Remove(0, VirtualPathUtility.AppendTrailingSlash(context.Request.ApplicationPath).Length) : directory;
            string slug = context.Server.UrlEncode(Path.GetFileNameWithoutExtension(filename));

            //List<string> slugs = new List<string>(VirtualPathUtility.RemoveTrailingSlash(directory).Remove(
            //    0, context.Request.ApplicationPath.Length).Split(
            //    new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));

            if (!slug.Equals("getpage", StringComparison.OrdinalIgnoreCase) && extension.Equals(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                Page content = Page.GetPageBySlug(sections + slug);

                if (content != null)
                {
#if GET_PAGE_ASPX
                    context.RewritePath(string.Format("{0}getpage.aspx?id={1}{2}", Utils.RelativeWebRoot, content.ID, GetQueryString(context)), false);
#else
                    context.RewritePath(string.Format("{0}page.aspx?id={1}{2}", Utils.RelativeWebRoot, content.ID, GetQueryString(context)), false);
#endif
                    return;
                }
            }
        }

        private void RewritePage(HttpContext context)
        {
            string slug = ExtractSlug(context);

            if (string.IsNullOrEmpty(slug))
                return;

            Page content = Page.GetPageBySlug(slug);

            if (content != null)
            {
                context.RewritePath(string.Format("{0}page.aspx?id={1}{2}", Utils.RelativeWebRoot,
                    content.ID, GetQueryString(context)), false);
            }
        }

        private string ExtractSlug(HttpContext context)
        {
            string url = context.Request.RawUrl.ToLowerInvariant();

            if (url.Contains(".aspx") && url.EndsWith("/"))
            {
                url = context.Request.RawUrl.Substring(0, context.Request.RawUrl.Length - 1);
                context.Response.AppendHeader("location", url);
                context.Response.StatusCode = 301;
                context.Response.Flush();
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(url) && !url.Contains(".aspx"))
            {
                url = context.Request.RawUrl + ".aspx";
                context.Response.AppendHeader("location", url);
                context.Response.StatusCode = 301;
                context.Response.Flush();
                return string.Empty;
            }

            url = url.Substring(0, url.IndexOf(".aspx"));
            int index = url.LastIndexOf("/") + 1;

            return context.Server.UrlEncode(url.Substring(index));
        }

        private string GetQueryString(HttpContext context)
        {
            string query = context.Request.QueryString.ToString();
            if (!string.IsNullOrEmpty(query))
                return "&" + query;

            return string.Empty;

        }
    }
}
