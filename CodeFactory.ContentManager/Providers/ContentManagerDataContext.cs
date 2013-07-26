using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace CodeFactory.ContentManager.Providers
{
    public class ContentManagerDataContext : DataContext
    {
        public ContentManagerDataContext(string fileOrServerConnection)
            : base(fileOrServerConnection)
        {
        }

        [Function(Name = "dbo.ContentManager_GetCategories")]
        public ISingleResult<GetCategoriesResult> GetCategories(
            [Parameter(Name = "ApplicationName", DbType = "NVarChar(512)")] string applicationName,
            [Parameter(Name = "ID", DbType = "UniqueIdentifier")] Nullable<Guid> id,
            [Parameter(Name = "Name", DbType = "NVarChar(512)")] string name,
            [Parameter(Name = "ParentID", DbType = "UniqueIdentifier")] Nullable<Guid> parentId,
            [Parameter(Name = "FirstIndex", DbType = "Int")] Nullable<int> firstIndex,
            [Parameter(Name = "LastIndex", DbType = "Int")] Nullable<int> lastIndex,
            [Parameter(Name = "TotalCount", DbType = "Int")] ref Nullable<int> totalCount)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this,
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                applicationName,
                id,
                name,
                parentId,
                firstIndex,
                lastIndex,
                totalCount);
            totalCount = ((Nullable<int>)(result.GetParameterValue(6)));
            return ((ISingleResult<GetCategoriesResult>)(result.ReturnValue));
        }

        [Function(Name = "dbo.ContentManager_GetSections")]
        public ISingleResult<GetSectionsResult> GetSections(
            [Parameter(Name = "ApplicationName", DbType = "NVarChar(512)")] string applicationName,
            [Parameter(Name = "ID", DbType = "UniqueIdentifier")] Nullable<Guid> id,
            [Parameter(Name = "Name", DbType = "NVarChar(512)")] string name,
            [Parameter(Name = "Slug", DbType = "NVarChar(512)")] string slug,
            [Parameter(Name = "IsVisible", DbType = "Bit")] Nullable<bool> isVisible,
            [Parameter(Name = "ParentID", DbType = "UniqueIdentifier")] Nullable<Guid> parentId,
            [Parameter(Name = "FirstIndex", DbType = "Int")] Nullable<int> firstIndex,
            [Parameter(Name = "LastIndex", DbType = "Int")] Nullable<int> lastIndex,
            [Parameter(Name = "TotalCount", DbType = "Int")] ref Nullable<int> totalCount)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this,
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                applicationName,
                id,
                name,
                slug,
                isVisible,
                parentId,
                firstIndex,
                lastIndex,
                totalCount);
            totalCount = ((Nullable<int>)(result.GetParameterValue(8)));
            return ((ISingleResult<GetSectionsResult>)(result.ReturnValue));
        }
    }

    public class GetCategoriesResult
    {
        private Guid _ID;

        public GetCategoriesResult()
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

    public class GetSectionsResult
    {
        private Guid _ID;

        public GetSectionsResult()
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

