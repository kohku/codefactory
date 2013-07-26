using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CodeFactory.DataAccess.Transactions
{
    public class transactionHandlingSettings : ConfigurationSection
    {
        [ConfigurationProperty("transactionHandler", IsRequired = true)]
        public transactionHandler transactionHandler
        {
            get { return (transactionHandler)base["transactionHandler"]; }
            set { base["transactionHandler"] = value; }
        }
    }

    public class transactionHandler : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("handlerType", IsRequired = true)]
        public string handlerType
        {
            get { return (string)base["handlerType"]; }
            set { base["handlerType"] = value; }
        }
    }
}
