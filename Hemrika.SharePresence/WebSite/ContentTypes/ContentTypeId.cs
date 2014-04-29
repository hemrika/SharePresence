// -----------------------------------------------------------------------
// <copyright file="ContentTypeId.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.ContentTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ContentTypeId
    {
        // Methods
        private ContentTypeId()
        {
        }

        public static SPContentTypeId Content
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B795");
            }
        }

        public static SPContentTypeId Image
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B795004E4DAC72072A4482B5BE12E109C385F0");
            }
        }

        public static SPContentTypeId Audio
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B79500E8C49E63F02F43F7A94182593CF0A385");
            }
        }

        public static SPContentTypeId Video
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B79500745355C921FB48549146166307BB4ED1");
            }
        }

        public static SPContentTypeId MP4
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B79500EB58EBAF84C84E7E9EC6662CD91C0D9E");
            }
        }

        public static SPContentTypeId OGG
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B7950043719C81EE934057ACCAD438B8B9A5B5");
            }
        }

        public static SPContentTypeId WebM
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B79500C30D8C4A5CF549198D36E68DF6895910");
            }
        }

        public static SPContentTypeId FLV
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B795001D9735D7D3274B39BE63E7481AC721DD");
            }
        }

        public static SPContentTypeId MPG
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B79500BB7672E7108E47C0BC018742B601D7B0");
            }
        }

        public static SPContentTypeId AVI
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B79500E1BB3FAC3C6942C8BB1094C63924AED2");
            }
        }

        public static SPContentTypeId MOV
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B795000FA90D277C8F47D18BCEEC8C17129A3D");
            }
        }

        public static SPContentTypeId WMV
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B79500EA53A380C5304FCE9B994E17FB5DFDA0");
            }
        }

        public static SPContentTypeId Caption
        {
            get
            {
                return new SPContentTypeId("0x010100DE7C86B75EA34F7EBC0D42B490F2B79500487D920FBFD74147939AF1E6CE85652E");
            }
        }


        public static SPContentTypeId MasterPage
        {
            get
            {
                return new SPContentTypeId("0x01010500AFA50A7CFA6B4D37AC006F4906166E4A");
            }
        }

        public static SPContentTypeId PageTemplate
        {
            get
            {
                return new SPContentTypeId("0x010100E2E99BB760B34490935A1126C04A810F");
            }
        }

        public static SPContentTypeId PageLayout
        {
            get
            {
                return new SPContentTypeId("0x010100E2E99BB760B34490935A1126C04A810F00CDC8B569999E4B2C9888D1BC77AC0A79");
            }
        }

        public static SPContentTypeId WebSitePage
        {
            get
            {
                return new SPContentTypeId("0x010100E2E99BB760B34490935A1126C04A810F00A825C106CE14487CAAF9E6EA127B8165");
            }
        }

        public static SPContentTypeId VideoPage
        {
            get
            {
                return new SPContentTypeId("0x010100E2E99BB760B34490935A1126C04A810F002CEF2C33A1F4934CBB3C35977645B818");
            }
        }

        public static SPContentTypeId BlogPage
        {
            get
            {
                return new SPContentTypeId("0x010100E2E99BB760B34490935A1126C04A810F00DEE5CA4621DECB44A434B4589E0FF4EC");
            }
        }

        public static SPContentTypeId PressRelease
        {
            get
            {
                return new SPContentTypeId("0x010100E2E99BB760B34490935A1126C04A810F00A825C106CE14487CAAF9E6EA127B81650077636cd95c294c198badcdb3e7ee62ab");
            }
        }

        #region GateKeeper
        public static SPContentTypeId GateKeeper_HoneyPot
        {
            get
            {
                return new SPContentTypeId("0x0100B02A6273A93A75478BFFA53CC49ABA75");
            }
        }
        public static SPContentTypeId GateKeeper_HTTP
        {
            get
            {
                return new SPContentTypeId("0x0100481213A85D86C04185CCC53E27B2E9DC");
            }
        }
        public static SPContentTypeId GateKeeper_Drone
        {
            get
            {
                return new SPContentTypeId("0x010046EA71B010823F4193BDCAC6F54AC4B3");
            }
        }
        public static SPContentTypeId GateKeeper_Proxy
        {
            get
            {
                return new SPContentTypeId("0x0100B153CB75BEE5844580A0A78C49EE8ABF");
            }
        }
        public static SPContentTypeId GateKeeper_Entry
        {
            get
            {
                return new SPContentTypeId("0x0100FE525E38AA64A640B310538E993529C8");
            }
        }

        #endregion
    }
}
