
function ULS_SP() {
    if (ULS_SP.caller) {
        ULS_SP.caller.ULSTeamName = "Windows SharePoint Services 4";
        ULS_SP.caller.ULSFileName = "/_layouts/Hemrika/WebSitePart/Hemrika.SharePresence.WebSitePart.js";
    }
};


//Dialog opening
function OpenWebSitePartDialog(WebId, ItemId, WebPartId) {

    var ctx = new SP.ClientContext.get_current();
    
        var dialogOptions = {
            url: SP.Utilities.Utility.getLayoutsPageUrl("Hemrika/Content/EditWebSitePart.aspx?webId="+WebId+"&itemId="+ItemId+"&webPartId="+WebPartId),
            title: "Edit WebSite Part",
            args: { webId: WebId, itemId: ItemId, webPartId: WebPartId },
            dialogReturnValueCallback: Function.createDelegate(null, CloseWebSitePartDialog)
        };

        SP.UI.ModalDialog.showModalDialog(dialogOptions);
    }

    var messageId;

    // Dialog callback
    function CloseWebSitePartDialog(result, eventArgs) {
        if (result == SP.UI.DialogResult.OK) {

            //Get id 
            //messageId = SP.UI.Notify.addNotification(eventArgs.Message, false);
            //SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);
            //window.location.href = window.location.pathname + "?PageView=Shared&DisplayMode=Design";
            //This invokes a postback update on the ASP.Net UpdatePanel to update the rendering
            __doPostBack('udpWebPartBody', '');
        }
    }

    var clientContext;
    var PostId;

    function RemoveWebSitePart(ControlId, WebPartId, ControlIdentifier) {

        this.clientContext = new SP.ClientContext();

        this.PostId = ControlId.toString();

        clientContext.executeQueryAsync(Function.createDelegate(this, this.onRemoveWebSitePartSucceeded), Function.createDelegate(this, this.onRemoveWebSitePartFailed));

    }

    function onRemoveWebSitePartSucceeded() {

        __doPostBack(this.PostId, 'DeleteWebPart');
        messageId = SP.UI.Notify.addNotification('Web Part deleted.', false);

    }

    function onRemoveWebSitePartFailed(sender, args) {

        alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
    }
