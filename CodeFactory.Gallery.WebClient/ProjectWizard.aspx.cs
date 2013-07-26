using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Gallery.Core;
using CodeFactory.Gallery.Core.Providers;
using CodeFactory.Web.Storage;
using System.IO;

public partial class _ProjectWizard : System.Web.UI.Page
{
    protected Gallery _gallery;

    protected void Page_Load(object sender, EventArgs e)
    {
        Guid? id = null;

        if (!string.IsNullOrEmpty(Request.QueryString["guid"]))
            id = new Guid(Request.QueryString["guid"]);
        else if (ViewState["id"] != null)
            id = (Guid)ViewState["id"];

        if (id.HasValue)
        {
            _gallery = Gallery.Load(id.Value);
        }

        if (!IsPostBack)
        {
            if (!id.HasValue)
            {
                _gallery = new Gallery();
                _gallery.Title = "Nueva galería";
                _gallery.Description = "Breve descripción de la galería";
                _gallery.Content = "Contenido de la galería";
                _gallery.Author = User.Identity.Name;
                UploadedFile file = UploadedFile.FromStream(File.OpenRead(
                    Path.Combine(Server.MapPath(Request.ApplicationPath), "images/file.jpg")));
                file.FileName = "File";
                file.Description = "Imagen de prueba";
                file.ContentType = "image/jpeg";
                _gallery.AddFile(file);
                _gallery.AddUser(User.Identity.Name);
                _gallery.AcceptChanges();
                ViewState["id"] = _gallery.ID;
            }

            BindGallery(_gallery);
        }
    }

    private void BindGallery(Gallery _gallery)
    {
        TitleTextBox.Text = _gallery.Title;
        DescriptionTextBox.Text = _gallery.Description;
        ContentTextBox.Text = _gallery.Content;
        VisibleCheckBox.Checked = _gallery.IsVisible;
        ProjectStatusList.SelectedIndex = (int)_gallery.Status;
    }

    protected void ProjectDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new UploadedFilesResult(_gallery);
    }

    protected void CommentsDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new CommentsResult(_gallery);
    }

    protected void ThumbnailsDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        if (!Page.IsValid)
            return;

        FileUpload uploadedFile = ThumbnailsDetailsView.FindControl("ProjectFileUpload") as FileUpload;

        if (uploadedFile != null && uploadedFile.HasFile)
        {
            e.Values.Add("ID", Guid.NewGuid());
            e.Values.Add("ContentType", uploadedFile.PostedFile.ContentType);
            e.Values.Add("ContentLength", uploadedFile.PostedFile.ContentLength);
            e.Values.Add("InputStream", uploadedFile.PostedFile.InputStream);
        }
    }

    protected void ThumbnailsDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        ThumbnailsDataList.DataBind();
    }

    protected void ThumbnailsDetailsView_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        ThumbnailsDataList.DataBind();
        ThumbnailsDetailsView.ChangeMode(DetailsViewMode.Insert);
    }

    protected void ThumbnailsDataList_SelectedIndexChanged(object sender, EventArgs e)
    {
        ThumbnailsDetailsView.PageIndex = ThumbnailsDataList.SelectedIndex;
        ThumbnailsDetailsView.ChangeMode(DetailsViewMode.Edit);
    }

    private void UpdateChanges()
    {
        if (_gallery != null)
        {
            switch (ProjectWizard.ActiveStepIndex)
            {
                // Information
                case 0:
                    if (_gallery.IsNew)
                        _gallery.Author = HttpContext.Current.User.Identity.Name;
                    _gallery.LastUpdatedBy = HttpContext.Current.User.Identity.Name;
                    _gallery.Title = TitleTextBox.Text;
                    _gallery.Description = DescriptionTextBox.Text;
                    _gallery.Content = ContentTextBox.Text;
                    break;
                // Files
                case 1:
                    break;
                // Comments
                case 2:
                    break;
                // Users
                case 3:
                    break;
                // Finish
                case 4:
                    _gallery.IsVisible = VisibleCheckBox.Checked;
                    _gallery.Status = (ProjectStatusList.SelectedValue != string.Empty ?
                        (GalleryStatus)Enum.Parse(typeof(GalleryStatus), ProjectStatusList.SelectedValue) :
                        GalleryStatus.Design);
                    break;
                default:
                    break;
            }

            if (!_gallery.Users.Contains(HttpContext.Current.User.Identity.Name))
                _gallery.AddUser(HttpContext.Current.User.Identity.Name);

            _gallery.AcceptChanges();
        }
    }

    protected void ProjectWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        try
        {
            UpdateChanges();
        }
        catch (Exception ex)
        {
            CompleteLabel.Text = ex.Message;
            e.Cancel = true;
        }
    }

    protected void CommentsDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        CommentsGridView.DataBind();
    }

    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        _gallery.Delete();
        _gallery.AcceptChanges();

        Response.Redirect("Default.aspx");
    }

    protected void ProjectWizard_CancelButtonClick(object sender, EventArgs e)
    {
    }

    protected void ProjectWizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (ProjectWizard.ActiveStepIndex != 1 && ProjectWizard.ActiveStepIndex != 2 &&
            ProjectWizard.ActiveStepIndex != 3 && !Page.IsValid)
            return;

        UpdateChanges();
    }

    protected void DeleteButton_PreRender(object sender, EventArgs e)
    {
        Button delete = sender as Button;

        if (delete != null && _gallery != null)
            delete.Visible = !_gallery.IsNew;
    }

    protected void CompleteLabel_PreRender(object sender, EventArgs e)
    {
        Label complete = sender as Label;

        if (complete != null && _gallery != null && !_gallery.IsValid)
            complete.Text = _gallery.ValidationMessage;
        else
            complete.Text = (string)GetLocalResourceObject("WizardCompleted.Text");
    }

    protected void FinishButton_PreRender(object sender, EventArgs e)
    {
        Button finish = sender as Button;

        if (finish != null && _gallery != null)
            finish.Enabled = _gallery.IsValid;
    }

    protected void UsersDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new ProjectUsersResult(_gallery);
    }

    protected void AddUserButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(AllUsersList.SelectedValue))
            _gallery.AddUser(AllUsersList.SelectedValue);

        _gallery.AcceptChanges();

        UsersListBox.DataBind();
    }

    protected void RemoveUserButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(UsersListBox.SelectedValue))
            _gallery.RemoveUser(UsersListBox.SelectedValue);

        _gallery.AcceptChanges();

        UsersListBox.DataBind();
    }
}
