using System;
using System.Collections;
using System.Data;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for RequiresNewTransactionContext.
	/// </summary>
	internal class RequiresNewTransactionContext : TransactionContext
	{
		public override TransactionAffinity Affinity 
		{ 
			get { return TransactionAffinity.RequiresNew; }
		}

		public override TransactionContext GetControllingContext() 
		{
			return this;
		}

	}
}
