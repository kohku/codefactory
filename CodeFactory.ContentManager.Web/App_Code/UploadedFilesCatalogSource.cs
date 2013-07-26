using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using CodeFactory.Web.Storage;

/// <summary>
/// Summary description for UploadedFiles
/// </summary>
public class UploadedFilesCatalogSource
{
    private Guid? _id;
    private Guid? _parentId;
    private string _filename;
    private DateTime? _initialDateCreated;
    private DateTime? _finalDateCreated;
    private DateTime? _initialLastUpdated;
    private DateTime? _finalLastUpdated;
    private string _contentType;

    public UploadedFilesCatalogSource(Guid? id, Guid? parentId, string fileName,
            DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated,
            string contentType)
    {
        _id = id;
        _parentId = parentId;
        _filename = fileName;
        _initialDateCreated = initialDateCreated;
        _finalDateCreated = finalDateCreated;
        _initialLastUpdated = initialLastUpdated;
        _finalLastUpdated = finalLastUpdated;
        _contentType = contentType;
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<UploadedFile> GetCatalog()
    {
        return GetCatalog(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<UploadedFile> GetCatalog(int maximumRows, int startRowIndex)
    {
        int totalCount;

        List<UploadedFile> catalog = UploadStorageService.GetFiles(
            _id, _parentId, _filename, _initialDateCreated, _finalDateCreated, _initialLastUpdated, _finalLastUpdated, _contentType, 
            false, maximumRows > 0 ? maximumRows : int.MaxValue, (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount);

        HttpContext.Current.Items["UploadedFiles_TotalCount"] = totalCount;

        return catalog;
    }

    public int TotalCount()
    {
        return TotalCount(int.MaxValue, 0);
    }

    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["UploadedFiles_TotalCount"];
    }

    [DataObjectMethod(DataObjectMethodType.Insert, true)]
    public void InsertFile(UploadedFile file)
    {
    }

    [DataObjectMethod(DataObjectMethodType.Update, true)]
    public void UpdateFile(UploadedFile file)
    {
    }

    [DataObjectMethod(DataObjectMethodType.Delete, true)]
    public void DeleteFile(UploadedFile file)
    {
    }
}
