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
using CodeFactory.Gallery.Core.Providers;

/// <summary>
/// Summary description for CommentsResult
/// </summary>
[Serializable]
public class CommentsResult
{
    protected Gallery _gallery;

    public CommentsResult(Gallery gallery)
    {
        if (gallery == null)
            throw new ArgumentNullException("gallery");

        this._gallery = gallery;
    }

    [DataObjectMethod(DataObjectMethodType.Insert)]
    public void InsertComment(Comment item)
    {
        if (item == null)
            throw new ArgumentNullException("item");

        item.Author = HttpContext.Current.User.Identity.Name;
        item.IPAddress = HttpContext.Current.Request.UserHostAddress;

        _gallery.AddComment(item);

        _gallery.AcceptChanges();
    }

    [DataObjectMethod(DataObjectMethodType.Update)]
    public void UpdateComment(Comment item)
    {
        if (item == null)
            throw new ArgumentNullException("item");

        item.IPAddress = HttpContext.Current.Request.UserHostAddress;
        //item.LastUpdatedBy = HttpContext.Current.User.Identity.Name;

        _gallery.UpdateComment(item);

        _gallery.AcceptChanges();
    }

    [DataObjectMethod(DataObjectMethodType.Delete)]
    public void DeleteComment(Comment item)
    {
        if (item == null)
            throw new ArgumentNullException("item");

        _gallery.RemoveComment(item);

        _gallery.AcceptChanges();
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<Comment> GetComments()
    {
        return GetComments(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<Comment> GetComments(int maximumRows, int startRowIndex)
    {
        List<Comment> results = new List<Comment>();

        if (startRowIndex < 0)
            return results;

        for (int index = startRowIndex; index < startRowIndex + maximumRows && index < _gallery.Comments.Count; index++)
            results.Add(_gallery.Comments[index]);

        return results;
    }

    public int TotalCount()
    {
        return _gallery.Comments.Count;
    }

    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return _gallery.Comments.Count;
    }

    public List<Comment> Comments
    {
        get { return _gallery.Comments; }
    }
}
