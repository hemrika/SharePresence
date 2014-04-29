var $AvailablePageList // the list of Pages available in the site from the dropdown list of internal links
var $SelectedListId

$(document).ready(function () {
    //loadThemeXml();

    //createVoodooCmsDialog("modalEditor", "Edit Item", 650, 270, true);
    //createVoodooCmsDialog("modalListEditor", "List Information", 650, 200, true);

    //getStyleTags($.cookie("SiteId"), "List", "uiCssClass")

    // Add Style and functinality to the Button Tags for the List Item editing    
    //$("button").button();

    // Add Events for Controls on the page
    $("#uiCssClass").change(function () {
        $("#listPreviewStyle").removeClass();
        // listPreviewStyle section is created dynamically in the drawListItems function below
        $("#listPreviewStyle").addClass($('#uiCssClass option:selected').text());
    });

    /*
    $("#uiTheme").change(function () {
        $.cookie("UiTheme", $('#uiTheme option:selected').text());
        $('#UiThemeTag').attr('href', 'Content/Css/jQueryUiThemes/' + $('#uiTheme option:selected').text() + '/jquery-ui-1.8.13.custom.css');
    });
    */

    $("#uiAddList").click(function () {
        $("#uiListId").text('');
        $("#uiListName").val('');
        $("#uiListDescription").val('');
        //openVoodooCmsDialog("modalListEditor");
        return false;
    });

    $("#uiListCancel").click(function () {
        //closeVoodooCmsDialog("modalListEditor");
        return false;
    });

    $("#uiListSave").click(function () {
        saveList();
        return false;
    });

    $("#uiCancelListItemEdit").click(function () {
        //closeVoodooCmsDialog("modalEditor");
    });

    $("#uiSaveListItemEdit").click(function () {
        saveListItem();
        //closeVoodooCmsDialog("modalEditor");
    });

    getLists();

});                // END DOCUMENT.READY

/*
function loadThemeXml() {
    $.get('content/Css/JQueryUiThemes/Themes.xml', function (d) {
        var options = "";

        $(d).find("THEME").each(function () {
            $("#uiTheme").append("<option value='" + $(this).attr("id") + "'>" + $(this).attr("id") + "</option>");
        });

        if ($.cookie("UiTheme")) {
            $("#uiTheme option[value='" + $.cookie("UiTheme") + "']")[0].selected = true;
            $('#UiThemeTag').attr('href', 'Content/Css/jQueryUiThemes/' + $.cookie("UiTheme") + '/jquery-ui-1.8.13.custom.css');
        }
    });

}
*/

// ANIMATES THE CLOSING OF THE LIST ITEMS INTERFACE AND RELOADS THE LIST OF LISTS
function closeListItemsInterface() {
    $("#PreviewList").hide("blind", {}, 1000, loadEditInterface);

    function loadEditInterface() {
        $("#EditList").hide("drop", {}, 600, reloadListsDelay);
    }

    function reloadListsDelay() {
        setTimeout(reloadLists, 300)
    }

    function reloadLists() {
        getLists();
        $("#uiAddList").removeAttr('disabled');
    }
}
// END ANIMATED CLOSE

// DRAWS THE ANIMATED INTERFACE FOR LIST ITEMS AND CLOSES LIST OF LISTS
function drawListItemsInterface(idList) {

    $SelectedListId = idList;

    getListItems(idList)
    $("#ListsDisplay").hide("blind", {}, 600, null);
    setTimeout(loadEditInterface, 650);
    return false;

    function loadEditInterface() {
        $("#EditList").show("drop", {}, 600, loadPreviewInterface);
    }

    function loadPreviewInterface() {
        $("#PreviewList").show("blind", {}, 1000, null);

    }
}
// END THE ANIMATED INTERFACE FOR LIST ITEMS


function editListItem(idListItem) {
    loadItem(idListItem);
    //openVoodooCmsDialog("modalEditor");
}

// Add A New List item to Current List
function addListItem() {
    $("#uiLinkText").val('');
    $("#uiLinkDescription").val('');
    $("#uiIdListItem").text('');
    //openVoodooCmsDialog("modalEditor");
}

