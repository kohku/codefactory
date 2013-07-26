using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Diagnostics;
using CodeFactory.DataAccess.Transactions;
using CodeFactory.Utilities;

namespace CodeFactory.DataAccess
{
	/// <summary>
	/// Summary description for DataSetAdapter.
	/// </summary>
	class DataSetAdapter : IDataSetAdapter
	{
		private DataSource _dataSource;
		private string _name;

		private IDbDataAdapter _dbDataAdapter;
		private object _commandBuilder;

		private IDataCommand _selectCommand;
		private IDataCommand _updateCommand;
		private IDataCommand _insertCommand;
		private IDataCommand _deleteCommand;

		public DataSetAdapter(string adapterName, DataSource dataSource)
		{
			_name = adapterName;
			_dataSource = dataSource;
		}

		public DataSetAdapter(IDbDataAdapter dbDataAdapter, DataSource dataSource) 
		{
			_dbDataAdapter = dbDataAdapter;
			_dataSource = dataSource;
		}

		public DataSetAdapter(string adapterName, IDbDataAdapter dbDataAdapter, DataSource dataSource) 
			: this(adapterName, dataSource)
		{
			_dbDataAdapter = dbDataAdapter;
		}

		public DataSetAdapter(
			string adapterName, 
			IDbDataAdapter dbDataAdapter,
			DataSource dataSource,
			IDataCommand selectCommand,
			IDataCommand updateCommand,
			IDataCommand insertCommand,
			IDataCommand deleteCommand) : this(adapterName, dataSource)
		{
			_dbDataAdapter = dbDataAdapter;
			_selectCommand = selectCommand;
			_updateCommand = updateCommand;
			_insertCommand = insertCommand;
			_deleteCommand = deleteCommand;
		}

		public object Clone() 
		{
			IDbDataAdapter dbDataAdapter = null;
			if(_dbDataAdapter is ICloneable)
			{
				dbDataAdapter = (IDbDataAdapter)((ICloneable)_dbDataAdapter).Clone();
			}
			else
			{
				//TO DO: remove duplicate code (the same is also in DataSource)
				dbDataAdapter = (IDbDataAdapter)Activator.CreateInstance(
					_dataSource.Provider.DataAdapterObjectType);
			}

			DataSetAdapter newDataSetAdapter = new DataSetAdapter(
				_name, dbDataAdapter, _dataSource);

			newDataSetAdapter.SelectCommand = (IDataCommand)_selectCommand.Clone();

			if(_updateCommand != null)
				newDataSetAdapter.UpdateCommand = (IDataCommand)_updateCommand.Clone();
			if(_insertCommand != null)
				newDataSetAdapter.InsertCommand = (IDataCommand)_insertCommand.Clone();
			if(_deleteCommand != null)
				newDataSetAdapter.DeleteCommand = (IDataCommand)_deleteCommand.Clone();

			return newDataSetAdapter;
		}

		public string Name { get { return _name; } set { _name = value; } }
		public IDataSource DataSource { get { return _dataSource; } }

		public int Fill(DataSet ds) 
		{
			int recordsAffected = 0;

			if(_selectCommand == null)
				throw new DataException(ResourceStringLoader.GetResourceString("invalid_dataset_adapter_fill"));

            _dbDataAdapter.SelectCommand = _selectCommand.DbCommand;

			IDbConnection con = _dataSource.CreateConnection();
			IDbTransaction tran = null;
			ITransactionHandler th = TransactionContextFactory.GetHandler();
			if(th != null)
				tran = th.GetTransaction(_dataSource.Name, con);

			if(tran != null)
			{
				con = tran.Connection;
			}
			else if(con.State != ConnectionState.Open) 
			{
				con.Open();
			}

			_dbDataAdapter.SelectCommand.Connection = con;
			_dbDataAdapter.SelectCommand.Transaction = tran;

			try
			{
				recordsAffected = _dbDataAdapter.Fill(ds);
			}
			finally
			{
				if(tran == null) 
				{
					con.Close();
				}
			}

			return recordsAffected;
		}

