using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Gallery.Core;

public partial class _Project : System.Web.UI.Page
{
    private Gallery _gallery;
    private Guid id;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && Request.QueryString["guid"] == null)
            throw new InvalidOperationException("Bad request. Gallery Unknown.");

        if (!IsPostBack)
            id = new Guid(Request.QueryString["guid"]);
        else
            id = (Guid)Session["guid"];

        Session["masterGraphic"] = false;
        Session["guid"] = id;

        _gallery = Gallery.Load(id);

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Page.Title = (string)GetLocalResourceObject("Title");
        ProjectTitleLabel.Text = _gallery.Title;
        switch (_gallery.Status)
        {
            case GalleryStatus.Design:
                DesingCheckBox.Checked = true;
                break;
            case GalleryStatus.Changes:
                ChangesCheckBox.Checked = true;
                break;
            case GalleryStatus.Authorized:
                AuthorizedCheckBox.Checked = true;
                break;
            default:
                throw new InvalidOperationException("Bad request. Undefined status.");
        }

        ManageButton.Visible = HttpContext.Current.User.IsInRole("Administrator");
    }

    protected void CommentsDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new CommentsResult(_gallery);
    }

    protected void NewButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProjectWizard.aspx");
    }

    protected void ManageButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("ProjectWizard.aspx?guid={0}", id));
    }

    protected void SendButton_Click(object sender, EventArgs e)
    {
        Comment item = new Comment();
        item.Title = TitleTextBox.Text;
        item.Content = ContentTextBox.Text.Substring(0, ContentTextBox.Text.Length < 1024 ? ContentTextBox.Text.Length : 1024);
        item.Author = item.LastUpdatedBy = HttpContext.Current.User.Identity.Name;
        item.IPAddress = HttpContext.Current.Request.UserHostAddress;

        _gallery.AddComment(item);

        _gallery.Save();

        CommentsDataSource.Select();
        CommentsGridView.DataBind();

        //Response.Redirect(_gallery.RelativeLink, true);

        // Notificando que se agregó exitósamente el comentario.
        TitleTextBox.Text = string.Empty;
        ContentTextBox.Text = string.Empty;
        MessagesBoard.Visible = true;
    }

    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}