function deleteListItem(idList, idListItem) {
    if (confirm("Delete this item?")) {
        $.ajax({
            type: "POST",
            url: "_layouts/Hemrika/Navigation/NavigationMethods.aspx/DeleteListItem",
            data: "{'idListItemString':'" + idListItem + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                getListItems(idList);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                showVoodooJavaScriptError(xhr, ajaxOptions, thrownError);
            }
        });
    }
}


function loadItem(idListItem) {

    $.ajax({
        type: "POST",
        url: "_layouts/Hemrika/Navigation/NavigationMethods.aspx/GetSortableListItem",
        data: "{'sortableListItemId':'" + idListItem + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#uiLinkText").val(data.d.Headline);
            $("#uiLinkDescription").val(data.d.Description);
            $("#uiLinkUrl").val(data.d.LinkUrl);
            $("#uiIdListItem").text(data.d.SortableListItemId);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            showVoodooJavaScriptError(xhr, ajaxOptions, thrownError);
        }
    });
}

// GET ITEMS OF A LIST BY LIST ID AND DRAW THE LISTS IN THE EDIT/PREVIEW EDIT INTERFACE
function getListItems(sortableListId) {
    $("#uiAddList").attr('disabled', 'disabled');

    $.ajax({
        type: "POST",
        url: "_layouts/Hemrika/Navigation/NavigationMethods.aspx/GetSortableListItems",
        data: "{'sortableListId':'" + sortableListId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            drawListItems(data);
            ////closeVoodooCmsAjaxWait()
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ////closeVoodooCmsAjaxWait()
            showVoodooJavaScriptError(xhr, ajaxOptions, thrownError);
        }
    });

    function drawListItems(listItems) {
        sortableList = "<div style=\"float: right; margin-bottom: 5px;\">";
        sortableList += "<button class=\"listbutton\" onclick=\"javascript: closeListItemsInterface(); return false;\">Done</button>";
        sortableList += "<button class=\"listbutton\" onclick=\"javascript: addListItem(); return false;\">Add List Item</button>";
        sortableList += "</div><br style=\"clear: both;\" />";
        sortableList += "<div class=\"SortableList\">";

        for (var index in listItems) {
            sortableList += "<div class=\"SortableListItem\" id=\"" + listItems[index].SortableListItemId + "\" style=\"margin-bottom: 5px;\">";
            sortableList += "<div class=\"SortableListHeader\" style=\"padding: 5px; position: relative;  \">";
            sortableList += "<div style=\"position: absolute; right: 5px; top: 2px; \">";
            sortableList += "<img id=\"btnEditLink\" "
                                       + "src=\"/content/images/icondelete.png\" "
                                       + "onclick='javascript:deleteListItem(\"" + listItems[index].SortableListId + "\", \"" + listItems[index].SortableListItemId + "\");'"
                                       + "alt=\"Delete\" "
                                       + "style=\"cursor: pointer;\" "
                                       + "align=\"ABSMIDDLE\" "
                                       + "/>";
            sortableList += "<img id=\"btnEditLink\" "
                                       + "src=\"/content/images/iconEdit.png\" "
                                       + "onclick='javascript:editListItem(\"" + listItems[index].SortableListItemId + "\");'"
                                       + "alt=\"Edit\" "
                                       + "style=\"cursor: pointer;\" "
                                       + "align=\"ABSMIDDLE\" "
                                       + "/>";
            sortableList += "</div>";
            sortableList += listItems[index].Headline;
            sortableList += "</div>";
            sortableList += "<div class=\"SortableListContent\" style=\"padding: 5px;\">";
            sortableList += "Desc: " + listItems[index].Description + "<br />";
            sortableList += "Link: " + listItems[index].LinkUrl + "<br />";
            sortableList += "</div>";
            sortableList += "</div>";
        }
        sortableList += '</div>';

        previewList = "<div id=\"sidebar\">";
        previewList += "<div id=\"listPreviewStyle\">";
        previewList += "<ul>";
        for (var indexP in listItems) {
            previewList += " <li>";
            previewList += "  <a href='" + listItems[indexP].LinkUrl + "' target='_blank'>" + listItems[indexP].Headline + "</a>";
            previewList += "<span>" + listItems[indexP].Description + "</span>";
            previewList += "</li>";
        }

        previewList += "</ul>";
        previewList += "</div>";
        previewList += "</div>";

        $("#PreviewListContent").html(previewList);
        $("#EditList").html(sortableList);
        //$(".listbutton").button();
        //$("#listPreviewStyle").addClass($('option:selected', '#uiCssClass').text());
        $("#listPreviewStyle").addClass($('#uiCssClass option:selected').text());
        var $StartIndex;
        // Make the list that was created above Sortable
        $(".SortableList").sortable({
            start: function (event, ui) {
                $StartIndex = ui.item.index() + 1;
            },
            stop: function (event, ui) {
                idListItem = ui.item[0].id;
                newListIndex = ui.item.index() + 1;
                if ($StartIndex != newListIndex) {
                    moveListItem(idListItem, newListIndex);
                }
            }
        });

        $(".SortableListItem").addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
    			    .find(".SortableListHeader")
    				    .addClass("ui-widget-header ui-corner-all")
        			    .end()
    			    .find(".SortableListContent");
        $(".SortableList").disableSelection();
    }
}
// END LIST ITEM EDIT/PREVIEW EDIT INTERFACE


