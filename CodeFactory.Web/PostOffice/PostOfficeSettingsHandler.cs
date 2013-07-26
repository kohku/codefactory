using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace CodeFactory.Web.PostOffice
{
    public class PostOfficeSettingsHandler : ConfigurationSection
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
        [ConfigurationProperty("defaultProvider", DefaultValue = "SmtpPostOfficeProvider")]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }
    }
}
