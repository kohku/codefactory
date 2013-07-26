#define INCLUDE_EMBEDDED_OBJECTS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using CodeFactory.DataAccess;
using CodeFactory.DataAccess.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace CodeFactory.DataAccess.Mapping
{
    /// <summary>
    /// Helps to populate commands for common data access operations.
    /// </summary>
    public class DataObjectManager : MarshalByRefObject
    {
        private static volatile DataObjectManager _uniqueInstance;
        private static object syncRoot = new object();

        [DebuggerStepThrough()]
        public DataObjectManager()
        {
        }

        public static DataObjectManager Current
        {
            [DebuggerStepThrough()]
            get
            {
                if (_uniqueInstance == null)
                {
                    lock(syncRoot)
                    {
                        if (_uniqueInstance == null)
                            _uniqueInstance = new DataObjectManager();
                    }
                }

                return _uniqueInstance;
            }
        }

        /// <summary>
        /// Retrieves data source information.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal DataSourceInfo GetDataSourceInfo(Type type)
        {
            DataSourceInfo info;

            if (type == null)
                throw new ArgumentNullException("type");

            try
            {
                CacheManager mappingCache = CacheFactory.GetCacheManager();

                if (mappingCache.Contains(string.Format("GetDataSourceInfo({0})", type.FullName)))
                    return (DataSourceInfo)mappingCache[string.Format("GetDataSourceInfo({0})", type.FullName)];

                Debug.WriteLine(string.Format("Getting data source information for type {0}", type.Name));

                info = new DataSourceInfo();

                DataSourceAttribute[] datasource = (DataSourceAttribute[])type.GetCustomAttributes(typeof(DataSourceAttribute), true);

                info.DataSourceName = datasource.Length > 0 ? datasource[0].Name : string.Empty;

                // Retrieving table name to map the object in the database.
                TableAttribute[] tables = (TableAttribute[])type.GetCustomAttributes(typeof(TableAttribute), true);

                info.DeleteCommandName = datasource.Length > 0 &&
                    !string.IsNullOrEmpty(datasource[0].DeleteCommandName) ?
                    datasource[0].DeleteCommandName : "Delete" + (tables.Length > 0 &&
                    !string.IsNullOrEmpty(tables[0].Name) ? tables[0].Name : type.Name);
                info.InsertCommandName = datasource.Length > 0 &&
                    !string.IsNullOrEmpty(datasource[0].InsertCommandName) ?
                    datasource[0].InsertCommandName : "Insert" + (tables.Length > 0 &&
                    !string.IsNullOrEmpty(tables[0].Name) ? tables[0].Name : type.Name);
                info.UpdateCommandName = datasource.Length > 0 &&
                    !string.IsNullOrEmpty(datasource[0].UpdateCommandName) ?
                    datasource[0].UpdateCommandName : "Update" + (tables.Length > 0 &&
                    !string.IsNullOrEmpty(tables[0].Name) ? tables[0].Name : type.Name);
                info.SelectCommandName = datasource.Length > 0 &&
                    !string.IsNullOrEmpty(datasource[0].SelectCommandName) ?
                    datasource[0].SelectCommandName : "Select" + (tables.Length > 0 &&
                    !string.IsNullOrEmpty(tables[0].Name) ? tables[0].Name : type.Name);
                info.DataSetAdapterName = datasource.Length > 0 &&
                    !string.IsNullOrEmpty(datasource[0].DataSetAdapterName) ?
                    datasource[0].DataSetAdapterName : (tables.Length > 0 &&
                    !string.IsNullOrEmpty(tables[0].Name) ? tables[0].Name : type.Name) + "Adapter";

                mappingCache.Add(string.Format("GetDataSourceInfo({0})", type.FullName), info,
                    CacheItemPriority.None, null, new SlidingTime(TimeSpan.FromMinutes(15)));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return info;
        }

        /// <summary>
        /// Gets the schema table required for the object.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        internal DataTable GetObjectSchema(Type type)
        {
            DataTable table;

            if (type == null)
                throw new ArgumentNullException("type");

            try
            {
                CacheManager mappingCache = CacheFactory.GetCacheManager();

                if (mappingCache.Contains(string.Format("GetObjectSchema({0})", type.FullName)))
                    return ((DataTable)mappingCache[string.Format("GetObjectSchema({0})", type.FullName)]).Clone();

                Debug.WriteLine(string.Format("Getting object schema for type {0}", type.Name));

                // Retrieving table name to map the object in the database.
                TableAttribute[] tables = (TableAttribute[])type.GetCustomAttributes(typeof(TableAttribute), true);

                string tableName = tables.Length > 0 && !string.IsNullOrEmpty(tables[0].Name) ?
                    tables[0].Name : type.Name;

                table = new DataTable(tableName);

                List<DataColumn> primaryKeys = new List<DataColumn>();

                // Retrieve all properties.
                List<PropertyInfo> properties = new List<PropertyInfo>(type.GetProperties(BindingFlags.Instance | BindingFlags.Public));

                // Remove full access properties, explicitly ignored properties, and embedded objects without column attribute.
                List<PropertyInfo> filteredProperties = properties.FindAll(delegate(PropertyInfo match)
                {
                    if (!match.CanRead || !match.CanWrite)
                        return false;

                    if (match.GetCustomAttributes(typeof(IgnorePropertyAttribute), true).Length > 0)
                        return false;

                    ColumnAttribute[] columns = (ColumnAttribute[])match.GetCustomAttributes(typeof(ColumnAttribute), true);

                    // Skip embedded objects columns without an explicit key or foreign key column attribute.
                    if (columns.Length == 0 && !match.PropertyType.Equals(typeof(string)) &&
                        !match.PropertyType.IsValueType && !match.PropertyType.IsGenericType)
                        return false;

                    return true;
                });

                foreach (PropertyInfo property in filteredProperties)
                {
                    ColumnAttribute[] columns = (ColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    // Add value types nob generics without column attribute.
                    if (columns.Length == 0 && !property.PropertyType.IsGenericType)
                    {
                        // Default column name
                        DataColumn defaultColumn = new DataColumn();
                        defaultColumn.ColumnName = property.Name;
                        defaultColumn.DataType = property.PropertyType;
                        table.Columns.Add(defaultColumn);
                        continue;
                    }

                    // Last case, columns with column attribute.
                    if (columns.Length > 0)
                    {
                        // Non-keyed value type object with column attribute. 
                        if (!columns[0].IsKey && !columns[0].IsForeignKey)
                        {
                            DataColumn declaredColumn = new DataColumn();
                            declaredColumn.ColumnName = !string.IsNullOrEmpty(columns[0].Name) ?
                                columns[0].Name : property.Name;
                            declaredColumn.DataType = property.PropertyType;
                            table.Columns.Add(declaredColumn);
                            continue;
                        }

                        // Key value type object with column attribute.
                        if (!property.PropertyType.IsGenericType &&
                            (property.PropertyType.Equals(typeof(string)) || property.PropertyType.IsValueType || property.PropertyType.IsArray))
                        {
                            // It is not an embedded object. Let's assign the id's data type.
                            DataColumn declaredColumn = new DataColumn();
                            declaredColumn.ColumnName = !string.IsNullOrEmpty(columns[0].Name) ?
                                columns[0].Name : property.Name;
                            declaredColumn.DataType = property.PropertyType;
                            table.Columns.Add(declaredColumn);
                            primaryKeys.Add(declaredColumn);
                            continue;
                        }

                        // Key embedded object with column attribute.
                        if (!property.PropertyType.IsGenericType && 
                            !property.PropertyType.Equals(typeof(string)) && !property.PropertyType.IsValueType)
                        {
                            // ATTENTION!
                            // Watch for circular references!!
                            DataTable embeddedObject = GetObjectSchema(property.PropertyType);

                            foreach (DataColumn key in embeddedObject.PrimaryKey)
                            {
                                DataColumn declaredColumn = new DataColumn();
                                declaredColumn.ColumnName = !string.IsNullOrEmpty(columns[0].Name) ?
                                    columns[0].Name + key.ColumnName : property.PropertyType.Name + key.ColumnName;
                                declaredColumn.DataType = key.DataType;
                                table.Columns.Add(declaredColumn);
                                primaryKeys.Add(declaredColumn);
                            }
                            continue;
                        }
                        
                        // Key generic value type
                        if (property.PropertyType.IsGenericType && property.PropertyType.IsValueType)
                        {
                            bool found = false;

                            foreach (PropertyInfo genericProperty in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                            {
                                if (!genericProperty.Name.Equals("Key"))
                                    continue;

                                // ATTENTION!
                                // Watch for circular references!!
                                DataTable embeddedObject = GetObjectSchema(genericProperty.PropertyType);

                                foreach (DataColumn key in embeddedObject.PrimaryKey)
                                {
                                    DataColumn declaredColumn = new DataColumn();
                                    declaredColumn.ColumnName = !string.IsNullOrEmpty(columns[0].Name) ?
                                        columns[0].Name + key.ColumnName : genericProperty.PropertyType.Name + key.ColumnName;
                                    declaredColumn.DataType = key.DataType;
                                    table.Columns.Add(declaredColumn);
                                    primaryKeys.Add(declaredColumn);

                                    found = true;
                                }

                                if (found)
                                    break;

                                #region Optimizing
                                //foreach (PropertyInfo foreignProperty in genericProperty.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                                //{
                                //    ColumnAttribute[] foreignColumns = (ColumnAttribute[])foreignProperty.GetCustomAttributes(typeof(ColumnAttribute), true);

                                //    if (foreignColumns.Length <= 0 || !foreignColumns[0].IsKey)
                                //        continue;

                                //    found = true;
                                //    declaredColumn.DataType = foreignProperty.PropertyType;
                                //    break;
                                //}
                                #endregion
                            }

                            if (!found)
                                throw new InvalidOperationException(string.Format(
                                    "There's no specified key column for the object {0}.", property.Name));

                            continue;
                        }

                        throw new InvalidOperationException(
                            string.Format("Unhandled property for type{0}. Type unhandled: {1}", type.Name, property.Name));
                    }
                }

                if (primaryKeys.Count > 0)
                    table.PrimaryKey = primaryKeys.ToArray();

                mappingCache.Add(string.Format("GetObjectSchema({0})", type.FullName), table,
                    CacheItemPriority.None, null, new SlidingTime(TimeSpan.FromMinutes(15)));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return table.Clone();
        }

        /// <summary>
        /// Retrieves object properties's values
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="item"></param>
        internal void GetObjectData(DataTable table, object item)
        {
            Type type = item.GetType();

            try
            {
                // Filling table with the element.
                DataRow row = table.NewRow();

                List<PropertyInfo> properties = new List<PropertyInfo>(type.GetProperties(BindingFlags.Instance | BindingFlags.Public));

                // Remove full access properties, explicitly ignored properties, and embedded objects without column attribute.
                List<PropertyInfo> filteredProperties = properties.FindAll(delegate(PropertyInfo match)
                {
                    if (!match.CanRead || !match.CanWrite)
                        return false;

                    if (match.GetCustomAttributes(typeof(IgnorePropertyAttribute), true).Length > 0)
                        return false;

                    ColumnAttribute[] columns = (ColumnAttribute[])match.GetCustomAttributes(typeof(ColumnAttribute), true);

                    // Skip embedded objects columns without an explicit key or foreign key column attribute.
                    if (columns.Length == 0 && !match.PropertyType.Equals(typeof(string)) &&
                        !match.PropertyType.IsValueType && !match.PropertyType.IsGenericType)
                        return false;

                    return true;
                });

                foreach (PropertyInfo property in filteredProperties)
                {
                    ColumnAttribute[] columns = (ColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    // Set value types non generics without column attribute.
                    if (columns.Length == 0 && !property.PropertyType.IsGenericType)
                    {
                        row[property.Name] = property.GetValue(item, null);
                        continue;
                    }

                    // Last case, columns with column attribute.
                    if (columns.Length > 0)
                    {
                        if (!property.PropertyType.IsGenericType &&
                            (property.PropertyType.Equals(typeof(string)) || property.PropertyType.IsValueType || property.PropertyType.IsArray))
                        {
                            // It is not an embedded object. Let's assign the id's value.
                            row[!string.IsNullOrEmpty(columns[0].Name) ? columns[0].Name : property.Name] =
                                property.GetValue(item, null);
                            continue;
                        }

                        // Key embedded object with column attribute.
                        if (!property.PropertyType.IsGenericType &&
                            !property.PropertyType.Equals(typeof(string)) && !property.PropertyType.IsValueType)
                        {
                            object embeddedObject = property.GetValue(item, null);

                            bool found = false;

                            foreach (PropertyInfo foreignProperty in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                            {
                                ColumnAttribute[] foreignColumns = (ColumnAttribute[])foreignProperty.GetCustomAttributes(typeof(ColumnAttribute), true);

                                if (foreignColumns.Length <= 0 || !foreignColumns[0].IsKey)
                                    continue;

                                found = true;
                                row[!string.IsNullOrEmpty(columns[0].Name) ? 
                                    columns[0].Name : property.PropertyType.Name + foreignProperty.Name] = 
                                    foreignProperty.GetValue(embeddedObject, null);
                                break;
                            }

                            if (!found)
                                throw new InvalidOperationException(string.Format(
                                    "There's no specified key column for the object {0}.", property.Name));

                            continue;
                        }

                        // Key generic value type
                        if (property.PropertyType.IsGenericType && property.PropertyType.IsValueType)
                        {
                            object embeddedObject = property.GetValue(item, null);

                            bool found = false;

                            foreach (PropertyInfo genericProperty in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                            {
                                if (!genericProperty.Name.Equals("Key"))
                                    continue;

                                object key = genericProperty.GetValue(embeddedObject, null);

                                List<PropertyInfo> foreignProperties = new List<PropertyInfo>(
                                    genericProperty.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public));

                                PropertyInfo foreignProperty = foreignProperties.Find(delegate(PropertyInfo match)
                                {
                                    ColumnAttribute[] foreignColumns = (ColumnAttribute[])match.GetCustomAttributes(typeof(ColumnAttribute), true);

                                    if (foreignColumns.Length <= 0 || !foreignColumns[0].IsKey)
                                        return false;

                                    found = true;
                                    row[genericProperty.PropertyType.Name + match.Name] = match.GetValue(key, null);
                                    return true;
                                });

                                if (found)
                                    break;
                            }
    
                            if (!found)
                                throw new InvalidOperationException(string.Format(
                                    "There's no specified key column for the object {0}.", property.Name));

                            continue;
                        }

                        throw new InvalidOperationException(
                            string.Format("Unhandled property for type{0}. Type unhandled: {1}", type.Name, property.Name));
                    }
                }

                table.Rows.Add(row);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fill object properties with values.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="item"></param>
        internal void SetObjectData(DataTable table, object item)
        {
            Type type = item.GetType();

            foreach (DataRow register in table.Rows)
            {
                List<PropertyInfo> properties = new List<PropertyInfo>(type.GetProperties(BindingFlags.Instance | BindingFlags.Public));

                // Remove full access properties, explicitly ignored properties, and embedded objects without column attribute.
                List<PropertyInfo> filteredProperties = properties.FindAll(delegate(PropertyInfo match)
                {
                    if (!match.CanRead || !match.CanWrite)
                        return false;

                    if (match.GetCustomAttributes(typeof(IgnorePropertyAttribute), true).Length > 0)
                        return false;

                    ColumnAttribute[] columns = (ColumnAttribute[])match.GetCustomAttributes(typeof(ColumnAttribute), true);

                    // Skip embedded objects columns without an explicit key or foreign key column attribute.
                    if (columns.Length == 0 && !match.PropertyType.Equals(typeof(string)) &&
                        !match.PropertyType.IsValueType && !match.PropertyType.IsGenericType)
                        return false;

                    return true;
                });

                foreach (PropertyInfo property in filteredProperties)
                {
                    ColumnAttribute[] columns = (ColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    // Set value types non generics without column attribute.
                    if (columns.Length == 0 && !property.PropertyType.IsGenericType)
                    {
                        if (!register[property.Name].Equals(DBNull.Value))
                            property.SetValue(item, register[property.Name], null);
                        //row[property.Name] = property.GetValue(item, null);
                        continue;
                    }

                    // Last case, columns with column attribute.
                    if (columns.Length > 0)
                    {
                        if (!property.PropertyType.IsGenericType &&
                            (property.PropertyType.Equals(typeof(string)) || property.PropertyType.IsValueType || property.PropertyType.IsArray))
                        {
                            // It is not an embedded object. Let's assign the id's value.
                            if (!register[!string.IsNullOrEmpty(columns[0].Name) ? columns[0].Name : property.Name].Equals(DBNull.Value))
                                property.SetValue(item, register[!string.IsNullOrEmpty(columns[0].Name) ? columns[0].Name : property.Name], null); 
                            //row[!string.IsNullOrEmpty(columns[0].Name) ? columns[0].Name : property.Name] =
                            //    property.GetValue(item, null);
                            continue;
                        }

                        // Key embedded object with column attribute.
                        if (!property.PropertyType.IsGenericType &&
                            !property.PropertyType.Equals(typeof(string)) && !property.PropertyType.IsValueType)
                        {
                            object embeddedObject = property.GetValue(item, null);

                            bool found = false;

                            foreach (PropertyInfo foreignProperty in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                            {
                                ColumnAttribute[] foreignColumns = (ColumnAttribute[])foreignProperty.GetCustomAttributes(typeof(ColumnAttribute), true);

                                if (foreignColumns.Length <= 0 || !foreignColumns[0].IsKey)
                                    continue;

                                found = true;
                                if (!register[columns[0].Name].Equals(DBNull.Value))
                                    foreignProperty.SetValue(embeddedObject, register[columns[0].Name], null);
                                //row[columns[0].Name] = foreignProperty.GetValue(embeddedObject, null);
                                break;
                            }

                            if (!found)
                                throw new InvalidOperationException(string.Format(
                                    "There's no specified key column for the object {0}.", property.Name));

                            continue;
                        }

                        // Key generic value type
                        if (property.PropertyType.IsGenericType && property.PropertyType.IsValueType)
                        {
                            object embeddedObject = property.GetValue(item, null);

                            bool found = false;

                            foreach (PropertyInfo genericProperty in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                            {
                                if (!genericProperty.Name.Equals("Key"))
                                    continue;

                                object key = genericProperty.GetValue(embeddedObject, null);

                                List<PropertyInfo> foreignProperties = new List<PropertyInfo>(
                                    genericProperty.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public));

                                PropertyInfo foreignProperty = foreignProperties.Find(delegate(PropertyInfo match)
                                {
                                    ColumnAttribute[] foreignColumns = (ColumnAttribute[])match.GetCustomAttributes(typeof(ColumnAttribute), true);

                                    if (foreignColumns.Length <= 0 || !foreignColumns[0].IsKey)
                                        return false;

                                    found = true;
                                    if (!register[genericProperty.PropertyType.Name + match.Name].Equals(DBNull.Value))
                                        match.SetValue(key, register[genericProperty.PropertyType.Name + match.Name], null);
                                    //row[genericProperty.PropertyType.Name + match.Name] = match.GetValue(key, null);
                                    return true;
                                });

                                if (found)
                                    break;
                            }

                            if (!found)
                                throw new InvalidOperationException(string.Format(
                                    "There's no specified key column for the object {0}.", property.Name));

                            continue;
                        }

                        throw new InvalidOperationException(
                            string.Format("Unhandled property for type{0}. Type unhandled: {1}", type.Name, property.Name));
                    }
                }
            }
        }

        private static void BuildDataCommand(IDataCommand cmd, DataTable table,   DataRow row)
        {
            List<DataColumn> primaryKeys = new List<DataColumn>(table.PrimaryKey);

            // Set primary keys parameters only.
            foreach (DataColumn column in primaryKeys)
            {
                // Special treatement for date time data type. 
                // DateTime.MinValue value differs from database's.
                if (column.DataType.Equals(typeof(DateTime)) &&
                    row[column.ColumnName].Equals(DateTime.MinValue))
                {
                    if (cmd.Parameters.Contains(column.ColumnName))
                        cmd.Parameters[column.ColumnName].Value = DBNull.Value;
                    continue;
                }
                else if (column.DataType.Equals(typeof(Type)))
                {
                    if (cmd.Parameters.Contains(column.ColumnName))
                        cmd.Parameters[column.ColumnName].Value = row[column.ColumnName].ToString();
                    continue;
                }

                if (cmd.Parameters.Contains(column.ColumnName))
                    cmd.Parameters[column.ColumnName].Value = row[column.ColumnName];
            }

            // Set all parameters, exept for primary keys.
            foreach (DataColumn column in table.Columns)
            {
                if (primaryKeys.Contains(column))
                    continue;

                // Special treatement for date time data type. 
                // DateTime.MinValue value differs from database's.
                if (column.DataType.Equals(typeof(DateTime)) &&
                    row[column.ColumnName].Equals(DateTime.MinValue))
                {
                    if (cmd.Parameters.Contains(column.ColumnName))
                        cmd.Parameters[column.ColumnName].Value = DBNull.Value;
                    continue;
                }
                else if (column.DataType.Equals(typeof(Type)))
                {
                    if (cmd.Parameters.Contains(column.ColumnName))
                        cmd.Parameters[column.ColumnName].Value = row[column.ColumnName].ToString();
                    continue;
                }

                if (cmd.Parameters.Contains(column.ColumnName))
                    cmd.Parameters[column.ColumnName].Value = row[column.ColumnName];
            }
        }

        private static void GetOutputParameters(IDataCommand cmd, DataTable table, DataRow row)
        {
            foreach (DataColumn column in table.Columns)
            {
                if (cmd.Parameters.Contains(column.ColumnName) && 
                    (cmd.Parameters[column.ColumnName].Direction == ParameterDirection.InputOutput ||
                    cmd.Parameters[column.ColumnName].Direction == ParameterDirection.Output ||
                    cmd.Parameters[column.ColumnName].Direction == ParameterDirection.ReturnValue))
                    row[column.ColumnName] = cmd.Parameters[column.ColumnName].Value;
            }
        }

        /// <summary>
        /// Executes the insert command for the object.
        /// </summary>
        /// <param name="item"></param>
        public static void ExecuteInsert(object item)
        {
            DataObjectHolder holder = DataObjectHolder.BuildContext(item);

            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                IDataSource datasource = !string.IsNullOrEmpty(holder.DataSourceInfo.DataSourceName) ?
                    DataSourceFactory.GetDataSource(holder.DataSourceInfo.DataSourceName) : DataSourceFactory.GetDataSource();

                IDataCommand cmd = datasource.GetCommand(holder.DataSourceInfo.InsertCommandName);

                foreach (DataRow row in holder.Table.Rows)
                {
                    DataObjectManager.BuildDataCommand(cmd, holder.Table, row);

                    int affectedRows = cmd.ExecuteNonQuery();

                    DataObjectManager.GetOutputParameters(cmd, holder.Table, row);
                }

                DataObjectManager.Current.SetObjectData(holder.Table, item);

                context.VoteCommit();
            }
        }

        /// <summary>
        /// Executes the update command for the object.
        /// </summary>
        /// <param name="item"></param>
        public static void ExecuteUpdate(object item)
        {
            DataObjectHolder holder = DataObjectHolder.BuildContext(item);

            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                IDataSource datasource = !string.IsNullOrEmpty(holder.DataSourceInfo.DataSourceName) ?
                    DataSourceFactory.GetDataSource(holder.DataSourceInfo.DataSourceName) : DataSourceFactory.GetDataSource();

                IDataCommand cmd = datasource.GetCommand(holder.DataSourceInfo.UpdateCommandName);

                foreach (DataRow row in holder.Table.Rows)
                {
                    DataObjectManager.BuildDataCommand(cmd, holder.Table, row);

                    int affectedRows = cmd.ExecuteNonQuery();

                    DataObjectManager.GetOutputParameters(cmd, holder.Table, row);
                }

                context.VoteCommit();
            }
        }

        /// <summary>
        /// Executes the delete command for the object.
        /// </summary>
        /// <param name="item"></param>
        public static void ExecuteDelete(object item)
        {
            DataObjectHolder holder = DataObjectHolder.BuildContext(item);

            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                IDataSource datasource = !string.IsNullOrEmpty(holder.DataSourceInfo.DataSourceName) ?
                    DataSourceFactory.GetDataSource(holder.DataSourceInfo.DataSourceName) : DataSourceFactory.GetDataSource();

                IDataCommand cmd = datasource.GetCommand(holder.DataSourceInfo.DeleteCommandName);

                foreach (DataRow row in holder.Table.Rows)
                {
                    DataObjectManager.BuildDataCommand(cmd, holder.Table, row);

                    int affectedRows = cmd.ExecuteNonQuery();

                    DataObjectManager.GetOutputParameters(cmd, holder.Table, row);
                }

                context.VoteCommit();
            }
        }

        /// <summary>
        /// Returns an instance of the object requested.
        /// </summary>
        /// <param name="item">Type of object</param>
        /// <param name="keys">Parameters for construction.</param>
        /// <returns></returns>
        public static object ExecuteSelect(Type item, object[] keys)
        {
            object instance = Activator.CreateInstance(item, keys);

            DataObjectHolder holder = DataObjectHolder.BuildContext(instance);

            using (TransactionContextFactory.EnterContext(TransactionAffinity.Supported))
            {
                IDataSource datasource = !string.IsNullOrEmpty(holder.DataSourceInfo.DataSourceName) ?
                    DataSourceFactory.GetDataSource(holder.DataSourceInfo.DataSourceName) : DataSourceFactory.GetDataSource();

                IDataCommand cmd = datasource.GetCommand(holder.DataSourceInfo.SelectCommandName);

                foreach (DataRow row in holder.Table.Rows)
                {
                    DataObjectManager.BuildDataCommand(cmd, holder.Table, row);

                    IDataReader reader = cmd.ExecuteReader();

                    DataObjectManager.GetOutputParameters(cmd, holder.Table, row);

                    try
                    {
                        holder.Table.Clear();

                        if (!reader.Read())
                            return null;

                        DataRow register = holder.Table.NewRow();

                        foreach (DataColumn column in holder.Table.Columns)
                        {
                            if (column.DataType.Equals(typeof(Type)))
                            {
                                register[column.ColumnName] = Assembly.GetCallingAssembly().GetType(
                                    reader[column.ColumnName].ToString());
                                continue;
                            }

                            if (reader[column.ColumnName] != DBNull.Value)
                                register[column.ColumnName] = reader[column.ColumnName];
                        }

                        holder.Table.Rows.Add(register);
                    }
                    finally
                    {
                        if (!reader.IsClosed)
                            reader.Close();
                    }

                    // Has to be found one row at most
                    break;
                }
            }

            DataObjectManager.Current.SetObjectData(holder.Table, instance);

            return instance;
        }

        /// <summary>
        /// Returns an instance of a dataset.with the type of the object requested.
        /// </summary>
        /// <param name="item">Type of object</param>
        /// <param name="keys">Parameters</param>
        /// <returns></returns>
        public static DataSet GetDataSet(Type item, object[] keys)
        {
            DataSet ds = new DataSet();

            object instance = Activator.CreateInstance(item, keys);

            DataObjectHolder holder = DataObjectHolder.BuildContext(item);

            using (TransactionContextFactory.EnterContext(TransactionAffinity.Supported))
            {
                IDataSource datasource = !string.IsNullOrEmpty(holder.DataSourceInfo.DataSourceName) ?
                    DataSourceFactory.GetDataSource(holder.DataSourceInfo.DataSourceName) : DataSourceFactory.GetDataSource();

                IDataSetAdapter adapter = datasource.GetDataSetAdapter(holder.DataSourceInfo.DataSetAdapterName);

                adapter.PopulateCommands();

                foreach (DataRow row in holder.Table.Rows)
                {
                    // Set primary keys parameters only.
                    foreach (DataColumn column in holder.Table.PrimaryKey)
                        if (row[column.ColumnName] != null)
                            adapter.SelectCommand.Parameters[column.ColumnName].Value = row[column.ColumnName];

                    int count = adapter.Fill(ds);
                }
            }

            return ds;
        }
    }
}
