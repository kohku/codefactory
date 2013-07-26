using System; 
using System.Data;
using System.EnterpriseServices; 

namespace CodeFactory.DataAccess.TransactionHandling.ESTransactionHandler
{
	/// <summary>
	/// Summary description for RequiredTransactionContext.
	/// </summary>
	[Transaction(TransactionOption.Required)]
	public class RequiredTransactionContext : ESTransactionContext	
	{

	}	
}
