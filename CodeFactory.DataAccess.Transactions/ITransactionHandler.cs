using System;
using System.Data;

namespace CodeFactory.DataAccess.Transactions
{
	/// <summary>
	/// Summary description for ITransactionHandler.
	/// </summary>
	public interface ITransactionHandler
	{
		void HandleTCCreated(object sender, TCCreatedEventArgs args);
		void HandleTCStateChangedEvent(object sender, TCStateChangedEventArgs args);

		IDbTransaction GetTransaction(string dataSourceName, IDbConnection con);
	}
}
