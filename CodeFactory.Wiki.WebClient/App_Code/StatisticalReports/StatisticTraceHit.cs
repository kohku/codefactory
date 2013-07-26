using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Wiki.Statistics;

/// <summary>
/// Summary description for StatisticTraceHit
/// </summary>
[Serializable]
public class StatisticTraceHit
{
    private TraceHit _hit;

    public StatisticTraceHit()
    {
    }

    public StatisticTraceHit(TraceHit hit)
    {
        _hit = hit;
    }

    public string Date
    {
        get { return _hit.TimeStamp.ToString("dd/MM/yyyy"); }
        set { }
    }

    public string Title
    {
        get { return _hit.Title; }
        set { }
    }

    public string UrlRequested
    {
        get { return _hit.UrlRequested; }
        set { }
    }

    public Guid? ID
    {
        get { return _hit.ID; }
        set { }
    }

    public string UserName
    {
        get { return _hit.UserName; }
        set { }
    }

    public string Type
    {
        get 
        {
            if (string.IsNullOrEmpty(_hit.Type))
                return string.Empty;

            string[] elements = _hit.Type.Split(new char[]{'.'}, StringSplitOptions.RemoveEmptyEntries);

            return elements != null ? elements[elements.Length - 1] : string.Empty;
        }
        set { }
    }

    public int Hits
    {
        get { return _hit.Hits; }
        set { }
    }
}
