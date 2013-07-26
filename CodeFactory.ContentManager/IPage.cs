using System;
using System.ComponentModel;
using System.Collections.Generic;
using CodeFactory.Web.Core;

namespace CodeFactory.ContentManager
{
    public interface IPage : CodeFactory.Web.Core.IPublishable<Guid>
    {
        ISection Section { get; set; }
        IPage Parent { get; set; }
        string Layout { get; set; }
        string AbsoluteSlug { get; }
        List<IPage> Childs { get; }
    }
}
