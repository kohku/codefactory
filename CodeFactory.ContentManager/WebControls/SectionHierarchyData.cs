using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace CodeFactory.ContentManager.WebControls
{
    public class SectionHierarchyData : IHierarchyData
    {
        private ISection item;

        public SectionHierarchyData(ISection item)
        {
            this.item = item;
        }

        #region IHierarchyData Members

        public IHierarchicalEnumerable GetChildren()
        {
            SectionCollection children = new SectionCollection();

            foreach (ISection child in item.Childs)
                children.Add(new SectionHierarchyData(child));

            return children;
        }

        public IHierarchyData GetParent()
        {
            if (item.Parent == null)
                return null;

            return new SectionHierarchyData(item.Parent);
        }

        public bool HasChildren
        {
            get
            {
                return item.Childs.Count > 0;
            }
        }

        public object Item
        {
            get { return item; }
        }

        public string Path
        {
            get { return item.Path; }
        }

        public string Type
        {
            get { return item.GetType().Name; }
        }

        public override string ToString()
        {
            return item.Name;
        }

        public override int GetHashCode()
        {
            return item.GetHashCode();
        }
        #endregion
    }
}
