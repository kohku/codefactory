using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class WikiSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void ResultsDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new WikiSearchResult(
            HttpUtility.UrlDecode(!string.IsNullOrEmpty(Request.QueryString["q"]) ?
            Request.QueryString["q"] : 
            (!string.IsNullOrEmpty(Request.QueryString["key"]) ?
            Request.QueryString["key"] : string.Empty)),
            HttpUtility.UrlDecode(!string.IsNullOrEmpty(Request.QueryString["cat"]) ?
            Request.QueryString["cat"] :
            (!string.IsNullOrEmpty(Request.QueryString["key"]) ?
            Request.QueryString["key"] : string.Empty)),
            HttpUtility.UrlDecode(!string.IsNullOrEmpty(Request.QueryString["key"]) ?
            Request.QueryString["key"] :
            (!string.IsNullOrEmpty(Request.QueryString["q"]) ?
            Request.QueryString["q"] : string.Empty)));
    }
}
