using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Gallery.Core;

/// <summary>
/// Summary description for ProjectUsersResult
/// </summary>
[Serializable]
public class ProjectUsersResult
{
    private Gallery _gallery;
    public static string Message = string.Empty;

    public ProjectUsersResult(Gallery gallery)
    {
        this._gallery = gallery;
    }

    [DataObjectMethod(DataObjectMethodType.Insert)]
    public void InsertUser(string user)
    {
        _gallery.AddUser(user);

        _gallery.AcceptChanges();
    }

    [DataObjectMethod(DataObjectMethodType.Delete)]
    public void DeleteUser(string user)
    {
        _gallery.RemoveUser(user);

        _gallery.AcceptChanges();
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<string> GetUsers()
    {
        return _gallery.Users;
    }

    public int TotalCount()
    {
        return _gallery.Users.Count; ;
    }
}
