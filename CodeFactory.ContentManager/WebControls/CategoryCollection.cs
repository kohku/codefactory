using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace CodeFactory.ContentManager.WebControls
{
    public class CategoryCollection : List<CategoryHierarchyData>, IHierarchicalEnumerable
    {
        public CategoryCollection()
        {
        }

        #region IHierarchicalEnumerable Members

        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return enumeratedItem as IHierarchyData;
        }

        #endregion
    }
}
