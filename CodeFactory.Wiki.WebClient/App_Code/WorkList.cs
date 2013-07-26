using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using CodeFactory.Wiki.Workflow;
using CodeFactory.Wiki;

/// <summary>
/// Summary description for WorkItemsResult
/// </summary>
[DataObject]
public class WorkList
{
    public WorkList()
    {
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<IWorkWikiItem> GetWorkList()
    {
        return GetWorkList(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<IWorkWikiItem> GetWorkList(int maximumRows, int startRowIndex)
    {
        int totalCount;

        List<IWorkWikiItem> workitems = WikiService.GetPendingAuthorizations(
            HttpContext.Current.User.IsInRole("Administrator") ? string.Empty :
            HttpContext.Current.User.Identity.Name, maximumRows,
            (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount);

        HttpContext.Current.Items["Worklist_TotalCount"] = totalCount;

        return workitems;
    }

    public int TotalCount()
    {
        return (int)HttpContext.Current.Items["Worklist_TotalCount"];
    }


    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["Worklist_TotalCount"];
    }
}
