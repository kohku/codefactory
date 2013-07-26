using System;
namespace CodeFactory.ContentManager
{
    public interface IModule : CodeFactory.Web.Core.IIdentifiable<Guid>
    {
        string Title { get; set; }
        byte[] ContentRaw { get; set; }
        DateTime DateCreated { get; }
        DateTime? LastUpdated { get; }
    }
}
