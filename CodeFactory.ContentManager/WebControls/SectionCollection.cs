﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace CodeFactory.ContentManager.WebControls
{
    public class SectionCollection : List<SectionHierarchyData>, IHierarchicalEnumerable
    {
        public SectionCollection()
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
