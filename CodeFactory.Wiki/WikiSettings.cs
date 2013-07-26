using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CodeFactory.Wiki
{
    public class WikiSettings : ConfigurationSection
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
        [ConfigurationProperty("defaultProvider", DefaultValue = "WorkflowServiceProvider")]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }

        [ConfigurationProperty("restrictedSlugs")]
        public RestrictedSlugCollection RestrictedSlugs
        {
            get { return (RestrictedSlugCollection)base["restrictedSlugs"]; }
            set { base["restrictedSlugs"] = value; }
        }
    }

    public class RestrictedSlugCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RestrictedSlug();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RestrictedSlug)element).Slug;
        }
    }

    public class RestrictedSlug : ConfigurationElement
    {
        [ConfigurationProperty("slug")]
        public string Slug
        {
            get { return (string)base["slug"]; }
            set { base["slug"] = value; }
        }
    }
}
