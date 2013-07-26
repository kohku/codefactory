using System;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for TransactionHandlingException.
	/// </summary>
	[Serializable]
	public class TransactionHandlingException : ApplicationException
	{
		public TransactionHandlingException() {}

		public TransactionHandlingException(string message) : base(message) {}

		public TransactionHandlingException(string message, Exception e) : base(message, e) {}
	}
}

