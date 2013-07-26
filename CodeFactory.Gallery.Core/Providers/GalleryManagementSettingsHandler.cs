using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CodeFactory.Gallery.Core.Providers
{
    /// <summary>
    /// A configuration section for web.config.
    /// </summary>
    /// <remarks>
    /// In the config section you can specify the provider you 
    /// want to use for gallery management.
    /// </remarks>
    public class GalleryManagementSettings : ConfigurationSection
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
        [ConfigurationProperty("defaultProvider", IsRequired = true)]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }

    }
}
