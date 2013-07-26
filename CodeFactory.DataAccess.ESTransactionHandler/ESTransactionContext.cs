using System; 
using System.Data;
using System.EnterpriseServices; 
using CodeFactory.DataAccess.Transactions;

namespace CodeFactory.DataAccess.TransactionHandling.ESTransactionHandler
{
	/// <summary>
	/// Summary description for ESTransactionContext.
	/// </summary>
	public class ESTransactionContext : ServicedComponent
	{
		public void VoteComplete()
		{
			//ContextUtil.SetComplete();
			if(this.IsInTransaction)
				ContextUtil.MyTransactionVote = TransactionVote.Commit;
		}

		public void VoteAbort()
		{
			//ContextUtil.SetAbort();
			if(this.IsInTransaction)
				ContextUtil.MyTransactionVote = TransactionVote.Abort;
		}

		public void OpenConnection(IDbConnection con)
		{
			con.Open();
		}

		public virtual void Exit()
		{
			if(this.IsInTransaction)
				ContextUtil.DeactivateOnReturn = true;
		}

		public bool IsInTransaction
		{
			get
			{
				return ContextUtil.IsInTransaction;
			}
		}

		public ESTransactionContext CreateContext(TransactionAffinity affinity)
		{
			return ESTransactionHandler.CreateContext(affinity);
		}
	}
}