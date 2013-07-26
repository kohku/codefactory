using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;

namespace CodeFactory.ContentManager.WebControls
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    ToolboxData("<{0}:CategoryDataSource runat=\"server\"> </{0}:CategoryDataSource>")]
    public class CategoryDataSource : HierarchicalDataSourceControl, IHierarchicalDataSource
    {
        private string viewPath;

        public event EventHandler DataSourceChanged;

        public CategoryDataSource()
        {
        }

        public CategoryDataSource(string viewPath)
        {
            this.viewPath = viewPath;
        }

        protected override HierarchicalDataSourceView GetHierarchicalView(string viewPath)
        {
            return new CategoryDataSourceView(!string.IsNullOrEmpty(this.viewPath) ? this.viewPath : 
                !string.IsNullOrEmpty(viewPath) ? viewPath : Category.Root);
        }

        // The TreeObjectDataSource can be used declaratively. To enable
        // declarative use, override the default implementation of
        // CreateControlCollection to return a ControlCollection that
        // you can add to.
        protected override ControlCollection CreateControlCollection()
        {
            return new ControlCollection(this);
        }

        // Properties
        public string ViewPath
        {
            get
            {
                return this.viewPath;
            }
            set
            {
                this.viewPath = value;
            }
        }

        public void OnDataSourceChanged()
        {
            if (DataSourceChanged != null)
                DataSourceChanged(this, EventArgs.Empty);
        }
    }
}
