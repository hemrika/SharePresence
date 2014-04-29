function UploadHTML5Video(videoControlID, pickerControlID, webId) {
    var dialogOptions = {
        url: SP.Utilities.Utility.getLayoutsPageUrl("Hemrika/Content/UploadVideo.aspx?webId=" + webId),
        title: "Upload an Video",
        args: { videoElementID: videoControlID, pickerElementID: pickerControlID },
        dialogReturnValueCallback: Function.createDelegate(null, UploadHTML5Video_OnClose)
    };

    SP.UI.ModalDialog.showModalDialog(dialogOptions);
}

function UploadHTML5Video_OnClose(result, eventArgs) {
    if (result == SP.UI.DialogResult.OK) {

        while (eventArgs.videoUrl.indexOf('"') != -1) {
            eventArgs.videoUrl = returnValue.videoUrl.replace('"', '');
        }

        var vidElement = document.getElementById(eventArgs.controlIDs.videoElementID);
        vidElement.src = eventArgs.videoUrl;

        var picker = document.getElementById(eventArgs.controlIDs.pickerElementID);

        if (picker != null) {
            var VideoPickerTag = GetTagFromIdentifierAndTitle(picker, 'div', 'VideoPicker_upLevelDiv', 'Object Picker');
            if (VideoPickerTag) {
                VideoPickerTag.innerHTML = eventArgs.videoUrl;
            }
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

function UpdateVideoAfterDialog(videoControlID, pickerControlID) {

    var picker = document.getElementById(pickerControlID);
    var video = document.getElementById(videoControlID);

    if (picker != null && video != null) {
        var VideoPickerTag = GetTagFromIdentifierAndTitle(picker, 'div', 'VideoPicker_upLevelDiv', 'Object Picker');

        if (VideoPickerTag) {
            var tags = VideoPickerTag.getElementsByTagName('span');
            var content = null;
            for (var i = 0; i < tags.length; i++) {
                if (tags[i].id == 'content') {
                    content = tags[i];
                }
            }
            if (content) {
                video.src = content.innerText;
            }
        }
    }
}