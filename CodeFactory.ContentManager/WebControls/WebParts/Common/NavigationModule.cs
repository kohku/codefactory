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
    [Guid("4D1DFA17-6901-4579-962C-F1FDC30C65D2")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class NavigationModule : PersistableModuleWebPart
    {
        public NavigationModule()
        {
            this.Title = ResourceStringLoader.GetResourceString("NavigationModule_Title");
            this.Description = ResourceStringLoader.GetResourceString("NavigationModule_Description");
        }
    }
}
