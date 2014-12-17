// -----------------------------------------------------------------------
// <copyright file="SearchOptions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.WebPartPages;

    public enum DropDownModesEx
    {
        [Resources("", "", "SearchBox_DDMode1")]
        HideScopeDD = 1,
        [Resources("", "", "SearchBox_DDMode7")]
        HideScopeDD_DefaultContextual = 0,
        [Resources("", "", "SearchBox_DDMode2")]
        ShowDD = 2,
        [Resources("", "", "SearchBox_DDMode4")]
        ShowDD_DefaultContextual = 4,
        [Resources("", "", "SearchBox_DDMode3")]
        ShowDD_DefaultURL = 3,
        [Resources("", "", "SearchBox_DDMode5")]
        ShowDD_NoContextual = 5,
        [Resources("", "", "SearchBox_DDMode6")]
        ShowDD_NoContextual_DefaultURL = 6
    }

    public enum DropDownModes
    {
        [Resources("", "", "SearchBox_DDMode4")]
        DisplayContextualScopeDD = 1,
        [Resources("", "", "SearchBox_DDMode2")]
        DisplayScopeDD = 0,
        [Resources("", "", "SearchBox_DDMode1")]
        HideDD_NoScope = 3,
        [Resources("", "", "SearchBox_DDMode1")]
        HideDD_useDefaultScope = 2,
        [Resources("", "", "SearchBox_DDMode1")]
        HideScopeDD = 4,
        [Resources("", "", "SearchBox_DDMode7")]
        HideScopeDD_DefaultContextual = 5,
        [Resources("", "", "SearchBox_DDMode2")]
        ShowDD = 6,
        [Resources("", "", "SearchBox_DDMode4")]
        ShowDD_DefaultContextual = 8,
        [Resources("", "", "SearchBox_DDMode3")]
        ShowDD_DefaultURL = 7,
        [Resources("", "", "SearchBox_DDMode5")]
        ShowDD_NoContextual = 9,
        [Resources("", "", "SearchBox_DDMode6")]
        ShowDD_NoContextual_DefaultURL = 10
    }

    public enum QueryResultsDateTimeFormatType
    {
        [Resources("", "", "SearchResult_DateTimeFormat_Customized_PropertyName")]
        Customized = 1,
        [Resources("", "", "SearchResult_DateTimeFormat_Standard_PropertyName")]
        Standard = 0
    }
}
