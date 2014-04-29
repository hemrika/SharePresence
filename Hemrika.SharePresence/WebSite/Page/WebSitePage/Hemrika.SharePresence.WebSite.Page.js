function ULS_SP() {
    if (ULS_SP.caller) {
        ULS_SP.caller.ULSTeamName = "Windows SharePoint Services 4";
        ULS_SP.caller.ULSFileName = "/_layouts/Hemrika/WebSitePage/Hemrika.SharePresence.WebSite.Page.js";
    }
};


Type.registerNamespace('Hemrika.SharePresence.WebSite.Page');

// RibbonApp Page Component
Hemrika.SharePresence.WebSite.Page.PageComponent = function () {
    ULS_SP();
    Hemrika.SharePresence.WebSite.Page.PageComponent.initializeBase(this);
};


Hemrika.SharePresence.WebSite.Page.PageComponent.initialize = function () {
    ULS_SP();
    ExecuteOrDelayUntilScriptLoaded(Function.createDelegate(null, Hemrika.SharePresence.WebSite.Page.PageComponent.initializePageComponent), 'SP.Ribbon.js');
    ExecuteOrDelayUntilScriptLoaded(DiscoverRobotsEntry, "SP.Ribbon.js");
    
};


Hemrika.SharePresence.WebSite.Page.PageComponent.initializePageComponent = function () {
    ULS_SP();
    var ribbonPageManager = SP.Ribbon.PageManager.get_instance();
    if (null !== ribbonPageManager) {
        ribbonPageManager.addPageComponent(Hemrika.SharePresence.WebSite.Page.PageComponent.instance);
        ribbonPageManager.get_focusManager().requestFocusForComponent(Hemrika.SharePresence.WebSite.Page.PageComponent.instance);
    }
};


Hemrika.SharePresence.WebSite.Page.PageComponent.refreshRibbonStatus = function () {
    SP.Ribbon.PageManager.get_instance().get_commandDispatcher().executeCommand(Commands.CommandIds.ApplicationStateChanged, null);
};


Hemrika.SharePresence.WebSite.Page.PageComponent.prototype = {
    getFocusedCommands: function () {
        ULS_SP();
        return [];
    },
    getGlobalCommands: function () {
        ULS_SP();
        return getGlobalCommands();
    },
    isFocusable: function () {
        ULS_SP();
        return true;
    },
    receiveFocus: function () {
        ULS_SP();
        return true;
    },
    yieldFocus: function () {
        ULS_SP();
        return true;
    },
    canHandleCommand: function (commandId) {
        ULS_SP();
        return commandEnabled(commandId);
    },
    handleCommand: function (commandId, properties, sequence) {
        ULS_SP();

        return handleCommand(commandId, properties, sequence);
    }
};


// Register classes
Hemrika.SharePresence.WebSite.Page.PageComponent.registerClass('Hemrika.SharePresence.WebSite.Page.PageComponent', CUI.Page.PageComponent);
Hemrika.SharePresence.WebSite.Page.PageComponent.instance = new Hemrika.SharePresence.WebSite.Page.PageComponent();

function getGlobalCommands() {
    return ['PageStateGroupSetHomePage', 'PageStateGroupIncomingLinks', 'AnalyticsReportPage', 'AnalyticsReportWeb', 'AnalyticsReportSite', 'NewRobotsButton', 'EditRobotsButton', 'SettingsMetaData', 'EditMetaData', 'EditOpenGraph', 'EditFaceBook', 'EditTwitter', 'SettingsSiteMap', 'EditSemantic', 'CompanySettings', 'ChangePageLayout', 'NavigationSettings'];
};

