using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.DataAccess.Mapping
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class IgnorePropertyAttribute : Attribute
    {
        public IgnorePropertyAttribute()
        {
        }
    }
}
