using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration.Provider;

namespace CodeFactory.Web.PostOffice
{
    public abstract class PostOfficeProvider : ProviderBase
    {
        public virtual HtmlMailMessage Create(object holder, Stream template, Dictionary<string, string> parameters)
        {
            HtmlMailMessage message = new HtmlMailMessage();

            Prepare(message, holder, template, parameters);

            return message;
        }

        protected abstract void Prepare(HtmlMailMessage message, object holder, Stream template, Dictionary<string, string> parameters);

        public abstract void SendMessage(HtmlMailMessage message);
    }
}
