using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CodeFactory.Wiki;

public partial class _Default : System.Web.UI.Page
{
    private const int ResumeMaxLength = 600;
    private string contentOfDay;
    private IWiki content1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (HttpContext.Current.Cache["contentOfDay"] == null &&
                HttpContext.Current.Cache["Article1Cover"] == null)
            {
                content1 = Wiki.GetRandomWiki();

                if (content1 == null)
                {
                    UpdateView();
                    return;
                }

                contentOfDay = content1.Title;

                DateTime absoluteExpiration = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                absoluteExpiration = absoluteExpiration.AddDays(1);

                HttpContext.Current.Cache.Add("contentOfDay", contentOfDay, null, absoluteExpiration,
                    Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                HttpContext.Current.Cache.Add("Article1Cover", content1.ID, null, absoluteExpiration, 
                    Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            else
            {
                contentOfDay = (string)HttpContext.Current.Cache["contentOfDay"];
                content1 = Wiki.Load((Guid)HttpContext.Current.Cache["Article1Cover"]);
            }

            UpdateView();
        }
    }

    private void UpdateView()
    {
        CurrentDateLabel.Text = DateTime.Now.ToLongDateString();

        if (!string.IsNullOrEmpty(contentOfDay) && content1 != null)
        {
            ContentLabel.Text = contentOfDay;
            TitleLabel1.Text = content1.Title;
            TitleLabel1.NavigateUrl = content1.RelativeLink;
            ContentLabel1.Text = content1.Content;
        }

        WorklistBullet.Visible = WorklistLink.Visible = User.IsInRole("Authorizer") || User.IsInRole("Administrator");
        AdministrationBullet.Visible = AdministrationLink.Visible = User.IsInRole("Administrator");
    }
}
