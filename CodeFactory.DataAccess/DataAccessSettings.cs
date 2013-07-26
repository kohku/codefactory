using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Collections.Specialized;
using CodeFactory.Utilities;

namespace CodeFactory.DataAccess
{
    public class dataAccessSettings : ConfigurationSection
    {
        /// <summary>
        /// A collection of registered data providers.
        /// </summary>
        [ConfigurationProperty("dataProviders", IsRequired = true)]
        public dataProviders dataProviders
        {
            get { return (dataProviders)base["dataProviders"]; }
        }

        /// <summary>
        /// A collection of registered data providers.
        /// </summary>
        [ConfigurationProperty("dataSources", IsRequired = true)]
        public dataSources dataSources
        {
            get { return (dataSources)base["dataSources"]; }
        }
    }

    public class dataSources : ConfigurationElementCollection
    {
        public dataSources()
        {
        }

        /// <summary>
        /// The name of the default data provider
        /// </summary>
        [ConfigurationProperty("defaultDataSource", DefaultValue = "default")]
        public string defaultDataSource
        {
            get { return (string)base["defaultDataSource"]; }
            set { base["defaultDataSource"] = value; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new dataSource();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((dataSource)element).name;
        }
    }

    public class dataSource : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("provider", IsRequired = true)]
        public string provider
        {
            get { return (string)base["provider"]; }
            set { base["provider"] = value; }
        }

        [ConfigurationProperty("connectionStringName", IsRequired = true)]
        public string connectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }

        [ConfigurationProperty("dataOperationsPath", IsRequired = true)]
        public string dataOperationsPath
        {
            get { return (string)base["dataOperationsPath"]; }
            set { base["dataOperationsPath"] = value; }
        }

        [ConfigurationProperty("commandTimeout", IsRequired = false, DefaultValue = 60)]
        public int commandTimeout
        {
            get { return (int)base["commandTimeout"]; }
            set { base["commandTimeout"] = value; }
        }
    }

    public class dataProviders : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new dataProvider();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((dataProvider)element).name;
        }
    }

    public class dataProvider : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("connectionType", IsRequired = true)]
        public string connectionType
        {
            get { return (string)base["connectionType"]; }
            set { base["connectionType"] = value; }
        }

        [ConfigurationProperty("commandType", IsRequired = true)]
        public string commandType
        {
            get { return (string)base["commandType"]; }
            set { base["commandType"] = value; }
        }

        [ConfigurationProperty("parameterType", IsRequired = true)]
        public string parameterType
        {
            get { return (string)base["parameterType"]; }
            set { base["parameterType"] = value; }
        }

        [ConfigurationProperty("parameterDbType", IsRequired = true)]
        public string parameterDbType
        {
            get { return (string)base["parameterDbType"]; }
            set { base["parameterDbType"] = value; }
        }

        [ConfigurationProperty("parameterDbTypeProperty", IsRequired = true)]
        public string parameterDbTypeProperty
        {
            get { return (string)base["parameterDbTypeProperty"]; }
            set { base["parameterDbTypeProperty"] = value; }
        }

        [ConfigurationProperty("dataAdapterType", IsRequired = true)]
        public string dataAdapterType
        {
            get { return (string)base["dataAdapterType"]; }
            set { base["dataAdapterType"] = value; }
        }

        [ConfigurationProperty("commandBuilderType", IsRequired = true)]
        public string commandBuilderType
        {
            get { return (string)base["commandBuilderType"]; }
            set { base["commandBuilderType"] = value; }
        }

        [ConfigurationProperty("parameterNamePrefix", IsRequired = true)]
        public string parameterNamePrefix
        {
            get { return (string)base["parameterNamePrefix"]; }
            set { base["parameterNamePrefix"] = value; }
        }

    }
}
