using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.ContentManager
{
    public interface IPageRepository
    {
        IPage GetPage(Guid id);
        void UpdatePage(IPage category);
        void InsertPage(IPage category);
        void DeletePage(IPage category);

        IPage GetPageBySlug(string slug);
        List<IPage> GetChildPages(Guid? parentId, int pageSize, int pageIndex, out int totalCount);

        List<string> GetRoles(IPage page);
        void UpdateRoles(IPage page);
    }
}
