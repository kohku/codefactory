using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CodeFactory.Web.Storage;
using System.ComponentModel;
using CodeFactory.Wiki;


/// <summary>
/// Summary description for FilesResult
/// </summary>
[DataObject]
public class FilesResult
{
    private IWiki wiki;

    public FilesResult(IWiki content)
    {
        wiki = content;
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<UploadedFile> GetFiles()
    {
        return wiki.Files.FindAll(delegate(UploadedFile file)
        {
            return !file.IsDeleted;
        });
    }

    [DataObjectMethod(DataObjectMethodType.Delete)]
    public void DeleteFile(Guid id)
    {
        UploadedFile file = wiki.Files.Find(delegate(UploadedFile match)
        {
            return match.ID.Equals(id);
        });

        if (file != null)
            wiki.RemoveFile(file);
    }
}
