using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.Web.Storage;
using AjaxControlToolkit;
using System.IO;
using CodeFactory.ContentManager;
using com.flajaxian;
using System.Text;
using System.Threading;

public partial class _UploadedFilesCatalog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SectionTreeView.DataBind();

            UpdateView();
        }

        #region Flaxian workarounds
        // Esto se tiene que hacer todo el tiempo
        RegisterFileUploadScript();

        // Esto es para saber que ha comenzado un upload con este control.
        if (!string.IsNullOrEmpty(Request.QueryString["__ID"]))
            RegisterUploadStart();

        // Aqui pues checamos los postbacks que lanza el update panel.
        if (Page.IsPostBack)
        {
            CheckPossibleResponses();

            FileUploadDataList.DataBind();
        }
        #endregion
    }

    private void RegisterUploadStart()
    {
        HttpContext.Current.Items["SyncUploadStorageAdapter"] = new ManualResetEvent(false);
    }

    private void CheckPossibleResponses()
    {
        HiddenField status = FileUploadDetailsView.FindControl("UploadStatus") as HiddenField;

        if (status == null)
            return;

        switch(status.Value.ToLower())
        {
            case "complete":
                ManualResetEvent e = HttpContext.Current.Items["SyncUploadStorageAdapter"] as ManualResetEvent;

                if (e != null)
                    e.WaitOne();

                PathTextBox.Text = "Ok";
                PathTextBox.BackColor = System.Drawing.Color.LightGreen;
                break;
            case "error":
                PathTextBox.Text = "Error while uploading";
                PathTextBox.BackColor = System.Drawing.Color.Red;
                break;
            default:
                PathTextBox.Text = string.Empty;
                PathTextBox.BackColor = System.Drawing.Color.White;
                break;
        }
    }

    private void RegisterFileUploadScript()
    {
        FileUploader uploader = FileUploadDetailsView.FindControl("TheFileUploader") as FileUploader;
        HiddenField __sectionId = FileUploadDetailsView.FindControl("__SectionId") as HiddenField;

        if (uploader == null || __sectionId == null)
            return;

        if (!Page.ClientScript.IsClientScriptBlockRegistered("FileStateChanged"))
        {
            HiddenField uploadStatus = FileUploadDetailsView.FindControl("__UploadStatus") as HiddenField;

            StringBuilder builder = new StringBuilder();

            builder.Append("function FileStateChanged(uploader, file, httpStatus, isLast){");
            builder.Append("Flajaxian.fileStateChanged(uploader, file, httpStatus, isLast);");
            builder.Append("if (file.state == Flajaxian.File_Error){");
            builder.AppendFormat("var statusbar = document.getElementById('{0}');", StatusBar.ClientID);
            builder.Append("statusbar.innerText = 'Cannot upload this file.';");
            builder.AppendFormat("setTimeout('CompleteRequestWError()', 50);", TheUpdatePanel.ClientID);
            builder.Append("return;");
            builder.Append("}");
            builder.Append("if (file.state == Flajaxian.File_Uploading){");
            builder.AppendFormat("var sectionId = document.getElementById('{0}');", __sectionId.ClientID);
            builder.AppendFormat("{0}.setStateVariable('sectionId', sectionId.value);", uploader.ClientID);
            builder.AppendFormat("var statusbar = document.getElementById('{0}');", StatusBar.ClientID);
            builder.Append("statusbar.innerText = 'Uploading...';");
            builder.Append("}");
            builder.Append("if (file.state == Flajaxian.File_Uploaded){");
            builder.AppendFormat("var statusbar = document.getElementById('{0}');", StatusBar.ClientID);
            builder.Append("statusbar.innerText = 'Upload complete. File name: ' + file.name + ' (' + file.bytes + ' bytes.)';");
            builder.AppendFormat("setTimeout('CompleteRequest()', 150);", TheUpdatePanel.ClientID);
            builder.Append("}");
            builder.Append("}");
            builder.Append("function CompleteRequest(){");
            builder.AppendFormat("var status = document.getElementById('{0}');", uploadStatus.ClientID);
            builder.Append("status.value = 'complete';");
            builder.AppendFormat("__doPostBack('{0}','');", TheUpdatePanel.ClientID);
            builder.Append("}");
            builder.Append("function CompleteRequestWError(){");
            builder.AppendFormat("var status = document.getElementById('{0}');", uploadStatus.ClientID);
            builder.Append("status.value = 'error';");
            builder.AppendFormat("__doPostBack('{0}','');", TheUpdatePanel.ClientID);
            builder.Append("}");

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileStateChanged", builder.ToString(), true);
        }

        Guid? sectionId = null;

        if (!string.IsNullOrEmpty(__sectionId.Value))
            sectionId = new Guid(__sectionId.Value);

        if (SectionTreeView.SelectedNode != null)
        {
            Section item = null;
                //ContentManagementService.GetSection(SectionTreeView.SelectedNode.DataPath);

            if (item != null)
            {
                sectionId = item.ID;
                __sectionId.Value = item.ID.ToString();
            }
        }
    }

    protected void SectionTreeView_SelectedNodeChanged(object sender, EventArgs e)
    {
        //FileUploadDataList.DataBind();

        UpdateView();
    }

    protected void TheUploadedFilesDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        Guid? sectionId = null;

        if (SectionTreeView.SelectedNode != null)
        {
            Section item = null;
                //ContentManagementService.GetSection(SectionTreeView.SelectedNode.DataPath);

            if (item != null)
                sectionId = item.ID;
        }

        e.ObjectInstance = new UploadedFilesCatalogSource(null,
            // Avoid to search all files
            sectionId.HasValue ? sectionId.Value : Guid.Empty, 
            null, null, null, null, null, null);
    }

    protected void TheFileUploader_OnFileReceived(object sender, FileReceivedEventArgs args)
    {
        //string parentId = Request.Form["sectionId"];

        //Guid fileId = (Guid)HttpContext.Current.Items["fileId"];
    }

    public void UpdateView()
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

    protected void FindPathButton_Click(object sender, EventArgs e)
    {
        RebuildView(PathTextBox.Text);
    }
}