		public int Update(DataSet ds) 
		{
			int recordsAffected = 0;

			if(_updateCommand == null &&
				_insertCommand == null &&
				_deleteCommand == null)
				throw new DataException(ResourceStringLoader.GetResourceString("invalid_dataset_adapter_update"));

			if(_updateCommand != null)
				_dbDataAdapter.UpdateCommand = _updateCommand.DbCommand;
			if(_insertCommand != null)
				_dbDataAdapter.InsertCommand = _insertCommand.DbCommand;
			if(_deleteCommand != null)
				_dbDataAdapter.DeleteCommand = _deleteCommand.DbCommand;

			IDbConnection con = _dataSource.CreateConnection();
			IDbTransaction tran = null;
			ITransactionHandler th = TransactionContextFactory.GetHandler();
			if(th != null)
				tran = th.GetTransaction(_dataSource.Name, con);

			if(tran != null)
			{
				con = tran.Connection;
			}
			else if(con.State != ConnectionState.Open) 
			{
				con.Open();
			}

			if(_dbDataAdapter.UpdateCommand != null) 
			{
				_dbDataAdapter.UpdateCommand.Connection = con;
				_dbDataAdapter.UpdateCommand.Transaction = tran;
			}
			if(_dbDataAdapter.InsertCommand != null) 
			{
				_dbDataAdapter.InsertCommand.Connection = con;
				_dbDataAdapter.InsertCommand.Transaction = tran;
			}
			if(_dbDataAdapter.DeleteCommand != null) 
			{
				_dbDataAdapter.DeleteCommand.Connection = con;
				_dbDataAdapter.DeleteCommand.Transaction = tran;
			}

			try
			{
				recordsAffected = _dbDataAdapter.Update(ds);
			}
			finally
			{
				if(tran == null) 
				{
					con.Close();
				}
			}

			return recordsAffected;
		}

		public IDataCommand SelectCommand 
		{ 
			get { return _selectCommand; }
 			set { _selectCommand = value; } 
		}
		public IDataCommand UpdateCommand
		{ 
			get { return _updateCommand; }
			set { _updateCommand = value; } 
		}
		public IDataCommand InsertCommand
		{ 
			get { return _insertCommand; }
			set { _insertCommand = value; } 
		}
		public IDataCommand DeleteCommand
		{ 
			get { return _deleteCommand; }
			set { _deleteCommand = value; } 
		}

		public DataTableMappingCollection TableMappings 
		{
			get { return (DataTableMappingCollection)_dbDataAdapter.TableMappings; }
		}

		public void PopulateCommands() 
		{
			//TO DO: check preconditions and raise exception if not fulfilled
			if(_insertCommand != null && _deleteCommand != null && _updateCommand != null)
			{
				Debug.WriteLine("Attempt to PopulateCommands but all commands already defined.");
				return;
			}

			if(_commandBuilder == null) 
			{
				_commandBuilder = 
					Activator.CreateInstance(_dataSource.Provider.CommandBuilderObjectType);
			}

			//here assign the selectcommand
			_dbDataAdapter.SelectCommand = _selectCommand.DbCommand;
			//and open a connection, because the CommandBuilders retrieves metadata from the db!
			IDbConnection con = _dataSource.CreateConnection();
			con.Open();
			_dbDataAdapter.SelectCommand.Connection = con;

			//TO DO fix this exception handling hack
			try 
			{
                //PropertyInfo selectCommandProperty = _commandBuilder.GetType().GetProperty(
                //    "DataAdapter", _dbDataAdapter.GetType());
                PropertyInfo selectCommandProperty = _commandBuilder.GetType().GetProperty(
                    "DataAdapter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                selectCommandProperty.SetValue(_commandBuilder, _dbDataAdapter, null);

				MethodInfo getCommandMethod = null;
				IDbCommand dbCommand = null;

				//after that get the other commands, wrap them and assign them to the current datasetadapter
				if(_insertCommand == null)
				{
                    getCommandMethod = _commandBuilder.GetType().GetMethod("GetInsertCommand",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly,
                        Type.DefaultBinder, new Type[0], null);
					dbCommand = (IDbCommand)getCommandMethod.Invoke(_commandBuilder, null);
					_insertCommand = new DataCommand(dbCommand, _dataSource);
				}
				if(_deleteCommand == null)
				{
                    getCommandMethod = _commandBuilder.GetType().GetMethod("GetDeleteCommand",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly,
                        Type.DefaultBinder, new Type[0], null);
					dbCommand = (IDbCommand)getCommandMethod.Invoke(_commandBuilder, null);
					_deleteCommand = new DataCommand(dbCommand, _dataSource);
				}
				if(_updateCommand == null)
				{
                    getCommandMethod = _commandBuilder.GetType().GetMethod("GetUpdateCommand",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly,
                        Type.DefaultBinder, new Type[0], null);
					dbCommand = (IDbCommand)getCommandMethod.Invoke(_commandBuilder, null);
					_updateCommand = new DataCommand(dbCommand, _dataSource);
				}
			}
			catch(Exception e) 
			{
				Debug.WriteLine(e.ToString());
				throw;
			}
			finally 
			{
				con.Close();
				_dbDataAdapter.SelectCommand.Connection = null;
				_dbDataAdapter.SelectCommand = null;
			}

		}
	}
}
