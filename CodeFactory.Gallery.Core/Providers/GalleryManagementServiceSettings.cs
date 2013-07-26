using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CodeFactory.Gallery.Core.Providers
{
    public class GalleryManagementServiceSettings : ConfigurationSection
    {
        [ConfigurationProperty("galleries", IsRequired = true)]
        public GalleriesElement Galleries
        {
            get
            {
                return (GalleriesElement)base["galleries"];
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

    public class GalleriesElement : ConfigurationElement
    {
        [ConfigurationProperty("pageUrl", IsRequired = true)]
        public string PageUrl
        {
            get
            {
                return (string)base["pageUrl"];
            }
            set
            {
                base["pageUrl"] = value;
            }
        }

        [ConfigurationProperty("notificationTemplate", IsRequired = false)]
        public string NotificationTemplate
        {
            get
            { 
                return (string)base["notificationTemplate"]; 
            }
            set 
            {
                base["notificationTemplate"] = value; 
            }
        }
    }

    public class ThumbnailElement : ConfigurationElement
    {
        [ConfigurationProperty("quality", IsRequired = true)]
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

        [ConfigurationProperty("width", IsRequired = true)]
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

        [ConfigurationProperty("heigth", IsRequired = true)]
        public int Heigth
        {
            get
            {
                return (int)base["heigth"];
            }
            set
            {
                base["heigth"] = value;
            }
        }

        [ConfigurationProperty("fileUrl", IsRequired = true)]
        public string FileUrl
        {
            get
            {
                return (string)base["fileUrl"];
            }
            set
            {
                base["fileIconUrl"] = value;
            }
        }

        [ConfigurationProperty("imageUrl", IsRequired = true)]
        public string ImageUrl
        {
            get
            {
                return (string)base["imageUrl"];
            }
            set
            {
                base["imageUrl"] = value;
            }
        }

        [ConfigurationProperty("notThumbnailAvailable", IsRequired = true)]
        public string NotThumbnailAvailable
        {
            get
            {
                return (string)base["notThumbnailAvailable"];
            }
            set
            {
                base["notThumbnailAvailable"] = value;
            }
        }

        [ConfigurationProperty("imageHandlerUrl", IsRequired = true)]
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

        [ConfigurationProperty("fileHandlerUrl", IsRequired = true)]
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
    }
}
