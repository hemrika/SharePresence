<%@ Control Language="C#" Debug="true" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" Namespace="Microsoft.SharePoint.WebControls" %>
<%@ Register Tagprefix="HTML5" Assembly="Hemrika.SharePresence.HTML5, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" namespace="Hemrika.SharePresence.Html5.WebControls" %>
<%@ Register TagPrefix="HTML5Field" Assembly="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" Namespace="Hemrika.SharePresence.WebSite.FieldTypes" %>
<SharePoint:RenderingTemplate ID="HTML5PublishingDate" runat="server">
  <Template>
      <HTML5:Time ID="html_date" runat="server" IsPubDate="true" CssClass="time" Pattern="dddd, MMMM d, yyyy" Style="max-height:100%; max-width:100%;"/>
 </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5PublishingDateDisplay" runat="server">
  <Template>
      <HTML5:Time ID="html_date" runat="server"  IsPubDate="true" CssClass="time" Pattern="dddd, MMMM d, yyyy" Style="max-height:100%; max-width:100%;"/>
 </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5Title" runat="server">
  <Template>
      <asp:HiddenField ID="html_title_hidden" runat="server" />
      <HTML5:Title ID="html_title" runat="server" class="HTML5-editable Title" Style="max-height:100%; max-width:100%;"></HTML5:Title>
 </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5TitleDisplay" runat="server">
  <Template>
      <HTML5:Title ID="html_title" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Title>
 </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5Header" runat="server">
    <Template>
    <asp:HiddenField ID="html_header_hidden" runat="server" />
    <HTML5:Header ID="html_header" runat="server" class="HTML5-editable Header" Style="max-height:100%; max-width:100%;"></HTML5:Header>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5HeaderDisplay" runat="server">
    <Template>
      <HTML5:Header ID="html_header" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Header>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5Details" runat="server">
    <Template>
        <asp:HiddenField ID="html_details_hidden" runat="server" />
        <HTML5:Details ID="html_details" runat="server" class="HTML5-editable Details" Style="max-height:100%; max-width:100%;"></HTML5:Details>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5DetailsDisplay" runat="server">
    <Template>
      <HTML5:Details ID="html_details" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Details>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5Section" runat="server">
    <Template>
        <asp:HiddenField ID="html_section_hidden" runat="server" />
        <HTML5:Section ID="html_section" runat="server" class="HTML5-editable Section" Style="max-height:100%; max-width:100%;" ></HTML5:Section>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5SectionDisplay" runat="server">
    <Template>
      <HTML5:Section ID="html_section" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Section>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5Footer" runat="server">
    <Template>
    <asp:HiddenField ID="html_footer_hidden" runat="server" />
    <HTML5:Footer ID="html_footer" runat="server" class="HTML5-editable Footer" Style="max-height:100%; max-width:100%;"></HTML5:Footer>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5FooterDisplay" runat="server">
    <Template>
      <HTML5:Footer ID="html_footer" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Footer>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5Image" runat="server">
    <Template>
      <HTML5Field:HTML5ImagePicker id="ImagePicker" Width="300" runat="server" ></HTML5Field:HTML5ImagePicker>
      <br /><br />
        <asp:HiddenField ID="html_image_hidden" runat="server" />
        <span class="HTML5-editable Image" >
            <HTML5:Image ID="html_image" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Image>
        </span>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5ImageDisplay" runat="server">
    <Template>
      <HTML5:Image ID="html_image" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Image>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5Video" runat="server">
    <Template>
      <HTML5:Video ID="html_video" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Video>
      <br /><br />
      <HTML5Field:HTML5VideoPicker id="VideoPicker" Width="300" runat="server" TypeLabelCssClass="ms-input ms-spellcheck-false" ></HTML5Field:HTML5VideoPicker>
      <!-- <HTML5:Image ID="html_image" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Image> -->
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5VideoDisplay" runat="server">
    <Template>
      <HTML5:Video ID="html_video" runat="server" Style="max-height:100%; max-width:100%;"></HTML5:Video>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5RatingDisplay" runat="server">
    <Template>
        <HTML5:RatingInput AutoPostBack="true" ID="RatingInput" runat="server" style="display:none;"></HTML5:RatingInput>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5Rating" runat="server">
    <Template>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5BookmarkDisplay" runat="server">
    <Template>
        <HTML5:NumberInput AutoPostBack="true" ID="BookmarkInput" runat="server" style="display:none;" OnClick="input_rating_TextChanged"></HTML5:NumberInput>
    </Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate ID="HTML5Bookmark" runat="server">
    <Template>
    </Template>
</SharePoint:RenderingTemplate>