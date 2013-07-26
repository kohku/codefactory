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

public partial class admin_Roles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        TextBox role = DetailsView1.FindControl("TextBox1") as TextBox;

        if (role != null)
            e.Values.Add("role", role.Text);

    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label role = GridView1.Rows[e.RowIndex].FindControl("Label1") as Label;

        if (role != null)
            e.Keys.Add("role", role.Text);
    }

}
