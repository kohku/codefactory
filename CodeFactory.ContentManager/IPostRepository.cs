using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web.Core;

namespace CodeFactory.ContentManager
{
    public interface IPostRepository
    {
        IPublishable<Guid> GetPost(Guid id);
        void UpdatePost(IPublishable<Guid> post);
        void InsertPost(IPublishable<Guid> post);
        void DeletePost(IPublishable<Guid> post);
    }
}
