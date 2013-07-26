using System;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for TransactionContextException.
	/// </summary>
	[Serializable]
	public class TransactionContextException : TransactionHandlingException
	{
		public TransactionContextException() {}

		public TransactionContextException(string message) : base(message) {}

		public TransactionContextException(string message, Exception e) : base(message, e) {}
	}
}
