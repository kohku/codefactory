using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.ContentManager
{
    public interface ICategoryRepository
    {
        ICategory GetCategory(Guid id);
        void UpdateCategory(ICategory category);
        void InsertCategory(ICategory category);
        void DeleteCategory(ICategory category);

        ICategory GetCategory(string path);
        List<ICategory> GetCategories(Guid? id, string name, Guid? parentId, int pageSize, int pageIndex, out int totalCount);
        List<ICategory> GetChildCategories(Guid? parentId, int pageSize, int pageIndex, out int totalCount);
    }
}
