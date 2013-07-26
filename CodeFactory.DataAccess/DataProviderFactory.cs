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
	/// DataProviderFactory returns DataProvider objects.
	/// Maintains/refreshes internal cache of DataProviders.
	/// Config info retrieved from CodeFactory.DataAccess.dll.config
	/// </summary>
	internal class DataProviderFactory
	{
		private static Hashtable _dataProviders;

		static DataProviderFactory()
		{
			LoadCache();
		}

		private static void LoadCache()
		{
			try
			{
                dataAccessSettings settings = (dataAccessSettings)
                    ConfigurationManager.GetSection("dataAccess/dataAccessSettings");

				_dataProviders = new Hashtable();

				foreach(dataProvider dp in settings.dataProviders)
				{
					Type connectionType = Type.GetType(dp.connectionType);
					if(connectionType == null)
						throw new DataAccessException(ResourceStringLoader.GetResourceString(
                            "could_not_load_connectiontype", dp.connectionType, dp.name));

					Type commandType = Type.GetType(dp.commandType);
					if(commandType == null)
                        throw new DataAccessException(ResourceStringLoader.GetResourceString(
                            "could_not_load_commandtype", dp.connectionType, dp.name));

					Type parameterType = Type.GetType(dp.parameterType);
					if(parameterType == null)
                        throw new DataAccessException(ResourceStringLoader.GetResourceString(
                            "could_not_load_parametertype", dp.connectionType, dp.name));

					PropertyInfo parameterDbTypeProperty = 
						parameterType.GetProperty(
						dp.parameterDbTypeProperty, 
						BindingFlags.Instance | BindingFlags.Public);
					if(parameterDbTypeProperty == null)
                        throw new DataAccessException(ResourceStringLoader.GetResourceString(
                            "could_not_load_parameterdbtypeproperty", dp.connectionType, dp.name));

					Type parameterDbType = Type.GetType(dp.parameterDbType);
					if(parameterDbType == null)
                        throw new DataAccessException(ResourceStringLoader.GetResourceString(
                            "could_not_load_parameterdbtype", dp.connectionType, dp.name));

					Type dataAdapterType = null;
					if(dp.dataAdapterType != null && dp.dataAdapterType != string.Empty) 
					{
						dataAdapterType = Type.GetType(dp.dataAdapterType);
						if(dataAdapterType == null)
                            throw new DataAccessException(ResourceStringLoader.GetResourceString(
                            "could_not_load_dataadaptertype", dp.connectionType, dp.name));
					}

					Type commandBuilderType = null;
					if(dp.commandBuilderType != null && dp.commandBuilderType != string.Empty) 
					{
						commandBuilderType = Type.GetType(dp.commandBuilderType);
						if(commandBuilderType == null)
							throw new DataAccessException(ResourceStringLoader.GetResourceString(
                            "could_not_load_commandbuildertype", dp.connectionType, dp.name));
					}
								
					if(dp.parameterNamePrefix == null)
						dp.parameterNamePrefix = string.Empty;

					_dataProviders.Add(dp.name, 
						new DataProvider(
						dp.name, connectionType, commandType, 
						parameterType, parameterDbType, parameterDbTypeProperty,
						dataAdapterType, commandBuilderType, dp.parameterNamePrefix));
				}
			}
			catch(Exception e)
			{
                throw new DataAccessException(ResourceStringLoader.GetResourceString(
                    "error_loading_cache_from_config", e.Message));
			}
		}

		//only static methods
		private DataProviderFactory() {}

		internal static DataProvider GetDataProvider(string dataProviderName)
		{
			DataProvider dp = _dataProviders[dataProviderName] as DataProvider;

			if(dp == null) 
			{
				//TO DO: specialize the exception, perhaps DataSourceNotFoundException
				throw new DataAccessException(ResourceStringLoader.GetResourceString(
                    "datasource_not_found", dataProviderName));
			}

			return dp;
		}
	}
}
