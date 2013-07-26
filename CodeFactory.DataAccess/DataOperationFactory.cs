using System;
using System.Collections;
using System.Xml;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using CodeFactory.Utilities;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace CodeFactory.DataAccess
{
	/// <summary>
	/// Each DataSource has one DataOperationFactory and delegates work to it.
	/// Returns DataCommands/DataSetAdapters from the cache.
	/// Maintains/refreshes internal DataCommand/DataSetAdapters caches.
	/// Config info retrieved from dataOperationsDir/dataOperationsFileMask from the DataSource.
	/// </summary>
	class DataOperationFactory
	{
		private DataSource _dataSource;
		private DataProvider _provider;

		private string _dataOperationsPath;

		private Hashtable _commands;
		private Hashtable _adapters;

		public DataOperationFactory(
			DataSource dataSource, string dataOperationsPath)
		{
			_dataSource = dataSource;
			_provider = dataSource.Provider;

			_commands = new Hashtable();
			_adapters = new Hashtable();

			if(!string.IsNullOrEmpty(dataOperationsPath))
			{
				if(!dataOperationsPath.Substring(1,1).Equals(":") && !dataOperationsPath.StartsWith("\\"))
					_dataOperationsPath = AppDomain.CurrentDomain.BaseDirectory 
						+ Path.DirectorySeparatorChar + dataOperationsPath;
				else
					_dataOperationsPath = dataOperationsPath;

				LoadCache();
			}
		}

		private void LoadCache()
		{
			try
			{
                string directory = Path.GetDirectoryName(_dataOperationsPath);
                directory = Path.GetFullPath(directory);

                string filename = Path.GetFileName(_dataOperationsPath);

                string[] fileNames = Directory.GetFiles(directory, filename);

                if (fileNames.Length == 0)
                    throw new DataAccessException(ResourceStringLoader.GetResourceString(
                        "no_dataoperations_config_files_found.", _dataOperationsPath));

                foreach (string fileName in fileNames)
                {
                    // Create and load the XML document.
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);

                    // Create an XmlNodeReader using the XML document.
                    XmlNodeReader nodeReader = new XmlNodeReader(doc);

                    // Set the validation settings on the XmlReaderSettings object.
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ValidationType = ValidationType.Schema;

                    Stream schemaStream = Assembly.GetAssembly(typeof(dataOperations)).GetManifestResourceStream(
                        "CodeFactory.DataAccess.DataOperations.xsd");

                    try
                    {
                        XmlTextReader sr = new XmlTextReader(schemaStream);

                        settings.Schemas.Add("CodeFactory.DataAccess", sr);
                        settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(ValidationCallBack);

                        // Create a validating reader that wraps the XmlNodeReader object.
                        XmlReader reader = XmlReader.Create(nodeReader, settings);

                        try
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(dataOperations));

                            dataOperations dops = (dataOperations)serializer.Deserialize(reader);

                            if (!dops.dataSource.Equals(_dataSource.Name))
                                throw new DataAccessException(ResourceStringLoader.GetResourceString(
                                    "dataoperations_with_different_datasource", dops.dataSource, _dataSource.Name));

                            if (dops.dataCommands != null)
                            {
                                foreach (dataCommand dc in dops.dataCommands)
                                {
                                    IDataCommand currentDataCommand = CreateDataCommand(dc);
                                    _commands.Add(currentDataCommand.Name, currentDataCommand);
                                }
                            }
                            if (dops.dataSetAdapters != null)
                            {
                                foreach (dataSetAdapter dsa in dops.dataSetAdapters)
                                {
                                    IDataSetAdapter dataSetAdapter = CreateDataSetAdapter(dsa);
                                    _adapters.Add(dataSetAdapter.Name, dataSetAdapter);
                                }
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                    finally
                    {
                        schemaStream.Close();
                    }
                }
            }
			catch(Exception e)
			{
				throw new DataAccessException(ResourceStringLoader.GetResourceString(
                    "dataoperations_cache_loading_failed.", e.Message));
			}
		}

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            Trace.WriteLine(ResourceStringLoader.GetResourceString("dataoperations_validation_error", e.Message));
        }

        private IDataCommand CreateDataCommand(dataCommand dc)
		{
			IDbCommand currentDbCommand = 
				(IDbCommand)Activator.CreateInstance(_provider.CommandObjectType);

			DataCommand currentDataCommand = new DataCommand(dc.name, currentDbCommand, 
				_dataSource);

			CommandType commandType = 
				(CommandType)Enum.Parse(typeof(CommandType), dc.type , true);
			currentDbCommand.CommandType = commandType;

			currentDbCommand.CommandText = dc.commandText.Trim('\t','\r','\n');

			if(dc.replaceByParamValues != null)
			{
				string defaultString;
				foreach(replaceByParamValue rbp in dc.replaceByParamValues)
				{
					defaultString = rbp.defaultString;
					if(defaultString != null)
						defaultString = defaultString.Trim('\t','\r','\n');

					currentDataCommand.AddReplaceByParamValue(
						rbp.paramName, 
						rbp.paramValue,
						rbp.oldString.Trim('\t','\r','\n'),
						rbp.newString.Trim('\t','\r','\n'),
						defaultString);
				}
			}

			if(dc.parameters != null)
			{
				foreach(param p in dc.parameters)
				{
					if(p.key == null) p.key = string.Empty;

					System.Enum paramType = 
						(System.Enum)Enum.Parse(_provider.ParameterDbType, p.type, true);

					ParameterDirection paramDirection
						= (ParameterDirection)Enum.Parse(
						typeof(ParameterDirection), p.direction);
									
					DataRowVersion sourceVersion = DataRowVersion.Default;
					if(p.sourceVersion != null) 
						sourceVersion = (DataRowVersion)Enum.Parse(
							typeof(DataRowVersion), p.sourceVersion, true);

					currentDataCommand.Parameters.Add(p.key, p.name, 
						paramType, p.size, paramDirection, p.sourceColumn, 
						sourceVersion, p.isNullable, (byte)p.scale);
				}
			}
			else if(dc.populateParameters)
			{
				//TO DO: fix this hack
				IDataCommand oldCmd = currentDataCommand;
				currentDataCommand = this.DeriveCommand(
					currentDataCommand.DbCommand.CommandText);
				currentDataCommand.Name = oldCmd.Name;
			}

			//set timeout if specified at command/datasource level
			if(dc.timeout != -1)
				currentDataCommand.DbCommand.CommandTimeout = dc.timeout;
			else if(_dataSource.CommandTimeout != -1)
				currentDataCommand.DbCommand.CommandTimeout = 
					_dataSource.CommandTimeout;

			return currentDataCommand;
		}

		private IDataSetAdapter CreateDataSetAdapter(dataSetAdapter dsa)
		{
			IDbDataAdapter currentDbDataAdapter = 
				(IDbDataAdapter)Activator.CreateInstance(_provider.DataAdapterObjectType);
			IDataSetAdapter currentDataSetAdapter = 
				new DataSetAdapter(dsa.name, currentDbDataAdapter, _dataSource);

			currentDataSetAdapter.SelectCommand = CreateDataCommand(dsa.selectCommand);
			if(dsa.insertCommand != null)
				currentDataSetAdapter.InsertCommand = CreateDataCommand(dsa.insertCommand);
			if(dsa.updateCommand != null)
				currentDataSetAdapter.UpdateCommand = CreateDataCommand(dsa.updateCommand);
			if(dsa.deleteCommand != null)
				currentDataSetAdapter.DeleteCommand = CreateDataCommand(dsa.deleteCommand);

			if(dsa.tableMappings != null)
			{
				foreach(tableMapping tm in dsa.tableMappings)
				{
					DataTableMapping dtm = currentDataSetAdapter.TableMappings.Add(
						tm.sourceTable, tm.dataSetTable);

					if(tm.columnMappings != null)
					{
						foreach(columnMapping cm in tm.columnMappings)
							dtm.ColumnMappings.Add(cm.sourceColumn, cm.dataSetColumn);
					}
				}
			}

			if(dsa.populateCommands)
				currentDataSetAdapter.PopulateCommands();

			return currentDataSetAdapter;
		}

		public IDataCommand GetCommand(string commandName) 
		{
			DataCommand cmd = _commands[commandName] as DataCommand;
			if(cmd == null) 
			{
				cmd = this.DeriveCommand(commandName);

				if(cmd != null)
					_commands[commandName] = cmd;
				else
					throw new DataAccessException(ResourceStringLoader.GetResourceString(
                        "datacommand_not_found", commandName));
			}

			DataCommand newCmd = (DataCommand)cmd.Clone();

			return newCmd;
		}

		private DataCommand DeriveCommand(string commandName)
		{
			DataCommand dcmd = null;

			using (IDbConnection con = _dataSource.CreateConnection())
			{
				con.Open();

				IDbCommand cmd = con.CreateCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = commandName;

				cmd.Connection = con;

				object commandBuilder = _dataSource.CreateCommandBuilder();

				try
				{
					_dataSource.Provider.DeriveParametersMethod
						.Invoke(commandBuilder, new object[] { cmd });
				}
				catch(InvalidOperationException)
				{
					//log this
					return dcmd;
				}

                cmd.Connection = null;

				//fix for InputOutput parameters (DeriveParameters creates always InputOutput 
				//and never Output parameters)
				//all InputOutput -> Output parameters, therefore
				//in the sproc definition an OUTPUT param cannot be used also as an INPUT
				foreach(IDataParameter param in cmd.Parameters)
				{
					if (param.Direction == ParameterDirection.InputOutput)
						param.Direction = ParameterDirection.Output;
				}

				dcmd = new DataCommand(cmd, _dataSource);
				dcmd.Name = commandName;
			}

			return dcmd;
		}

		public IDataSetAdapter GetAdapter(string adapterName) 
		{
			DataSetAdapter adapter = _adapters[adapterName] as DataSetAdapter;

			if(adapter == null) 
			{
				throw new DataAccessException(ResourceStringLoader.GetResourceString(
                    "datasetadapter_not_found", adapterName));
			}

			DataSetAdapter newAdapter = (DataSetAdapter)adapter.Clone();

			return newAdapter;
		}
	}
}
