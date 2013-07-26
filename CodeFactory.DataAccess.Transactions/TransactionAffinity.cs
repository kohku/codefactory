using System;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for TransactionAffinity.
	/// </summary>
	public enum TransactionAffinity
	{
		//creates new transaction
		RequiresNew,
		//creates new transaction if no current transaction
		Required,
		//uses current transaction if present
		Supported,
		//does not use a transaction
		NotSupported
	}
}
