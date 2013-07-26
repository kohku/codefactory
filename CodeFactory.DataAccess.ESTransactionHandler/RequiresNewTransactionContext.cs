using System; 
using System.Data;
using System.EnterpriseServices; 

namespace CodeFactory.DataAccess.TransactionHandling.ESTransactionHandler
{
	/// <summary>
	/// Summary description for RequiresNewTransactionContext.
	/// </summary>
	[Transaction(TransactionOption.RequiresNew)]
	public class RequiresNewTransactionContext : ESTransactionContext
	{

	}
}
