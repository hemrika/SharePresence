<%@ Page language="C#" MasterPageFile="~masterurl/custom.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"  %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SPSWC" Namespace="Microsoft.SharePoint.Portal.WebControls" Assembly="Microsoft.SharePoint.Portal, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SEARCHWC" Namespace="Microsoft.Office.Server.Search.WebControls" Assembly="Microsoft.Office.Server.Search, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
    <SharePoint:EncodedLiteral runat="server" text="<%$Resources:Microsoft.Office.Server.Search, SearchCenterLiteOnet_Title%>" EncodeMethod='HtmlEncode'/> : <SEARCHWC:SearchTermFromUrl runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderPageImage" runat="server"><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate>
<label><asp:literal runat="server" Text="<%$Resources:Microsoft.Office.Server.Search, Search_Results_Page_Title1%>" /></label>
</ContentTemplate>
</SharePoint:UIVersionedContent>
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate>
<style>
body{
background-color:transparent;
}

TD.ms-titleareaframe, .ms-pagetitleareaframe {
    height: 10px; 
}

Div.ms-titleareaframe {
    height: 100%;
}

.ms-pagetitleareaframe table {
    height: 10px;
}
.ms-PartSpacingVertical
{
    margin-top:1px;
}
</style>
</ContentTemplate>
</SharePoint:UIVersionedContent>
<SharePoint:UIVersionedContent UIVersion="3" runat="server">
<ContentTemplate>
<style>
TD.ms-titleareaframe, .ms-pagetitleareaframe {
    height: 10px; 
}

Div.ms-titleareaframe {
    height: 100%;
}

.ms-pagetitleareaframe table {
    height: 10px;
}
.ms-PartSpacingVertical
{
    margin-top:1px;
}
</style>
</ContentTemplate>
</SharePoint:UIVersionedContent>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<style type="text/css">
TD.ms-titleareaframe, Div.ms-titleareaframe, .ms-pagetitleareaframe {
    height: 85px;
    text-align:center; 
}
.ms-pagetitleareaframe table {
    background-position:400px 14px;
    height: 0px;
}
.ms-bodyareaframe {
	PADDING-RIGHT: 0px; PADDING-LEFT: 18px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px
}
</style>  
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderLeftNavBar" runat="server">
<div height=100% class="ms-pagemargin"><IMG SRC="/_layouts/images/blank.gif" width=8 height=1 alt=""></div>
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderNavSpacer" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderBodyLeftBorder" runat="server">
<div height=100% class="ms-pagemargin"><IMG SRC="/_layouts/images/blank.gif" width=6 height=1 alt=""></div>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderTitleLeftBorder"  runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderSearchArea"  runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderTitleBreadcrumb"  runat="server">
<SharePoint:UIVersionedContent UIVersion="3" runat="server">
<ContentTemplate>
<A name="mainContent"></A>
<div style="height:100%; width:100%;padding-left: 2px; padding-top: 16px; padding-bottom: 14px;">
<div style="width:390px;">
</ContentTemplate>
</SharePoint:UIVersionedContent>
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate>
<div class="srch-sb-results">
<div class="srch-sb-results2">
</ContentTemplate>
</SharePoint:UIVersionedContent>
<WebPartPages:WebPartZone runat="server" AllowPersonalization="false" title="<%$Resources:Microsoft.Office.Server.Search, LayoutPageZone_TopZone%>" id="TopZone" orientation="Vertical" QuickAdd-GroupNames="Search" QuickAdd-ShowListsAndLibraries="false"/> 
</div>
</div>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderTitleAreaSeparator"  runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<SharePoint:VersionedPlaceHolder UIVersion="4" runat="server">
    <tr> 
         <td colspan="3">
              <div style="border-color:#b6babf; border-style:solid; border-width:0px 0px 1px; height:1px; width:100%;" />
         </td>
    </tr>
    <tr>
         <td class="srchctr_leftcell" id="LeftCell">
            <div class="srch-refinearea">
              <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" FrameType="TitleBarOnly" title="<%$Resources:Microsoft.Office.Server.Search,LayoutPageZone_LeftZone%>" id="LeftZone" orientation="Vertical" QuickAdd-GroupNames="Search" QuickAdd-ShowListsAndLibraries="false"/>
            </div> 
         </td>		   
		 <td class="srchctr_mainleftcell" id="MainLeftCell" rowspan="2">
		     <div>
                     <span class="srch-maintopleft">   
