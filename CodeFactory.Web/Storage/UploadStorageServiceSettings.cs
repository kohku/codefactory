using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CodeFactory.Web.Storage
{
    /// <summary>
    /// A configuration section for web.config.
    /// </summary>
    /// <remarks>
    /// In the config section you can specify the provider you 
    /// want to use for gallery management.
    /// </remarks>
    public class UploadStorageServiceSettings : ConfigurationSection
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
        [ConfigurationProperty("defaultProvider", DefaultValue = "DefaultUploadStorageProvider")]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }

        [ConfigurationProperty("fileHandlerUrl", IsRequired = false, DefaultValue = "file.axd")]
        public string FileHandlerUrl
        {
            get
            {
                return (string)base["fileHandlerUrl"];
            }
            set
            {
                base["fileHandlerUrl"] = value;
            }
        }

        [ConfigurationProperty("imageHandlerUrl", IsRequired = false, DefaultValue = "image.axd")]
        public string ImageHandlerUrl
        {
            get
            {
                return (string)base["imageHandlerUrl"];
            }
            set
            {
                base["imageHandlerUrl"] = value;
            }
        }

        [ConfigurationProperty("thumbnail", IsRequired = true)]
        public ThumbnailElement Thumbnail
        {
            get
            {
                return (ThumbnailElement)base["thumbnail"];
            }
        }
    }

    public class ThumbnailElement : ConfigurationElement
    {
        [ConfigurationProperty("quality", DefaultValue = true)]
        public bool Quality
        {
            get
            {
                return (bool)base["quality"];
            }
            set
            {
                base["quality"] = value;
            }
        }

        [ConfigurationProperty("width", DefaultValue = 50)]
        public int Width
        {
            get
            {
                return (int)base["width"];
            }
            set
            {
                base["width"] = value;
            }
        }

        [ConfigurationProperty("height", DefaultValue = 100)]
        public int Height
        {
            get
            {
                return (int)base["height"];
            }
            set
            {
                base["height"] = value;
            }
        }

        [ConfigurationProperty("noThumbnailAvailable", DefaultValue = "noThumbnailAvailable.jpg")]
        public string NotThumbnailAvailable
        {
            get
            {
                return (string)base["noThumbnailAvailable"];
            }
            set
            {
                base["noThumbnailAvailable"] = value;
            }
        }
    }
}
