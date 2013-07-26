using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Collections.Generic;
using CodeFactory.Wiki.Statistics;

/// <summary>
/// Summary description for StatisticTraceReport
/// </summary>
[DataObject]
public class StatisticTraceReport
{
    private DateTime _initialDate;
    private DateTime _finalDate;
    private string _title;
    private string _urlRequested;
    private Guid? _id;
    private string _username;
    private string _type;
    private int _totalCount;

    public StatisticTraceReport()
    {
    }

    public StatisticTraceReport(DateTime initialDate, DateTime finalDate, string title, string urlRequested, Guid? id,
        string username, string type)
    {
        _initialDate = initialDate;
        _finalDate = finalDate;
        _title = title;
        _urlRequested = urlRequested;
        _id = id;
        _username = username;
        _type = type;
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public List<StatisticTraceHit> Select()
    {
        return Select(int.MaxValue, 0);
    }

    [DataObjectMethod(DataObjectMethodType.Select)]
    public List<StatisticTraceHit> Select(int maximumRows, int startRowIndex)
    {
        List<StatisticTraceHit> results = new List<StatisticTraceHit>();

        foreach (TraceHit item in TraceStatistics.ConsultaReporteEstadistico(
            _initialDate, _finalDate, _title, _urlRequested, _username, _id, _type,
            maximumRows, maximumRows > 0 ? startRowIndex / maximumRows : 0, out _totalCount))
            results.Add(new StatisticTraceHit(item));

        HttpContext.Current.Items["StatisticTraceReport_TotalCount"] = _totalCount;

        return results;
    }

    public int TotalCount()
    {
        return _totalCount;
    }
}
