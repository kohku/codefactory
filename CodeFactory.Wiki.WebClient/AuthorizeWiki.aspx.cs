using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.Wiki.Workflow;
using CodeFactory.Wiki;
using CodeFactory.Web;

public partial class AuthorizeWiki : System.Web.UI.Page
{
    private WorkWikiItem workItem;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            BindWikiContent();
        }   
    }

    private void BindWikiContent()
    {
        if (string.IsNullOrEmpty(Request.QueryString["trackingNumber"]))
            throw new InvalidOperationException("A valid identifier is requested.");

        workItem = string.IsNullOrEmpty(Request.QueryString["ischanged"]) ||
            !Convert.ToBoolean(Request.QueryString["ischanged"]) ?
            WorkWikiItem.Load(new Guid(Request.QueryString["trackingNumber"])) : (WorkWikiItem)Session[Request.QueryString["trackingNumber"]];

        if (workItem == null)
            return;

        Page.Title = string.Format("Wiki - {0}", workItem.Title);
        TitleLabel.Text = string.Format("{0} ({1})", workItem.Title,
            workItem.Action == CodeFactory.Web.Core.SaveAction.Update ? "Actualización" :
            workItem.Action == CodeFactory.Web.Core.SaveAction.Delete ? "Eliminación" : "Nuevo");
        EditorLabel.Text = string.Format("Autor: {0}{1}", !string.IsNullOrEmpty(workItem.Editor) ? workItem.Editor : "ND",
            !string.IsNullOrEmpty(workItem.DepartmentArea) ? string.Format(" - {0}", workItem.DepartmentArea) : string.Empty);
        ContentLabel.Text = workItem.Content;
        AuthorizeButton.Visible = RejectButton.Visible = workItem.Status.Equals(WikiStatus.AuthorizationRequested);
        CommentsTextBox.Enabled = workItem.Status.Equals(WikiStatus.AuthorizationRequested);

        MessagesBoard.Visible = !workItem.Status.Equals(WikiStatus.AuthorizationRequested);
        switch (workItem.Status)
        {
            case WikiStatus.AuthorizationExpired:
                MessagesBoardLabel.Text = "La solicitud ha sido rechazada por falta de autorización.";
                break;
            case WikiStatus.AuthorizationRejected:
                MessagesBoardLabel.Text = "La solicitud ha sido rechazada por el administrador.";
                break;
            default:
                break;
        }
    }

    protected void AuthorizeButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["trackingNumber"]))
            throw new InvalidOperationException("A valid identifier is requested.");

        workItem = string.IsNullOrEmpty(Request.QueryString["ischanged"]) ||
            !Convert.ToBoolean(Request.QueryString["ischanged"]) ?
            WorkWikiItem.Load(new Guid(Request.QueryString["trackingNumber"])) : (WorkWikiItem)Session[Request.QueryString["trackingNumber"]];

        if (workItem == null)
            return;

        WikiService.AuthorizeWiki(workItem);

        Response.Redirect(Utils.RelativeWebRoot);
    }

    protected void RejectButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["trackingNumber"]))
            throw new InvalidOperationException("A valid identifier is requested.");

        workItem = string.IsNullOrEmpty(Request.QueryString["ischanged"]) ||
            !Convert.ToBoolean(Request.QueryString["ischanged"]) ?
            WorkWikiItem.Load(new Guid(Request.QueryString["trackingNumber"])) : (WorkWikiItem)Session[Request.QueryString["trackingNumber"]];

        if (workItem == null)
            return;

        workItem.Messages = CommentsTextBox.Text;

        WikiService.RejectAuthorization(workItem);

        Response.Redirect(Utils.RelativeWebRoot);
    }
}
