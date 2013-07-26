using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using CodeFactory.Wiki.Workflow;

namespace CodeFactory.Wiki
{
    public sealed class WikiService
    {
        private static WikiProvider _defaultProvider;
        private static WikiProviderCollection _providers;

        private static List<string> _restrictedSlugs;

        static WikiService()
        {
            _providers = new WikiProviderCollection();
            _restrictedSlugs = new List<string>();

            LoadProviders();
        }

        private static void LoadProviders()
        {
            try
            {
                WikiSettings settings = (WikiSettings)
                    ConfigurationManager.GetSection("WikiSettings");

                // There's no settings available, so we're gonna use default provider.
                if (settings == null)
                    throw new ApplicationException("There's not wiki management service settings handler section.");

                // Initializing and adding all specified providers.
                foreach (ProviderSettings element in settings.Providers)
                {
                    Type handler = Type.GetType(element.Type);

                    if (handler == null)
                        throw new ApplicationException(
                            string.Format("Could not load handler named {0} of type {1}.", element.Name, element.Type));

                    WikiProvider provider = (WikiProvider)Activator.CreateInstance(handler);

                    if (provider == null)
                        continue;

                    // Initializes the provider.
                    provider.Initialize(element.Name, element.Parameters);

                    if (provider.Name.Equals(settings.DefaultProvider))
                        _defaultProvider = provider;

                    _providers.Add(provider);
                }

                foreach (RestrictedSlug item in settings.RestrictedSlugs)
                    _restrictedSlugs.Add(item.Slug);

                 //Set the default provider to the provider included within the assembly, if not specified.
                if (_defaultProvider == null && _providers.Count == 0)
                    throw new ApplicationException("There's no wiki management service provider.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while loading wiki management service settings from config.", ex);
            }
        }

        public static WikiProvider Provider
        {
            get { return _defaultProvider; }
        }

        public static WikiProviderCollection Providers
        {
            get { return _providers; }
        }

        public static List<string> RestrictedSlugs
        {
            get { return _restrictedSlugs; }
        }

        public static IWiki SelectWiki(Guid id)
        {
            return _defaultProvider.SelectWiki(id);
        }

        public static void InsertWiki(IWiki item)
        {
            _defaultProvider.InsertWiki(item);
        }

        public static void UpdateWiki(IWiki item)
        {
            _defaultProvider.UpdateWiki(item);
        }

        public static void DeleteWiki(IWiki item)
        {
            _defaultProvider.DeleteWiki(item);
        }

        public static IWiki GetRandomWiki()
        {
            return _defaultProvider.GetRandomWiki();
        }

        public static IWiki GetRelatedWiki(string keyword)
        {
            return null;
            //return GetRelatedWiki(keyword);
        }

        public static List<string> GetCategories()
        {
            return _defaultProvider.GetCategories();
        }

        public static void InsertCategory(string category)
        {
            _defaultProvider.InsertCategory(category);
        }

        public static bool DeleteCategory(string category)
        {
            return _defaultProvider.DeleteCategory(category);
        }

        public static List<Guid> GetWiki(Guid? id, string title, string description, string content, string author,
            string slug, string category, string keywords,
            int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetWiki(id, title, description, content, author, slug, category, keywords, pageSize, pageIndex, out totalCount);
        }

        public static List<Guid> GetWiki(Guid? id, string title, string description, string content, string author, string slug,
            bool? isVisible, string category, string keywords, ReachLevel? level, DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated, string lastUpdatedBy,
            int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetWiki(id, title, description, content, author, slug, isVisible, category, keywords, level,
                initialDateCreated, finalDateCreated, initialLastUpdated, finalLastUpdated, lastUpdatedBy, 
                pageSize, pageIndex, out totalCount);
        }

        public static List<Guid> SearchWiki(string title, string description, string content, string author, string slug,
            string category, string keywords, int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.SearchWiki(title, description, content, author, slug, category, keywords, pageSize, pageIndex, out totalCount);
        }

        public static List<Guid> SearchWiki(string title, string description, string content, string author, string slug,
            string category, string keywords, ReachLevel? level, int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.SearchWiki(title, description, content, author, slug, category, keywords, level, pageSize, pageIndex, out totalCount);
        }

        public static List<Guid> SearchWiki(string title, string description, string content, string author, string slug,
            bool? isVisible, string category, string keywords, ReachLevel? level,
            DateTime? initialDateCreated, DateTime? finalDateCreated, DateTime? initialLastUpdated, DateTime? finalLastUpdated,
            string lastUpdatedBy, int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.SearchWiki(title, description, content, author, slug, isVisible, category, keywords, level,
                initialDateCreated, finalDateCreated, initialLastUpdated, finalLastUpdated, lastUpdatedBy, pageSize, pageIndex, out totalCount);
        }

