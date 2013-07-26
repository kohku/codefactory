using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Configuration;
using CodeFactory.Utilities;

namespace CodeFactory.DataAccess
{
	/// <summary>
	/// DataSource is a facade class and represents a certain database.
	/// Produces connections and commands.
	/// Uses DataCommandFactory internally.
	/// </summary>
	class DataSource : IDataSource
	{
		private string _name;
		private DataProvider _provider;
		private string _connectionStringName;
		private DataOperationFactory _operationFactory;
		private IDbConnection _templateConnection;
		private IDbCommand _templateCommand;
		private IDbDataAdapter _templateDataAdapter;
		private string _parameterNamePrefix;

		public DataSource(
			string name, DataProvider provider, string connectionStringName, 
			string dataOperationsPath, 
			string parameterNamePrefix,
			int commandTimeout)
		{
			_name = name;
			_provider = provider;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (settings == null)
                throw new DataAccessException(ResourceStringLoader.GetResourceString(
                    "connection_string_not_found", connectionStringName));

            _connectionStringName = connectionStringName;

			_templateConnection = (IDbConnection)Activator.CreateInstance(
				_provider.ConnectionObjectType);
			_templateCommand = (IDbCommand)Activator.CreateInstance(
				_provider.CommandObjectType);
			if(_provider.DataAdapterObjectType != null)
				_templateDataAdapter = (IDbDataAdapter)Activator.CreateInstance(
					_provider.DataAdapterObjectType);

			_parameterNamePrefix = provider.ParameterNamePrefix;
			_commandTimeout = commandTimeout;

			_operationFactory = new DataOperationFactory(this, dataOperationsPath);
		}

		public string Name { get { return _name; } }

		public DataProvider Provider { get { return _provider; } }
		public string ParameterNamePrefix { get { return _parameterNamePrefix; } }

		private int _commandTimeout = -1;

		public int CommandTimeout { get { return _commandTimeout; } }

		public IDbConnection CreateConnection() 
		{
			IDbConnection dbCon = null;

            if (_templateConnection is ICloneable)
                dbCon = (IDbConnection)((ICloneable)_templateConnection).Clone();
            else
                dbCon = (IDbConnection)Activator.CreateInstance(
                    _provider.ConnectionObjectType);

            dbCon.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;

			return dbCon;
		}

		public IDataCommand GetCommand(string commandName) 
		{
			return _operationFactory.GetCommand(commandName);
		}

		public IDataCommand CreateCommand() 
		{
			IDbCommand dbCmd = null;
			if(_templateCommand is ICloneable)
			{
				dbCmd = (IDbCommand)((ICloneable)_templateCommand).Clone();	
			}
			else
			{
				dbCmd = (IDbCommand)Activator.CreateInstance(_provider.CommandObjectType);	
			}
			
			IDataCommand cmd = new DataCommand(dbCmd, this);

			return cmd;
		}

		public IDataCommand CreateCommand(string commandText, CommandType commandType) 
		{
			//TO DO: decide whether to delegate or do it here
			IDataCommand cmd = CreateCommand();

			cmd.DbCommand.CommandText = commandText;
			cmd.DbCommand.CommandType = commandType;

			return cmd;
		}

		public IDataSetAdapter GetDataSetAdapter(string adapterName) 
		{
			return _operationFactory.GetAdapter(adapterName);
		}
		public IDataSetAdapter CreateDataSetAdapter() 
		{
			if(_templateDataAdapter == null)
				throw new DataAccessException(ResourceStringLoader.GetResourceString(
                    "dataadapters_not_supported"));

            IDbDataAdapter dbDataAdapter = null;

			if(_templateDataAdapter is ICloneable)
				dbDataAdapter = (IDbDataAdapter)((ICloneable)_templateDataAdapter).Clone();
			else
				dbDataAdapter = (IDbDataAdapter)Activator.CreateInstance(
					_provider.DataAdapterObjectType);
				
			IDataSetAdapter dataSetAdapter = new DataSetAdapter(dbDataAdapter, this);

			return dataSetAdapter;
		}

		public IDataSetAdapter CreateDataSetAdapter(IDataCommand selectCommand) 
		{
			IDataSetAdapter dataSetAdapter = CreateDataSetAdapter();
			dataSetAdapter.SelectCommand = selectCommand;

			return dataSetAdapter;
		}

		public object CreateCommandBuilder()
		{
			return Activator.CreateInstance(_provider.CommandBuilderObjectType);
		}
	}
}
