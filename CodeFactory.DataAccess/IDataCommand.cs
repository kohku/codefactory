using System;
using System.Data;
using System.Collections;

namespace CodeFactory.DataAccess
{
	/// <summary>
	/// Summary description for IDataCommand.
	/// </summary>
	public interface IDataCommand : ICloneable
	{
		string Name { get; set; }
		IDataSource DataSource { get; }

		IDbCommand DbCommand { get; }

		int ExecuteNonQuery();
		int ExecuteNonQuery(IDbConnection con, IDbTransaction tran);
		IDataReader ExecuteReader();
        IDataReader ExecuteReader(CommandBehavior commandBehavior);
		object ExecuteScalar();

		DataParameterCollection Parameters { get; }
	}
}



