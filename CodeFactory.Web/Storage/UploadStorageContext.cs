using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace CodeFactory.Web.Storage
{
    public class UploadStorageDataContext : DataContext
    {
        public UploadStorageDataContext(string fileOrServerConnection)
            : base(fileOrServerConnection)
        {
        }

        [Function(Name = "dbo.UploadStorage_GetFiles")]
        public ISingleResult<GetFilesResult> GetFiles(
            [Parameter(Name = "ApplicationName", DbType = "NVarChar(512)")] string applicationName,
            [Parameter(Name = "ID", DbType = "UniqueIdentifier")] Nullable<Guid> id,
            [Parameter(Name = "ContentType", DbType = "NVarChar(50)")] string contentType,
            [Parameter(Name = "InitialDateCreated",DbType = "DateTime")] Nullable<DateTime> initialDateCreated,
            [Parameter(Name = "FinalDateCreated", DbType = "DateTime")] Nullable<DateTime> finalDateCreated,
            [Parameter(Name = "FileName", DbType = "NVarChar(1024)")] string fileName,
            [Parameter(Name = "InitialLastUpdated", DbType = "DateTime")] Nullable<DateTime> initialLastUpdated,
            [Parameter(Name = "FinalLastUpdated", DbType = "DateTime")] Nullable<DateTime> finalLastUpdated,
            [Parameter(Name = "ParentId", DbType = "UniqueIdentifier")] Nullable<Guid> parentId,
            [Parameter(Name = "FirstIndex", DbType = "Int")] Nullable<int> firstIndex,
            [Parameter(Name = "LastIndex", DbType = "Int")] Nullable<int> lastIndex,
            [Parameter(Name = "TotalCount", DbType = "Int")] ref Nullable<int> totalCount)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())), 
                applicationName, 
                id, 
                contentType, 
                initialDateCreated, 
                finalDateCreated, 
                fileName, 
                initialLastUpdated, 
                finalLastUpdated, 
                parentId, 
                firstIndex, 
                lastIndex, 
                totalCount);
            totalCount = ((Nullable<int>)(result.GetParameterValue(11)));
            return ((ISingleResult<GetFilesResult>)(result.ReturnValue));
        }
    }

    public class GetFilesResult
    {
        private Guid _ID;

        public GetFilesResult()
        {
        }

        [Column(Storage = "_ID", DbType = "UniqueIdentifier NOT NULL")]
        public Guid ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                if ((this._ID != value))
                {
                    this._ID = value;
                }
            }
        }
    }
}
