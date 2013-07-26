using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.Security;

namespace CodeFactory.Wiki.HttpModules
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class CredentialsScreeningModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AuthorizeRequest += new EventHandler(context_AuthorizeRequest);
        }

        void context_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            if (context.SkipAuthorization || context.Request.IsAuthenticated)
                return;

            string username = context.Request.ServerVariables["LOGON_USER"];

            bool skipAuthentication = !string.IsNullOrEmpty(context.Request.QueryString["SkipAuthentication"]) ?
                Convert.ToBoolean(context.Request.QueryString["SkipAuthentication"]) : false;

            if (!skipAuthentication && string.IsNullOrEmpty(username))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<html>");
                builder.Append("<head>");
                builder.Append("<script language=\"javascript\">");
                builder.Append("function credentialScreening(){");
                if (!context.Request.RawUrl.ToLowerInvariant().Contains("authenticated.aspx?returnurl="))
                {
                    builder.Append("if (canAuthenticate()){");
                    builder.AppendFormat("window.location = \"authenticated.aspx?ReturnUrl={0}\";", context.Request.RawUrl);
                    builder.Append("}");

                    if (!context.Request.RawUrl.ToLowerInvariant().Contains("skipauthentication="))
                    {
                        builder.Append("else{");
                        builder.AppendFormat("window.location = \"{0}{1}SkipAuthentication=true\";",
                            context.Request.RawUrl, !context.Request.RawUrl.Contains("?") ? "?" : "&");
                        builder.Append("}");
                    }
                }
                builder.Append("}");
                builder.Append("function canAuthenticate() {");
                builder.Append("try {");
                builder.Append("var dom = new ActiveXObject(\"Msxml2.DOMDocument\");");
                builder.Append("dom.async = false;");
                builder.AppendFormat("dom.load(\"{0}\");", VirtualPathUtility.ToAbsolute("~/authentication/WinLogin.aspx"));
                builder.Append("}");
                builder.Append("catch (e) {");
                builder.Append("return false;");
                builder.Append("}");
                builder.Append("return true;");

                builder.Append("}");
                builder.Append("</script>");
                builder.Append("</head>");
                builder.Append("<body onload=\"credentialScreening();\">");
                builder.Append("</body>");
                builder.Append("</html>");
                context.Response.Write(builder.ToString());
                context.Response.End();
                return;
            }

            if (!skipAuthentication && !string.IsNullOrEmpty(username) &&
                !context.Request.RawUrl.ToLowerInvariant().Contains("skipauthentication="))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                StringBuilder builder = new StringBuilder();
                builder.Append("<html>");
                builder.Append("<head>");
                builder.Append("<script language=\"javascript\">");
                builder.Append("function reload(){");
                builder.AppendFormat("window.location = \"{0}{1}SkipAuthentication=true\";", 
                    context.Request.RawUrl,
                    !context.Request.RawUrl.Contains("?") ? "?" : "&");
                builder.Append("}");
                builder.Append("</script>");
                builder.Append("</head>");
                builder.Append("<body onload=\"reload();\">");
                builder.Append("</body>");
                builder.Append("</html>");
                context.Response.Write(builder.ToString());
                context.Response.End();
                return;
            }
        }

        #endregion
    }
}
