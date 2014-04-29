using System;

namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    public class Node
    {
        private string id;
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

    }

    [Serializable]
    public class ipAddress : Node, IComparable<ipAddress>
    {
        private string ipaddress;
        public string IPAddress
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }


        public static Comparison<ipAddress> IPComparison = delegate(ipAddress p1, ipAddress p2)
        {
            return p1.ipaddress.CompareTo(p2.ipaddress);
        };

        public int CompareTo(ipAddress other)
        {
            return ipaddress.CompareTo(other.ipaddress);
        }
    
    }

    [Serializable]
    public class SafeUrl : Node, IComparable<SafeUrl>
    {
        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public static Comparison<SafeUrl> UrlComparison = delegate(SafeUrl p1, SafeUrl p2)
        {
            return p1.url.CompareTo(p2.url);
        };

        public int CompareTo(SafeUrl other)
        {
            return url.CompareTo(other.url);
        }

    }

   
    [Serializable]
    public class honeyPot : Node, IComparable<honeyPot>
    {
        private string ipaddress;
        public string IPAddress
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }

        private string useragent;
        public string UserAgent
        {
            get { return useragent; }
            set { useragent = value; }
        }

        private string referrer;
        public string Referrer
        {
            get { return referrer; }
            set { referrer = value; }
        }

        public static Comparison<honeyPot> UAComparison = delegate(honeyPot p1, honeyPot p2)
        {
            return p1.useragent.CompareTo(p2.useragent);
        };

        public int CompareTo(honeyPot other)
        {
            return useragent.CompareTo(other.useragent);
        }
    }

    [Serializable]
    public class httpBL : Node, IComparable<httpBL>
    {
        private string ipaddress;
        public string IPAddress
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }

        private string threatlevel;
        public string ThreatLevel
        {
            get { return threatlevel; }
            set { threatlevel = value; }
        }
        private string visitortype;
        public string VisitorType
        {
            get { return visitortype; }
            set { visitortype = value; }
        }

        private string lastactivity;
        public string LastActivity
        {
            get { return lastactivity; }
            set { lastactivity = value; }
        }
        private string useragent;
        public string UserAgent
        {
            get { return useragent; }
            set { useragent = value; }
        }
        private string referrer;
        public string Referrer
        {
            get { return referrer; }
            set { referrer = value; }
        }

        public static Comparison<httpBL> IPComparison = delegate(httpBL p1, httpBL p2)
        {
            return p1.ipaddress.CompareTo(p2.ipaddress);
        };

        public int CompareTo(httpBL other)
        {
            return ipaddress.CompareTo(other.ipaddress);
        }

    }

    [Serializable]
    public class proxyBL : Node, IComparable<proxyBL>
    {
        private string ipaddress;
        public string IPAddress
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }

        private string useragent;
        public string UserAgent
        {
            get { return useragent; }
            set { useragent = value; }
        }

        private string referrer;
        public string Referrer
        {
            get { return referrer; }
            set { referrer = value; }
        }

        public static Comparison<proxyBL> UAComparison = delegate(proxyBL p1, proxyBL p2)
        {
            return p1.useragent.CompareTo(p2.useragent);
        };

        public int CompareTo(proxyBL other)
        {
            return useragent.CompareTo(other.useragent);
        }
    }

        [Serializable]
    public class droneBL : Node, IComparable<droneBL>
    {
        private string ipaddress;
        public string IPAddress
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }

        private string useragent;
        public string UserAgent
        {
            get { return useragent; }
            set { useragent = value; }
        }

        private string referrer;
        public string Referrer
        {
            get { return referrer; }
            set { referrer = value; }
        }

        public static Comparison<droneBL> UAComparison = delegate(droneBL p1, droneBL p2)
        {
            return p1.useragent.CompareTo(p2.useragent);
        };

        public int CompareTo(droneBL other)
        {
            return useragent.CompareTo(other.useragent);
        }
    }
    
}
