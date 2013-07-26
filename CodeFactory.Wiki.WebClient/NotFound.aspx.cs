using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.Web;

public partial class NotFound : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SearchWiki();
    }

    private void SearchWiki()
    {
        string query = Request.QueryString["aspxerrorpath"].ToLowerInvariant();

        if (query.EndsWith("/"))
            query = HttpContext.Current.Request.RawUrl.Substring(0, HttpContext.Current.Request.RawUrl.Length - 1);

        if (query.Contains(".aspx"))
            query = query.Substring(0, query.IndexOf(".aspx"));
        int index = query.LastIndexOf("/") + 1;

        query = query.Substring(index);

        string url = string.Format("{0}WikiSearch.aspx?q={1}", Utils.RelativeWebRoot, query);

        HttpContext.Current.Response.AppendHeader("location", url);
        HttpContext.Current.Response.StatusCode = 301;
        HttpContext.Current.Response.End();
    }
}