</SharePoint:VersionedPlaceHolder>
           <SharePoint:UIVersionedContent UIVersion="3" runat="server">
           <ContentTemplate> 
              <tr>
               <td id="MainLeftCell" width="75%" valign="top">
                <table border="0" cellpadding="0" cellspacing="0" ID="LeftZoneTable" width="100%" class="ms-tztable">
				<tr width="100%">                      
                    <td width="100%">
                             <table width="100%" cellspacing="0" width="100%">
                               <tr width="100%">
                                   <td id="MidUpperLeftCell" align="left"> 
            </ContentTemplate>
            </SharePoint:UIVersionedContent>
								   <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" title="<%$Resources:Microsoft.Office.Server.Search,LayoutPageZone_MiddleUpperLeftZone%>" id="MidUpperLeftZone" orientation="Vertical" QuickAdd-GroupNames="Search" QuickAdd-ShowListsAndLibraries="false"/>
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate>      
                                     </span>
                      <span class="srch-maintopright">
                          <div>
</ContentTemplate>
</SharePoint:UIVersionedContent>
            <SharePoint:UIVersionedContent UIVersion="3" runat="server">
            <ContentTemplate>
                                 </td>
                                 <td id="MidUpperRightCell" align="right">
            </ContentTemplate>
            </SharePoint:UIVersionedContent>
								 <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" title="<%$Resources:Microsoft.Office.Server.Search,LayoutPageZone_MiddleUpperRightZone%>" id="MidUpperRightZone" orientation="Vertical" QuickAdd-GroupNames="Search" QuickAdd-ShowListsAndLibraries="false"/> 
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate> 
                                       </div>                            
	                           </span>
				</div>
                                <div class="srch-maintop">
                                    <span class="srch-maintopleft">
</ContentTemplate>
</SharePoint:UIVersionedContent>
            <SharePoint:UIVersionedContent UIVersion="3" runat="server">
            <ContentTemplate>
                                 </td>
                               </tr>
                             </table>
                    </td>
                </tr>
				 <tr class="ms-srchresultstop" width="100%">
                   <td width="100%">
                    <table width="100%">
				       <tr width="100%">                                   
                          <td id="MidLowerLeftCell" colspan="1" align="left">
            </ContentTemplate>
            </SharePoint:UIVersionedContent>
                   <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" title="<%$Resources:Microsoft.Office.Server.Search,LayoutPageZone_MiddleLowerLeftZone%>" id="MidLowerLeftZone" orientation="Vertical" QuickAdd-GroupNames="Search" QuickAdd-ShowListsAndLibraries="false"/>    
			<SharePoint:UIVersionedContent UIVersion="3" runat="server">
	                <ContentTemplate>                        
	                      </td>
						  <td id="MidLowerRightCell" colspan="1" align="right">  

			</ContentTemplate>
			</SharePoint:UIVersionedContent>
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate>      
                                     </span>
                                     <span class="srch-maintopright">
</ContentTemplate>
</SharePoint:UIVersionedContent>
	     <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" title="<%$Resources:Microsoft.Office.Server.Search,LayoutPageZone_MiddleLowerRightZone%>" id="MidLowerRightZone" orientation="Vertical" QuickAdd-GroupNames="Search" QuickAdd-ShowListsAndLibraries="false"/>                             
              <SharePoint:UIVersionedContent UIVersion="3" runat="server">
              <ContentTemplate>                        
	                      </td>
                      </tr>
				  </table>
				</td>
			  </tr>
               <tr>
                 <td valign="top" ID="BottomCell" width="100%" style="padding-top: 10px"> 
              </ContentTemplate>
              </SharePoint:UIVersionedContent>
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate>                        
	                                  </span>
				</div>
                                <div class="srch-maintop2">
</ContentTemplate>
</SharePoint:UIVersionedContent>
                  <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" title="<%$Resources:Microsoft.Office.Server.Search,LayoutPageZone_BottomZone%>" id="BottomZone" orientation="Vertical" QuickAdd-GroupNames="Search" QuickAdd-ShowListsAndLibraries="false"/>
              <SharePoint:UIVersionedContent UIVersion="3" runat="server">
              <ContentTemplate>                        
	                 </td>
                   </tr>
				  </table>
				</td>

			     <td><img src="/_layouts/images/blank.gif" width="10" height="1" alt=""></td>                
                 <td id="RightCell" width="25%" valign="top">
              </ContentTemplate>
              </SharePoint:UIVersionedContent>
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate>                        
	                  </div>
                </td>
				<td class="srchctr_rightcell" id="RightCell" rowspan="1">
                                   <div class="srch-federationarea"> 
