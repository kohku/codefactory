using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;

namespace CodeFactory.Wiki
{
    public class WikiProviderCollection : ProviderCollection
    {
        /// <summary>
        /// Gets a provider by its name.
        /// </summary>
        public new WikiProvider this[string name]
        {
            get { return (WikiProvider)base[name]; }
        }

        /// <summary>
        /// Add a provider to the collection.
        /// </summary>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is WikiProvider))
                throw new ArgumentException("Invalid provider type", "provider");

            base.Add(provider);
        }
    }
}
