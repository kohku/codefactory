using System;
using System.Collections;
using System.Data;
using System.Runtime.Remoting.Messaging;
using CodeFactory.Utilities;

namespace CodeFactory.DataAccess.Transactions
{
	// A delegate type for hooking up transaction context state notifications.
	public delegate void TCStateChangedEventHandler(object sender, TCStateChangedEventArgs e);

	public class TCStateChangedEventArgs : EventArgs 
	{
		public TransactionContextState FromState;

		public TCStateChangedEventArgs(TransactionContextState fromState) 
		{
			this.FromState = fromState;
		}
	}

	/// <summary>
	/// Summary description for TransactionContext.
	/// </summary>
	public abstract class TransactionContext : IDisposable
	{
		private const string THREAD_CURRENT_CONTEXT__KEY = "TCCK";
		private TransactionIsolationLevel _isolationLevel = TransactionIsolationLevel.ReadCommitted;
		private TransactionContextState _state = TransactionContextState.Created;
		private TransactionContextState _stateFromChildren = TransactionContextState.ToBeCommitted;
		protected TransactionContext parentContext = null;

		// An event that clients can use to be notified whenever the
		// the state of the transaction context changes.
		public event TCStateChangedEventHandler StateChanged;

		public static TransactionContext Current 
		{
			get 
			{
				TransactionContext currentCtx = 
					CallContext.GetData(THREAD_CURRENT_CONTEXT__KEY) as TransactionContext;

				return currentCtx;
			}
		}

		public abstract TransactionContext GetControllingContext();
	
		public virtual TransactionContext Enter()
		{
			if(_state != TransactionContextState.Created
				&& _state != TransactionContextState.Exitted)
				throw new InvalidContextStateException(_state, new TransactionContextState[] { TransactionContextState.Created, TransactionContextState.Exitted }, "Context should be in one of the following states in Enter - {Created, Exitted}.");

			TransactionContextState fromState = _state;

			parentContext = CallContext.GetData(THREAD_CURRENT_CONTEXT__KEY) as TransactionContext;
			CallContext.SetData(THREAD_CURRENT_CONTEXT__KEY, this);
			_state = TransactionContextState.Entered;

			RaiseStateChangedEvent(fromState);

			return this;
		}

		public virtual void Exit()
		{
			if(_state != TransactionContextState.Entered
				&& _state != TransactionContextState.ToBeCommitted
				&& _state != TransactionContextState.ToBeRollbacked)
					throw new InvalidContextStateException(_state, new TransactionContextState[] { TransactionContextState.Entered, TransactionContextState.ToBeCommitted, TransactionContextState.ToBeRollbacked }, "Context should be in one of the following states in Exit - {Entered, ToBeCommitted, ToBeRollbacked}.");

			TransactionContextState fromState = _state;

			CallContext.SetData(THREAD_CURRENT_CONTEXT__KEY, parentContext);
			_state = TransactionContextState.Exitted;

			RaiseStateChangedEvent(fromState);
		}

		private void RaiseStateChangedEvent(TransactionContextState fromState) 
		{
			try 
			{
				if(this.StateChanged != null)
					this.StateChanged(this, new TCStateChangedEventArgs(fromState));
			}
			catch(Exception e) 
			{
				throw new TransactionContextException(ResourceStringLoader.GetResourceString(
                    "error_executing_transaction"), e);                    
			}
		}

		internal void VoteCommitFromChild()
		{
			_stateFromChildren = TransactionContextState.ToBeCommitted;
		}

		public virtual void VoteCommit() 
		{
			if(_state != TransactionContextState.Entered
				&& _state != TransactionContextState.ToBeCommitted)
				throw new InvalidContextStateException(_state, new TransactionContextState[] { TransactionContextState.Entered, TransactionContextState.ToBeCommitted }, "Context should be in one of the following states in VoteCommit - {Entered, ToBeCommitted}.");

			TransactionContextState fromState = _state;

			_state = TransactionContextState.ToBeCommitted;
			TransactionContext contrCtx = this.GetControllingContext();
			if(contrCtx != null && contrCtx != this) contrCtx.VoteCommitFromChild();

			RaiseStateChangedEvent(fromState);		
		}

		internal void VoteRollbackFromChild()
		{
			_stateFromChildren = TransactionContextState.ToBeRollbacked;
		}

		public virtual void VoteRollback() 
		{
			if(_state != TransactionContextState.Entered
				&& _state != TransactionContextState.ToBeCommitted
				&& _state != TransactionContextState.ToBeRollbacked)
					throw new InvalidContextStateException(_state,
                        new TransactionContextState[] {
                            TransactionContextState.Entered,
                            TransactionContextState.ToBeCommitted,
                            TransactionContextState.ToBeRollbacked
                        },
                        ResourceStringLoader.GetResourceString("invalid_context_state"));

			TransactionContextState fromState = _state;

			_state = TransactionContextState.ToBeRollbacked;
			TransactionContext contrCtx = this.GetControllingContext();
			if(contrCtx != null && contrCtx != this) contrCtx.VoteRollbackFromChild();

			RaiseStateChangedEvent(fromState);
		}

		public abstract TransactionAffinity Affinity { get; }

		public TransactionIsolationLevel IsolationLevel
		{
			get { return _isolationLevel; }
			set { _isolationLevel = value; }
		}

		public virtual TransactionContextState State
		{
			get { return _state; }
		}

		#region IDisposable Members

		public void Dispose()
		{
			if(_state == TransactionContextState.Entered)
			{
				//no VoteCommit has been called -> call VoteRollback
				this.VoteRollback();
			}
			else if(_state == TransactionContextState.ToBeCommitted 
				&& _stateFromChildren == TransactionContextState.ToBeRollbacked)
			{
				this.VoteRollback();
			}

			this.Exit();
		}

		#endregion
	}
}
