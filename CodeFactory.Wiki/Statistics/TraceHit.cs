using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.Wiki.Statistics
{
    [Serializable]
	public class TraceHit
	{
        private DateTime _timestamp;
        private string _title;
        private string _urlRequested;
        private string _username;
        private Guid? _id;
        private string _type;
        private int _hits;

        public TraceHit(DateTime timestamp, string title, string urlRequested, Guid? id, string type, string username, int hits)
        {
            _timestamp = timestamp;
            _title = title;
            _urlRequested = urlRequested;
            _id = id;
            _type = type;
            _username = username;
            _hits = hits;
        }

        public DateTime TimeStamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string UrlRequested
        {
            get { return _urlRequested; }
            set { _urlRequested = value; }
        }

        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        public Guid? ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int Hits
        {
            get { return _hits; }
            set { _hits = value; }
        }
    }
}
