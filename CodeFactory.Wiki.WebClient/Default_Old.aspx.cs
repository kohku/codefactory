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

public partial class _Default_Old : System.Web.UI.Page
{
    private const int ResumeMaxLength = 600;
    private string contentOfDay;
    private IWiki content1;
    private IWiki content2;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (HttpContext.Current.Cache["contentOfDay"] == null &&
                HttpContext.Current.Cache["Article1Cover"] == null && 
                HttpContext.Current.Cache["Article2Cover"] == null)
            {
                content1 = Wiki.GetRandomWiki();

                if (content1 == null)
                {
                    UpdateView();
                    return;
                }

                contentOfDay = content1.Title;

                content2 = Wiki.GetRandomWiki();

                /// Mmm... aki puede haber problemas, cuando Wiki.GetRandomWiki() regrese siempre el mismo 
                /// (solo haya un wiki) y también cuando regresa null.
                int max = 20;

                while (max-- > 0 && content1.Equals(content2))
                    content2 = Wiki.GetRandomWiki();

                DateTime absoluteExpiration = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                absoluteExpiration = absoluteExpiration.AddDays(1);

                HttpContext.Current.Cache.Add("contentOfDay", contentOfDay, null, absoluteExpiration,
                    Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                HttpContext.Current.Cache.Add("Article1Cover", content1.ID, null, absoluteExpiration, 
                    Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                HttpContext.Current.Cache.Add("Article2Cover", content2.ID, null, absoluteExpiration, 
                    Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            else
            {
                contentOfDay = (string)HttpContext.Current.Cache["contentOfDay"];
                content1 = Wiki.Load((Guid)HttpContext.Current.Cache["Article1Cover"]);
                content2 = Wiki.Load((Guid)HttpContext.Current.Cache["Article2Cover"]);
            }

            UpdateView();
        }
    }

    private void UpdateView()
    {
        CurrentDateLabel.Text = DateTime.Now.ToLongDateString();

        if (!string.IsNullOrEmpty(contentOfDay) && content1 != null && content2 != null)
        {
            ContentLabel.Text = contentOfDay;
            TitleLabel1.Text = content1.Title;
            ContentLabel1.Text = content1.Content.Length > ResumeMaxLength ?
                content1.Content.Substring(0, ResumeMaxLength - 1) + "..." : content1.Content;
            ArticleLink1.NavigateUrl = TitleLabel1.NavigateUrl = content1.RelativeLink;

            TitleLabel2.Text = content2.Title;
            ContentLabel2.Text = content2.Content.Length > ResumeMaxLength ? 
                content2.Content.Substring(0, ResumeMaxLength - 1) + "..." : content2.Content;
            ArticleLink2.NavigateUrl = TitleLabel2.NavigateUrl = content2.RelativeLink;
        }

        WorklistBullet.Visible = WorklistLink.Visible = User.IsInRole("Authorizer") || User.IsInRole("Administrator");
        AdministrationBullet.Visible = AdministrationLink.Visible = User.IsInRole("Administrator");
    }
}
