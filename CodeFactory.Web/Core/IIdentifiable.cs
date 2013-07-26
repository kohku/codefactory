using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.Web.Core
{
    public interface IIdentifiable<KEY>
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        KEY ID { get; set; }
    }
}
