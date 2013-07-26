using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Configuration;
using System.Diagnostics;
using CodeFactory.ContentManager.Settings;
using System.Web.Configuration;
using CodeFactory.ContentManager.Providers;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;

namespace CodeFactory.ContentManager
{
    public sealed class ContentManagementService
    {
        private static object syncRoot = new object();

        private static ContentManagerSettings _settings;

        private static ContentManagementProvider _provider;
        private static ContentManagementProviderCollection _providers;

        static ContentManagementService()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            if (_provider == null)
            {
                lock (syncRoot)
                {
                    if (_provider == null)
                    {
                        _settings = (ContentManagerSettings)
                            ConfigurationManager.GetSection("contentManagerSettings");

                        if (_settings == null)
                            throw new ConfigurationErrorsException("Falta la configuración del content manager service");

                        _providers = new ContentManagementProviderCollection();

                        if (System.Web.HttpContext.Current != null)
                        {
                            ProvidersHelper.InstantiateProviders(
                                _settings.Providers, _providers, typeof(ContentManagementProvider));
                        }
                        else
                        {
                            _ProviderHelper.InstantiateProviders(
                                _settings.Providers, _providers, typeof(ContentManagementProvider));
                        }

                        _provider = _providers[_settings.DefaultProvider];

                        StringDictionary dic = _provider.LoadSettings();

                        Type settingsType = _settings.GetType();

                        foreach (string key in dic.Keys)
                        {
                            string name = key;
                            string value = dic[key];

                            foreach (PropertyInfo propertyInformation in settingsType.GetProperties())
                            {
                                if (propertyInformation.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {
                                        propertyInformation.SetValue(_settings, Convert.ChangeType(value,
                                            propertyInformation.PropertyType, CultureInfo.CurrentCulture), null);
                                    }
                                    catch
                                    {
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static ContentManagerSettings Settings
        {
            get { return _settings; }
        }

        public static void SaveSettings()
        {
            lock (_settings)
            {
                StringDictionary dic = _provider.LoadSettings();

                Type settingsType = _settings.GetType();

                foreach (PropertyInfo propertyInformation in settingsType.GetProperties())
                {
                    foreach (string key in dic.Keys)
                    {
                        string name = key;
                        string value = dic[key];

                        if (propertyInformation.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                dic[key] = propertyInformation.GetValue(_settings, null).ToString();
                            }
                            catch
                            {
                            }
                            break;
                        }
                    }
                }

                _provider.SaveSettings(dic);
            }
        }

        public static ContentManagementProvider Provider
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _provider; }
        }

        public static ContentManagementProviderCollection Providers
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _providers; }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static ICategory GetCategory(Guid id)
        {
            return _provider.GetCategory(id);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void UpdateCategory(ICategory category)
        {
            _provider.UpdateCategory(category);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void InsertCategory(ICategory category)
        {
            _provider.InsertCategory(category);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void DeleteCategory(ICategory category)
        {
            _provider.DeleteCategory(category);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static ICategory GetCategory(string path)
        {
            return _provider.GetCategory(path);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<ICategory> GetCategories(Guid? id, string name, Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return _provider.GetCategories(id, name, parentId, pageSize, pageIndex, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<ICategory> GetChildCategories(Guid? parentId)
        {
            int totalCount;

            return ContentManagementService.GetChildCategories(parentId, int.MaxValue, 0, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<ICategory> GetChildCategories(Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return _provider.GetChildCategories(parentId, pageSize, pageIndex, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static ISection GetSection(Guid id)
        {
            return _provider.GetSection(id);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void InsertSection(ISection section)
        {
            _provider.InsertSection(section);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void UpdateSection(ISection section)
        {
            _provider.UpdateSection(section);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void DeleteSection(ISection section)
        {
            _provider.DeleteSection(section);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static ISection GetSection(string path)
        {
            return _provider.GetSection(path);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<ISection> GetSections(Guid? id, string name, string slug, Guid? parentId)
        {
            int totalCount;

            return _provider.GetSections(id, name, slug, true, parentId, int.MaxValue, 0, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<ISection> GetSections(Guid? id, string name, string slug, bool? isVisible, Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return _provider.GetSections(id, name, slug, isVisible, parentId, pageSize, pageIndex, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<ISection> GetChildSections(Guid? parentId)
        {
            int totalCount;

            return ContentManagementService.GetChildSections(parentId, int.MaxValue, 0, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<ISection> GetChildSections(Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return _provider.GetChildSections(parentId, pageSize, pageIndex, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<string> GetRoles(ISection section)
        {
            return _provider.GetRoles(section);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void UpdateRoles(ISection section)
        {
            _provider.UpdateRoles(section);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static IPage GetPage(Guid id)
        {
            return _provider.GetPage(id);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void InsertPage(IPage page)
        {
            _provider.InsertPage(page);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void UpdatePage(IPage page)
        {
            _provider.UpdatePage(page);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void DeletePage(IPage page)
        {
            _provider.DeletePage(page);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static IPage GetPageBySlug(string slug)
        {
            return _provider.GetPageBySlug(slug);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<IPage> GetChildPages(Guid? parentId)
        {
            int totalCount;

            return ContentManagementService.GetChildPages(parentId, int.MaxValue, 0, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<IPage> GetChildPages(Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return _provider.GetChildPages(parentId, pageSize, pageIndex, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<IPage> GetPages(Guid? id, Guid? parentId, Guid? sectionId, string slug, bool? isVisible, int pageSize, int pageIndex, out int totalCount)
        {
            return _provider.GetPages(id, parentId, sectionId, slug, isVisible, pageSize, pageIndex, out totalCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<string> GetRoles(IPage page)
        {
            return _provider.GetRoles(page);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void UpdateRoles(IPage page)
        {
            _provider.UpdateRoles(page);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<IPage> GetSectionPages(Guid sectionId)
        {
            return _provider.GetSectionPages(sectionId);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static IModule GetModule(Guid id)
        {
            return _provider.GetModule(id);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void InsertModule(IModule module)
        {
            _provider.InsertModule(module);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void UpdateModule(IModule module)
        {
            _provider.UpdateModule(module);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void DeleteModule(IModule module)
        {
            _provider.DeleteModule(module);
        }

        //[System.Diagnostics.DebuggerStepThrough]
        //public static void UpdatePublication(LinqPublication publication)
        //{
        //    _provider.UpdatePublication(publication);
        //}

        //[System.Diagnostics.DebuggerStepThrough]
        //public static void InsertPublication(LinqPublication publication)
        //{
        //    _provider.InsertPublication(publication);
        //}

        //[System.Diagnostics.DebuggerStepThrough]
        //public static void DeletePublication(LinqPublication publication)
        //{
        //    _provider.DeletePublication(publication);
        //}

        //[System.Diagnostics.DebuggerStepThrough]
        //public static LinqPublication GetPublication(Guid id)
        //{
        //    return _provider.GetPublication(id);
        //}

        //[System.Diagnostics.DebuggerStepThrough]
        //public static LinqPost GetPost(Guid id)
        //{
        //    return _provider.GetPost(id);
        //}

        //[System.Diagnostics.DebuggerStepThrough]
        //public static void UpdatePost(LinqPost post)
        //{
        //    _provider.UpdatePost(post);
        //}

        //[System.Diagnostics.DebuggerStepThrough]
        //public static void InsertPost(LinqPost post)
        //{
        //    _provider.InsertPost(post);
        //}

        //[System.Diagnostics.DebuggerStepThrough]
        //public static void DeletePost(LinqPost post)
        //{
        //    _provider.DeletePost(post);
        //}

        internal class _ProviderHelper
        {
            internal static void InstantiateProviders(ProviderSettingsCollection configProviders, ContentManagementProviderCollection providers, Type providerType)
            {
                foreach (ProviderSettings settings in configProviders)
                {
                    providers.Add(InstantiateProvider(settings, providerType));
                }
            }

            internal static System.Configuration.Provider.ProviderBase InstantiateProvider(ProviderSettings providerSettings, Type providerType)
            {
                System.Configuration.Provider.ProviderBase provider = null;

                try
                {
                    string typename = providerSettings.Type == null ? null : providerSettings.Type.Trim();

                    if (string.IsNullOrEmpty(typename))
                        throw new ArgumentException("No provider type name");

                    provider = (System.Configuration.Provider.ProviderBase)Activator.CreateInstance(Type.GetType(typename));

                    NameValueCollection config = new NameValueCollection(providerSettings.Parameters.Count, StringComparer.Ordinal);

                    foreach (string parameter in providerSettings.Parameters)
                        config[parameter] = providerSettings.Parameters[parameter];

                    provider.Initialize(providerSettings.Name, config);
                }
                catch (Exception exception)
                {
                    if (exception is ConfigurationException)
                        throw;

                    throw new ConfigurationErrorsException(exception.Message, providerSettings.ElementInformation.Properties["type"].Source, providerSettings.ElementInformation.Properties["type"].LineNumber);
                }

                return provider;
            }
        }

    }
}
