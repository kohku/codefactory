using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;

namespace CodeFactory.Web.PostOffice
{
    public class PostOfficeProviderCollection : ProviderCollection
    {
        /// <summary>
        /// Gets a provider by its name.
        /// </summary>
        public new PostOfficeProvider this[string name]
        {
            get { return (PostOfficeProvider)base[name]; }
        }

        /// <summary>
        /// Add a provider to the collection.
        /// </summary>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is PostOfficeProvider))
                throw new ArgumentException
                    ("Invalid provider type", "provider");

            base.Add(provider);
        }
    }
}
