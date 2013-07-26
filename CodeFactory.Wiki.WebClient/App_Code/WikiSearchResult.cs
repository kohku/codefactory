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
using System.Windows.Forms;
using System.ComponentModel;
using CodeFactory.Wiki;

/// <summary>
/// Summary description for WikiSearchResult
/// </summary>
[DataObject]
public class WikiSearchResult
{
    private string _query;
    private string _category;
    private string _keywords;

    public WikiSearchResult()
    {
        _query = string.Empty;
    }

    public WikiSearchResult(string query, string category, string keywords)
    {
        _query = query;
        _category = category;
        _keywords = keywords;
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<Wiki> GetResults()
    {
        return GetResults(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<Wiki> GetResults(int maximumRows, int startRowIndex)
    {
        int totalCount;

        List<Wiki> results = new List<Wiki>();

        System.Security.Principal.IIdentity user = HttpContext.Current.User.Identity;

        List<Guid> articles = WikiService.SearchWiki(
        _query, _query, string.Empty, string.Empty, _query, _category, _keywords,
        user.IsAuthenticated ? ReachLevel.Intranet : ReachLevel.Internet,
        maximumRows, (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount);

        foreach (Guid id in articles)
        {
            Wiki item = Wiki.Load(id);

            if (item != null)
                results.Add(item);
        }

        HttpContext.Current.Items["WikiSearchResult_TotalCount"] = totalCount;

        return results;
    }

    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["WikiSearchResult_TotalCount"];
    }
}
