using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using CodeFactory.ContentManager;

/// <summary>
/// Summary description for SectionsSource
/// </summary>
[DataObject]
public class SectionsSource
{
    private Guid? _id;
    private string _name;
    private string _slug;
    private bool? _isVisible;
    private Guid? _parentId;

    public SectionsSource()
    {
    }

    public SectionsSource(Guid? parentId)
    {
        _parentId = parentId;
    }

    public SectionsSource(Guid? id, string name, string slug, bool? isVisible, Guid? parentId)
    {
        _id = id;
        _name = name;
        _slug = slug;
        _isVisible = isVisible;
        _parentId = parentId;
    }

    [DataObjectMethod(DataObjectMethodType.Insert, true)]
    public void InsertSection(string name, string slug, int index, bool isVisible, Guid? parentId)
    {
        Section p = null;

        if (parentId.HasValue)
            p = Section.Load(parentId.Value);

        Section s = new Section();

        if (s == null)
            return;

        s.Name = name;
        s.Slug = slug;
        s.Index = index;
        s.IsVisible = isVisible;

        if (p != null)
            p.AddChild(s);

        s.Save();
    }

    [DataObjectMethod(DataObjectMethodType.Update, true)]
    public void UpdateSection(Guid id, string name, string slug, int index, bool isVisible, Guid? parentId)
    {
        Section p = null;

        if (parentId.HasValue)
            p = Section.Load(parentId.Value);

        Section s = Section.Load(id);

        if (s == null)
            return;

        s.Name = name;
        s.Slug = slug;
        s.Index = index;
        s.IsVisible = isVisible;

        if (p != null)
            p.AddChild(s);

        s.Save();
    }

    [DataObjectMethod(DataObjectMethodType.Delete, true)]
    public void DeleteSection(Guid id)
    {
        Section s = Section.Load(id);

        if (s == null)
            return;

        s.Delete();

        s.Save();
    }

    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<ISection> GetSections()
    {
        return GetSections(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<ISection> GetSections(int maximumRows, int startRowIndex)
    {
        int totalCount = 0;

        List<ISection> items = ContentManagementService.GetSections(_id, _name, _slug, _isVisible, _parentId,
            maximumRows > 0 ? maximumRows : int.MaxValue, (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount);

        HttpContext.Current.Items["SectionsSource_TotalCount"] = totalCount;

        return items;
    }

    public int TotalCount()
    {
        return TotalCount(int.MaxValue, 0);
    }

    public int TotalCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["SectionsSource_TotalCount"];
    }

    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<ISection> GetChildSections()
    {
        return GetChildSections(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<ISection> GetChildSections(int maximumRows, int startRowIndex)
    {
        int totalCount = 0;

        List<ISection> items = ContentManagementService.GetChildSections(_parentId, maximumRows,
            (maximumRows > 0 ? startRowIndex / maximumRows : 0), out totalCount);

        HttpContext.Current.Items["ChildSectionsSource_TotalCount"] = totalCount;

        return items;
    }

    public int TotalChildCount()
    {
        return TotalChildCount(int.MaxValue, 0);
    }

    public int TotalChildCount(int maximumRows, int startRowIndex)
    {
        return (int)HttpContext.Current.Items["ChildSectionsSource_TotalCount"];
    }
}
