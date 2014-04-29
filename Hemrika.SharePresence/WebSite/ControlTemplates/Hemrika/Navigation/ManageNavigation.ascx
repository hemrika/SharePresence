<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="SharePresence" namespace="Hemrika.SharePresence.WebSite.Navigation" Assembly="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageNavigation.ascx.cs" Inherits="Hemrika.SharePresence.WebSite.ControlTemplates.ManageNavigation" %>
<div data-role="page active" class="type-interior" data-theme="b">
	<div data-role="content" data-theme="b">
        <div data-role="fieldcontain">
            
             <br style="clear: both;" />

            <div class="ui-widget-header" style="padding: 15px; margin-bottom: 20px;">
                <div style="float: right; margin-top: -5px;">
                    <button id="uiAddList">Add New List</button>
                </div>         
                <h1>Sortable List Demo</h1>
            </div>

            <br style="clear: both;" />

  
            <!-- INITIAL LIST OF LISTS TO EDIT AND INTERFACE TO ADD NEW LISTS-->
            <div id="ListsDisplay" style="min-width: 350px; 
                                          margin-left: auto; 
                                          margin-right: auto;">
                              
            </div>


            <!-- INTERFACE TO EDIT INDIVIDUAL ITEMS OF THE LIST-->
            <!-- SORTABLE LIST FOR EDITING CONTENT AND ORDER -->
            <div id="EditList" style="width: 600px; 
                                      float: left; 
                                      min-height: 400px; 
                                      visibility: hidden; 
                                      padding: 10px;" 
                                      class="ui-widget-content ui-corner-all">
            </div>

            <!-- PREVIEW PANEL TO VIEW THE LIST WITH DIFFERENT REGISTERED CSS STYLES -->
            <div id="PreviewList" style="width: 295px; 
                                  float: right; 
                                  min-height: 400px; 
                                  visibility: hidden; 
                                  padding: 10px;" 
                                  class="ui-widget-content ui-corner-all"
                                  >
                 Preview Style: <select id="uiCssClass" >
                                    <option>hoverliststyle</option>
                                    <option>sidemenu</option>
                                 </select>

                <br /><br />
                <div id="PreviewListContent">
            
                </div>

            </div>

            <!-- Editor for List Information -->
                <div id="modalListEditor" class="VoodooForm" style="position: relative;">
                     <ul>
                        <li>
                            <label>Description:</label>
                            <input type="text"
                                   id="uiListDescription" 
                                   class="textField"
                                   style="width: 410px;"
                                   />
                        </li>
                    </ul> 

                    <div style="position: absolute;
                                bottom: 5px; 
                                left: 5px;">
                         List Id: <label id="uiListId"></label>
                    </div>

                    <div style="position: absolute;
                                bottom: 5px; 
                                right: 5px;">
                                <button id="uiListCancel">cancel</button>
                                <button id="uiListSave">save</button>
                    </div>
                </div>

 
            <!-- Editor modal pupup for editing list items.  -->
                <div id="modalEditor" class="VoodooForm" style="position: relative;">
                    <ul>
                        <li>
                            <label>Link Text:</label>
                            <input type="text"
                                   id="uiLinkText" 
                                   class="textField"
                                   style="width: 410px;"
                                   />
                        </li>
                        <li>
                            <label>Description:</label>
                            <input type="text"
                                   id="uiLinkDescription" 
                                   class="textField"
                                   style="width: 410px;"
                                   />
                        </li>
                        <li>
                            <label>Url:</label>
                            <input type="text"
                                   id="uiLinkUrl" 
                                   class="textField"
                                   style="width: 410px;"
                                   />
                        </li>
               
                
                    </ul>
            

                    <div style="position: absolute;
                                bottom: 5px; 
                                left: 5px;">
                         Link Id: <label id="uiIdListItem"></label>
                    </div>

                    <div style="position: absolute;
                                bottom: 5px; 
                                right: 5px;">
                                <button id="uiCancelListItemEdit">cancel</button>
                                <button id="uiSaveListItemEdit">save</button>
                    </div>

                </div>

        <br />
        <asp:Button ID="btn_Save" runat="server" OnClick="btn_Save_Click" Text="Save" data-inline="true"
            CssClass="ui-btn-up-b" data-icon="gear" />
        <asp:Button ID="btn_Cancel" runat="server" OnClientClick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.CANCEL,'No changes have been saved.'); return false;" Text="Cancel"
            data-inline="true" CssClass="ui-btn-up-b" data-icon="back"/>
        </div>
    </div>
</div>