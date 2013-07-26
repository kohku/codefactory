using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web.Storage;
using CodeFactory.Web.Core;

namespace CodeFactory.Wiki
{
    public interface IWiki : IPublishable<Guid>
    {
        string Category { get; }
        string DepartmentArea { get; }
        bool Editable { get; }
        string Editor { get; }
        ReachLevel ReachLevel { get; }

        List<UploadedFile> Files { get; }
        List<UploadedFile> Images { get; }

        void AddFile(UploadedFile item);
        void RemoveFile(UploadedFile item);

    }
}
