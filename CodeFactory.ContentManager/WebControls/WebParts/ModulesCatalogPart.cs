using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using System.Web;
using System.Collections;
using System.Collections.ObjectModel;

namespace CodeFactory.ContentManager.WebControls.WebParts
{
    public class ModulesCatalogPart : CatalogPart
    {
        private Dictionary<WebPartDescription, WebPart> webparts;

        public ModulesCatalogPart()
        {
            webparts = new Dictionary<WebPartDescription, WebPart>();
        }

        /// <summary>
        /// Loads the descriptions that display in the catalog part at run time.
        /// </summary>
        /// <returns></returns>
        public override WebPartDescriptionCollection GetAvailableWebPartDescriptions()
        {
            //If this is at runtime, call CacheParts which caches the results
            if (IsWebApp)
                return CacheParts(true);
            else
                return DesignerParts(); 
        }

        /// <summary>
        /// Caches the results of the LoadPluggableWebParts() function to avoid accessing the file system and parsing the dll's on each request.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private WebPartDescriptionCollection CacheParts(bool cache)
        {
            //If both objects are not null, pull them from cache
            if (HttpContext.Current.Cache["ReflectionPartsDescriptions"] != null && HttpContext.Current.Cache["ReflectionParts"] != null)
            {
                webparts = (Dictionary<WebPartDescription, WebPart>)HttpContext.Current.Cache["ReflectionParts"];
                return (WebPartDescriptionCollection)HttpContext.Current.Cache["ReflectionPartsDescriptions"];
            }

            //If either are missing, run LoadPluggableWebParts and cache the results
            else
            {
                WebPartDescriptionCollection wpd = LoadPluggableWebParts();
                HttpContext.Current.Cache["ReflectionPartsDescriptions"] = wpd;
                HttpContext.Current.Cache["ReflectionParts"] = webparts;
                return wpd;
            }
        }

        // If they are in the designer, call LoadPluggableWebParts without caching
        // Probably be better to build a designer for the control and do that work there
        private WebPartDescriptionCollection DesignerParts()
        {
            //If they are in the designer, call ReflectParts without caching
            //Probably be better to build a designer for the control and do that work there
            return LoadPluggableWebParts();
        }

        public override WebPart GetWebPart(WebPartDescription description)
        {
            return (WebPart)webparts[description];
        }

        private WebPartDescriptionCollection LoadPluggableWebParts()
        {
            webparts.Clear();
            Collection<WebPartDescription> collection = new Collection<WebPartDescription>();

            //Do this check to keep the Designer from throwing an exception when rendering the catalog.
            if (WebPartManager != null)
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();

                // Working with a different AppDomain.
                AppDomain domain = AppDomain.CreateDomain("PluginLoader");
                ModulesWebPartFinder finder = (ModulesWebPartFinder)domain.CreateInstanceFromAndUnwrap(
                    executingAssembly.CodeBase, typeof(ModulesWebPartFinder).ToString());

                List<Type> pluginsfound = finder.SearchPath(AppDomain.CurrentDomain.RelativeSearchPath);

                AppDomain.Unload(domain);

                // Working without a different AppDomain.
                foreach (Type type in pluginsfound)
                {
                    try
                    {
                        WebPart plugin = (WebPart)Activator.CreateInstance(type);

                        if (plugin == null)
                            continue;

                        plugin.ID = type.ToString() + "1"; //have to give an ID to the WebPart.

                        WebPartDescription item = new WebPartDescription(plugin);
                        collection.Add(item);
                        webparts.Add(item, plugin);
                    }
                    catch (Exception) { /* Ignore exception */ }
                }
            }
            // We can also create a generic web part to display something withing the catalog.

            return new WebPartDescriptionCollection(collection);
        }

        private bool IsWebApp
        {
            get
            {
                //HttpContext.Current will be null at design time
                return (HttpContext.Current != null) ? true : false;
            }
        }
    }
}
