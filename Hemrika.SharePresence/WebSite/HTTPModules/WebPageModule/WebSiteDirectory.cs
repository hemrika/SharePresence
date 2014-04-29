using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Collections;

namespace Hemrika.SharePresence.WebSite.Modules.WebPageModule
{
    class WebSiteDirectory: VirtualDirectory
    {
        public WebSiteDirectory(string virtualDir, WebSitePathProvider provider)
            : base(virtualDir)
        {
            this.provider = provider;
        }

        #region "Class Variables"
        private ArrayList children = new ArrayList();
        private ArrayList directories = new ArrayList();
        private ArrayList files = new ArrayList();
        private WebSitePathProvider provider;
        #endregion

        #region "Properties"
        public override IEnumerable Children
        {
            get { return children; }
        }

        public override IEnumerable Directories
        {
            get { return directories; }
        }

        public override IEnumerable Files
        {
            get { return files; }
        }

        public override bool IsDirectory
        {
            get
            {
                return base.IsDirectory;
            }
        }
        #endregion
    }
}
