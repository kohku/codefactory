using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Settings : System.Web.UI.Page
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if ((this.TheWebPartManager.DisplayMode == WebPartManager.EditDisplayMode) &&
            (this.TheWebPartManager.SelectedWebPart != null))
        {
            this.TheMultiView.ActiveViewIndex = 1;
        }
        else
        {
            this.TheMultiView.ActiveViewIndex = 0;
        }
    }

}