function editListInfo(idList) {
    $.ajax({
        type: "POST",
        url: "_layouts/Hemrika/Navigation/NavigationMethods.aspx/GetSortableList",
        data: "{'sortableListId':'" + idList + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#uiListDescription").val(data.d.Description);
            $("#uiListId").text(data.d.SortableListId);
            //openVoodooCmsDialog("modalListEditor");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            showVoodooJavaScriptError(xhr, ajaxOptions, thrownError);

        }
    });

}


// Gets Lists for the Site and Draws the animated edit screen.
function getLists() {
    //openVoodooCmsAjaxWait("blue");
    $.ajax({
        type: "POST",
        url: "_layouts/Hemrika/Navigation/NavigationMethods.aspx/GetSortableLists",
        data: "{'pageNumber':'1','pageSize':'20'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        
        success: function (data) {
            drawListofLists(data.GetSortableListsResult);
            ////closeVoodooCmsAjaxWait();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            showVoodooJavaScriptError(xhr, ajaxOptions, thrownError);
            ////closeVoodooCmsAjaxWait();
        }
    });

    function drawListofLists(lists) {
        $("#EditList").hide();
        $("#PreviewList").hide()
        $("#EditList").css("visibility", "visible");
        $("#PreviewList").css("visibility", "visible");
        $lists = $("#ListsDisplay");
        $lists.html("");
        $lists.append("<h3 class=\"ui-widget-header ui-corner-all\">Available Lists</h3>");
        $ul = $lists.append("<ul />");
        $lists.prepend();
        for (var index in lists) {
            var xmlDoc = loadXMLString(lists[index]);
            var node = xmlDoc.firstChild.firstChild;
            link = "<li style=\"overflow: hidden; padding: 10px; border-bottom: 1px dotted #666; list-style-type: none;  \">";
            link += "<div style=\"float: right;\"><button class=\"linkbutton\" onclick=\" drawListItemsInterface('" + node.attributes.getNamedItem("id").value + "'); return false;\">edit items.</button></div>";
            link += "<div style=\"float: right;\"><button class=\"linkbutton\" onclick=\"editListInfo('" + node.attributes.getNamedItem("id").value + "'); return false;\">list name.</button></div>";
            link += "<a href=\"#\" style=\" display: block; font-size: 1.3em;\" onclick=\"javascript: drawListItemsInterface('" + node.attributes.getNamedItem("id").value + "');\">" + node.attributes.getNamedItem("description").value + "</a>";
            link += "</li>";
            $ul.append(link);
        }
        // Wrap the header and content created above as a UI widget with style tags
        $lists.wrapInner("<div class=\"ui-widget-content ui-corner-all\" style=\"overflow: hidden; line-height: 1.5; min-height: 300px; min-width: 300px; padding: 2px;\" />");
        //$(".linkbutton").button();
        $lists.show("blind", {}, 1000, null);


    }
}
// End get and draw the lists for the site

// SAVE FUNCTIONS
function saveList() {
    //openVoodooCmsAjaxWait("blue");
    var listId = 0;
    if ($("#uiListId").text())
        listId = $("#uiListId").text();

    $.ajax({
        type: "POST",
        url: "_layouts/Hemrika/Navigation/NavigationMethods.aspx/SaveSortableList",
        data: "{'sortableListId':'" + listId + "',"
                 + "'listDescription':'" + jsonEncode($("#uiListDescription").val()) + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (listId == 0) {
                drawListItemsInterface(data.d);
            } else {
                getLists();
            }
            //closeVoodooCmsDialog("modalListEditor");
            ////closeVoodooCmsAjaxWait();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ////closeVoodooCmsAjaxWait();
            showVoodooJavaScriptError(xhr, ajaxOptions, thrownError);
        }
    });
}

