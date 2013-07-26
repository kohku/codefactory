using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web.Storage;

namespace CodeFactory.Gallery.Core
{
    public interface IGallery : CodeFactory.Web.Core.IPublishable<Guid>
    {
        string ApplicationName { get; }

        GalleryStatus Status { get; }

        List<UploadedFile> Files { get; }

        List<Comment> Comments { get; }

        List<string> Users { get; }
    }
}
