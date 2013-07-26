using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using CodeFactory.Wiki;

/// <summary>
/// Summary description for WikiResult
/// </summary>
[DataObject]
public class WikiResult
{
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
    private string _lastModifiedBy;

    public WikiResult(string title, string description, string content, string author,
        string slug, bool? isVisible, string category, string keywords, DateTime initialDateCreated,
        DateTime finalDateCreated, DateTime initialDateModified, DateTime finalDateModified, string lastModifiedBy)
    {
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
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<Wiki> GetResults()
    {
        return GetResults(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<Wiki> GetResults(int maximumRows, int startRowIndex)
    {
        List<Wiki> results = new List<Wiki>();

        int totalCount = 0;

        List<Guid> items = WikiService.GetWiki(null,
            _title, _description, _content, _author, _slug, _isVisible, _category,
            _keywords, null, _initialDateCreated, _finalDateCreated, _initialDateModified, _finalDateModified, _lastModifiedBy,
            maximumRows > 0 ? maximumRows : int.MaxValue,
            (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount);

        HttpContext.Current.Items["WikiResultSet_TotalCount"] = totalCount;

        foreach (Guid id in items)
            results.Add(Wiki.Load(id));

        return results;
    }

    public int TotalCount()
    {
        return TotalCount(int.MaxValue, 0);
    }

    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["WikiResultSet_TotalCount"];
    }
}
