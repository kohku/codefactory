using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.ContentManager
{
    /// <summary>
    /// Unidad de división virtual o física.
    /// </summary>
    public interface ISection : CodeFactory.Web.Core.IPublishable<Guid>
    {
        string Name { get; set; }
        ISection Parent { get; set; }
        int Index { get; set; }
        string Path { get; }
        string AbsoluteSlug { get; }
        List<ISection> Childs { get; }
        List<IPage> Pages { get; }
    }
}
