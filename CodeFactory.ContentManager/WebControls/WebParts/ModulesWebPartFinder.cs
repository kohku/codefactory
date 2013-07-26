#define INCLUDING_GENERIC_WEB_PARTS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls.WebParts;

namespace CodeFactory.ContentManager.WebControls.WebParts
{
    [Serializable]
    class ModulesWebPartFinder
    {
        private List<Type> _goodtypes = null;

        public ModulesWebPartFinder()
        {
            _goodtypes = new List<Type>();
        }

        public List<Type> SearchPath(string path)
        {
            _goodtypes.Clear();

            foreach (string file in Directory.GetFiles(path, "*.dll"))
                TryLoadingPlugin(file);

            return _goodtypes;
        }

        private void TryLoadingPlugin(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                path = file.Name.Replace(file.Extension, "");
                Assembly assembly = AppDomain.CurrentDomain.Load(path);

                foreach (Type type in assembly.GetTypes())
                {
#if DEBUG
                    Type[] interfaces = type.FindInterfaces(new TypeFilter(delegate(Type typeObj, Object criteriaObj){
                        if (typeObj == null || criteriaObj == null)
                            return false;
                        return typeObj.ToString().Equals(criteriaObj.ToString(), StringComparison.OrdinalIgnoreCase);
                    }), "System.Web.UI.WebControls.WebParts.IWebPart");
#endif

#if INCLUDING_GENERIC_WEB_PARTS
                    if (type.IsSubclassOf(typeof(WebPart)) && !type.IsAbstract)
                        _goodtypes.Add(type);
#else
                    if (type.IsSubclassOf(typeof(ModuleWebPart)) && !type.IsAbstract)
                        _goodtypes.Add(type);
#endif
                }
            }
            catch (Exception) { /* Ignore exception */ }
        }
    }
}