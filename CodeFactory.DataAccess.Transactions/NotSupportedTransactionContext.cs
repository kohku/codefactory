using System;
using System.Collections;
using System.Data;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for NotSupportedTransactionContext.
	/// </summary>
	internal class NotSupportedTransactionContext : TransactionContext
	{
		public override TransactionAffinity Affinity 
		{ 
			get { return TransactionAffinity.NotSupported; }
		}

		public override TransactionContext GetControllingContext() 
		{
			return null;
		}
	}
}
