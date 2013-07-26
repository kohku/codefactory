using System;
using System.Data;
using System.Collections;
using CodeFactory.DataAccess.Transactions;
using System.EnterpriseServices;
using CodeFactory.DataAccess.TransactionHandling;
using System.Runtime.Remoting.Messaging;

namespace CodeFactory.DataAccess.TransactionHandling.ESTransactionHandler
{
	/// <summary>
	/// ESTransactionHandler handles TransactionContext events.
	/// Implementation using COM+ DTC Transactions.
	/// </summary>
	public class ESTransactionHandler : ITransactionHandler
	{
		private const string THREAD_CURRENT_CONTEXT__KEY = "DTCCK";

		static ESTransactionHandler()
		{

		}

		public ESTransactionHandler()
		{
			
		}

		private static ArrayList Contexts
		{
			get 
			{
				ArrayList contexts = CallContext.GetData(THREAD_CURRENT_CONTEXT__KEY) as ArrayList;
				if(contexts == null)
				{
					contexts = new ArrayList();
					CallContext.SetData(THREAD_CURRENT_CONTEXT__KEY, contexts);
				}
				return contexts;
			}
		}

		private static ESTransactionContext PeekLastContext()
		{
			return PeekContext(1);
		}

		private static ESTransactionContext PeekLastButOneContext()
		{
			return PeekContext(2);
		}

		private static ESTransactionContext PeekContext(int lastIndex)
		{
			if(Contexts.Count >= lastIndex)
				return (ESTransactionContext)Contexts[Contexts.Count - lastIndex];
			else
				return null;
		}

		#region ITransactionHandler Members

		public void HandleTCCreated(object sender, TCCreatedEventArgs args)
		{
			//attach to the event handlers of the newly created transaction context
			TransactionContext trCtx = args.Context;
			trCtx.StateChanged += new TCStateChangedEventHandler(HandleTCStateChangedEvent);
		}

		public void HandleTCStateChangedEvent(object sender, TCStateChangedEventArgs args)
		{
			TransactionContext trCtx = (TransactionContext)sender;

			ESTransactionContext currentDtrCtx = PeekLastContext();			

			//here check the event & commit/rollback etc
			switch(trCtx.State) 
			{
				case TransactionContextState.Entered:
					ESTransactionContext newDtrCtx = null;
					if(currentDtrCtx != null)
						newDtrCtx = currentDtrCtx.CreateContext(trCtx.Affinity);
					else
						newDtrCtx = CreateContext(trCtx.Affinity);

					Contexts.Add(newDtrCtx);
					break;

				case TransactionContextState.ToBeCommitted:
					currentDtrCtx.VoteComplete();
					break;

				case TransactionContextState.ToBeRollbacked:
					try 
					{ 
						currentDtrCtx.VoteAbort();
					}
					catch {}	//suppress CONTEXT_E_ABORTED exception for a parent ctx(better way?)
					break;

				case TransactionContextState.Exitted:
					Contexts.RemoveAt(Contexts.Count - 1);

					try 
					{ 
						currentDtrCtx.Exit(); 
						currentDtrCtx.Dispose();
					} 
					catch {}	//suppress CONTEXT_E_ABORTED exception for a parent ctx(better way?)
					break;

				default:
					throw new Exception("Unexpected TransactionContextState:" + args.FromState.ToString());
			}
		}

		public static ESTransactionContext CreateContext(TransactionAffinity affinity)
		{
			switch(affinity)
			{
				case TransactionAffinity.RequiresNew:
					return new RequiresNewTransactionContext();
					
				case TransactionAffinity.Required:
					return new RequiredTransactionContext();

				case TransactionAffinity.Supported:
					return new SupportedTransactionContext();

				case TransactionAffinity.NotSupported:
					return new NotSupportedTransactionContext();

				default:
					throw new TransactionContextException("Invalid affinity(" + affinity.ToString() + ". Mapping failed.");
			}
		}

		public IDbTransaction GetTransaction(string dataSourceName, IDbConnection con)
		{
			ESTransactionContext currentDtrCtx = PeekLastContext();
			if(currentDtrCtx != null)
				currentDtrCtx.OpenConnection(con);

			return null;
		}

		#endregion
	}


}
