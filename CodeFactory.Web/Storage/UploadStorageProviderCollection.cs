using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Text;

namespace CodeFactory.Web.Storage
{
    /// <summary>
    /// A collection of all registered providers.
    /// </summary>
    public class UploadStorageProviderCollection : ProviderCollection
    {
        /// <summary>
        /// Gets a provider by its name.
        /// </summary>
        public new UploadStorageProvider this[string name]
        {
            get { return (UploadStorageProvider)base[name]; }
        }

        /// <summary>
        /// Add a provider to the collection.
        /// </summary>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is UploadStorageProvider))
                throw new ArgumentException
                    ("Invalid provider type", "provider");

            base.Add(provider);
        }
    }
}