</ContentTemplate>
</SharePoint:UIVersionedContent>
                 <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" title="<%$Resources:Microsoft.Office.Server.Search,LayoutPageZone_RightZone%>" id="RightZone" orientation="Vertical" QuickAdd-GroupNames="Search" QuickAdd-ShowListsAndLibraries="false"/>
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate>
             </div> 
</ContentTemplate>
</SharePoint:UIVersionedContent>
               <SharePoint:UIVersionedContent UIVersion="3" runat="server">
              <ContentTemplate>                        
	                 </td>
                   </tr>
				  </table>
			  </ContentTemplate>
              </SharePoint:UIVersionedContent>
			  </td>
              </tr>
            </table>
</asp:Content>

<asp:Content ContentPlaceHolderID="SPNavigation" runat="server">
<SharePoint:UIVersionedContent UIVersion="4" runat="server">
<ContentTemplate>

<div id="s4-ribbonrow" class="s4-pr s4-ribbonrowhidetitle">
<div id="s4-ribboncont">  
                        <SharePoint:SPRibbon 
                            runat="server" 
                            CssFile = "corev4.css"
                            PlaceholderElementId="RibbonContainer"
                            FixedPositioningEnabled="true"
                            PermissionsString="EditListItems, AddAndCustomizePages"
                            PermissionMode="Any"
                            ApplyPermissionsToRibbonOnly="false">
                            <SharePoint:SPRibbonPeripheralContent
                                runat="server"
                                Location="TabRowLeft"
                                CssClass="ms-siteactionscontainer s4-notdlg">  

                       <span class="ms-siteactionsmenu" id="siteactiontd">

                       <SharePoint:SiteActions runat="server" accesskey="<%$Resources:wss,tb_SiteActions_AK%>" id="SiteActionsMenuMain"

                        PrefixHtml=""
                        SuffixHtml=""

                        MenuNotVisibleHtml="&amp;nbsp;"
                        >
                        <CustomTemplate>
                        <SharePoint:FeatureMenuTemplate runat="server"
                            FeatureScope="Site"
                            Location="Microsoft.SharePoint.StandardMenu"
                            GroupId="SiteActions"
                            UseShortId="true"
                            >

                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_EditPage"
                                Text="<%$Resources:wss,siteactions_editpage%>"
                                Description="<%$Resources:wss,siteactions_editpagedescriptionv4%>"
                                ImageUrl="/_layouts/images/ActionsEditPage.png" 
                                MenuGroupId="100"
                                Sequence="110"
                                ClientOnClickNavigateUrl="javascript:ChangeLayoutMode(false);"
                                />
                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_TakeOffline"
                                Text="<%$Resources:wss,siteactions_takeoffline%>"
                                Description="<%$Resources:wss,siteactions_takeofflinedescription%>"
                                ImageUrl="/_layouts/images/connecttospworkspace32.png"
                                MenuGroupId="100"
                                Sequence="120"
                                />

                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_CreatePage" 
                                Text="<%$Resources:wss,siteactions_createpage%>"
                                Description="<%$Resources:wss,siteactions_createpagedesc%>"
                                ImageUrl="/_layouts/images/NewContentPageHH.png"
                                MenuGroupId="200"
                                Sequence="210"
                                UseShortId="true"
                                ClientOnClickScriptContainingPrefixedUrl="if (LaunchCreateHandler('Page')) { OpenCreateWebPageDialog('~site/_layouts/createwebpage.aspx') }"
                                PermissionsString="AddListItems, EditListItems"
                                PermissionMode="All" />
                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_CreateDocLib" 
                                Text="<%$Resources:wss,siteactions_createdoclib%>"
                                Description="<%$Resources:wss,siteactions_createdoclibdesc%>"
                                ImageUrl="/_layouts/images/NewDocLibHH.png"
                                MenuGroupId="200"
                                Sequence="220"
                                UseShortId="true"
                                ClientOnClickScriptContainingPrefixedUrl="if (LaunchCreateHandler('DocLib')) { GoToPage('~site/_layouts/new.aspx?FeatureId={00bfea71-e717-4e80-aa17-d0c71b360101}&amp;ListTemplate=101') }"
                                PermissionsString="ManageLists"
                                PermissionMode="Any"
                                VisibilityFeatureId="00BFEA71-E717-4E80-AA17-D0C71B360101" /> 

                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_CreateSite" 
                                Text="<%$Resources:wss,siteactions_createsite%>"
                                Description="<%$Resources:wss,siteactions_createsitedesc%>"
                                ImageUrl="/_layouts/images/newweb32.png"
                                MenuGroupId="200"
                                Sequence="230"
                                UseShortId="true"
                                ClientOnClickScriptContainingPrefixedUrl="if (LaunchCreateHandler('Site')) { STSNavigate('~site/_layouts/newsbweb.aspx') }"
                                PermissionsString="ManageSubwebs,ViewFormPages"
                                PermissionMode="All" />

                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_Create" 
                                Text="<%$Resources:wss,siteactions_create%>"
                                Description="<%$Resources:wss,siteactions_createdesc%>"
                                MenuGroupId="200"
                                Sequence="240"
                                UseShortId="true"
                                ClientOnClickScriptContainingPrefixedUrl="if (LaunchCreateHandler('All')) { STSNavigate('~site/_layouts/create.aspx') }"
                                PermissionsString="ManageLists, ManageSubwebs"
                                PermissionMode="Any" />

                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_ViewAllSiteContents"
                                Text="<%$Resources:wss,quiklnch_allcontent%>"
                                Description="<%$Resources:wss,siteactions_allcontentdescription%>"
                                ImageUrl="/_layouts/images/allcontent32.png"
                                MenuGroupId="300"
                                Sequence="302"
                                UseShortId="true"
                                ClientOnClickNavigateUrl="~site/_layouts/viewlsts.aspx"
                                PermissionsString="ViewFormPages"
                                PermissionMode="Any" />
                             <SharePoint:MenuItemTemplate runat="server" id="MenuItem_EditSite"
                                Text="<%$Resources:wss,siteactions_editsite%>"  
                                Description="<%$Resources:wss,siteactions_editsitedescription%>"
                                ImageUrl="/_layouts/images/SharePointDesigner32.png" 
                                MenuGroupId="300"
                                Sequence="304"
                                UseShortId="true"
                                ClientOnClickScriptContainingPrefixedUrl="EditInSPD('~site/',true);"
                                PermissionsString="AddAndCustomizePages"
                                PermissionMode="Any"
                            />
                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_SitePermissions" 
                                Text="<%$Resources:wss,people_sitepermissions%>"
                                Description="<%$Resources:wss,siteactions_sitepermissiondescriptionv4%>"
                                ImageUrl="/_layouts/images/Permissions32.png"
                                MenuGroupId="300"
                                Sequence="310"
                                UseShortId="true" 
                                ClientOnClickNavigateUrl="~site/_layouts/user.aspx" 
                                PermissionsString="EnumeratePermissions"
                                PermissionMode="Any" />
                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_Settings"
                                Text="<%$Resources:wss,settings_pagetitle%>"  
                                Description="<%$Resources:wss,siteactions_sitesettingsdescriptionv4%>"
                                ImageUrl="/_layouts/images/settingsIcon.png" 
                                MenuGroupId="300"
                                Sequence="320"
                                UseShortId="true"
                                ClientOnClickNavigateUrl="~site/_layouts/settings.aspx"

                                PermissionsString="EnumeratePermissions,ManageWeb,ManageSubwebs,AddAndCustomizePages,ApplyThemeAndBorder,ManageAlerts,ManageLists,ViewUsageData"
                                PermissionMode="Any" />
                            <SharePoint:MenuItemTemplate runat="server" id="MenuItem_CommitNewUI" 
                                Text="<%$Resources:wss,siteactions_commitnewui%>"
                                Description="<%$Resources:wss,siteactions_commitnewuidescription%>"
                                ImageUrl="/_layouts/images/visualupgradehh.png"
                                MenuGroupId="300"
                                Sequence="330"
                                UseShortId="true"
                                ClientOnClickScriptContainingPrefixedUrl="GoToPage('~site/_layouts/prjsetng.aspx')"
                                PermissionsString="ManageWeb"
                                PermissionMode="Any"
                                ShowOnlyIfUIVersionConfigurationEnabled="true" />

                        </SharePoint:FeatureMenuTemplate>
                        </CustomTemplate>

                      </SharePoint:SiteActions></span>

                            </SharePoint:SPRibbonPeripheralContent>

                        </SharePoint:SPRibbon>
</div>
</div>
<SharePoint:SPPageStateControl runat="server" />
<div id="notificationArea" class="s4-noti">
</div>
<div>
                        <WebPartPages:WebPartAdder ID="WebPartAdder" runat="server" />
</div>

</ContentTemplate>
</SharePoint:UIVersionedContent>
</asp:Content>

