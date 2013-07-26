using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.Web.Web.UI
{
    public interface IHierarchyData<T>
    {
        bool HasChildren { get; }
        T Item { get; }
        string Path { get; }
        string Type { get; }

        IHierarchicalEnumerable<T> GetChildren();
        IHierarchyData<T> GetParent();
    }
}
