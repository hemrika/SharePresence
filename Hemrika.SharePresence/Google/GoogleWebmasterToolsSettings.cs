using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.Google
{
    [Serializable]
    public class GoogleWebmasterToolsSettings : IDisposable
    {
        private string _Domain;

        public string Domain
        {
            get { return _Domain; }
            set { _Domain = value; }
        }

        private string _EncodedSiteId;

        public string EncodedSiteId
        {
            get { return _EncodedSiteId; }
            set { _EncodedSiteId = value; }
        }

        private string _MetaTag;

        public string MetaTag
        {
            get { return _MetaTag; }
            set { _MetaTag = value; }
        }

        private bool _Verified;

        public bool Verified
        {
            get { return _Verified; }
            set { _Verified = value; }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GoogleWebmasterToolsSettings()
        {
            Dispose(false);
        }
    }
}
