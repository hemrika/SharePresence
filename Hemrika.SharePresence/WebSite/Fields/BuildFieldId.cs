// -----------------------------------------------------------------------
// <copyright file="FieldId.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.Security;
    using System.Security.Permissions;
    using Microsoft.SharePoint;

    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public sealed class BuildFieldId
    {
        private BuildFieldId()
        {
        }

        public static Guid ArticleDate
        {
            get
            {
                return new Guid("71316CEA-40A0-49f3-8659-F0CEFDBDBD4F");
            }
        }

        public static Guid PublishingAssociatedContentType
        {
            get
            {
                return new Guid("4B3AC8E4-69A7-4B10-A8FD-386841B277A7");
            }
        }


        public static Guid AverageRatings
        {
            get
            {
                return new Guid("5a14d1ab-1513-48c7-97b3-657a5ba6c742");
            }
        }

        public static Guid ByLine
        {
            get
            {
                return new Guid("D3429CC9-ADC4-439b-84A8-5679070F84CB");
            }
        }


        public static Guid Contact
        {
            get
            {
                return new Guid("aea1a4dd-0f19-417d-8721-95a1d28762ab");
            }
        }

        public static Guid ContentType
        {
            get
            {
                return SPBuiltInFieldId.ContentType;
            }
        }

        public static Guid ContentTypeId
        {
            get
            {
                return SPBuiltInFieldId.ContentTypeId;
            }
        }

        public static Guid CreatedBy
        {
            get
            {
                return SPBuiltInFieldId.Author;
            }
        }

        public static Guid CreatedDate
        {
            get
            {
                return SPBuiltInFieldId.Created;
            }
        }

        public static Guid Description
        {
            get
            {
                return SPBuiltInFieldId.Comments;
            }
        }


        public static Guid PublishingPageDesign
        {
            get
            {
                return new Guid("C5EC9DDF-5436-4D9E-8CF1-4D143E8FBD39");
            }
        }

        public static Guid Title
        {
            get
            {
                return SPBuiltInFieldId.Title;
            }
        }

        public static Guid NewsGenres
        {
            get
            {
                return new Guid("E4C27D5D-E49B-4F28-BB0C-A2B089337EAF");
            }
        }

        public static Guid SchemaType
        {
            get
            {
                return new Guid("FD160718-1FCB-4AD2-A87E-B884D7362BB4");
            }
        }

        public static Guid SchemaProperty
        {
            get
            {
                return new Guid("D4F88259-AD77-41BD-987E-673F4FB53D2A");
            }
        }

        public static Guid Rating
        {
            get
            {
                return new Guid("6B16C783-5F64-4B2D-8D2B-0379ADE9EE2F");
            }
        }

        public static Guid Bookmark
        {
            get
            {
                return new Guid("00BFEFE9-DBA7-4987-A135-CD3377CA1FF2");
            }
        }

        public static Guid Video
        {
            get
            {
                return new Guid("6652B606-AC4F-4434-8375-2E3076D850ED");
            }
        }

        #region Content Files

        public static Guid Content_Width
        {
            get
            {
                return new Guid("208A8EE7-3397-404D-AF5D-35C5B4852DA5");
            }
        }

        public static Guid Content_Height
        {
            get
            {
                return new Guid("EE538D0E-28BC-420E-A166-291FABC0D286");
            }
        }

        public static Guid Content_Bitrate
        {
            get
            {
                return new Guid("D4BD8AEA-045E-47EF-8BED-CCCFE1B44D6A");
            }
        }

        public static Guid Content_Duration
        {
            get
            {
                return new Guid("E8985BC3-6B9A-4477-B090-CB22EC2D88D2");
            }
        }

        public static Guid Content_Loop
        {
            get
            {
                return new Guid("C89BB975-9279-417F-AE91-A7683ECC0686");
            }
        }

        public static Guid Content_Controls
        {
            get
            {
                return new Guid("457D932B-7F3F-459B-A94C-98A0517E0B4E");
            }
        }

        public static Guid Content_Buffer
        {
            get
            {
                return new Guid("F522CAF4-9CB2-4599-BA89-B71DFB4BCE92");
            }
        }

        public static Guid Content_AutoPlay
        {
            get
            {
                return new Guid("03C33677-8271-42E2-BC6A-6F241BA61E27");
            }
        }

        public static Guid Media_Query
        {
            get
            {
                return new Guid("D3644DFD-480C-436C-86D2-965084114B0A");
            }
        }

        public static Guid Video_Format
        {
            get
            {
                return new Guid("5EA75FF2-A707-46BA-BC46-3AFA00F8EA6D");
            }
        }

        public static Guid Audio_Format
        {
            get
            {
                return new Guid("C634790B-3CAD-4496-B1F8-6A09074B962B");
            }
        }

        public static Guid Track_Type
        {
            get
            {
                return new Guid("2BF1B941-D653-4C09-B33B-A3450ADB5371");
            }
        }

        public static Guid Track_Label
        {
            get
            {
                return new Guid("16103487-CE5F-44E9-8C51-E8B4250ACA63");
            }
        }

        public static Guid Track_Language
        {
            get
            {
                return new Guid("C1FA5C5E-ABA5-4B82-9AFA-E862F4BA362E");
            }
        }

        #endregion
    }
}
