using System;
using System.Collections;
using System.Data;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for RequiredTransactionContext.
	/// </summary>
	internal class RequiredTransactionContext : TransactionContext
	{
		public override TransactionAffinity Affinity 
		{ 
			get { return TransactionAffinity.Required; }
		}

		public override TransactionContext GetControllingContext() 
		{
			TransactionContext controllingContext = 
				((parentContext != null) ? parentContext.GetControllingContext() : null);
			return ((controllingContext != null) ? controllingContext : this);
		}


	}
}
