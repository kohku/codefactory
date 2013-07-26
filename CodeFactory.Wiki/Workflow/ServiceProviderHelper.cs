using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;

namespace CodeFactory.Wiki.Workflow
{
    [Serializable]
    public class ServiceProviderHelper : IWikiServiceProvider
    {
        #region IWikiServiceProvider Members

        public event EventHandler<WikiEntryEventArgs> AuthorizationAccepted;

        public event EventHandler<WikiEntryEventArgs> AuthorizationRejected;

        #endregion

        #region Event raises

        public void AcceptAuthorization(Guid instanceId, Guid id)
        {
            WikiEntryEventArgs args = new WikiEntryEventArgs(instanceId, id);

            args.Identity = HttpContext.Current.User.Identity.Name;

            // Raise the event to the workflow
            ThreadPool.QueueUserWorkItem(AcceptTheAuthorization, args);
        }

        private void AcceptTheAuthorization(object o)
        {
            WikiEntryEventArgs e = (WikiEntryEventArgs)o;

            // Everything parameter going to the event must be serializable! Including the sender parameter.
            // The workflow instance doesn't actually need a reference to our default service provider
            // (if it needs to invoke a method on the service, it can use the CallExternalEvent activity),
            // so we can fix this problem by leaving the sender parameter as null.
            if (AuthorizationAccepted != null)
                AuthorizationAccepted(null, e);
        }

        public void RejectAuthorization(Guid instanceId, Guid id)
        {
            WikiEntryEventArgs args = new WikiEntryEventArgs(instanceId, id);

            args.Identity = HttpContext.Current.User.Identity.Name;

            // Raise the event to the workflow
            ThreadPool.QueueUserWorkItem(RejectTheAuthorization, args);
        }

        private void RejectTheAuthorization(object o)
        {
            WikiEntryEventArgs e = (WikiEntryEventArgs)o;

            if (AuthorizationRejected != null)
                AuthorizationRejected(null, e);
        }

        #endregion
    }
}
