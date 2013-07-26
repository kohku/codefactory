using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.ContentManager
{
    public interface ISectionRepository
    {
        ISection GetSection(Guid id);
        void UpdateSection(ISection category);
        void InsertSection(ISection category);
        void DeleteSection(ISection category);

        ISection GetSection(string path);
        List<ISection> GetSections(Guid? id, string name, string slug, bool? isVisible, Guid? parentId, int pageSize, int pageIndex, out int totalCount);
        List<ISection> GetChildSections(Guid? parentId, int pageSize, int pageIndex, out int totalCount);

        List<string> GetRoles(ISection section);
        void UpdateRoles(ISection section);
    }
}
