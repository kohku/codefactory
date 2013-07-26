using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Utilities;
using System.Runtime.InteropServices;
using System.Web;
using System.Security.Permissions;

namespace CodeFactory.ContentManager.WebControls.WebParts.Common
{
    [Guid("A0E8991C-5D49-4317-93DE-D79F006F9BEA")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class HeaderModule : PersistableModuleWebPart
    {
        // Methods
        public HeaderModule()
        {
            this.Title = ResourceStringLoader.GetResourceString("HeaderModule_Title");
            this.Description = ResourceStringLoader.GetResourceString("HeaderModule_Description");
        }
    }
}
