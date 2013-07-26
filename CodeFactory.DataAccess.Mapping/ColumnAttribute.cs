using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CodeFactory.DataAccess.Mapping
{
    /// <summary>
    /// Designates a property as an entity associated with a database column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        private string _name = string.Empty;
        private bool _isKey;
        private bool _isForeignKey;
        private bool _enableIdentity;

        public ColumnAttribute()
        {
        }

        public ColumnAttribute(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool IsKey
        {
            get { return _isKey; }
            set { _isKey = value; }
        }

        public bool IsForeignKey
        {
            get { return _isForeignKey; }
            set { _isForeignKey = value; }
        }

        public bool EnableIdentity
        {
            get { return _enableIdentity; }
            set { _enableIdentity = value; }
        }
    }
}
