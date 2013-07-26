#define INCLUDE_EMBEDDED_OBJECTS

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace CodeFactory.DataAccess.Mapping
{
    internal class DataObjectHolder
    {
        private object _top;
        private DataTable _table;
        private DataSourceInfo _dataSourceInfo;

        internal DataObjectHolder(object item)
        {
            _top = item;

            DataSet holder = new DataSet(item.GetType().Name);

            // Recupera la información de la fuente de datos, select command, etc.
            _dataSourceInfo = DataObjectManager.Current.GetDataSourceInfo(item.GetType());

            // Recupera la información de la tabla, nombres de columnas, etc.
            _table = DataObjectManager.Current.GetObjectSchema(item.GetType());

            holder.Tables.Add(_table);

            // enlaza los datos de la tabla al objeto.
            DataObjectManager.Current.GetObjectData(_table, item);
       }

        internal object Top
        {
            [DebuggerStepThrough]
            get { return _top; }
        }

        internal DataTable Table
        {
            [DebuggerStepThrough]
            get { return _table; }
            [DebuggerStepThrough]
            set { _table = value; }
        }

        internal DataSourceInfo DataSourceInfo
        {
            [DebuggerStepThrough]
            get { return _dataSourceInfo; }
            [DebuggerStepThrough]
            set { _dataSourceInfo = value; }
        }

        internal static DataObjectHolder BuildContext(object item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return new DataObjectHolder(item);
        }
    }
}
