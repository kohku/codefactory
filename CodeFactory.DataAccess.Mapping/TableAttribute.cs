using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CodeFactory.DataAccess.Mapping
{
    /// <summary>
    /// Designates a class as an entity class associated with a database table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        private string _name = string.Empty;

        public TableAttribute()
        {
        }

        public TableAttribute(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Table name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
