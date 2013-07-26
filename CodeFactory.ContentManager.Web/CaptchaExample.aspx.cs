using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CaptchaExample : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblResult.Visible = true;

        if (Page.IsValid)
        {
            lblResult.Text = "You got it!";
            lblResult.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            lblResult.Text = "Incorrect";
            lblResult.ForeColor = System.Drawing.Color.Red;
        }
    }
}
