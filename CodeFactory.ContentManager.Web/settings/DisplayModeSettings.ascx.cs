using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class settings_DisplayModeSettings : System.Web.UI.UserControl
{
    //protected void DisplayModeTabContainer_ActiveTabChanged(object sender, EventArgs e)
    //{
    //    if (HttpContext.Current.User.Identity.IsAuthenticated)
    //    {
    //        WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
    //        WebPartDisplayMode mode = currentWebPartManager.SupportedDisplayModes[
    //            this.DisplayModeTabContainer.ActiveTab.HeaderText];
    //        if (mode != null)
    //        {
    //            currentWebPartManager.DisplayMode = mode;
    //        }
    //    }
    //}

    protected void Page_InitComplete(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);

            if (currentWebPartManager.Personalization.CanEnterSharedScope && 
                (currentWebPartManager.Personalization.Scope == PersonalizationScope.User))
            {
                currentWebPartManager.Personalization.ToggleScope();
            }

            foreach (WebPartDisplayMode mode in currentWebPartManager.SupportedDisplayModes)
            {
                if (mode.Equals(WebPartManager.BrowseDisplayMode))
                {
                    this.BrowsePanelLabel.Text = mode.Name;
                    this.BrowsePanel.Visible = true;
                }
                if (mode.Equals(WebPartManager.CatalogDisplayMode))
                {
                    this.CatalogPanelLabel.Text = mode.Name;
                    this.CatalogPanel.Visible = true;
                }
                else
                {
                    if (mode.Equals(WebPartManager.DesignDisplayMode))
                    {
                        this.DesignPanelLabel.Text = mode.Name;
                        this.DesignPanel.Visible = true;
                        continue;
                    }
                    if (mode.Equals(WebPartManager.EditDisplayMode))
                    {
                        this.EditPanelLabel.Text = mode.Name;
                        this.EditPanel.Visible = true;
                    }
                }
            }

            if (!base.IsPostBack)
            {
                //currentWebPartManager.DisplayMode = WebPartManager.DesignDisplayMode;
                currentWebPartManager.DisplayMode = WebPartManager.CatalogDisplayMode;
            }

            //foreach (TabPanel panel in this.DisplayModeTabContainer.Tabs)
            //{
            //    if (panel.HeaderText == currentWebPartManager.DisplayMode.Name)
            //    {
            //        this.DisplayModeTabContainer.ActiveTab = panel;
            //        break;
            //    }
            //}
        }
    }
}
