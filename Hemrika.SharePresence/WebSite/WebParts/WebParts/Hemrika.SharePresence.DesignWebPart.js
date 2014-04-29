
function ULS_SP() {
    if (ULS_SP.caller) {
        ULS_SP.caller.ULSTeamName = "Windows SharePoint Services 4";
        ULS_SP.caller.ULSFileName = "/_layouts/Hemrika/WebSitePart/Hemrika.SharePresence.DesignWebPart.js";
    }
};

//var DesignEditorPart;
//var designDataSource;
//var datasources = [];
var updatepanel;
/*
function LoadDesignData() {
    if (datasources.length > 0) {
        alert(datasources.length);
        alert(datasources[0].Title);
    }
};
*/

//Dialog opening
function OpenDesignDataDialog(datasource) {

    var ctx = new SP.ClientContext.get_current();

    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Content/DesignDataSourceEditor.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);

    for (var prop in datasource) {
        if (datasource.hasOwnProperty(prop)) {

            var data = datasource[prop];
            if (data != null) {
                urlBuilder.addKeyValueQueryString(prop, datasource[prop]);
            }
        }
    }

    var dialogOptions = {
        url: urlBuilder.get_url(),
        title: "Edit Data Source",
        allowMaximize: true,
        showClose: true,
        width: 800,
        args: { DataSource: datasource },
        dialogReturnValueCallback: Function.createDelegate(null, CloseDesignDataDialog)
    };

    SP.UI.ModalDialog.showModalDialog(dialogOptions);
};

// Dialog callback
function CloseDesignDataDialog(result, eventArgs) {

    if (result == 1) {
        var ctx = new SP.ClientContext.get_current();

        var argument = "Update#;" + JSON.stringify(eventArgs);

        UpdateDataSources(argument, '');
    }
};

function DeleteDesignDataDialog(datasource) {

    var ctx = new SP.ClientContext.get_current();

    var argument = "Delete#;" + JSON.stringify(datasource);
    DeleteDataSource(argument, '');
};

function RefreshDesignData(arg, context) {
    //alert(arg);
    __doPostBack(updatepanel, '');
};
