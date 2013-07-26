using System;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for TransactionContextState.
	/// </summary>
	public enum TransactionContextState
	{
		Created,
		Entered,
		Exitted,
		ToBeCommitted,
		ToBeRollbacked
	}
}
