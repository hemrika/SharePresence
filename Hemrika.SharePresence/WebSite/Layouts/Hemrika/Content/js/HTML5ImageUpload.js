function UploadHTML5Image(imageControlID, pickerControlID, webId) {
    var dialogOptions = {
        url: SP.Utilities.Utility.getLayoutsPageUrl("Hemrika/Content/UploadImage.aspx?webId=" + webId),
        title: "Upload an Image",
        args: { imageElementID: imageControlID, pickerElementID: pickerControlID },
        dialogReturnValueCallback: Function.createDelegate(null, UploadHTML5Image_OnClose)
    };

    SP.UI.ModalDialog.showModalDialog(dialogOptions);
}

function UploadHTML5Image_OnClose(result, eventArgs) {
    if (result == SP.UI.DialogResult.OK) {
        
        while (eventArgs.imageUrl.indexOf('"') != -1) {
            eventArgs.imageUrl = returnValue.imageUrl.replace('"', '');
        }

        var imgElement = document.getElementById(eventArgs.controlIDs.imageElementID);
        imgElement.src = eventArgs.imageUrl;

        var picker = document.getElementById(eventArgs.controlIDs.pickerElementID);

        if (picker != null) {
            var ImagePickerTag = GetTagFromIdentifierAndTitle(picker, 'div', 'ImagePicker_upLevelDiv', 'Object Picker');
            if (ImagePickerTag) {
                ImagePickerTag.innerHTML = eventArgs.imageUrl;            }
        }
    }
}

function GetTagFromIdentifierAndTitle(parentElement, tagName, identifier, title) {
    var len = identifier.length;
    var tags = parentElement.getElementsByTagName(tagName);
    for (var i = 0; i < tags.length; i++) {
        var tempString = tags[i].id;
        if (tags[i].title == title && (identifier == "" ||
                                tempString.indexOf(identifier) == tempString.length - len)) {
            return tags[i];
        }
    }
    return null;
}

function UpdateImageAfterDialog(imageControlID, pickerControlID) {

    var picker = document.getElementById(pickerControlID);
    var image_hidden = document.getElementById(imageControlID);
    var image = $(image_hidden).parent().find('.HTML5-editable').children("img")[0];
    //var image = $(image_hidden).parent().next('span').next('img');

    if (picker != null && image != null) {
        var ImagePickerTag = GetTagFromIdentifierAndTitle(picker, 'div', 'ImagePicker_upLevelDiv', 'Object Picker');

        if (ImagePickerTag) {            
            var tags = ImagePickerTag.getElementsByTagName('span');
            var content = null;
            for (var i = 0; i < tags.length; i++) {
                if (tags[i].id == 'content') {
                    content = tags[i];
                }
            }
            if (content) {
                image.src = content.innerText;
            }
        }
    }
}