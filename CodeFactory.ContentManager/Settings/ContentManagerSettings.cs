using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CodeFactory.ContentManager.Settings
{
    public class ContentManagerSettings : ConfigurationSection
    {
        /// <summary>
        /// A collection of registered providers.
        /// </summary>
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get { return (ProviderSettingsCollection)base["providers"]; }
        }

        /// <summary>
        /// The name of the default provider
        /// </summary>
        [StringValidator(MinLength = 1)]
        [ConfigurationProperty("defaultProvider", DefaultValue = "LinqContentManagerProvider")]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }

        public string DefaultPage
        {
            get { return _defaultPage; }
            set { _defaultPage = value; }
        }

        private string _defaultPage;

        [ConfigurationProperty("defaultLayout")]
        public string DefaultLayout
        {
            get { return _defaultLayout; }
            set { _defaultLayout = value; }
        }

        private string _defaultLayout;

        [ConfigurationProperty("siteMapRootName")]
        public string SiteMapRootName
        {
            get { return _siteMapRootName; }
            set { _siteMapRootName = value; }
        }

        private string _siteMapRootName;
    }
}
