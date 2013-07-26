using System;
namespace CodeFactory.Web.Storage
{
    public interface IUploadedFile : CodeFactory.Web.Core.IIdentifiable<Guid>
    {
        string ApplicationName { get; }
        Guid ParentID { get; }
        string FileName { get; }
        string Description { get; }
        byte[] Data { get; }
        DateTime DateCreated { get; }
        DateTime? LastUpdated { get; }
        string ContentType { get; }
        int ContentLength { get; }
    }
}
