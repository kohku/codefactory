using System;
using System.Data;
using System.Data.Common;

namespace CodeFactory.DataAccess
{
	/// <summary>
	/// Summary description for IDataSetAdapter.
	/// </summary>
	public interface IDataSetAdapter : ICloneable
	{
		string Name { get; set; }
		IDataSource DataSource { get; }

		int Fill(DataSet ds);
		int Update(DataSet ds);

		IDataCommand SelectCommand { get; set; }
		IDataCommand UpdateCommand { get; set; }
		IDataCommand InsertCommand { get; set; }
		IDataCommand DeleteCommand { get; set; }

		void PopulateCommands();

		DataTableMappingCollection TableMappings { get; }
	}
}
