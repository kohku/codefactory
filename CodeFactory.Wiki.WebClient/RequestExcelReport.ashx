<%@ WebHandler Language="C#" Class="RequestExcelReport" %>

using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Xsl;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Xml.XPath;

public class RequestExcelReport : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        int totalCount = 0;
        object input = null;
        Type type = null;
        Stream template = Stream.Null;

        string request = context.Request.QueryString["request"];
        string initialDate = context.Request.QueryString["sd"];
        string finalDate = context.Request.QueryString["fd"];

        if (string.IsNullOrEmpty(request))
            UrlNotFound(context);

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        switch (request)
        {
            case "statisticsTrace":
                input = new EstadisticaConsulta(
                    DateTime.FromBinary(long.Parse(initialDate)), DateTime.FromBinary(long.Parse(finalDate)),
                    string.Empty, string.Empty, string.Empty, null, string.Empty,
                    CodeFactory.Wiki.Statistics.TraceStatistics.ConsultaReporteEstadistico(
                    DateTime.FromBinary(long.Parse(initialDate)), DateTime.FromBinary(long.Parse(finalDate)),
                    string.Empty, string.Empty, string.Empty, null, string.Empty, int.MaxValue, 0, out totalCount));

                template = File.Open(context.Server.MapPath("~/App_Data/EstadisticaConsulta.xslt"), FileMode.Open);

                type = typeof(EstadisticaConsulta);

                parameters["totalcount"] = totalCount;
                parameters["ext:DateTimeNow"] = DateTime.Now.ToString(new CultureInfo("es-MX"));

                BuildExcelFile(context, input, template, type, parameters);

                break;
            default:
                UrlNotFound(context);
                break;
        }
    }

    private void BuildExcelFile(HttpContext context, object input, Stream template, Type type, Dictionary<string, object> parameters)
    {
        if (input == null)
            throw new ArgumentNullException("input");

        if (template == null || template.Equals(Stream.Null))
            throw new ArgumentNullException("template");

        if (type == null)
            throw new ArgumentNullException("type");

        XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();

        try
        {
            using (XmlReader reader = XmlReader.Create(template))
            {
                xslCompiledTransform.Load(reader);

                StringBuilder builder = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(builder);

                XmlSerializer serializer = new XmlSerializer(type);

                serializer.Serialize(writer, input);

                XmlDocument document = new XmlDocument();

                document.LoadXml(builder.ToString());

                XPathNavigator navigator = document.CreateNavigator();

                XsltArgumentList arguments = new XsltArgumentList();

                foreach (KeyValuePair<string, object> entry in parameters)
                {
                    if (entry.Key.StartsWith("ext:", StringComparison.OrdinalIgnoreCase))
                        arguments.AddExtensionObject(entry.Key, entry.Value);
                    else
                        arguments.AddParam(entry.Key, string.Empty, entry.Value);
                }

                context.Response.Cache.SetExpires(DateTime.Now);
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.AddHeader("content-disposition",
                    string.Format("attachment; filename={0}.xls", context.Request.QueryString["request"]));

                xslCompiledTransform.Transform(navigator, arguments, context.Response.OutputStream);

                context.Response.End();
            }
        }
        finally
        {
            template.Close();
        }
    }

    public bool IsReusable{
        get{
            return false;
        }
    }

    private void UrlNotFound(HttpContext context){
        context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
        context.Response.End();
    }
}