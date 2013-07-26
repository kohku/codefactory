using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using CodeFactory.DataAccess.Transactions;
using CodeFactory.DataAccess;
using System.Data;

namespace CodeFactory.Wiki.Statistics
{
	public sealed class TraceStatistics
	{
        public static void RegistraEvento(DateTime timestamp, string title, string urlRequested, string username,
            Guid id, string type)
        {
            using (TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("StatisticsTrace");

                IDataCommand cmd = datasource.GetCommand("TraceData");

                cmd.Parameters["timestamp"].Value = timestamp;
                cmd.Parameters["title"].Value = title;
                cmd.Parameters["urlRequested"].Value = urlRequested;
                cmd.Parameters["username"].Value = username;
                cmd.Parameters["id"].Value = id;
                cmd.Parameters["type"].Value = type;
                cmd.Parameters["applicationName"].Value = WikiService.Provider.ApplicationName;

                int affectedRows = cmd.ExecuteNonQuery();
            }
        }

        public static List<TraceHit> ConsultaReporteEstadistico(DateTime fechaInicio, DateTime fechaFin, 
            string title, string urlRequested, string username, Guid? id, string type, int pageSize, int pageIndex, out int totalCount)
        {
            totalCount = 0;

            List<TraceHit> results = new List<TraceHit>();

            using (TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("StatisticsTrace");

                IDataCommand cmd = datasource.GetCommand("ReporteEstadistico");

                if (fechaInicio != DateTime.MinValue && fechaFin != DateTime.MinValue)
                {
                    cmd.Parameters["fechaInicio"].Value = fechaInicio;
                    cmd.Parameters["fechaFin"].Value = fechaFin;
                }
                else
                {
                    cmd.Parameters["fechaInicio"].Value = DBNull.Value;
                    cmd.Parameters["fechaFin"].Value = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(urlRequested))
                    cmd.Parameters["title"].Value = title;
                else
                    cmd.Parameters["title"].Value = DBNull.Value;

                if (!string.IsNullOrEmpty(urlRequested))
                    cmd.Parameters["urlRequested"].Value = urlRequested;
                else
                    cmd.Parameters["urlRequested"].Value = DBNull.Value;

                if (!string.IsNullOrEmpty(username))
                    cmd.Parameters["username"].Value = username;
                else
                    cmd.Parameters["username"].Value = DBNull.Value;

                if (id.HasValue)
                    cmd.Parameters["id"].Value = id.Value;
                else
                    cmd.Parameters["id"].Value = DBNull.Value;

                if (!string.IsNullOrEmpty(type))
                    cmd.Parameters["type"].Value = type;
                else
                    cmd.Parameters["type"].Value = DBNull.Value;

                cmd.Parameters["applicationName"].Value = WikiService.Provider.ApplicationName;
                cmd.Parameters["firstIndex"].Value = pageIndex * pageSize + 1;
                cmd.Parameters["lastIndex"].Value = (pageIndex * pageSize) + pageSize;

                IDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        results.Add(new TraceHit(
                            Convert.ToDateTime(reader["fecha"]),
                            reader["title"].ToString(),
                            reader["urlRequested"].ToString(),
                            (Guid?)reader["id"],
                            reader["type"].ToString(),
                            reader["username"].ToString(),
                            Convert.ToInt32(reader["hits"])));
                    }
                }
                finally
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

                totalCount = Convert.ToInt32(cmd.Parameters["totalCount"].Value);
            }

            return results;
        }
    }
}
