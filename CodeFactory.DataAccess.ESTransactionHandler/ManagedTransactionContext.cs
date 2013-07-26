//using System; 
//using System.Data;
//using System.Data.SqlClient;
//using System.EnterpriseServices; 
//
//namespace CodeFactory.DataAccess.TransactionHandling.ESTransactionHandler
//{ 
//	// simple no-argument delegate for your callback routine 
//	public delegate void ContextCallback(); 
// 
//	// simple interface that allows you to invoke code in the configured 
//	// component's context 
//	public interface IManagedTransactionContext 
//	{ 
//		void DoCallback(ContextCallback callback); 
//		void SetComplete();
//		void SetAbort();
//		void OpenConnection(IDbConnection con);
//		IManagedTransactionContext ParentContext { get; }
//		IManagedTransactionContext GetControllingContext();
//	} 
// 
//	// The obvious class that does what you'd expect 
//	//[ Transaction(TransactionOption.Required) ] 
//	public class BaseTransactionContext 
//		: ServicedComponent, IManagedTransactionContext 
//	{ 
//		private IManagedTransactionContext _parentTC = null;
//
//		public BaseTransactionContext(IManagedTransactionContext parentTc)
//		{
//			_parentTC = parentTc;
//		}
//
//		public IManagedTransactionContext ParentContext
//		{
//			get
//			{
//				return _parentTC;
//			}
//		}
//
//		//[AutoComplete] 
//		void IManagedTransactionContext.DoCallback(ContextCallback callback) 
//		{ 
//			if (callback == null) throw new ArgumentNullException("callback"); 
//			callback(); 
//		} 
//
//		void IManagedTransactionContext.SetComplete()
//		{
//			ContextUtil.SetComplete();
//		}
//
//		void IManagedTransactionContext.SetAbort()
//		{
//			ContextUtil.SetAbort();
//		}
//
//		void IManagedTransactionContext.OpenConnection(IDbConnection con)
//		{
//			con.Open();
//		}
//
//		public virtual IManagedTransactionContext GetControllingContext()
//		{
//			return this;
//		}
//
//	} 
//
//	[Transaction(TransactionOption.Required)]
//	public class RequiredTransactionContext : BaseTransactionContext 
//	{
//		public RequiredTransactionContext(IManagedTransactionContext parentTc)
//			: base(parentTc) {}
//
//		public override IManagedTransactionContext GetControllingContext()
//		{
//			if(this.ParentContext != null)
//				return this.ParentContext;
//			else
//				return this;
//		}
//	}
//	[Transaction(TransactionOption.RequiresNew)]
//	public class RequiresNewTransactionContext : BaseTransactionContext 
//	{
//		public RequiresNewTransactionContext(IManagedTransactionContext parentTc)
//			: base(parentTc) {}
//
//		public override IManagedTransactionContext GetControllingContext()
//		{
//			return this;
//		}	
//	}
//	[Transaction(TransactionOption.Supported)]
//	public class SupportedTransactionContext : BaseTransactionContext 
//	{
//		public SupportedTransactionContext(IManagedTransactionContext parentTc)
//			: base(parentTc) {}
//
//		public override IManagedTransactionContext GetControllingContext()
//		{
//			if(this.ParentContext != null)
//				return this.ParentContext;
//			else
//				return null;
//		}	
//	}
//	[Transaction(TransactionOption.NotSupported)]
//	public class NotSupportedTransactionContext : BaseTransactionContext 
//	{
//		public NotSupportedTransactionContext(IManagedTransactionContext parentTc)
//			: base(parentTc) {}
//
//		public override IManagedTransactionContext GetControllingContext()
//		{
//			return null;
//		}	
//	}
//
//}