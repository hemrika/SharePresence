using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hemrika.SharePresence.Google.Client;

namespace Hemrika.SharePresence.Google.Analytics
{
    public class ProfilesEntry : AbstractEntry
    {

    /// <summary>
    /// Lazy loading for the properties and tableId.
    /// </summary>
    private List<Property> properties;
    private List<CustomVariable> customVariables;
    //private List<Goal> goals;
    //private TableId tableId;

    /// <summary>
    /// Constructs a new AccountEntry instance
    /// </summary>
    public ProfilesEntry()
      : base()
    {
      this.AddExtension(new Property());
      //this.AddExtension(new TableId());
      this.AddExtension(new CustomVariable());
      //this.AddExtension(new Goal());
    }

    /// <summary>
    /// This field controls the properties.
    /// </summary>
    public List<Property> Properties
    {
      get
      {
        if (properties == null)
        {
          properties = FindExtensions<Property>(AnalyticsNameTable.XmlPropertyElement,
                              AnalyticsNameTable.gAnalyticsNamspace);
        }
        return properties;
      }
    }

    /// <summary>
    /// searches through the property list to find a specific one
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string FindPropertyValue(string name)
    {
      foreach (Property p in this.Properties)
      {
        if (p.Name == name)
        {
          return p.Value;
        }
      }
      return null;
    }

    public string AccountId
    {
        get
        {
            return FindPropertyValue("ga:accountId");
        }
    }

    public string WebPropertyId
    {
        get
        {
            return FindPropertyValue("ga:webPropertyId");
        }
    }

    public string ProfileName
    {
        get
        {
            return FindPropertyValue("ga:profileName");
        }
    }

    public string ProfileId
    {
        get
        {
            return FindPropertyValue("ga:profileId");
        }
    }

    public string TableId
    {
        get
        {
            return FindPropertyValue("ga:tableId");
        }
    }

    public string Currency
    {
        get
        {
            return FindPropertyValue("ga:currency");
        }
    }

    public string Timezone
    {
        get
        {
            return FindPropertyValue("ga:timezone");
        }
    }

        /*
    public ProfilesQuery[] Profiles
    {
        get
        {
            ProfilesQuery[] webproperties = null;
            List<AtomLink> webs = Links.Where(x => x.Rel.EndsWith("#child")).ToList<AtomLink>();
            if (webs.Count > 0)
            {
                List<ProfilesQuery> queries = new List<ProfilesQuery>();
                foreach (AtomLink link in webs)
                {
                    ProfilesQuery query = new ProfilesQuery(link.HRef.Content);
                    queries.Add(query);
                }
                webproperties = queries.ToArray<ProfilesQuery>();
            }

            return webproperties;
        }
    }
    */

    /// <summary>
    /// This field controls the Custom Variables.
    /// </summary>
    public List<CustomVariable> CustomVariables
    {
      get
      {
        if (customVariables == null)
        {
          customVariables = FindExtensions<CustomVariable>(AnalyticsNameTable.XmlCustomVariableElement,
                                                           AnalyticsNameTable.gaNamespace);
        }
        return customVariables;
      }
    }

  }
}