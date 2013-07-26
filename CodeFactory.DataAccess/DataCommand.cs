using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using CodeFactory.DataAccess.Transactions;
using CodeFactory.Utilities;

namespace CodeFactory.DataAccess
{
	/// <summary>
	/// DataCommand wraps one (or more?) IDbCommands.
	/// </summary>
	class DataCommand : IDataCommand
	{
		private IDbCommand _dbCommand = null;
		private DataParameterCollection _parameters = null;
		private string _commandName = "";
		private DataSource _dataSource = null;

		public DataCommand(IDbCommand dbCommand, DataSource ds) 
		{
			_dbCommand = dbCommand;
			_dataSource = ds;
			_parameters = new DataParameterCollection(dbCommand, ds.Provider.ParameterDbType, 
				ds.Provider.ParameterDbTypeProperty, ds.ParameterNamePrefix);
		}

		public DataCommand(string commandName, IDbCommand dbCommand, DataSource ds)
			: this(dbCommand, ds)
		{
			_commandName = commandName;
		}

		internal DataCommand(string commandName, IDbCommand dbCommand, DataSource ds, 
			IDictionary parameterKeyNames,
			IList replaceByParams)
			: this (commandName, dbCommand, ds)
		{
			_parameters.ParameterKeyNames = parameterKeyNames;

			_replaceByParams = replaceByParams;
		}

		public string Name 
		{ 
			get { return _commandName; }
			set { _commandName = value; } 
		}
		public IDataSource DataSource { get { return _dataSource; } }


		public DataParameterCollection Parameters { get { return _parameters; } }

		public object Clone() 
		{
			IDbCommand dbCommand = null;
            
			if(_dbCommand is ICloneable)
			{
				dbCommand = (IDbCommand)((ICloneable)_dbCommand).Clone();
			}
			else
			{
                // Cloning manually
				dbCommand = (IDbCommand)Activator.CreateInstance(
					_dataSource.Provider.CommandObjectType);	
				dbCommand.CommandType = _dbCommand.CommandType;
				dbCommand.CommandText = _dbCommand.CommandText;
				dbCommand.CommandTimeout = _dbCommand.CommandTimeout;

                if (_dbCommand.Parameters.Count > 0)
                {
                    foreach (IDataParameter p in _dbCommand.Parameters)
                    {
                        IDataParameter param = dbCommand.CreateParameter();
                        param.DbType = p.DbType;
                        param.Direction = p.Direction;
                        param.ParameterName = p.ParameterName;
                        param.SourceColumn = p.SourceColumn;
                        param.SourceVersion = p.SourceVersion;
                        param.Value = p.Value;

                        dbCommand.Parameters.Add(param);
                    }
                }
			}

            return new DataCommand(
				_commandName, dbCommand, _dataSource, 
				_parameters.ParameterKeyNames, 
				_replaceByParams);
		}

		public int ExecuteNonQuery() 
		{
			int recordsAffected = 0;

			PrepareCommand();

			bool isLocallyManaged = this.AllocateDbContext();

			try
			{
				recordsAffected = _dbCommand.ExecuteNonQuery();
			}
			finally
			{
				DeallocateDbContext(isLocallyManaged);
			}

			return recordsAffected;
		}

		public int ExecuteNonQuery(IDbConnection con, IDbTransaction tran) 
		{
			PrepareCommand();

			_dbCommand.Connection = con;
			_dbCommand.Transaction = tran;

			return _dbCommand.ExecuteNonQuery();
		}

		public IDataReader ExecuteReader() 
		{
            return ExecuteReader(CommandBehavior.Default);
		}

        public IDataReader ExecuteReader(CommandBehavior commandBehavior)
        {
            PrepareCommand();

            bool isLocallyManaged = this.AllocateDbContext();

            //if (isLocallyManaged && commandBehavior == CommandBehavior.Default)
            if (isLocallyManaged)
                commandBehavior = CommandBehavior.CloseConnection;

            IDataReader dr = null;

            try
            {
                dr = _dbCommand.ExecuteReader(commandBehavior);
            }
            catch (Exception ex)	//the connection is automatically closed, but still catch & close does not hurt
            {
                DeallocateDbContext(isLocallyManaged);
                throw ex;
            }

            return dr;
        }

