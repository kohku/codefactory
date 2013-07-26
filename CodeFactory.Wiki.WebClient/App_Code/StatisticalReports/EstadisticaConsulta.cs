using System;
using System.Collections.Generic;
using System.Web;
using CodeFactory.Wiki.Statistics;

/// <summary>
/// Summary description for EstadisticaConsulta
/// </summary>
[Serializable]
public class EstadisticaConsulta
{
    private DateTime _fechaInicio;
    private DateTime _fechaFin;
    private string _title;
    private string _urlRequested;
    private string _username;
    private Guid? _id;
    private string _type;
    private List<StatisticTraceHit> _hits;

    public EstadisticaConsulta()
    {
        _hits = new List<StatisticTraceHit>();
    }

    public EstadisticaConsulta(DateTime fechaInicio, DateTime fechaFin, string title, string urlRequested,
        string username, Guid? id, string type, List<TraceHit> hits) : this()
    {
        _fechaInicio = fechaInicio;
        _fechaFin = fechaFin;
        _title = title;
        _urlRequested = urlRequested;
        _username = username;
        _id = id;
        _type = type;

        foreach (TraceHit hit in hits)
            _hits.Add(new StatisticTraceHit(hit));
    }

    public string FechaInicio
    {
        get { return _fechaInicio.ToString("dd/MM/yyyy"); }
        set { }
    }

    public string FechaFin
    {
        get { return _fechaFin.ToString("dd/MM/yyyy"); }
        set { }
    }

    public string Title
    {
        get { return _title; }
        set { }
    }

    public string UrlRequested
    {
        get { return _urlRequested; }
        set { }
    }

    public string ID
    {
        get { return _id.HasValue ? _id.Value.ToString() : Guid.Empty.ToString(); }
        set { }
    }

    public string Type
    {
        get { return _type; }
        set { }
    }

    public string UserName
    {
        get { return _username; }
        set { }
    }

    public List<StatisticTraceHit> Hits
    {
        get { return _hits; }
    }
}
