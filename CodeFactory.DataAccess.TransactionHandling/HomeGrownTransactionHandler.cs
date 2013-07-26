using System;
using System.Data;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using CodeFactory.DataAccess.Transactions;

namespace CodeFactory.DataAccess.TransactionHandling
{
	/// <summary>
	/// HomeGrownTransactionHandler handles TransactionContext* events.
	/// Impementation using manual enlistment and control of data connections and transactions.
	/// Singlenton?
	/// </summary>
	public class HomeGrownTransactionHandler : ITransactionHandler
	{
		private const string THREAD_TRANSACTIONS_HASHTABLE_KEY = "TTHK";

		public HomeGrownTransactionHandler()
		{
			////TO DO: find a way to invoke the constructor at TransactionContextFactory init???
			//TransactionContextFactory.ContextCreated += 
			//	new TCCreatedEventHandler(this.HandleTCCreated);
		}

		private static Hashtable dataSourceTransactionsByTrCtx 
		{
			get 
			{
				Hashtable table = 
					CallContext.GetData(THREAD_TRANSACTIONS_HASHTABLE_KEY) as Hashtable;
				if(table == null) 
				{
					table = new Hashtable();
					CallContext.SetData(THREAD_TRANSACTIONS_HASHTABLE_KEY, table);
				}
				return table;
			}
		}

		#region ITransactionHandler Members

		public void HandleTCCreated(object sender, TCCreatedEventArgs args)
		{
			//here attach to the event handlers of the newly created transaction context
			//(not in GetTransaction)
			TransactionContext trCtx = args.Context;
			trCtx.StateChanged += new TCStateChangedEventHandler(HandleTCStateChangedEvent);
		}

		public void HandleTCStateChangedEvent(object sender, TCStateChangedEventArgs args)
		{
			TransactionContext trCtx = (TransactionContext)sender;

			//here check the event & commit/rollback etc
			switch(trCtx.State) 
			{
				case TransactionContextState.Entered:
					TransactionContext contrTrCtx = trCtx.GetControllingContext();
					if(contrTrCtx != null && !dataSourceTransactionsByTrCtx.Contains(contrTrCtx))
						dataSourceTransactionsByTrCtx.Add(contrTrCtx, new Hashtable());
					break;

				case TransactionContextState.ToBeCommitted:
					break;

				case TransactionContextState.ToBeRollbacked:
					break;

				case TransactionContextState.Exitted:
					if(dataSourceTransactionsByTrCtx.Contains(trCtx)) 
					{
						switch(args.FromState)
						{
							case TransactionContextState.ToBeCommitted:
								CommitTransactions(trCtx);
								break;
							case TransactionContextState.ToBeRollbacked:
								RollbackTransactions(trCtx);
								break;
						}
						dataSourceTransactionsByTrCtx.Remove(trCtx);
					}
					break;

				default:
					throw new Exception("Unexpected TransactionContextState:" + args.FromState.ToString());
			}		
		}

		private void CommitTransactions(TransactionContext trCtx) 
		{
			Hashtable transactionsByDataSourceToCommit = 
				dataSourceTransactionsByTrCtx[trCtx] as Hashtable;
			if(transactionsByDataSourceToCommit != null) 
			{
				foreach(DictionaryEntry entry in transactionsByDataSourceToCommit) 
				{
					DataSession ds = (DataSession)entry.Value;
					ds.Transaction.Commit();
					ds.Connection.Close();
				}
				transactionsByDataSourceToCommit.Clear();
			}
		}

		private void RollbackTransactions(TransactionContext trCtx) 
		{
			Hashtable transactionsByDataSourceToRollback = 
				dataSourceTransactionsByTrCtx[trCtx] as Hashtable;
			if(transactionsByDataSourceToRollback != null) 
			{
				foreach(DictionaryEntry entry in transactionsByDataSourceToRollback) 
				{
					DataSession ds = (DataSession)entry.Value;
					ds.Transaction.Rollback();
					ds.Connection.Close();
				}
				transactionsByDataSourceToRollback.Clear();
			}
		}

		private class DataSession
		{
			private IDbConnection _con = null;
			private IDbTransaction _tran = null;

			public DataSession(IDbConnection con, IDbTransaction tran) 
			{
				_con = con;
				_tran = tran;
			}

			public IDbConnection Connection { get { return _con; } }
			public IDbTransaction Transaction { get { return _tran; } }
		}

		public IDbTransaction GetTransaction(string dataSourceName, IDbConnection con)
		{
			IDbTransaction tran = null;

			//first get the current ***controlling*** context
			TransactionContext trCtx = TransactionContextFactory.GetCurrentContext();
			TransactionContext contrTrCtx = null;
			if(trCtx != null)
				contrTrCtx = trCtx.GetControllingContext();
			//if it's not null
			if(contrTrCtx != null) 
			{
				DataSession ds = null;
				//get the Hashtable with DataSessions per DataSource
				Hashtable transactionsByDataSource = dataSourceTransactionsByTrCtx[contrTrCtx] as Hashtable;
				//if not existing create :(
				if(transactionsByDataSource == null) 
				{
					//add it to it
					transactionsByDataSource = new Hashtable();
					dataSourceTransactionsByTrCtx.Add(contrTrCtx, transactionsByDataSource);
					//and subscribe for it's events
					contrTrCtx.StateChanged += new TCStateChangedEventHandler(HandleTCStateChangedEvent);
				}
				else 
				{
					ds = transactionsByDataSource[dataSourceName] as DataSession;
				}

				//if not existing create new and add it
				if(ds == null) 
				{
					//IDbConnection con = ds.CreateConnection();
					con.Open();
					IsolationLevel isolationLevel = 
						(IsolationLevel)Enum.Parse(typeof(IsolationLevel), contrTrCtx.IsolationLevel.ToString());
					tran = con.BeginTransaction(isolationLevel);
					ds = new DataSession(con, tran);
					transactionsByDataSource.Add(dataSourceName, ds);
				}
				else
				{
					tran = ds.Transaction;
				}
			}

			return tran;
		}


		#endregion
	}
}
