using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities;

namespace CodeFactory.Wiki.Workflow
{
    [ExternalDataExchange]
    public interface IWikiServiceProvider
    {
        event EventHandler<WikiEntryEventArgs> AuthorizationAccepted;
        event EventHandler<WikiEntryEventArgs> AuthorizationRejected;
    }
}
