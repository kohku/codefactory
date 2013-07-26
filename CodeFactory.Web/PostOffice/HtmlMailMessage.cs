using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net.Mail;

namespace CodeFactory.Web.PostOffice
{
    public class HtmlMailMessage : MailMessage
    {
        private Dictionary<string, Stream> _linkedResources;

        [DebuggerStepThrough]
        public HtmlMailMessage() 
            : base()
        {
            _linkedResources = new Dictionary<string, Stream>();
        }

        [DebuggerStepThrough]
        public HtmlMailMessage(MailAddress from, MailAddress to)
        {
            _linkedResources = new Dictionary<string, Stream>();
        }

        [DebuggerStepThrough]
        public HtmlMailMessage(string from, string to, string subject, string body)
            : base(from, to, subject, body)
        {
            _linkedResources = new Dictionary<string, Stream>();
        }

        public Dictionary<string, Stream> LinkedResources
        {
            [DebuggerStepThrough]
            get { return _linkedResources; }
        }
    }
}
