// -----------------------------------------------------------------------
// <copyright file="SPListItemExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.SharePoint;


    public static class SPListItemExtensions
    {
        /// <summary>
        /// Returns the login name of an User-Field.
        /// </summary>
        public static string GetFieldValueUserLogin(this SPListItem item,
          string fieldName)
        {
            if (item != null)
            {
                SPFieldUserValue userValue =
                  new SPFieldUserValue(
                    item.Web, item[fieldName] as string);
                return userValue.User.LoginName;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets the value of a User-Field to a login name.
        /// </summary>
        public static void SetFieldValueUser(this SPListItem item,
          string fieldName, string loginName)
        {
            if (item != null)
            {
                item[fieldName] = item.Web.EnsureUser(loginName);
            }
        }

        /// <summary>
        /// Sets the value of a User-Field to an SPPrincipal 
        /// (SPGroup or SPUser).
        /// </summary>
        public static void SetFieldValueUser(this SPListItem item,
          string fieldName, SPPrincipal principal)
        {
            if (item != null)
            {
                item[fieldName] = principal;
            }
        }

        public static void SetFieldValueUser(this SPListItem item,
          string fieldName, IEnumerable<SPPrincipal> principals)
        {
            if (item != null)
            {
                SPFieldUserValueCollection fieldValues =
                  new SPFieldUserValueCollection();

                foreach (SPPrincipal principal in principals)
                {
                    fieldValues.Add(
                      new SPFieldUserValue(
                        item.Web, principal.ID, principal.Name));
                }
                item[fieldName] = fieldValues;
            }
        }

        /// <summary>
        /// Sets the value of a multivalue User-Field to 
        /// a list of user names.
        /// </summary>
        public static void SetFieldValueUser(this SPListItem item,
          string fieldName, IEnumerable<string> loginNames)
        {
            if (item != null)
            {
                SPFieldUserValueCollection fieldValues =
                  new SPFieldUserValueCollection();

                foreach (string loginName in loginNames)
                {
                    SPUser user = item.Web.EnsureUser(loginName);
                    fieldValues.Add(
                      new SPFieldUserValue(
                        item.Web, user.ID, user.Name));
                }

                item[fieldName] = fieldValues;
            }
        }

        /// <summary>
        /// Returns the value of a Lookup Field.
        /// </summary>
        private static string GetFieldValueLookup(this SPListItem item,
            string fieldName)
        {
            if (item != null)
            {
                SPFieldLookupValue lookupValue =
                    new SPFieldLookupValue(item[fieldName] as string);
                return lookupValue.LookupValue;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the value of a Lookup-Field with multiple values.
        /// </summary>
        public static IEnumerable<string> GetFieldValueLookupCollection(
            this SPListItem item, string fieldName)
        {
            List<string> result = new List<string>();
            if (item != null)
            {
                SPFieldLookupValueCollection values =
                    item[fieldName] as SPFieldLookupValueCollection;

                foreach (SPFieldLookupValue value in values)
                {
                    result.Add(value.LookupValue);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the SPFieldLookupValue instance of a lookup value. 
        /// The ID value will be obtained using SPQuery.
        /// </summary>
        private static SPFieldLookupValue GetLookupValue(
            SPWeb web, SPFieldLookup field, string lookupValue)
        {
            string queryFormat =
                @"<Where>
            <Eq>
                <FieldRef Name='{0}' />
                <Value Type='Text'>{1}</Value>
            </Eq>
          </Where>";

            string queryText =
                string.Format(queryFormat, field.LookupField, lookupValue);
            SPList lookupList = web.Lists[new Guid(field.LookupList)];

            SPListItemCollection lookupItems =
                lookupList.GetItems(new SPQuery() { Query = queryText });

            if (lookupItems.Count > 0)
            {
                int lookupId =
                    Convert.ToInt32(lookupItems[0][SPBuiltInFieldId.ID]);

                return new SPFieldLookupValue(lookupId, lookupValue);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the value of a Lookup-Field.
        /// </summary>
        public static void SetFieldValueLookup(
            this SPListItem item, string fieldName, string lookupValue)
        {
            if (item != null)
            {
                SPFieldLookup field =
                    item.Fields.GetField(fieldName) as SPFieldLookup;
                item[fieldName] = GetLookupValue(item.Web, field, lookupValue);
            }
            else
            {
                item[fieldName] = null;
            }
        }

        /// <summary>
        /// Set the values of a Lookup-Field with multiple values allowed.
        /// </summary>
        public static void SetFieldValueLookup(this SPListItem item,
            string fieldName, IEnumerable<string> lookupValues)
        {
            if (item != null)
            {
                SPFieldLookup field =
                    item.Fields.GetField(fieldName) as SPFieldLookup;

                SPFieldLookupValueCollection fieldValues =
                    new SPFieldLookupValueCollection();

                foreach (string lookupValue in lookupValues)
                {
                    fieldValues.Add(
                        GetLookupValue(item.Web, field, lookupValue));
                }
                item[fieldName] = fieldValues;
            }
        }

        /// <summary>
        /// Returns the value of an Url-Field.
        /// </summary>
        public static string GetFieldValueUrl(
            this SPListItem item, string fieldName)
        {
            if (item != null)
            {
                SPFieldUrlValue urlValue =
                    new SPFieldUrlValue(item[fieldName] as string);
                return urlValue.Url;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets the value of an URL-Field.
        /// </summary>
        public static void SetFieldValueUrl(this SPListItem item,
            string fieldName, string url, string description)
        {
            if (item != null)
            {
                item[fieldName] =
                    new SPFieldUrlValue()
                    {
                        Description = description,
                        Url = url
                    };
            }
        }
    }
}