        public static List<Guid> SearchWiki(string alphabeticalIndex, int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.SearchWiki(alphabeticalIndex, pageSize, pageIndex, out totalCount);
        }

        public static Dictionary<string, int> GetTagCloud()
        {
            return _defaultProvider.GetTagCloud();
        }

        public static IWorkWikiItem SelectWorkWikiItem(Guid id)
        {
            return _defaultProvider.SelectWorkWikiItem(id);
        }

        public static void InsertWorkWikiItem(IWorkWikiItem item)
        {
            _defaultProvider.InsertWorkWikiItem(item);
        }

        public static void UpdateWorkWikiItem(IWorkWikiItem item)
        {
            _defaultProvider.UpdateWorkWikiItem(item);
        }

        public static void DeleteWorkWikiItem(IWorkWikiItem item)
        {
            _defaultProvider.DeleteWorkWikiItem(item);
        }

        public static IWorkWikiItem SelectWikiHistory(Guid id)
        {
            return _defaultProvider.SelectWikiHistory(id);
        }

        public static void InsertWikiHistory(IWorkWikiItem item)
        {
            _defaultProvider.InsertWikiHistory(item);
        }

        public static void UpdateWikiHistory(IWorkWikiItem item)
        {
            _defaultProvider.UpdateWikiHistory(item);
        }

        public static void DeleteWikiHistory(IWorkWikiItem item)
        {
            _defaultProvider.DeleteWikiHistory(item);
        }

        public static List<Guid> GetWikiHistory(Guid? trackingNumber, Guid? id, string title, string description, string content, string author, string slug,
            string category, string keywords, int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetWikiHistory(trackingNumber, id, title, description, content, author, slug,
                category, keywords, pageSize, pageIndex, out totalCount);
        }

        public static List<Guid> GetWikiHistory(Guid? trackingNumber, Guid? id, string title, string description, string content, string author, string slug,
            bool? isVisible, string category, string keywords, ReachLevel? level, DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated, DateTime? initialExpirationDate, DateTime? finalExpirationDate,
            string lastUpdatedBy, int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetWikiHistory(trackingNumber, id, title, description, content, author, slug,
                isVisible, category, keywords, level, initialDateCreated, finalDateCreated,
                initialLastUpdated, finalLastUpdated, initialExpirationDate, finalExpirationDate,
                lastUpdatedBy, pageSize, pageIndex, out totalCount);
        }

        public static List<Guid> GetWorkWikiItem(Guid? trackingNumber, Guid? id, string title, string description, string content, string author, string slug,
            string category, string keywords, int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetWorkWikiItem(trackingNumber, id, title, description, content, author, slug,
                category, keywords, pageSize, pageIndex, out totalCount);
        }

        public static List<Guid> GetWorkWikiItem(Guid? trackingNumber, Guid id, string title, string description, string content, string author, string slug,
            bool? isVisible, string category, string keywords, ReachLevel? level, DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated, DateTime? initialExpirationDate, DateTime? finalExpirationDate, string lastUpdatedBy,
            int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetWorkWikiItem(trackingNumber, id, title, description, content, author, slug,
                isVisible, category, keywords, level, initialDateCreated, finalDateCreated,
                initialLastUpdated, finalLastUpdated, initialExpirationDate, finalExpirationDate,
                lastUpdatedBy, pageSize, pageIndex, out totalCount);
        }

        public static void RequestAuthorization(IWorkWikiItem item)
        {
            _defaultProvider.RequestAuthorization(item);
        }

        public static void AuthorizeWiki(IWorkWikiItem item)
        {
            _defaultProvider.AuthorizeWiki(item);
        }

        public static void RejectAuthorization(IWorkWikiItem item)
        {
            _defaultProvider.RejectAuthorization(item);
        }

        public static List<IWorkWikiItem> GetPendingAuthorizations(string user)
        {
            return _defaultProvider.GetPendingAuthorizations(user);
        }

        public static List<IWorkWikiItem> GetPendingAuthorizations(string user, int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetPendingAuthorizations(user, pageSize, pageIndex, out totalCount);
        }

        public static Dictionary<string, string> GetAuthorizersByCategory(int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetAuthorizersByCategory(pageSize, pageIndex, out totalCount);
        }

        public static void InsertAuthorizerByCategory(string category, string username)
        {
            _defaultProvider.InsertAuthorizerByCategory(category, username);
        }

        public static void DeleteAuthorizerByCategory(string category)
        {
            _defaultProvider.DeleteAuthorizerByCategory(category);
        }
    }
}
