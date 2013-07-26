using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web.Core;

namespace CodeFactory.Wiki.Workflow
{
    public interface IWorkWikiItem : IWiki
    {
        SaveAction Action { get; set; }
        string Authorizer { get; set; }
        string Messages { get; set; }
        WikiStatus Status { get; set; }
        Guid TrackingNumber { get; set; }
        DateTime? ExpirationDate { get; set; }
        string TrackingLink { get; }
        SaveAction Save();
        string IPAddress { get; set; }
    }

    public enum WikiStatus
    {
        Created,
        AuthorizationRequested,
        AuthorizationAccepted,
        AuthorizationRejected,
        AuthorizationExpired,
        Processing
    }
}
