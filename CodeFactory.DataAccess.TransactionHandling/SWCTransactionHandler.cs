using System;
using System.Data;
using CodeFactory.DataAccess.Transactions;
using System.EnterpriseServices;

namespace CodeFactory.DataAccess.TransactionHandling
{
	/// <summary>
	/// SWCTransactionHandler handles TransactionContext* events.
	/// Impementation using Services Without Components.
	/// Singlenton?
	/// </summary>
	public class SWCTransactionHandler : ITransactionHandler
	{
		public SWCTransactionHandler()
		{
			////TO DO: find a way to invoke the constructor at TransactionContextFactory init???
			//TransactionContextFactory.ContextCreated += 
			//	new TCCreatedEventHandler(this.HandleTCCreated);
		}

		#region ITransactionHandler Members

		public void HandleTCCreated(object sender, TCCreatedEventArgs args)
		{
			//here attach to the event handlers of the newly created transaction context
			//(not in GetDataSession)
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
					ServiceDomain.Enter(CreateServiceConfig(trCtx));
					break;

				case TransactionContextState.ToBeCommitted:
					ContextUtil.SetComplete();
					break;

				case TransactionContextState.ToBeRollbacked:
					ContextUtil.SetAbort();
					break;

				case TransactionContextState.Exitted:
					ServiceDomain.Leave();
					break;

				default:
					throw new Exception("Unexpected TransactionContextState:" + args.FromState.ToString());
			}
		}

		public IDbTransaction GetTransaction(string dataSourceName, IDbConnection con)
		{
			//here hack the first context not being entered ...
			//first get the current ***controlling*** context
			TransactionContext trCtx = TransactionContextFactory.GetCurrentContext();
			TransactionContext contrTrCtx = null;
			if(trCtx != null)
				contrTrCtx = trCtx.GetControllingContext();
			if(contrTrCtx != null) 
			{
				//here enter it if not entered :(
				ServiceDomain.Enter(CreateServiceConfig(trCtx));
			}

			return null;			
		}


		private ServiceConfig CreateServiceConfig(TransactionContext trCtx) 
		{
			ServiceConfig config = new ServiceConfig();

			TransactionOption transactionOption = 
				(TransactionOption)Enum.Parse(typeof(TransactionOption), trCtx.Affinity.ToString());
			config.Transaction = transactionOption;
			System.EnterpriseServices.TransactionIsolationLevel isolationLevel = 
				(System.EnterpriseServices.TransactionIsolationLevel)Enum.Parse(typeof(System.EnterpriseServices.TransactionIsolationLevel), trCtx.IsolationLevel.ToString());
			config.IsolationLevel = isolationLevel;

			return config;
		}

		#endregion
	}
}
