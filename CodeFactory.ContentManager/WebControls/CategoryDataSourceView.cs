using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace CodeFactory.ContentManager.WebControls
{
    public class CategoryDataSourceView : HierarchicalDataSourceView
    {
        private string viewPath;

        public CategoryDataSourceView(string viewPath)
        {
            this.viewPath = viewPath;
        }

        public override IHierarchicalEnumerable Select()
        {
            HttpRequest currentRequest = HttpContext.Current.Request;
 
            //if (!currentRequest.IsAuthenticated)
            //    throw new NotSupportedException("The CategoryDataSourceView only presents data in an authenticated context.");

            CategoryCollection categories = new CategoryCollection();

            if (this.viewPath == Category.Root)
            {
                Category root = new Category(Guid.Empty);

                foreach (Category c in root.Childs)
                    categories.Add(new CategoryHierarchyData(c));

                return categories;
            }

            Category category = (Category)ContentManagementService.GetCategory(this.viewPath);

            if (category != null)
            {
                foreach (Category child in category.Childs)
                {
                    categories.Add(new CategoryHierarchyData(child));
                }
            }

            return categories;
        }
    }
}
