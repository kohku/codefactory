using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Collections;
using System.Configuration;
using CodeFactory.Utilities;

namespace CodeFactory.DataAccess
{
	/// <summary>
	/// DataSourceFactory returns DataSource objects.
	/// Maintains/refreshes internal cache of DataSources.
	/// Config info retrieved from config application file.
	/// </summary>
	public class DataSourceFactory
	{
        private static object syncRoot = new Object();
        
        private const string DEFAULT_DATASOURCE_NAME = "DEFAULT_DATASOURCE_NAME";

        private static Hashtable _dataSources;
		private static IDataSource _defaultDataSource;

		static DataSourceFactory()
		{
            if (_dataSources == null)
            {
                lock (syncRoot)
                {
                    if (_dataSources == null)
                        LoadCache();
                }
            }
		}

		private static void LoadCache()
		{
			try
			{
				dataAccessSettings settings = 
					(dataAccessSettings)ConfigurationManager.GetSection("dataAccess/dataAccessSettings");

                Hashtable dataSources = new Hashtable();

				foreach(dataSource ds in settings.dataSources)
				{
					DataProvider provider = 
						DataProviderFactory.GetDataProvider(ds.provider);

					DataSource dataSource = 
						new DataSource(ds.name, provider, ds.connectionStringName, 
						ds.dataOperationsPath, provider.ParameterNamePrefix,
						ds.commandTimeout);
                    dataSources.Add(dataSource.Name, dataSource);

                    if (!string.IsNullOrEmpty(settings.dataSources.defaultDataSource) &&
                        settings.dataSources.defaultDataSource.Equals(dataSource.Name))
                        _defaultDataSource = dataSource;
				}

                // The cache loading process is complete.
                _dataSources = dataSources;
			}
			catch(Exception e)
			{
				throw new DataAccessException(ResourceStringLoader.GetResourceString(
                    "error_loading_cache_from_config", e));
			}
		}

		//only static methods
		private DataSourceFactory() {}

		public static IDataSource GetDataSource()
		{
			return GetDataSource(DEFAULT_DATASOURCE_NAME);
		}

		public static IDataSource GetDataSource(string dataSourceName)
		{
			IDataSource ds = null;

			if(dataSourceName.Equals(DEFAULT_DATASOURCE_NAME)) 
				ds = _defaultDataSource;
			else
				ds = _dataSources[dataSourceName] as DataSource;

			if(ds == null) 
			{
				//TO DO: specialize the exception, perhaps DataSourceNotFoundException
				throw new DataAccessException(ResourceStringLoader.GetResourceString(
                    "datasource_not_found", dataSourceName));
			}

			return ds;
		}

	}
}
