using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Gallery.Core;
using CodeFactory.Gallery.Core.Providers;

/// <summary>
/// Summary description for ProjectsResult
/// </summary>
[Serializable]
public class GalleriesResult
{
    protected List<Gallery> galleries;

    private Guid? id = null;
    private string userId = null;
    private bool? visible = null;
    private string author = null;
    private string title = null;
    private GalleryStatus? status = null;
    private DateTime? initialDateCreated = null;
    private DateTime? finalDateCreated = null;
    private DateTime? initialDateModified = null;
    private DateTime? finalDateModified = null;
    private int totalCount;

    public GalleriesResult(string userId, string author, string title, GalleryStatus? status, 
        DateTime? initialDateCreated, DateTime? finalDateCreated,
        DateTime? initialDateModified, DateTime? finalDateModified)
    {
        this.userId = userId;
        this.author = author;
        this.title = title;
        this.status = status;
        this.initialDateCreated = initialDateCreated;
        this.finalDateCreated = finalDateCreated;
        this.initialDateModified = initialDateModified;
        this.finalDateModified = finalDateModified;
    }

    public List<Gallery> GetProjects()
    {
        return GetProjects(int.MaxValue, 0);
    }

    public List<Gallery> GetProjects(int maximumRows, int startRowIndex)
    {
        List<Guid> identifiers = GalleryManagementService.GetGalleryList(
            id,
            userId,
            visible,
            author,
            title,
            status,
            maximumRows,
            (maximumRows > 0 ? startRowIndex / maximumRows : 0),
            out totalCount);

        List<Gallery> galleries = GalleryManagementService.GetGalleries(identifiers, false);

        HttpContext.Current.Items["ProjectsResult_TotalCount"] = totalCount;

        return galleries;
    }

    public int TotalCount()
    {
        return (int)HttpContext.Current.Items["ProjectsResult_TotalCount"];
    }

    public List<Gallery> Projects
    {
        get { return galleries; }
    }
}
