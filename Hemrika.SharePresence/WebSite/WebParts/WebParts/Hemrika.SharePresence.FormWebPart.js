
function ULS_SP() {
    if (ULS_SP.caller) {
        ULS_SP.caller.ULSTeamName = "Windows SharePoint Services 4";
        ULS_SP.caller.ULSFileName = "/_layouts/Hemrika/WebSitePart/Hemrika.SharePresence.FormWebPart.js";
    }
};

var updatepanel;

//Dialog opening
function OpenFormDataDialog(datasource) {

    var ctx = new SP.ClientContext.get_current();

    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Content/FormDataSourceEditor.aspx');
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
        dialogReturnValueCallback: Function.createDelegate(null, CloseFormDataDialog)
    };

    SP.UI.ModalDialog.showModalDialog(dialogOptions);
};

// Dialog callback
function CloseFormDataDialog(result, eventArgs) {

    if (result == 1) {
        var ctx = new SP.ClientContext.get_current();

        var argument = "Update#;" + JSON.stringify(eventArgs);

        UpdateDataSources(argument, '');
    }
};

function DeleteFormDataDialog(datasource) {

    var ctx = new SP.ClientContext.get_current();

    var argument = "Delete#;" + JSON.stringify(datasource);
    DeleteDataSource(argument, '');
};

function RefreshFormData(arg, context) {
    __doPostBack(updatepanel, '');
};
