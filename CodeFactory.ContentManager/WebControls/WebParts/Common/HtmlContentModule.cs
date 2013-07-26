using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;
using CodeFactory.Utilities;

namespace CodeFactory.ContentManager.WebControls.WebParts.Common
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class HtmlContentModule : ModuleWebPart
    {
        public HtmlContentModule()
        {
            this.Title = ResourceStringLoader.GetResourceString("HtmlContentModule_Title");
            this.Description = ResourceStringLoader.GetResourceString("HtmlContentModule_Description");
        }
    }
}
