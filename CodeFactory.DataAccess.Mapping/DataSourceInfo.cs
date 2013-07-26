using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.DataAccess.Mapping
{
    public class DataSourceInfo
    {
        private string _dataSourceName;
        private string _insertCommandName;
        private string _updateCommandName;
        private string _deleteCommandName;
        private string _selectCommandName;
        private string _dataSetAdapterName;

        public string DataSourceName
        {
            get { return _dataSourceName; }
            set { _dataSourceName = value; }
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
