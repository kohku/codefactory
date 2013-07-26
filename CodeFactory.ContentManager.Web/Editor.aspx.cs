using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Editor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void EditButton_Click(object sender, EventArgs e)
    {
        ContentEditor.Content = ContentLabel.Text;
        ContentEditorModalPopupExtender.Show();
    }

    protected void AcceptButton_Click(object sender, EventArgs e)
    {
        ContentLabel.Text = ContentEditor.Content;
        ContentEditorModalPopupExtender.Hide();
    }
}
