using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CodeFactory.Web;
using CodeFactory.Web.Storage;
using CodeFactory.Wiki;

public partial class Entry : System.Web.UI.Page, ICallbackEventHandler
{
    private Wiki wiki;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.MaintainScrollPositionOnPostBack = true;

        TimeStamp.Text = DateTime.Now.ToString();

        if (!Page.IsPostBack && !Page.IsCallback)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                Page.Title = "Wiki - Editar Contenido";
                BindWikiContent();
            }
            else
            {
                Page.Title = "Wiki - Contenido Nuevo";
            }

            UpdateView();
        }

        //RegisterSlugBuilderScript();
        RegisterOtherCategoryScript();

        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(30));
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.Cache.VaryByParams["id"] = true;
            Response.Cache.SetNoServerCaching();
        }
    }

    private void RegisterOtherCategoryScript()
    {
        if (!Page.ClientScript.IsStartupScriptRegistered("OtherCategory"))
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("function displayothercategories(){");
            builder.AppendFormat("var otherlabel = document.getElementById(\"{0}\");", SpecifyLabel.ClientID);
            builder.AppendFormat("var othertextbox = document.getElementById(\"{0}\");", SpecifyTextBox.ClientID);
            builder.AppendFormat("var items = document.getElementById(\"{0}\");", CategoryList.ClientID);
            builder.Append("var optionselected = items.options[items.selectedIndex];");
            builder.Append("otherlabel.style.visibility = othertextbox.style.visibility = ");
            builder.Append("(optionselected.value == \"Other\" ? \"visible\" : \"hidden\");");
            builder.Append("if (optionselected.value == \"Other\")");
            builder.Append("othertextbox.focus();");
            builder.Append("}");
            builder.Append("function validateothercategories(source, arguments){");
            builder.AppendFormat("var otherlabel = document.getElementById(\"{0}\");", SpecifyLabel.ClientID);
            builder.AppendFormat("var othertextbox = document.getElementById(\"{0}\");", SpecifyTextBox.ClientID);
            builder.AppendFormat("var items = document.getElementById(\"{0}\");", CategoryList.ClientID);
            builder.Append("var optionselected = items.options[items.selectedIndex];");
            builder.Append("arguments.IsValid = (optionselected.value != \"Other\") || ");
            builder.Append("(optionselected.value == \"Other\" && othertextbox.value != \"\");");
            builder.Append("}");
            builder.Append("displayothercategories();");

            Page.ClientScript.RegisterStartupScript(this.GetType(), "OtherCategory", builder.ToString(), true);

            CategoryList.Attributes.Add("onchange", "displayothercategories();");
        }
    }

    //private void RegisterSlugBuilderScript()
    //{
    //    if (!Page.ClientScript.IsClientScriptBlockRegistered("SlugBuilder"))
    //    {
    //        StringBuilder slugBuilder = new StringBuilder();

    //        string callbackReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ApplyCallback", "context");

    //        slugBuilder.Append("function GetSlug(){");
    //        slugBuilder.AppendFormat("var title = document.getElementById('{0}').value;", TitleTextBox.ClientID);
    //        slugBuilder.Append("CallServer(title, \"slug\");");
    //        slugBuilder.Append("}");

    //        slugBuilder.AppendFormat("function CallServer(arg, context){{{0};}}", callbackReference);

    //        slugBuilder.Append("function ApplyCallback(arg, context){");
    //        slugBuilder.Append("if (context == \"slug\")");
    //        slugBuilder.Append("ApplySlug(arg);");
    //        slugBuilder.Append("}");

    //        slugBuilder.Append("function ApplySlug(arg){");
    //        slugBuilder.AppendFormat("var slug = document.getElementById('{0}');", SlugTextBox.ClientID);
    //        slugBuilder.Append("slug.value = arg;");
    //        slugBuilder.Append("}");

    //        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "SlugBuilder", slugBuilder.ToString(), true);
    //    }
    //}

    private void BindWikiContent()
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
            throw new InvalidOperationException("A valid identifier is requested.");

        wiki = string.IsNullOrEmpty(Request.QueryString["ischanged"]) || 
            !Convert.ToBoolean(Request.QueryString["ischanged"]) ?
            Wiki.Load(new Guid(Request.QueryString["id"])) : (Wiki)Session[Request.QueryString["id"]];

        if (wiki == null)
            return;

        if (!wiki.Editable && !User.IsInRole("Administrator"))
            Response.Redirect(wiki.RelativeLink);

        EditableTableRow.Visible = User.IsInRole("Administrator");
        EditableCheckBox.Checked = wiki.Editable;

        // Debemos asegurarnos de que se recuperen
        CategoryList.DataBind();

        ListItem item = CategoryList.Items.FindByText(wiki.Category);
        if (item == null)
            item = CategoryList.Items.FindByValue("Other");
        CategoryList.SelectedIndex = CategoryList.Items.IndexOf(item);
        SpecifyTextBox.Text = !wiki.Category.Equals(item.Text) ? wiki.Category : string.Empty;

        ContentHtmlEditor.Content = wiki.Content;
        DescriptionTextBox.Text = wiki.Description;
        AreaTextBox.Text = wiki.DepartmentArea;
        EditorTextBox.Text = wiki.Editor;
        KeywordsTextBox.Text = wiki.Keywords;
        TitleTextBox.Text = wiki.Title;
        ReachLevelRadioButtonList.SelectedIndex = ReachLevelRadioButtonList.Items.IndexOf(
            ReachLevelRadioButtonList.Items.FindByValue(Convert.ToString((int)wiki.ReachLevel)));
        CancelButton.Visible = wiki.IsChanged;
        DeleteButton.Visible = !wiki.IsNew;
    }

    private void UpdateView()
    {
        Page.DataBind();

        AuthorLabel.Text = string.Format("Autor {0}", User.Identity.Name);
        DateLabel.Text = string.Format("Fecha {0}", DateTime.Now.ToString());
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        if (SaveWiki())
            Response.Redirect(Utils.RelativeWebRoot);
    }

    private bool SaveWiki()
    {
        // One or more validators are invalid.
        if (!Page.IsValid)
            return false;

        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            wiki = string.IsNullOrEmpty(Request.QueryString["ischanged"]) || !Convert.ToBoolean(Request.QueryString["ischanged"]) ?
                Wiki.Load(new Guid(Request.QueryString["id"])) : (Wiki)Session[Request.QueryString["id"]];
        else
            wiki = new Wiki(Guid.NewGuid());

        UpdateWiki();

        try
        {
            MessagesBoardLabel.Text = string.Empty;

            wiki.Save();
        }
        catch (InvalidOperationException ex)
        {
            MessagesBoardLabel.Text = ex.Message;
            return false;
        }
        catch (InvalidConstraintException)
        {
            MessagesBoardLabel.Text = "El título para este contenido ya existe. Por favor, elige uno distinto.";
            return false;
        }

        return true;
    }

    private void UpdateWiki()
    {
        if (wiki.IsNew)
            wiki.Author = User.Identity.Name;
        wiki.Category = CategoryList.SelectedValue != "Other" ? CategoryList.SelectedValue : SpecifyTextBox.Text;
        wiki.Content = ContentHtmlEditor.Content;
        wiki.Description = DescriptionTextBox.Text.Trim();
        if (!wiki.IsNew)
            wiki.LastUpdated = DateTime.Now;
        wiki.DepartmentArea = AreaTextBox.Text;
        wiki.Editor = EditorTextBox.Text;

        if (User.IsInRole("Administrator"))
            wiki.Editable = EditableCheckBox.Checked;

        wiki.IsVisible = true;
        if (!string.IsNullOrEmpty(KeywordsTextBox.Text))
            wiki.Keywords = KeywordsTextBox.Text;
        if (!wiki.IsNew)
            wiki.LastUpdatedBy = User.Identity.Name;
        wiki.Title = TitleTextBox.Text.Trim();
        wiki.Slug = Utils.RemoveIllegalCharacters(wiki.Title);
        wiki.ReachLevel = (ReachLevel)Enum.Parse(typeof(ReachLevel), ReachLevelRadioButtonList.SelectedValue);
        //wiki.IPAddress = Request.ServerVariables["REMOTE_ADDR"];
    }

    protected void UploadImageButton_Click(object sender, EventArgs e)
    {
        // One or more validators are invalid.
        if (!Page.IsValid)
            return;

        if (ImageUpload.HasFile)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                wiki = string.IsNullOrEmpty(Request.QueryString["ischanged"]) || !Convert.ToBoolean(Request.QueryString["ischanged"]) ?
                    Wiki.Load(new Guid(Request.QueryString["id"])) : (Wiki)Session[Request.QueryString["id"]];
            else
                wiki = new Wiki(Guid.NewGuid());

            UpdateWiki();

            try
            {
                ImageUploadBoard.Text = string.Empty;
            
                UploadedFile file = UploadStorageService.CreateFile4Storage(FileUpload.PostedFile.ContentType);

                file.ContentLength = ImageUpload.PostedFile.ContentLength;
                file.ContentType = ImageUpload.PostedFile.ContentType;
                file.FileName = ImageUpload.PostedFile.FileName;
                file.InputStream = ImageUpload.PostedFile.InputStream;

                wiki.AddFile(file);

                wiki.Content += file.HtmlLink;

                //wiki.Save();
                Session[wiki.ID.ToString()] = wiki;

                file.Save();

                Response.Redirect(string.Format("{0}?id={1}&ischanged={2}", Request.Url.AbsolutePath,
                    Server.UrlEncode(wiki.ID.ToString()), wiki.IsChanged));
            }
            catch (System.Configuration.Provider.ProviderException)
            {
                ImageUploadBoard.Text = "Formato no permitido.";
            }
        }
    }

    protected void UploadFileButton_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            throw new InvalidOperationException("One or more validators are invalid.");

        if (FileUpload.HasFile)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                wiki = Wiki.Load(new Guid(Request.QueryString["id"]));
            else
                wiki = new Wiki(Guid.NewGuid());

            UpdateWiki();

            try
            {
                FileUploadBoard.Text = string.Empty;

                UploadedFile file = UploadStorageService.CreateFile4Storage(FileUpload.PostedFile.ContentType);

                file.ContentLength = FileUpload.PostedFile.ContentLength;
                file.ContentType = FileUpload.PostedFile.ContentType;
                file.FileName = FileUpload.PostedFile.FileName;
                file.InputStream = FileUpload.PostedFile.InputStream;

                wiki.AddFile(file);

                wiki.Content += file.HtmlLink;

                //wiki.Save();
                Session[wiki.ID.ToString()] = wiki;

                file.Save();

                Response.Redirect(string.Format("{0}?id={1}&ischanged={2}", Request.Url.AbsolutePath,
                    Server.UrlEncode(wiki.ID.ToString()), wiki.IsChanged));
            }
            catch (System.Configuration.Provider.ProviderException)
            {
                FileUploadBoard.Text = "Formato no permitido.";
            }
        }
    }

    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
                return;

            wiki = Wiki.Load(new Guid(Request.QueryString["id"]));

            if (wiki == null)
                return;

            wiki.Delete();
            wiki.Save();

        }
        finally
        {
            Response.Redirect(Utils.RelativeWebRoot);
        }
    }

    protected void FilesDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        if (wiki == null)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                wiki = string.IsNullOrEmpty(Request.QueryString["ischanged"]) || !Convert.ToBoolean(Request.QueryString["ischanged"]) ?
                    Wiki.Load(new Guid(Request.QueryString["id"])) : (Wiki)Session[Request.QueryString["id"]];
            else
                wiki = new Wiki(Guid.NewGuid());
        }

        e.ObjectInstance = new FilesResult(wiki);
    }

    protected void FilesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label fileName = e.Row.FindControl("FileNameLabel") as Label;

            if (fileName != null)
                fileName.Text = Server.HtmlDecode(wiki.Files[e.Row.RowIndex].FileName);

            Label contentLength = e.Row.FindControl("ContentLengthLabel") as Label;

            if (contentLength != null)
                contentLength.Text = Utils.SizeFormat(wiki.Files[e.Row.RowIndex].ContentLength, "N");
        }
    }

    protected void FilesGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
            return;

        wiki = Wiki.Load(new Guid(Request.QueryString["id"]));

        if (wiki == null)
            return;

        wiki.RemoveFile(wiki.Files[e.RowIndex]);

        wiki.Save();

        Response.Redirect(string.Format("{0}?id={1}", Request.Url.AbsolutePath, Server.UrlEncode(wiki.ID.ToString())));
    }

    protected void FilesGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
            return;

        wiki = Wiki.Load(new Guid(Request.QueryString["id"]));

        if (wiki == null)
            return;

        wiki.Content += wiki.Files[e.NewSelectedIndex].HtmlLink;

        wiki.Save();

        Response.Redirect(string.Format("{0}?id={1}", Request.Url.AbsolutePath, Server.UrlEncode(wiki.ID.ToString())));
    }

    protected void ContentButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
            return;

        wiki = Wiki.Load(new Guid(Request.QueryString["id"]));

        if (wiki == null)
            return;

        Response.Redirect(wiki.RelativeLink);
    }

    #region ICallbackEventHandler Members

    private string _callback;

    public string GetCallbackResult()
    {
        return _callback;
    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        _callback = Utils.RemoveIllegalCharacters(eventArgument.Trim());
    }

    #endregion

    protected void SpecifyValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !CategoryList.Equals("Other") || (CategoryList.SelectedValue.Equals("Other") &&
            !string.IsNullOrEmpty(SpecifyTextBox.Text.Trim()));
    }

    protected void PreviewButton_Click(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
            Response.Redirect(string.Format("{0}SessionExpired.aspx", Utils.AbsoluteWebRoot.ToString()));

       // One or more validators are invalid.
        if (!Page.IsValid)
            return;

        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            wiki = string.IsNullOrEmpty(Request.QueryString["ischanged"]) || !Convert.ToBoolean(Request.QueryString["ischanged"]) ?
                Wiki.Load(new Guid(Request.QueryString["id"])) : (Wiki)Session[Request.QueryString["id"]];
        else
            wiki = new Wiki(Guid.NewGuid());

        UpdateWiki();

        Session[wiki.ID.ToString()] = wiki;

        Response.Redirect(string.Format("{0}Preview.aspx?id={1}&ischanged={2}", Utils.RelativeWebRoot,
            wiki.ID, wiki.IsChanged));
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            wiki = Wiki.Load(new Guid(Request.QueryString["id"]));

            if (wiki != null)
            {
                Session.Remove(wiki.ID.ToString());
                Response.Redirect(wiki.RelativeLink);
            }
        }

        Response.Redirect("~/Default.aspx");
    }
}