function commandEnabled(commandId) {
    if (commandId === 'PageStateGroupSetHomePage') {
        if (_WebSitePageContextInfo.wspHomePage == "True") {

            return false;
        }
        return true;
        //return SP.Ribbon.Utility.get_$U() && SP.Ribbon.Utility.get_$U().has(31);
    }
    
    if (commandId == 'PageStateGroupIncomingLinks') {
        return true;
    }
    if (commandId == 'AnalyticsReportPage' || commandId == 'AnalyticsReportSite') {
        return true;
    }

    if (commandId == 'NewRobotsButton' || commandId == 'EditRobotsButton') {
        DiscoverRobotsEntry();

        if (commandId == "NewRobotsButton" && robotsId == null) {
            return true;
        }

        if (commandId == "EditRobotsButton" && robotsId != null) {
            return true;
        }
        return false;
    }

    if (commandId == 'EditOpenGraph' || commandId == 'EditFaceBook' || commandId == 'EditTwitter' || commandId == 'EditMetaData' || commandId == 'SettingsMetaData') {
        if (_WebSitePageContextInfo.wspCheckedOut == "None") {

            return false;
        }
        return true;
    }

    if (commandId == 'SettingsSiteMap') {
        return true;
    }

    if (commandId == 'EditSemantic') {
        return true;
    }

    if (commandId == 'CompanySettings') {
        return true;
    }
    if (commandId == 'NavigationSettings') {
        return true;
    }
    
    if (commandId == 'ChangePageLayout') {
        if (document.forms[MSOWebPartPageFormName].MSOLayout_InDesignMode.value == 0) {
            return true;
        }
        return false;
    }
    
    if (commandId == 'AnalyticsReportWeb') {
        if (_spPageContextInfo.webServerRelativeUrl.length > 1) {
            return true;
        }
        else {
            return false;
        }
    }

 return false;
};

