using System;
using System.Collections;
using System.Data;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for SupportedTransactionContext.
	/// </summary>
	internal class SupportedTransactionContext : TransactionContext
	{
		public override TransactionAffinity Affinity 
		{ 
			get { return TransactionAffinity.Supported; }
		}

		public override TransactionContext GetControllingContext() 
		{
			return ((parentContext != null) ? parentContext.GetControllingContext() : null);
		}
	}
}

