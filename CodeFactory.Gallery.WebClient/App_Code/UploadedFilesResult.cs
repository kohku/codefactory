using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Gallery.Core;
using CodeFactory.Gallery.Core.Providers;
using CodeFactory.Web.Storage;

/// <summary>
/// Summary description for UploadedFilesResult
/// </summary>
[DataObject]
[Serializable]
public class UploadedFilesResult
{
    private Gallery _gallery;

    public UploadedFilesResult(Gallery gallery)
    {
        if (gallery == null)
            throw new ArgumentNullException("gallery");

        this._gallery = gallery;
    }

    [DataObjectMethod(DataObjectMethodType.Insert, true)]
    public void InsertFile(UploadedFile item)
    {
        UploadedFile replacement = (UploadedFile)UploadStorageService.CreateFile4Storage(item.ContentType);

        replacement.ID = item.ID;           
        replacement.FileName = item.FileName;
        replacement.Description = item.Description;
        replacement.ContentLength = item.ContentLength;
        replacement.InputStream = item.InputStream;

        // Adds the new image to the current list.
        //replacement.Save();
        // It is saved before adding it to the gallery in order to insert 
        // an empty guid in the projectId for the storage provider.
        _gallery.AddFile(replacement);

        _gallery.AcceptChanges();
    }

    [DataObjectMethod(DataObjectMethodType.Update, true)]
    public void UpdateFile(UploadedFile item)
    {
        UploadedFile file = _gallery.Files.Find(delegate(UploadedFile match)
        {
            if (match.ID.Equals(item.ID))
                return true;
            return false;
        });

        if (file != null)
        {
            file.FileName = item.FileName;
            file.Description = item.Description;
        }
    }

    [DataObjectMethod(DataObjectMethodType.Delete, true)]
    public void DeleteFile(UploadedFile item)
    {
        UploadedFile file = _gallery.Files.Find(delegate(UploadedFile match)
        {
            if (match.ID.Equals(item.ID))
                return true;
            return false;
        });

        if (file != null)
        {
            // Removes the image from the current list.
            file.Delete();
            file.Save();

            _gallery.RemoveFile(file);
        }
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<UploadedFile> GetFiles()
    {
        return GetFiles(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<UploadedFile> GetFiles(int maximumRows, int startRowIndex)
    {
        List<UploadedFile> results = new List<UploadedFile>();

        if (startRowIndex < 0)
            return results;

        for (int index = startRowIndex; index < startRowIndex + maximumRows && index < _gallery.Files.Count; index++)
            results.Add(_gallery.Files[index]);

        return results;
    }

    public int TotalCount()
    {
        return _gallery.Files.Count;
    }

    public List<UploadedFile> Files
    {
        get { return _gallery.Files; }
    }
}