function handleCommand(commandId, properties, sequence) {
    if (commandId === 'PageStateGroupSetHomePage') {
        if (!window.confirm(SP.Res.confirmWelcomePage))
            return;
        PageStateGroupSetHomePage(commandId, properties, sequence);
        return true;
    }

    if (commandId === 'PageStateGroupIncomingLinks') {

        PageStateGroupIncomingLinks(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'AnalyticsReportPage') {
        ShowAnalyticsReportPage(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'AnalyticsReportWeb') {
        ShowAnalyticsReportWeb(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'AnalyticsReportSite') {
        ShowAnalyticsReportSite(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'EditRobotsButton') {
        EditRobotsItem(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'NewRobotsButton') {
        NewRobotsItem(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'EditMetaData') {
        EditMetaData(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'EditOpenGraph') {
        EditOpenGraph(commandId, properties, sequence);
        return true;
    }
    if (commandId == 'EditFaceBook') {
        EditFaceBook(commandId, properties, sequence);
        return true;
    }
    if (commandId == 'EditTwitter') {
        EditTwitter(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'SettingsMetaData') {
        SettingsMetaData(commandId, properties, sequence);
        return true;
    }
    
    if (commandId == 'SettingsSiteMap') {
        SettingsSiteMap(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'EditSemantic') {
        EditSemantic(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'CompanySettings') {
        CompanySettings(commandId, properties, sequence);
        return true;
    }

    if (commandId == 'NavigationSettings') {
        NavigationSettings(commandId, properties, sequence);
        return true;
    }
    
    return false;
};

function PageStateGroupSetHomePage(commandId, properties, sequence) {
    if (commandId === 'PageStateGroupSetHomePage') {
        SP.Ribbon.PageState.PageStateHandler.popupWaitScreen('Page State', '');
        __theFormPostData = "";
        __theFormPostCollection = new Array();
        WebForm_OnSubmit(); WebForm_InitCallback();
        _spResetFormOnSubmitCalledFlag();
        WebForm_DoCallback('ctl02', 'PageStateGroupSetHomePage', SP.Ribbon.PageState.Handlers.GenericCompleteHandler, SP.Ribbon.PageState.Handlers.GenericError, null, true);
        //return true;
        __doPostBack(MSOWebPartPageFormName, '');
   }
 return false;
};

function PageStateGroupIncomingLinks(commandId, properties, sequence) {
    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('BackLinks.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('List', (new SP.Guid(_spPageContextInfo.pageListId)).toString());
    urlBuilder.addKeyValueQueryString('ID', _spPageContextInfo.pageItemId.toString());
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Page Incoming Links',
        allowMaximize: true,
        showClose: true,
        width: 800,
        height: 600,
        dialogReturnValueCallback: PageStateGroupIncomingLinksCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function PageStateGroupIncomingLinksCallBack(dialogResult, returnValue) {
    SP.UI.Notify.addNotification('Operation Successful!');
    return true;
};

function ShowAnalyticsReportPage(commandId, properties, sequence) {

    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Analytics/GoogleAnalyticsPage.aspx');
    var report_array = properties['SourceControlId'].toString().split(".");
    var report = report_array[report_array.length - 1].toString();
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('Path', _WebSitePageContextInfo.wspUrl);
    urlBuilder.addKeyValueQueryString('Report', report);
    var options = {
        url: urlBuilder.get_url(),
        tite: report,
        allowMaximize: true,
        showClose: true//,
        //dialogReturnValueCallback: ShowAnalyticsReportCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function ShowAnalyticsReportWeb(commandId, properties, sequence) {

    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Analytics/GoogleAnalyticsWeb.aspx');
    var report_array = properties['SourceControlId'].toString().split(".");
    var report = report_array[report_array.length - 1].toString();

    var ctx = new SP.ClientContext.get_current();
    var path = '~'+_spPageContextInfo.webServerRelativeUrl.toString()+'*';
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('Path', path);
    urlBuilder.addKeyValueQueryString('Report', report);

    var newWindow = window.open(urlBuilder.get_url(), '_blank');
    newWindow.focus();
    return false;

    /*
    var options = {
        url: urlBuilder.get_url(),
        tite: report,
        allowMaximize: true,
        showClose: true,
        width: 800,
        height: 600,
        dialogReturnValueCallback: ShowAnalyticsReportCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
    */
};

function ShowAnalyticsReportSite(commandId, properties, sequence) {

    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Analytics/GoogleAnalyticsSite.aspx');
    var report_array = properties['SourceControlId'].toString().split(".");
    var report = report_array[report_array.length - 1].toString();
    
    if (report == "Settings") {
        layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Analytics/ManageGoogleAccountSettings.aspx');
    }

    if (report == "GoogleAnalytics") {
        layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Analytics/GoogleAnalytics.aspx');
    }

    if (report == "CustomReport") {
        layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Analytics/GoogleAnalytics.aspx');
    }

    var ctx = new SP.ClientContext.get_current();
    var path = '~' + _spPageContextInfo.siteServerRelativeUrl.toString() + '*';
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('Path', path);
    urlBuilder.addKeyValueQueryString('Report', report);

    var newWindow = window.open(urlBuilder.get_url(), '_blank');
    newWindow.focus();
    return false;

    /*
    var options = {
        url: urlBuilder.get_url(),
        tite: report,
        allowMaximize: true,
        showClose: true,
        width: 800,
        height: 600,
        dialogReturnValueCallback: ShowAnalyticsReportCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
    */
};

function ShowMaintenancePage(commandId, properties, sequence) {

    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('spcontnt.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('Url', _WebSitePageContextInfo.wspUrl);
    var options = {
        url: urlBuilder.get_url(),
        tite: "Webpart Maintenance",
        allowMaximize: true,
        showClose: true,
        width: 820
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

var webpartzone = null;

function SetWebpartZone(zoneId) {
    webpartzone = zoneId;
    
    if (webpartadder) {
        webpartadder._setZone(webpartzone);
    }

    var label = document.getElementById("Ribbon.Hemrika.SharePresence.Content.WebpartZone.SelectedZoneLabel-Medium").lastElementChild;
    label.textContent = webpartzone;
    label.innerText = "Selected: " + webpartzone;
    SP.Ribbon.PageManager.get_instance().get_commandDispatcher().executeCommand(Commands.CommandIds.ApplicationStateChanged, null);
    return true;
}

function AddWebPartToPage(controlId, webpartId) {
    SP.Ribbon.PageState.PageStateHandler.popupWaitScreen("Adding WebPart", "Adding WebPart on Page.");
    __doPostBack(controlId, webpartId + '#;' + webpartzone);
/*
    SP.Ribbon.PageState.PageStateHandler.popupWaitScreen('Adding WebPart in ' + webpartzone,"Adding Webpart on Page.");
    __theFormPostData = "";
    __theFormPostCollection = new Array();
    WebForm_OnSubmit(); 
    WebForm_InitCallback();
    _spResetFormOnSubmitCalledFlag();
    if (webpartzone != null) {
        WebForm.
        WebForm_DoCallback(controlId, webpartId + '#;' + webpartzone, SP.Ribbon.PageState.Handlers.GenericCompleteHandler, SP.Ribbon.PageState.Handlers.GenericError, null, false);
        SP.Ribbon.PageManager.get_instance().get_commandDispatcher().executeCommand(Commands.CommandIds.ApplicationStateChanged, null);
        //__doPostBack(MSOWebPartPageFormName, '');
        //SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);
        //history.go(0);
        //window.location.href = window.location.href
        return true;
    }
    return false;
    */
};

function AddWebPartToPageCallback(dialogResult, returnValue) {
    SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);
    //window.location.href.reload(true);
};

var clientContext;
var ControlId;
var ContentType;
var PageLayout;

function ChangePageLayout(controlId, contentType, pageLayout) {

    this.clientContext = new SP.ClientContext();

    this.ControlId = controlId;
    this.ContentType = contentType;
    this.PageLayout = pageLayout;
    SP.Ribbon.PageState.PageStateHandler.popupWaitScreen("Changing PageLayout for this page.", "Working...");
    __doPostBack(ControlId, ContentType + '#;' + PageLayout);
    clientContext.executeQueryAsync(Function.createDelegate(this, this.onChangePageLayoutSucceeded), Function.createDelegate(this, this.onChangePageLayoutFailed));
};

function onChangePageLayoutSucceeded() {

    SP.Ribbon.PageState.PageStateHandler.ignoreNextUnload = true;
    SP.Ribbon.PageState.PageStateHandler.EnableSaveBeforeNavigate(true);

    SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);

    clientContext.executeQueryAsync(Function.createDelegate(this, this.onChangePageLayoutCallBack), Function.createDelegate(this, this.onChangePageLayoutFailed));

    var dt = new Date();
    while ((new Date()) - dt <= 3000) { /* Do nothing */ }

    SP.Utilities.HttpUtility.navigateTo(_WebSitePageContextInfo.wspUrl);
}

function onChangePageLayoutCallBack(sender, args) {

/*
    SP.Ribbon.PageState.PageStateHandler.ignoreNextUnload = true;
    SP.Ribbon.PageState.PageStateHandler.EnableSaveBeforeNavigate(true);

    var $v_0 = SP.Ribbon.PageManager.get_instance();
    if ($v_0) {
        $v_0.get_commandDispatcher().executeCommand('PageStateGroupSave', null);
    }
*/
}

function onChangePageLayoutFailed(sender, args) {

    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}

function ShowPagelayoutList() {
    var ctx = SP.ClientContext.get_current();
    var site = '{SiteUrl}';
    var url = '/_catalogs/masterpage/Forms/AllItems.aspx';
    var options = { url: url, tite: 'Page Layouts', allowMaximize: true, showClose: true, width: 800, height: 600, status: false, help: false };
    SP.UI.ModalDialog.showModalDialog(options);
}

function closeModalDialog() {
    SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.OK, "close");
};

var webpartadder = null;

function ShowWebPartAdder(commandId) {
    var toggle = false;

    if (commandId == "insertWebPart") {
        if (webpartadder) {
            if (webpartadder._visible) {
                toggle = true;
            }
        }
    }
    else {
        webpartzone = commandId;
        SelectRibbonTab('Ribbon.Hemrika.SharePresence.Content', true);
        SetWebpartZone(webpartzone) 
    }

    webpartadder = SP.Ribbon.WebPartComponent.getWebPartAdder();

    if (!webpartadder) {
        window.LoadWPAdderOnDemand();
        //return;
    }

    if (webpartadder) {
        var category = this.WebPartAdderCategory(webpartadder, commandId);

        if (!toggle) {
            var useCategory = true;
            if (category) {
                var catTitle = category;
                webpartadder.selectCategoryByTitle(catTitle);
                useCategory = false;
            }
            webpartadder._showCategoryColumn(useCategory);
            webpartadder._setZone(webpartzone);
            webpartadder.show();
        }
        else {
            webpartadder.hide();
        }
    }
};

function WebPartAdderCategory(adder, commandId) {
    var categoryname = null;
    if (adder) {
        var map = adder._ribbonMap;
        if (map) {
            categoryname = map[commandId];
        }
    }
    return categoryname;
};

function SelectRibbonTab(tabId, force) {
    var ribbon = null;
    try {
        ribbon = SP.Ribbon.PageManager.get_instance().get_ribbon();
    }
    catch (e) {
    }
    if (!ribbon) {
        if (typeof (_ribbonStartInit) == "function")
            _ribbonStartInit(tabId, false, null);
    }
    else if (force || ribbon.get_selectedTabId() == "Ribbon.Read") {
        ribbon.selectTabById(tabId);
    }
};

var proceed_robots = true;
var robotsCollection;
var robotsId;

function DiscoverRobotsEntry() {

    var clientContext = new SP.ClientContext.get_current();
    var site = SP.ClientContext.get_current().get_site();
    var web = site.get_rootWeb();
    var robots = web.get_lists().getByTitle("Robots");

    var itemUrl = document.location.pathname;
    var camlQuery = new SP.CamlQuery();
    camlQuery.set_viewXml('<View><Query><Where><Eq><FieldRef Name="Entry"/>' +
    '<Value Type="Text">' + itemUrl + '</Value></Eq>' +
    '</Where></Query><RowLimit>1</RowLimit></View>');

    this.robotsCollection = robots.getItems(camlQuery);

    var robotsC = robots.getItems(camlQuery);

    clientContext.load(this.robotsCollection);

    clientContext.executeQueryAsync(
    Function.createDelegate(this, this.onQuerySucceeded),
    Function.createDelegate(this, this.onQueryFailed));

};

function onQuerySucceeded(sender, args) {

    try {
        var item = robotsCollection.itemAt(0);
        if (item) {
            robotsId = item.get_item('ID');
        }
    } catch (e) {
        proceed_robots = false;
    }
};

function onQueryFailed(sender, args) {
    proceed_robots = false;
};

function EditRobotsItem(commandId, properties, sequence) {
    if (proceed_robots == true) {
        if (this.robotsId != null) {
            var options = {
                url: '/Robots/EditForm.aspx?ID=' + this.robotsId + '&Rootfolder=' + escape(document.location.pathname) + '&ContentTypeId=0x0100E4C8C759A0494725B6C8886AEBFE5B6000F62ED78736F1D643825239FAF0883198&IsDlg=1',
                title: 'Bewerken Robots.txt Item',
                allowMaximize: false,
                showClose: true,
                width: 700,
                height: 600,
                dialogReturnValueCallback: EditRobotsItemCallback
            };
            SP.UI.ModalDialog.showModalDialog(options);
        }
    }
};

function EditRobotsItemCallback(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {

        SP.UI.Notify.addNotification("You pressed OK, " + returnValue, false);
    }
    else {
        SP.UI.Notify.addNotification("You cancelled the operation, ", false);
    }

    RefreshCommandUI();
};

function NewRobotsItem(commandId, properties, sequence) {
    if (robotsId == null) {

        var options = {
            url: '/Robots/NewForm.aspx?Rootfolder=' + document.location.pathname + '&amp;IsDlg=1',
            title: 'Opnemen in Robots.txt.',
            allowMaximize: false,
            showClose: true,
            width: 700,
            height: 600,
            dialogReturnValueCallback: NewRobotsItemCallback
        };

        SP.UI.ModalDialog.showModalDialog(options);
    }
};

function NewRobotsItemCallback(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification("You pressed OK, " + returnValue, false);
    }
    else {
        SP.UI.Notify.addNotification("You cancelled the operation, ", false);
    }

    RefreshCommandUI();
};

function EditMetaData(commandId, properties, sequence) {
    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/MetaData/MetaDataEdit.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('List', (new SP.Guid(_spPageContextInfo.pageListId)).toString());
    urlBuilder.addKeyValueQueryString('ID', _spPageContextInfo.pageItemId.toString());
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Edit MetaData for Page',
        allowMaximize: true,
        showClose: true,
        width: 640, height: 480,
        dialogReturnValueCallback: EditMetaDataCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function EditMetaDataCallBack(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation Successful!');
        RefreshCommandUI();
    }
    return true;
};

function EditOpenGraph(commandId, properties, sequence) {
    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/MetaData/OpenGraphedit.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('List', (new SP.Guid(_spPageContextInfo.pageListId)).toString());
    urlBuilder.addKeyValueQueryString('ID', _spPageContextInfo.pageItemId.toString());
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Edit OpenGraph for Page',
        allowMaximize: true,
        showClose: true,
        width: 640, height: 480,
        dialogReturnValueCallback: EditOpenGraphCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function EditOpenGraphCallBack(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation Successful!');
        RefreshCommandUI();
    }
    return true;
};

function EditFaceBook(commandId, properties, sequence) {
    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/MetaData/FaceBookEdit.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('List', (new SP.Guid(_spPageContextInfo.pageListId)).toString());
    urlBuilder.addKeyValueQueryString('ID', _spPageContextInfo.pageItemId.toString());
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Edit FaceBook for Page',
        allowMaximize: true,
        showClose: true,
        width: 640, height: 480,
        dialogReturnValueCallback: EditFaceBookCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function EditFaceBookCallBack(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation Successful!');
        RefreshCommandUI();
    }
    return true;
};

function EditTwitter(commandId, properties, sequence) {
    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/MetaData/TwitterEdit.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('List', (new SP.Guid(_spPageContextInfo.pageListId)).toString());
    urlBuilder.addKeyValueQueryString('ID', _spPageContextInfo.pageItemId.toString());
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Edit Twitter for Page',
        allowMaximize: true,
        showClose: true,
        width: 640, height: 480,
        dialogReturnValueCallback: EditTwitterCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function EditTwitterCallBack(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation Successful!');
        RefreshCommandUI();
    }
    return true;
};

function SettingsMetaData(commandId, properties, sequence) {
    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/MetaData/MetaDataManage.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Global MetaData Settings',
        allowMaximize: true,
        showClose: true,
        dialogReturnValueCallback: SetttingsMetaDataCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function SetttingsMetaDataCallBack(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation Successful!');
        RefreshCommandUI();
    }
    return true;
};

function SettingsSiteMap(commandId, properties, sequence) {
    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/SiteMap/SiteMapManage.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Global SiteMap Settings',
        allowMaximize: true,
        showClose: true,
        width: 800,
        height: 600,
        dialogReturnValueCallback: SetttingsSiteMapCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function SetttingsSiteMapCallBack(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation Successful!');
        RefreshCommandUI();
    }
    return true;
};

function EditSemantic(commandId, properties, sequence) {
    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Semantic/SemanticManage.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('List', (new SP.Guid(_spPageContextInfo.pageListId)).toString());
    urlBuilder.addKeyValueQueryString('ID', _spPageContextInfo.pageItemId.toString());
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Manage Semantic Urls',
        allowMaximize: true,
        showClose: true,
        dialogReturnValueCallback: EditSemanticCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function EditSemanticCallBack(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation Successful!');
        RefreshCommandUI();
    }
    return true;
};

function CompanySettings(commandId, properties, sequence) {
    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('Hemrika/Company/CompanyManage.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    urlBuilder.addKeyValueQueryString('List', (new SP.Guid(_spPageContextInfo.pageListId)).toString());
    urlBuilder.addKeyValueQueryString('ID', _spPageContextInfo.pageItemId.toString());
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Manage Company Settings',
        allowMaximize: true,
        showClose: true,
        dialogReturnValueCallback: CompanySettingsCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function CompanySettingsCallBack(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation Successful!');
        RefreshCommandUI();
    }
    return true;
};

function NavigationSettings(commandId, properties, sequence) {

    var clientContext = new SP.ClientContext.get_current();
    var site = SP.ClientContext.get_current().get_site();
    var web = site.get_rootWeb();
    var sitemap = web.get_lists().getByTitle("SiteMap");

    var layoutsUrl = SP.Utilities.Utility.getLayoutsPageUrl('listform.aspx');
    var urlBuilder = new SP.Utilities.UrlBuilder(layoutsUrl);
    var listid = new SP.Guid(sitemap.listId);
    urlBuilder.addKeyValueQueryString('ListId', listid.toString('B'));
    var options = {
        url: urlBuilder.get_url(),
        tite: 'Manage Navigation Settings',
        allowMaximize: true,
        showClose: true,
        dialogReturnValueCallback: NavigationSettingsCallBack
    };

    SP.UI.ModalDialog.showModalDialog(options);
};

function NavigationSettingsCallBack(dialogResult, returnValue) {

    if (dialogResult == SP.UI.DialogResult.OK) {
        SP.UI.Notify.addNotification('Operation Successful!');
        RefreshCommandUI();
    }
    return true;
};

function MSOLayout_UpdatePartOrderAfterMove(Zone, StartingIndex) {
    ULSfXY: ;
    var index;
    if (Zone.orientation == "Horizontal") {
        var parentRow = Zone.rows[0];
        for (index = StartingIndex;
        index < parentRow.cells.length;
        index++) MSOLayout_AddChange(eval(parentRow.cells[index].relatedwebpart), "ZoneIndex", index)
    }
    else for (index = StartingIndex; index < Zone.rows.length; index++) {
        var wpitem = eval(Zone.rows[index].cells[0]);
        var relatedwebpart = wpitem.getAttribute("relatedwebpart");
        if (relatedwebpart != null) {
            var wprelated = document.getElementById(relatedwebpart);
            MSOLayout_AddChange(eval(wprelated), "ZoneIndex", index);
        }
    }
};

function MSOLayout_CreateIBar() {
    ULSfXY: ;
    if (!MSOLayout_vertZoneIBar || !MSOLayout_horzZoneIBar) {
        var f = "";
        if (FV4UI()) f = " margin:1px;";
        var a = document.createElement("TABLE");
        a.style.cssText = "font-size:1pt; position:absolute; display:none; border-collapse:collapse;" + f;
        a.className = "ms-SPZoneIBar";
        a.cellSpacing = "0";
        a.cellPadding = "0";
        AddEvtHandler(a, "ondragenter", MSOLayout_MoveWebPartStopEventBubble);
        AddEvtHandler(a, "ondragover", MSOLayout_MoveWebPartStopEventBubble);
        var e = a.insertRow().insertCell();
        e.align = "center";
        var b = e.insertBefore(document.createElement("DIV"));
        b.name = "MSOLayout_insideIBar";
        b.className = "ms-SPZoneIBar";
        b.style.backgroundColor = a.currentStyle.borderColor;
        b.style.background = "transparent";
        b.style.borderWidth = "2px";
        b.style.position = "relative";
        if (!MSOLayout_topObject) MSOLayout_topObject = document.body;
        MSOLayout_horzZoneIBar = MSOLayout_topObject.appendChild(a.cloneNode(true));
        MSOLayout_vertZoneIBar = MSOLayout_topObject.appendChild(a.cloneNode(true));
        var c = getFirstElementByName(MSOLayout_horzZoneIBar, "MSOLayout_insideIBar"), d = getFirstElementByName(MSOLayout_vertZoneIBar, "MSOLayout_insideIBar");
        MSOLayout_horzZoneIBar.style.width = 6;
        MSOLayout_horzZoneIBar.style.borderStyle = "solid none";
        try {

            c.style.height = "100%";
            c.style.width = "33%";
            c.style.borderStyle = "none solid none none";
            c.style.posTop = 0;
        } catch (e) {

        }

        MSOLayout_vertZoneIBar.style.height = 6;
        MSOLayout_vertZoneIBar.style.borderStyle = "none solid";
        try {
            d.style.width = "100%";
            d.style.height = "2";
            d.style.borderStyle = "solid none none none";
            d.style.posTop = 1;
        } catch (e) {

        }

        if (MSOLayout_topObject != document.body) {
            MSOLayout_horzBodyZoneIBar = document.body.appendChild(MSOLayout_horzZoneIBar.cloneNode(true));
            MSOLayout_vertBodyZoneIBar = document.body.appendChild(MSOLayout_vertZoneIBar.cloneNode(true))
        }

    }
    MSOLayout_iBar = MSOLayout_vertZoneIBar
};

$(document).ready(function () {
    /*
    if (_WebSitePageContextInfo.wspFormMode == "Edit") {
        HTML5EditorEnable();
        Aloha.jQuery('.HTML5-editable').aloha();
    };
    */

    var _tpContainer = document.getElementById("MSO_tblPageBody");
    if (_tpContainer == null)
        return;

    _tpContainer = $(_tpContainer);
    _tpContainer.find("> tbody > tr > td:first").remove();

    var _tpTitle = $("#MSOTlPn_TlPnCaptionSpan").text();
    $("#MSOTlPn_Tbl").find("> tbody > tr:first").remove();
    $("#MSOTlPn_MainTD").css({ 'width': '100%' });
    ExecuteOrDelayUntilScriptLoaded(function () {
        SP.UI.ModalDialog.showModalDialog({
            title: _tpTitle,
            html: _tpContainer[0],
            dialogReturnValueCallback: function (result, target) { MSOTlPn_onToolPaneCloseClick(); }
        });

        var _modalContainer = $(".ms-dlgContent");
        $(_modalContainer).ready(function () { $("form").append(_modalContainer); });

    }, "sp.js");
});

// Notify waiting jobs
NotifyScriptLoadedAndExecuteWaitingJobs("/_layouts/Hemrika/WebSitePage/Hemrika.SharePresence.WebSite.Page.js");
