using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace CodeFactory.Gallery.Core.Providers
{
    public class GalleryDataContext : DataContext
    {
        // TODO: Quizás hacer esto un singletón
        // TODO: Añadir las tablas de Gallery, Comments, etc....
        public GalleryDataContext(string fileOrServerConnection)
            : base(fileOrServerConnection)
        {
        }

        [Function(Name = "dbo.GalleryManagement_GetGalleries")]
        public ISingleResult<GetGalleriesResult> GetGalleries(
            [Parameter(Name = "ApplicationName", DbType = "NVarChar(512)")] string applicationName,
            [Parameter(Name = "ID", DbType = "UniqueIdentifier")] Nullable<Guid> id,
            [Parameter(Name = "Author", DbType = "NVarChar(512)")] string author,
            [Parameter(Name = "InitialDateCreated", DbType = "DateTime")] Nullable<DateTime> initialDateCreated,
            [Parameter(Name = "FinalDateCreated", DbType = "DateTime")] Nullable<DateTime> finalDateCreated,
            [Parameter(Name = "Description", DbType = "NVarChar(512)")] string description,
            [Parameter(Name = "IsVisible", DbType = "Bit")] Nullable<bool> isVisible,
            [Parameter(Name = "Keywords", DbType = "NVarChar(1024)")] string keywords,
            [Parameter(Name = "InitialLastUpdated", DbType = "DateTime")] Nullable<DateTime> initialLastUpdated,
            [Parameter(Name = "FinalLastUpdated", DbType = "DateTime")] Nullable<DateTime> finalLastUpdated,
            [Parameter(Name = "LastUpdatedBy", DbType = "NVarChar(512)")] string lastUpdatedBy,
            [Parameter(Name = "Slug", DbType = "NVarChar(512)")] string slug,
            [Parameter(Name = "Title", DbType = "NVarChar(512)")] string title,
            [Parameter(Name = "Status", DbType = "NVarChar(50)")] string status,
            [Parameter(Name = "FirstIndex", DbType = "Int")] Nullable<int> firstIndex,
            [Parameter(Name = "LastIndex", DbType = "Int")] Nullable<int> lastIndex,
            [Parameter(Name = "TotalCount", DbType = "Int")] ref Nullable<int> totalCount)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this,
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                applicationName,
                id,
                author,
                initialDateCreated,
                finalDateCreated,
                description,
                isVisible,
                keywords,
                initialLastUpdated,
                finalLastUpdated,
                lastUpdatedBy,
                slug,
                title,
                status,
                firstIndex,
                lastIndex,
                totalCount);
            totalCount = ((Nullable<int>)(result.GetParameterValue(16)));
            return ((ISingleResult<GetGalleriesResult>)(result.ReturnValue));
        }
    }

    public class GetGalleriesResult
    {
        private Guid _ID;

        public GetGalleriesResult()
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
