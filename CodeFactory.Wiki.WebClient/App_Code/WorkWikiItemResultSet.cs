using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using CodeFactory.Wiki.Workflow;
using CodeFactory.Wiki;

/// <summary>
/// Summary description for WikiHistoryResult
/// </summary>
[DataObject]
public class WorkWikiItemResultSet
{
    private Guid _id;
    private string _title;
    private string _description;
    private string _content;
    private string _author;
    private string _slug;
    private bool? _isVisible;
    private string _category;
    private string _keywords;
    private DateTime _initialDateCreated;
    private DateTime _finalDateCreated;
    private DateTime _initialDateModified;
    private DateTime _finalDateModified;
    private DateTime _initialExpirationDate;
    private DateTime _finalExpirationDate;
    private string _lastModifiedBy;

    public WorkWikiItemResultSet(Guid id, string title, string description, string content, string author,
        string slug, bool? isVisible, string category, string keywords, DateTime initialDateCreated,
        DateTime finalDateCreated, DateTime initialDateModified, DateTime finalDateModified,
        DateTime initialExpirationDate, DateTime finalExpirationDate, string lastModifiedBy)
    {
        _id = id;
        _title = title;
        _description = description;
        _content = content;
        _author = author;
        _slug = slug;
        _isVisible = isVisible;
        _category = category;
        _keywords = keywords;
        _initialDateCreated = initialDateCreated;
        _finalDateCreated = finalDateCreated;
        _initialDateModified = initialDateModified;
        _finalDateModified = finalDateModified;
        _lastModifiedBy = lastModifiedBy;
        _initialExpirationDate = initialExpirationDate;
        _finalExpirationDate = finalExpirationDate;
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<WorkWikiItem> GetResults()
    {
        return GetResults(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<WorkWikiItem> GetResults(int maximumRows, int startRowIndex)
    {
        List<WorkWikiItem> results = new List<WorkWikiItem>();

        int totalCount = 0;

        List<Guid> items = WikiService.GetWorkWikiItem(null,
            _id, _title, _description, _content, _author, _slug, _isVisible, _category,
            _keywords, null, _initialDateCreated, _finalDateCreated, _initialDateModified, _finalDateModified,
            _initialExpirationDate, _finalExpirationDate, _lastModifiedBy,
            maximumRows > 0 ? maximumRows : int.MaxValue,
            (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount);

        HttpContext.Current.Items["WorkWikiItemResultSet_TotalCount"] = totalCount;

        foreach (Guid id in items)
            results.Add(WorkWikiItem.Load(id));

        return results;
    }

    public int TotalCount()
    {
        return TotalCount(int.MaxValue, 0);
    }

    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["WorkWikiItemResultSet_TotalCount"];
    }
}
