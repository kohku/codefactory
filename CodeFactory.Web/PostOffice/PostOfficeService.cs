using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;
using System.Web.Configuration;

namespace CodeFactory.Web.PostOffice
{
    public sealed class PostOfficeService
    {
        private static object syncRoot = new Object();

        private static PostOfficeProvider _defaultProvider;
        private static PostOfficeProviderCollection _providers;

        static PostOfficeService()
        {
            LoadProviders();
        }

        public static PostOfficeProvider Provider
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return _defaultProvider;
            }
        }

        public static PostOfficeProviderCollection Providers
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return _providers;
            }
        }
        
        private static void LoadProviders()
        {
            if (_defaultProvider == null)
            {
                lock (syncRoot)
                {
                    if (_defaultProvider == null)
                    {
                        PostOfficeSettingsHandler postOffice = (PostOfficeSettingsHandler)
                            WebConfigurationManager.GetSection("postOfficeSettings");

                        if (postOffice == null)
                            throw new ConfigurationErrorsException("Post office settings sections is missed.");

                        _providers = new PostOfficeProviderCollection();

                        ProvidersHelper.InstantiateProviders(
                            postOffice.Providers, _providers, typeof(PostOfficeProvider));

                        _defaultProvider = _providers[postOffice.DefaultProvider];
                    }
                }
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static HtmlMailMessage Create(object holder, Stream template, Dictionary<string, string> parameters)
        {
            return _defaultProvider.Create(holder, template, parameters);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void SendMessage(HtmlMailMessage message)
        {
            _defaultProvider.SendMessage(message);
        }
    }
}
