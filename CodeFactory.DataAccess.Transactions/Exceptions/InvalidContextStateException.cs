using System;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for InvalidContextStateException.
	/// </summary>
	public class InvalidContextStateException : TransactionContextException
	{
		private TransactionContextState _currentState;
		private TransactionContextState[] _allowedStates;

		public InvalidContextStateException() : base() { }
		public InvalidContextStateException(string message) : base(message) { }
		public InvalidContextStateException(
			TransactionContextState currentState, TransactionContextState[] allowedStates, string message) 
			: base(message) 
		{ 
			_currentState = currentState;
			_allowedStates = allowedStates;
		}

		public TransactionContextState CurrentState 
		{
			get { return _currentState; }
		}
		public TransactionContextState[] AllowedStates
		{
			get { return _allowedStates; }
		}
	}
}
