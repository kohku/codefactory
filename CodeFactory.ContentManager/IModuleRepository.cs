using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.ContentManager
{
    public interface IModuleRepository
    {
        IModule GetModule(Guid id);
        void InsertModule(IModule module);
        void UpdateModule(IModule module);
        void DeleteModule(IModule module);
    }
}
