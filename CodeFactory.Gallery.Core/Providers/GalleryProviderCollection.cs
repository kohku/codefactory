using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Text;

namespace CodeFactory.Gallery.Core.Providers
{
    /// <summary>
    /// A collection of all registered providers.
    /// </summary>
    public class GalleryManagementProviderCollection : ProviderCollection
    {
        /// <summary>
        /// Gets a provider by its name.
        /// </summary>
        public new GalleryProvider this[string name]
        {
            get { return (GalleryProvider)base[name]; }
        }

        /// <summary>
        /// Add a provider to the collection.
        /// </summary>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is GalleryProvider))
                throw new ArgumentException
                    ("Invalid provider type", "provider");

            base.Add(provider);
        }
    }
}
