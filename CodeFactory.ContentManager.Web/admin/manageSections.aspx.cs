using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.ContentManager;

public partial class admin_manageSections : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SectionTreeView.DataBind();

            UpdateView();
        }
    }

    protected void SectionTreeView_SelectedNodeChanged(object sender, EventArgs e)
    {
        SectionGridView.DataBind();

        UpdateView();
    }

    protected void SectionDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        Guid? sectionId = Guid.Empty;

        if (SectionTreeView.SelectedNode != null)
        {
            ISection item = ContentManagementService.GetSection(SectionTreeView.SelectedNode.DataPath);

            if (item != null)
                sectionId = item.ID;
        }

        e.ObjectInstance = new SectionsSource(sectionId);
    }

    protected void SectionDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ISection item = null;

        if (SectionTreeView.SelectedNode != null)
            item = ContentManagementService.GetSection(SectionTreeView.SelectedNode.DataPath);

        if (item != null)
            e.InputParameters["parentId"] = item.ID;
    }

    protected void SectionDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        RebuildView(null);
    }

    protected void SectionDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        RebuildView(null);
    }

    protected void SectionDataSource_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        RebuildView(null);
    }

    private void UpdateView()
    {
        if (SectionTreeView.SelectedNode != null)
        {
            PathTextBox.Text = SectionTreeView.SelectedNode.DataPath;
        }
        else
        {
            PathTextBox.Text = Section.Root;
        }
    }

    private void RebuildView(string specifiedPath)
    {
        string path = null;

        if (!string.IsNullOrEmpty(specifiedPath))
            path = specifiedPath;
        else if (SectionTreeView.SelectedNode != null)
            path = SectionTreeView.SelectedNode.DataPath;

        SectionTreeView.DataBind();

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
                    valuePath += SectionTreeView.PathSeparator;

                valuePath += item;

                TreeNode node = SectionTreeView.FindNode(valuePath);

                if (node == null)
                    return;

                node.Expand();
                node.Select();
            }
        }
        finally
        {
            SectionGridView.DataBind();
        }
    }

    protected void FindPathButton_Click(object sender, EventArgs e)
    {
        RebuildView(PathTextBox.Text);
    }

    protected void SectionGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Up")
        {
            int gridIndex = Convert.ToInt32(e.CommandArgument);

            // There's one above
            if (gridIndex > 0)
            {
                Guid aboveId = (Guid)SectionGridView.DataKeys[gridIndex - 1].Value;
                Guid selectedId = (Guid)SectionGridView.DataKeys[gridIndex].Value;

                Section sectionAbove = Section.Load(aboveId);

                if (sectionAbove == null)
                    return;

                Section selectedSection = Section.Load(selectedId);

                if (selectedSection == null)
                    return;

                int switchIndex = selectedSection.Index;

                selectedSection.Index = sectionAbove.Index;
                sectionAbove.Index = switchIndex;

                selectedSection.Save();
                sectionAbove.Save();

            }

            RebuildView(null);
        }
    }
}
