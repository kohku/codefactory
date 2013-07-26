using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities;

namespace CodeFactory.Wiki.Workflow
{
    [Serializable]
    public class WikiEntryEventArgs : ExternalDataEventArgs
    {
        private Guid _id;

        public WikiEntryEventArgs(Guid instanceId, Guid id)
            : base(instanceId)
        {
            _id = id;
        }

        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
