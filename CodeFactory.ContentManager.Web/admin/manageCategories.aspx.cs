using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.ContentManager;

public partial class admin_manageCategories : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CategoryTreeView.DataBind();

            UpdateView();
        }
    }

    protected void CategoryTreeView_SelectedNodeChanged(object sender, EventArgs e)
    {
        CategoryGridView.DataBind();

        UpdateView();
    }

    protected void CategoryDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        ICategory item = null;

        if (CategoryTreeView.SelectedNode != null)
            item = ContentManagementService.GetCategory(CategoryTreeView.SelectedNode.DataPath);

        if (item != null)
            e.ObjectInstance = new CategoriesSource(item.ID);
        else
            e.ObjectInstance = new CategoriesSource(null);
    }

    protected void CategoryDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ICategory item = null;

        if (CategoryTreeView.SelectedNode != null)
            item = ContentManagementService.GetCategory(CategoryTreeView.SelectedNode.DataPath);

        if (item != null)
            e.InputParameters["parentId"] = item.ID;
    }

    protected void CategoryDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        RebuildView(null);
    }

    protected void CategoryDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        RebuildView(null);
    }

    protected void CategoryDataSource_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        RebuildView(null);
    }

    private void UpdateView()
    {
        if (CategoryTreeView.SelectedNode != null)
        {
            PathTextBox.Text = CategoryTreeView.SelectedNode.DataPath;
        }
        else
        {
            PathTextBox.Text = Category.Root;
        }
    }

    private void RebuildView(string specifiedPath)
    {
        string path = null;

        if (!string.IsNullOrEmpty(specifiedPath))
            path = specifiedPath;
        else if (CategoryTreeView.SelectedNode != null)
            path = CategoryTreeView.SelectedNode.DataPath;

        CategoryTreeView.DataBind();

        try
        {
            if (string.IsNullOrEmpty(path))
                return;

            string[] names = path.Split(
                        new string[] { System.IO.Path.DirectorySeparatorChar.ToString() },
                        StringSplitOptions.RemoveEmptyEntries);

            string valuePath = null;

            foreach (string item in names)
            {
                if (!string.IsNullOrEmpty(valuePath))
                    valuePath += CategoryTreeView.PathSeparator;

                valuePath += item;

                TreeNode node = CategoryTreeView.FindNode(valuePath);

                if (node == null)
                    return;

                node.Expand();
                node.Select();
            }
        }
        finally
        {
            CategoryGridView.DataBind();
        }
    }

    protected void FindPathButton_Click(object sender, EventArgs e)
    {
        RebuildView(PathTextBox.Text);
    }
}
