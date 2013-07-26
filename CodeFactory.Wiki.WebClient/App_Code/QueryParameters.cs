using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for QueryParameters
/// </summary>
[Serializable]
public class QueryParameters<T> where T : new()
{
    private T _layout;

    /// <summary>
    /// Default constructor
    /// </summary>
    public QueryParameters(T layout)
    {
        _layout = layout;
    }

    /// <summary>
    /// An base object with parameters to look for.
    /// </summary>
    public T Layout
    {
        get { return _layout; }
    }
    /// <summary>
    /// Number of items retrieved in the query.
    /// </summary>
    public int PageSize;
    /// <summary>
    /// Page index for the query.
    /// </summary>
    public int PageIndex;
    /// <summary>
    /// The total row count.
    /// </summary>
    public int TotalCount;
}