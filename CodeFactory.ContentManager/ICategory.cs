using System;
using System.Collections.Generic;
namespace CodeFactory.ContentManager
{
    /// <summary>
    /// Define clase, condición o clasificación.
    /// </summary>
    public interface ICategory : CodeFactory.Web.Core.IIdentifiable<Guid>
    {
        string Name { get; set; }
        string Path { get; }
        ICategory Parent { get; set; }
        List<ICategory> Childs { get; }
        DateTime DateCreated { get; set; }
        DateTime? LastUpdated { get; set; }
    }
}
