using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CodeFactory.Utilities;
using System.Web;
using System.Security.Permissions;

namespace CodeFactory.ContentManager.WebControls.WebParts.Common
{
    [Guid("F7239123-964E-4471-9D0E-070F46290B61")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class FooterModule : PersistableModuleWebPart
    {
        public FooterModule()
        {
            this.Title = ResourceStringLoader.GetResourceString("FooterModule_Title");
            this.Description = ResourceStringLoader.GetResourceString("FooterModule_Description");
        }
    }
}
