using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.Web.Web.UI
{
    public interface IHierarchicalEnumerable<T> : IEnumerable<T>
    {
        IHierarchyData<T> GetHierarchyData(T enumeratedItem);
    }
}
