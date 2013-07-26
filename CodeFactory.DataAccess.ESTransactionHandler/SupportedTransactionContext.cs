using System; 
using System.Data;
using System.EnterpriseServices; 

namespace CodeFactory.DataAccess.TransactionHandling.ESTransactionHandler
{
	/// <summary>
	/// Summary description for SupportedTransactionContext.
	/// </summary>
	[Transaction(TransactionOption.Supported)]
	public class SupportedTransactionContext : ESTransactionContext
	{

	}
}
