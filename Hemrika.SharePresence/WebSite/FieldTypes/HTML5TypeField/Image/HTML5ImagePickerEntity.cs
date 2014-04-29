// -----------------------------------------------------------------------
// <copyright file="HTML5ImagePickerEntity.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint;
    using System.Collections;
    using System.Xml.Serialization;
    using System.Xml;
    using System.IO;
    using System.Globalization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HTML5ImagePickerEntity : PickerEntity
    {
        public HTML5ImagePickerEntity() : base() { }

        public HTML5ImagePickerEntity(SPListItem item)
            : base()
        {
            if (item != null)
            {
                ItemId = item.UniqueId;
                ListId = item.ParentList.ID;
                WebId = item.Web.ID;
                Src = Convert.ToString(item.File.ServerRelativeUrl);
                Width = Convert.ToInt32(item["ows_ImageWidth"]);
                Height = Convert.ToInt32(item["ows_ImageHeight"]);
                Preview = Convert.ToString(item["ows_EncodedAbsThumbnailUrl"]);
                Keywords = Convert.ToString(item["ows_Keywords"]);
                Alt = Convert.ToString(item["ows_Description"]);
                Key = Src;
                DisplayText = Src;
                IsResolved = true;
            }
            else
            {
                IsResolved = false;
            }
        }

        public HTML5ImagePickerEntity(Hashtable entityData)
            : base()
        {
            this.EntityData = entityData;
            this.Key = this.Src;
            this.DisplayText = this.Alt;
        }

        public HTML5ImagePickerEntity(PickerEntity entity)
        {
            //HTML5ImagePickerEntity entity = new HTML5ImagePickerEntity(pickerEntity);
            this.EntityData = entity.EntityData;
            this.Key = entity.Key;
            this.Src = entity.Key;
            this.DisplayText = entity.DisplayText;
            try
            {
                if (entity.EntityData.ContainsKey("ItemId"))
                {
                    this.ItemId = new Guid(entity.EntityData["ItemId"].ToString());
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }            
            
            try
            {
                if (entity.EntityData.ContainsKey("WebId"))
                {
                    this.WebId = new Guid(entity.EntityData["WebId"].ToString());
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            this.IsResolved = entity.IsResolved;
        }

        public HTML5ImagePickerEntity(HTML5ImageField field)
        {
            this.Alt = field.Alt;
            this.DisplayText = field.Src;
            this.Description = field.Src;
            this.IsResolved = true;
            this.Key = field.Src;
            this.Src = field.Src;
            this.ItemId = field.ItemId;
            this.WebId = field.WebId;
        }

        public Guid WebId
        {
            get
            {
                if (this.EntityData.ContainsKey("WebId"))
                {
                    return new Guid(this.EntityData["WebId"].ToString());
                }
                else
                {
                    return Guid.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("WebId"))
                {
                    this.EntityData["WebId"] = value;
                }
                else
                {
                    this.EntityData.Add("WebId", value);
                }
            }
        }

        public Guid ListId
        {
            get
            {
                if (this.EntityData.ContainsKey("ListId"))
                {
                    return new Guid(this.EntityData["ListId"].ToString());
                }
                else
                {
                    return Guid.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("ListId"))
                {
                    this.EntityData["ListId"] = value;
                }
                else
                {
                    this.EntityData.Add("ListId", value);
                }
            }
        }

        public Guid ItemId
        {
            get
            {
                if (this.EntityData.ContainsKey("ItemId"))
                {
                    return new Guid(this.EntityData["ItemId"].ToString());
                }
                else
                {
                    return Guid.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("ItemId"))
                {
                    this.EntityData["ItemId"] = value;
                }
                else
                {
                    this.EntityData.Add("ItemId", value);
                }
            }
        }

        public string Src
        {
            get
            {
                if (this.EntityData.ContainsKey("Src"))
                {
                    return this.EntityData["Src"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("Src"))
                {
                    this.EntityData["Src"] = value;
                }
                else
                {
                    this.EntityData.Add("Src", value);
                }
            }
        }

        public string Alt
        {
            get
            {
                if (this.EntityData.ContainsKey("Alt"))
                {
                    return this.EntityData["Alt"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("Alt"))
                {
                    this.EntityData["Alt"] = value;
                }
                else
                {
                    this.EntityData.Add("Alt", value);
                }
            }
        }

        /*
        public string Ismap
        {
            get
            {
                if (this.EntityData.ContainsKey("Ismap"))
                {
                    return this.EntityData["Ismap"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("Ismap"))
                {
                    this.EntityData["Ismap"] = value;
                }
                else
                {
                    this.EntityData.Add("Ismap", value);
                }
            }
        }
        */

        /*
        public string Usemap
        {
            get
            {
                if (this.EntityData.ContainsKey("Usemap"))
                {
                    return this.EntityData["Usemap"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("Usemap"))
                {
                    this.EntityData["Usemap"] = value;
                }
                else
                {
                    this.EntityData.Add("Usemap", value);
                }
            }
        }
        */

        public int Height
        {
            get
            {
                if (this.EntityData.ContainsKey("Height"))
                {
                    return Convert.ToInt32(this.EntityData["Height"]);
                }
                else
                {
                    return 32;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("Height"))
                {
                    this.EntityData["Height"] = value;
                }
                else
                {
                    this.EntityData.Add("Height", value);
                }
            }
        }

        public int Width
        {
            get
            {
                if (this.EntityData.ContainsKey("Width"))
                {
                    return Convert.ToInt32(this.EntityData["Width"]);
                }
                else
                {
                    return 32;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("Width"))
                {
                    this.EntityData["Width"] = value;
                }
                else
                {
                    this.EntityData.Add("Width", value);
                }
            }
        }

        public string Preview
        {
            get
            {
                if (this.EntityData.ContainsKey("Preview"))
                {
                    return Convert.ToString(this.EntityData["Preview"]);
                }
                else
                {
                    return string.Empty; ;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("Preview"))
                {
                    this.EntityData["Preview"] = value;
                }
                else
                {
                    this.EntityData.Add("Preview", value);
                }
            }
        }

        public string Keywords
        {
            get
            {
                if (this.EntityData.ContainsKey("Keywords"))
                {
                    return this.EntityData["Keywords"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("Keywords"))
                {
                    this.EntityData["Keywords"] = value;
                }
                else
                {
                    this.EntityData.Add("Keywords", value);
                }
            }
        }


        public string Web
        {
            get
            {
                if (this.EntityData.ContainsKey("Web"))
                {
                    return this.EntityData["Web"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("Web"))
                {
                    this.EntityData["Web"] = value;
                }
                else
                {
                    this.EntityData.Add("Web", value);
                }
            }
        }

        public string List
        {
            get
            {
                if (this.EntityData.ContainsKey("List"))
                {
                    return this.EntityData["List"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.EntityData.ContainsKey("List"))
                {
                    this.EntityData["List"] = value;
                }
                else
                {
                    this.EntityData.Add("List", value);
                }
            }
        }      
    }
}
