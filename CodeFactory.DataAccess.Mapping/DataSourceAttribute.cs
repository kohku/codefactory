using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.DataAccess.Mapping
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class DataSourceAttribute : Attribute
    {
        private string _name;
        private string _insertCommandName;
        private string _updateCommandName;
        private string _deleteCommandName;
        private string _selectCommandName;

        private string _dataSetAdapterName;

        public DataSourceAttribute()
        {
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string InsertCommandName
        {
            get { return _insertCommandName; }
            set { _insertCommandName = value; }
        }

        public string UpdateCommandName
        {
            get { return _updateCommandName; }
            set { _updateCommandName = value; }
        }

        public string DeleteCommandName
        {
            get { return _deleteCommandName; }
            set { _deleteCommandName = value; }
        }

        public string SelectCommandName
        {
            get { return _selectCommandName; }
            set { _selectCommandName = value; }
        }

        public string DataSetAdapterName
        {
            get { return _dataSetAdapterName; }
            set { _dataSetAdapterName = value; }
        }
    }
}
