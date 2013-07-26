using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web.Core;

namespace CodeFactory.ContentManager
{
    public interface IPublicationRepository
    {
        IPublishable<Guid> GetPublication(Guid id);
        void UpdatePublication(IPublishable<Guid> publication);
        void InsertPublication(IPublishable<Guid> publication);
        void DeletePublication(IPublishable<Guid> publication);
    }
}