// SAVE LIST ITEM
function saveListItem() {
    //openVoodooCmsAjaxWait("blue");
    var sortableListItemId = 0;
    if ($("#uiIdListItem").text())
        sortableListItemId = $("#uiIdListItem").text();
    $.ajax({
        type: "POST",
        url: "_layouts/Hemrika/Navigation/NavigationMethods.aspx/SaveSortableListItem",
        data: "{'sortableListId':'" + $SelectedListId + "',"
                 + "'sortableListItemId':'" + sortableListItemId + "',"
                 + "'headline':'" + jsonEncode($("#uiLinkText").val()) + "',"
                 + "'description':'" + jsonEncode($("#uiLinkDescription").val()) + "',"
                 + "'linkUrl':'" + jsonEncode($("#uiLinkUrl").val()) + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            getListItems($SelectedListId)
            ////closeVoodooCmsAjaxWait();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            showVoodooJavaScriptError(xhr, ajaxOptions, thrownError);
            ////closeVoodooCmsAjaxWait();
        }
    });
}

// SAVE POSITION OF MOVED LIST ITEM
function moveListItem(sortableListItemId, newPosition) {
    //openVoodooCmsAjaxWait("blue");
    $.ajax({
        type: "POST",
        url: "_layouts/Hemrika/Navigation/NavigationMethods.aspx/SaveListPosition",
        data: "{'sortableListItemId':'" + sortableListItemId + "',"
                 + "'newPosition':'" + newPosition + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ////closeVoodooCmsAjaxWait();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            showVoodooJavaScriptError(xhr, ajaxOptions, thrownError);
            ////closeVoodooCmsAjaxWait();
        }
    });

}

// SUPPORT FUNCTIONS

// Get list of pages for the current site
// result of call to getPages in the PageMethods script that is included on every page
function getPagesSuccess(data) {
    $AvailablePageList = data.d;
    $("#uiPageList").append("<option value=''>No Change</option>");
    $.each(data.d, function (index, item) {
        //$("#uiPageList").append(new Option(item.PageName + " (" + item.FriendlyName + ")", item.idPage));
        $("#uiPageList").append("<option value='" + item.idPage + "'>" + item.PageName + " (" + item.FriendlyName + ")</option>");
    });
}

var $VoodooAjaxDialog


function jsonEncode(stringToEncode) {
    if (stringToEncode) {
        stringToEncode = stringToEncode.replace(/\\/g, "\\\\");
        stringToEncode = stringToEncode.replace(/'/g, "\\\'");
        return stringToEncode;
    }
    return "";
}

/*
$(document).ready(function () {
    $VoodooAjaxDialog = $('<div></div>').dialog({ autoOpen: false,
        title: "Loading...",
        width: 100,
        height: 150,
        modal: false
    });
});
*/

function createVoodooCmsDialog(modalId, title, width, height, isModal) {
    $("#" + modalId).dialog({
        autoOpen: false,
        title: title,
        width: width,
        height: height,
        modal: isModal
    });
}

function openVoodooCmsDialog(modalId) {
    $("#" + modalId).dialog('open');
}

function closeVoodooCmsDialog(modalId) {
    $("#" + modalId).dialog('close');
}


function showVoodooJavaScriptError(xhr, ajaxOptions, thrownError) {
    alert(xhr.status + ":" + thrownError + "<br />" + xhr.responseText);
}

function openVoodooCmsAjaxWait(color) {
    $VoodooAjaxDialog.html("<img src=\"/content/images/Ajax-Loader-" + color + ".gif\" style=\"margin-left: 10px;\" />");
    $VoodooAjaxDialog.dialog('open');
}

function closeVoodooCmsAjaxWait() {
    $VoodooAjaxDialog.dialog('close');
}

function loadXMLString(txt) {
    if (window.DOMParser) {
        parser = new DOMParser();
        xmlDoc = parser.parseFromString(txt, "text/xml");
    }
    else // Internet Explorer
    {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = false;
        xmlDoc.loadXML(txt);
    }
    return xmlDoc;
}