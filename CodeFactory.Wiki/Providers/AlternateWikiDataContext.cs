using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace CodeFactory.Wiki.Providers
{
    public class AlternateWikiDataContext : DataContext
    {
        public AlternateWikiDataContext(string fileOrServerOrConnection)
            : base(fileOrServerOrConnection)
        {
        }

        [Function(Name = "dbo.TagCloud")]
        public ISingleResult<TagCloudResult> GetTagCloud(
            [Parameter(Name = "ApplicationName", DbType = "NVarChar(512)")] string applicationName)
        {
            IExecuteResult result = this.ExecuteMethodCall(
                this,
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                applicationName);
            return ((ISingleResult<TagCloudResult>)(result.ReturnValue));
        }
    }

    public class TagCloudResult
    {
        private string _keyword;
        private int _hits;

        public TagCloudResult()
        {
        }

        [Column(Storage = "_keyword", DbType = "VarChar(900) NOT NULL")]
        public string Keyword
        {
            get
            {
                return this._keyword;
            }
            set
            {
                if ((this._keyword != value))
                {
                    this._keyword = value;
                }
            }
        }

        [Column(Storage = "_hits", DbType = "Int NOT NULL")]
        public int Hits
        {
            get
            {
                return this._hits;
            }
            set
            {
                if ((this._hits != value))
                {
                    this._hits = value;
                }
            }
        }
    }
}
    