		public IDataReader ExecuteReader(IDbConnection con, IDbTransaction tran)
		{
			PrepareCommand();

			_dbCommand.Connection = con;
			_dbCommand.Transaction = tran;

			return _dbCommand.ExecuteReader();
		}

		public object ExecuteScalar() 
		{
			object tmp;

			PrepareCommand();

			bool isLocallyManaged = this.AllocateDbContext();

			try
			{
				tmp = _dbCommand.ExecuteScalar();
			}
			finally
			{
				DeallocateDbContext(isLocallyManaged);
			}

			return tmp;
		}

		public object ExecuteScalar(IDbConnection con, IDbTransaction tran)
		{
			PrepareCommand();

			_dbCommand.Connection = con;
			_dbCommand.Transaction = tran;

			return _dbCommand.ExecuteScalar();
		}

		private bool AllocateDbContext()
		{
			bool isLocallyManaged = false;

			IDbConnection con = _dataSource.CreateConnection();
			IDbTransaction tran = null;

			ITransactionHandler th = TransactionContextFactory.GetHandler();
			if(th != null)
			{
				tran = th.GetTransaction(_dataSource.Name, con);	
			}

			if(tran != null)
			{
				con = tran.Connection;
				isLocallyManaged = false;
			}
			else if(con.State != ConnectionState.Open) 
			{
				con.Open();
				isLocallyManaged = true;
			}

			_dbCommand.Connection = con;
			_dbCommand.Transaction = tran;
			
			return isLocallyManaged;
		}

		private void DeallocateDbContext(bool isLocallyManaged)
		{
			if(isLocallyManaged)
			{
				_dbCommand.Connection.Close();
			}
		}

		public IDbCommand DbCommand 
		{
            [DebuggerStepThrough]
			get { return _dbCommand; }
			//set { _dbCommand = value; }
		}

		private IList _replaceByParams = new ArrayList();

		private class ReplaceByParam
		{
			public string ParamName;
			public string ParamValue;
			public string OldString;
			public string NewString;
			public string DefaultString;

			public ReplaceByParam(string paramName, string paramValue, string oldString, string newString, string defaultString)
			{
				this.ParamName = paramName;
				this.ParamValue = paramValue;
				this.OldString = oldString;
				this.NewString = newString;
				this.DefaultString = defaultString;
			}
		}

		public void AddReplaceByParamValue(string paramName, string paramValue, 
			string oldString, string newString, string defaultString)
		{
			_replaceByParams.Add(new ReplaceByParam(paramName, paramValue, 
				oldString, newString, defaultString));
		}

		private void PrepareCommand()
		{
			//changes the commandText if necessary etc...

			if(_replaceByParams.Count > 0)
			{	
				StringBuilder sb = new StringBuilder(_dbCommand.CommandText);
				IDbDataParameter param;
				
				foreach(ReplaceByParam rbp in _replaceByParams)
				{
					param = _parameters[rbp.ParamName];
					if(param == null)
						throw new DataAccessException(ResourceStringLoader.GetResourceString(
                            "invalid_replacebyparam_parameter", rbp.ParamName));

					if(param.Value != null 
							&& (rbp.ParamValue.ToLower(CultureInfo.InvariantCulture) != "dbnull.value" &&
                            param.Value.ToString().Equals(rbp.ParamValue) ||
                            (rbp.ParamValue == "DBNull.Value" && param.Value == DBNull.Value)))

						 sb.Replace(rbp.OldString, rbp.NewString);
					else if(rbp.DefaultString != null)
						sb.Replace(rbp.OldString, rbp.DefaultString);
				}

				_dbCommand.CommandText = sb.ToString();
			}
		}

	}
}
