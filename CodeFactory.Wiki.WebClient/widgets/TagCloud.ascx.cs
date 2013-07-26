using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using CodeFactory.Wiki;

public partial class widgets_TagCloud : System.Web.UI.UserControl
{
    private static object _syncRoot = new object();

    private int _minimumVotes = 1;

    public int MinimumVotes
    {
        get
        { 
            // TODO use application settings
            return _minimumVotes; 
        }
    }

    private Dictionary<string, string> WeightedList
    {
        get
        {
            Dictionary<string, string> weightedList = new Dictionary<string,string>();

            Dictionary<string, int> cloud = WikiService.GetTagCloud();

            int max = 0;

            foreach (int value in cloud.Values)
            {
                if (value > max)
                    max = value;
            }

            foreach (string key in cloud.Keys)
            {
                if (cloud[key] < MinimumVotes)
                    continue;

                double weight = ((double)cloud[key] / max) * 100;
                if (weight >= 99)
                    weightedList.Add(key, "biggest");
                else if (weight >= 70)
                    weightedList.Add(key, "big");
                else if (weight >= 40)
                    weightedList.Add(key, "medium");
                else if (weight >= 20)
                    weightedList.Add(key, "small");
                else if (weight >= 3)
                    weightedList.Add(key, "smallest");
            }

            return weightedList;
        }
    }

    private void SortList()
    {
        
    }

    protected override void OnLoad(EventArgs e)
    {
        HtmlLink css = new HtmlLink();
        css.ID = "widgets_css";
        css.Href = "~/widgets/widgets.css";
        css.Attributes["type"] = "text/css";
        css.Attributes["rel"] = "stylesheet";

        if (!Page.Header.Controls.Contains(css))
            Page.Header.Controls.Add(css);

        foreach (string key in WeightedList.Keys)
        {
            HyperLink tag = new HyperLink();
            tag.NavigateUrl = string.Format("~/WikiSearch.aspx?key={0}", HttpUtility.UrlEncode(key));
            tag.CssClass = WeightedList[key];
            tag.Text = string.Format("{0}&nbsp;", HttpUtility.HtmlEncode(key));
            TagCloud.Controls.Add(tag);
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<div id=\"tagcloud\">");
        writer.Write(sb.ToString());
        base.Render(writer);
        writer.Write("</div>");
    }
}
