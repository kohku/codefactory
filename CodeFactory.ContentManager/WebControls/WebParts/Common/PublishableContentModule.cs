using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Security.Permissions;
using CodeFactory.Utilities;

namespace CodeFactory.ContentManager.WebControls.WebParts.Common
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class PublishableContentModule : PublishableContentModuleWebPart
    {
        public PublishableContentModule()
        {
            this.Title = ResourceStringLoader.GetResourceString("PublishableContentModule_Title");
            this.Description = ResourceStringLoader.GetResourceString("PublishableContentModule_Description");
        }
    }
}
