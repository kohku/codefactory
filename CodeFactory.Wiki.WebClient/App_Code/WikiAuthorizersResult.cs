using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using CodeFactory.Wiki;

/// <summary>
/// Summary description for WikiAuthorizersResult
/// </summary>
[DataObject]
public class WikiAuthorizersResult
{
    public WikiAuthorizersResult()
    {
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public Dictionary<string, string> GetWikiAuthorizers()
    {
        return GetWikiAuthorizers(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public Dictionary<string, string> GetWikiAuthorizers(int maximumRows, int startRowIndex)
    {
        int totalCount;

        Dictionary<string, string> authorizers = WikiService.GetAuthorizersByCategory(
            maximumRows, (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount);

        HttpContext.Current.Items["WikiAuthorizers_TotalCount"] = totalCount;

        return authorizers;
    }

    public int TotalCount()
    {
        return TotalCount(int.MaxValue, 0);
    }
    
    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["WikiAuthorizers_TotalCount"];
    }

    [DataObjectMethod(DataObjectMethodType.Delete, true)]
    public void DeleteWikiAuthorizer(string key)
    {
        WikiService.DeleteAuthorizerByCategory(key);
    }
}
