using System; 
using System.Data;
using System.EnterpriseServices; 

namespace CodeFactory.DataAccess.TransactionHandling.ESTransactionHandler
{
	/// <summary>
	/// Summary description for NotSupportedTransactionContext.
	/// </summary>
	[Transaction(TransactionOption.NotSupported)]
	public class NotSupportedTransactionContext : ESTransactionContext	
	{

	}
}
