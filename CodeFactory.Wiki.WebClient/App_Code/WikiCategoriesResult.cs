using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using CodeFactory.Wiki;

/// <summary>
/// Summary description for WikiCategoriesResult
/// </summary>
[DataObject]
public class WikiCategoriesResult
{
    private bool _manage;

    public WikiCategoriesResult()
    {
    }

    public WikiCategoriesResult(bool manage)
    {
        _manage = manage;
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public Dictionary<string, string> GetCategories()
    {
        Dictionary<string, string> categories = new Dictionary<string, string>();

        if (!_manage)
            categories.Add("Seleccione...", string.Empty);

        foreach (string category in WikiService.GetCategories())
            categories.Add(category, category);

        if (!_manage)
            categories.Add("Otra", "Other");

        return categories;
    }

    [DataObjectMethod(DataObjectMethodType.Delete)]
    public bool DeleteCategory(string category)
    {
        return WikiService.DeleteCategory(category);
    }
}
