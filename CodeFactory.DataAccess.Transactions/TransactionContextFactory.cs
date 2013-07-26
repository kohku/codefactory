using System;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.IO;
using CodeFactory.Utilities;

namespace CodeFactory.DataAccess.Transactions
{
	// A delegate type for hooking up transaction context state notifications.
	public delegate void TCCreatedEventHandler(object sender, TCCreatedEventArgs e);

	/// <summary>
	/// A customized EventArgs holding the created transaction context
	/// </summary>
	public class TCCreatedEventArgs : EventArgs 
	{
		public TransactionContext Context;

		public TCCreatedEventArgs(TransactionContext context) 
		{
			this.Context = context;
		}
	}

	/// <summary>
	/// TransactionContextFactory creates different transaction contexts 
	/// and raises a ContextCreated event.
	/// Additionally initializes the transaction handler factory. 
	/// Config info retrieved from Web.config
	/// </summary>
	public class TransactionContextFactory
	{
        private static object syncRoot = new object();

        private TransactionContextFactory() {}

		private static ITransactionHandler _th;

		public static ITransactionHandler GetHandler()
		{
			return _th;
		}

		static TransactionContextFactory()
		{
            if (_th == null)
            {
                lock (syncRoot)
                {
                    if (_th == null)
                    {
                        try
                        {
                            transactionHandlingSettings settings =
                                (transactionHandlingSettings)ConfigurationManager.GetSection("dataAccess/transactionHandlingSettings");

                            Type handlerType = Type.GetType(settings.transactionHandler.handlerType);

                            if (handlerType == null)
                                throw new ApplicationException(ResourceStringLoader.GetResourceString(
                                    "handlertype_cannot_be_loaded", settings.transactionHandler.handlerType,
                                    settings.transactionHandler.name));

                            _th = (ITransactionHandler)Activator.CreateInstance(handlerType);

                            ContextCreated += new TCCreatedEventHandler(_th.HandleTCCreated);
                        }
                        catch (Exception e)
                        {
                            throw new TransactionHandlingException(ResourceStringLoader.GetResourceString(
                                "error_loading_transactionhandler", e.Message));
                        }
                    }
                }
            }
		}

		// An event that clients can use to be notified whenever the
		// the a new transaction context is created.
		public static event TCCreatedEventHandler ContextCreated;

		public static TransactionContext GetContext(TransactionAffinity transactionAffinity) 
		{
			TransactionContext ctx = null;

			switch(transactionAffinity)
			{
				case TransactionAffinity.RequiresNew:
					ctx = new RequiresNewTransactionContext();
					break;
				case TransactionAffinity.Required:
					ctx = new RequiredTransactionContext();
					break;
				case TransactionAffinity.Supported:
					ctx = new SupportedTransactionContext();
					break;
				case TransactionAffinity.NotSupported:
					ctx = new NotSupportedTransactionContext();
					break;
				default:
					throw new TransactionContextException(transactionAffinity.ToString() + "is not currently supported.");
			}

			if(ContextCreated != null)
				ContextCreated(null, new TCCreatedEventArgs(ctx));

			return ctx;
		}

		public static TransactionContext GetCurrentContext() 
		{
			return TransactionContext.Current;
		}

		public static TransactionContext EnterContext(TransactionAffinity transactionAffinity) 
		{
			TransactionContext ctx = GetContext(transactionAffinity);
			ctx.Enter();
			return ctx;
		}
	}
}
