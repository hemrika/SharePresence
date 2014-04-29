using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using Hemrika.SharePresence.SPLicense.LicenseFile;
using Microsoft.SharePoint.Administration;
using System.Collections.Generic;
using Hemrika.SharePresence.SPLicense.LicenseFile.Constraints;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class UploadLicense : UserControl
    {
        private SPLicenseFile license;

        protected void Page_Load(object sender, EventArgs e)
        {
            //tbx_Result.Text = SPFarm.Local.Id.ToString();
            tbx_Result.Text = String.Empty;
            license = new SPLicenseFile(null, String.Empty);
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            HttpFileCollection hfc = Request.Files;
            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile file = hfc[i];

                Stream openStream;

                if ((openStream = file.InputStream) != null)
                {
                    BufferedStream bufferStream = new BufferedStream(openStream);
                    StreamReader streamReader = new StreamReader(bufferStream);
                    string str = streamReader.ReadToEnd();

                    bufferStream.Position = 0;

                    license = SPLicenseFile.LoadFile(bufferStream, null, false, string.Empty);

                    streamReader.Close();
                    bufferStream.Close();
                    openStream.Close();

                }

                List<IConstraint> contraints = license.Constraints;

                foreach (IConstraint contraint in contraints)
                {
                    FarmConstraint farmContraint = contraint as FarmConstraint;
                    if (farmContraint != null)
                    {
                        foreach (string farm in farmContraint.Farms)
                        {
                            tbx_Result.Text += farm + Environment.NewLine;
                        }
                    }

                    DomainConstraint domainContraint = contraint as DomainConstraint;
                    if (domainContraint != null)
                    {
                        foreach (string domain in domainContraint.Domains)
                        {
                            tbx_Result.Text += domain + Environment.NewLine;
                        }
                    }
                }
                //SPLicense.LicenseFile.Constraints.FarmConstraint
                //tbx_Result.Text = license.LicenseKey+ Environment.NewLine;
                tbx_Result.Text += license.Product.ShortName+Environment.NewLine;
                tbx_Result.Text += license.User.Organization + Environment.NewLine;
                tbx_Result.Text += license.Issuer.FullName + Environment.NewLine;

                /*
                if (license.Validate())
                {
                    tbx_Result.Text += license.ToXmlString();
                }
                */
            }
        }
    }
}
