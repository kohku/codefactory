using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using CodeFactory.ContentManager;

/// <summary>
/// Summary description for CategoriesSource
/// </summary>
[DataObject]
public class CategoriesSource
{
    private Guid? _id;
    private string _name;
    private Guid? _parentId;

    public CategoriesSource()
    {
    }

    public CategoriesSource(Guid? parentId)
    {
        _parentId = parentId;
    }

    public CategoriesSource(Guid? id, string name, Guid? parentId)
    {
        _id = id;
        _name = name;
        _parentId = parentId;
    }

    [DataObjectMethod(DataObjectMethodType.Insert, true)]
    public void InsertCategory(string name, Guid? parentId)
    {
        Category p = null;

        if (parentId.HasValue)
            p = Category.Load(parentId.Value);

        Category c = new Category();

        if (c == null)
            return;

        c.Name = name;

        if (p != null)
            p.AddChild(c);

        c.Save();
    }

    [DataObjectMethod(DataObjectMethodType.Update, true)]
    public void UpdateCategory(Guid id, string name, Guid? parentId)
    {
        Category p = null;

        if (parentId.HasValue)
            p = Category.Load(parentId.Value);

        Category c = Category.Load(id);

        if (c == null)
            return;

        c.Name = name;

        if (p != null)
            p.AddChild(c);

        c.Save();
    }

    [DataObjectMethod(DataObjectMethodType.Delete, true)]
    public void DeleteCategory(Guid id)
    {
        Category c = Category.Load(id);

        if (c == null)
            return;

        c.Delete();

        c.Save();
    }

    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<Category> GetCategories()
    {
        return GetCategories(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<Category> GetCategories(int maximumRows, int startRowIndex)
    {
        int totalCount = 0;

        List<Category> items = new List<Category>();

        foreach (Category item in ContentManagementService.GetCategories(_id, _name, _parentId,
            maximumRows > 0 ? maximumRows : int.MaxValue, (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount))
            items.Add(item);

        HttpContext.Current.Items["CategoriesSource_TotalCount"] = totalCount;

        return items;
    }

    public int TotalCount()
    {
        return TotalCount(int.MaxValue, 0);
    }

    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["CategoriesSource_TotalCount"];
    }

    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<Category> GetChildCategories()
    {
        return GetChildCategories(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<Category> GetChildCategories(int maximumRows, int startRowIndex)
    {
        int totalCount = 0;

        List<Category> items = new List<Category>();

        foreach (Category item in ContentManagementService.GetChildCategories(_parentId, maximumRows,
            (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount))
            items.Add(item);

        HttpContext.Current.Items["ChildCategoriesSource_TotalCount"] = totalCount;

        return items;
    }

    public int TotalChildCount()
    {
        return TotalChildCount(int.MaxValue, 0);
    }

    public int TotalChildCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["ChildCategoriesSource_TotalCount"];
    }
}

