<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema"
                xmlns:d="http://schemas.microsoft.com/sharepoint/dsp"
                version="1.0"
                exclude-result-prefixes="xsl msxsl ddwrt"
                xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime"
                xmlns:asp="http://schemas.microsoft.com/ASPNET/20"
                xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:SharePoint="Microsoft.SharePoint.WebControls"
                xmlns:ddwrt2="urn:frontpage:internal">
  <xsl:template match="FieldRef[@Name = 'DocIcon']" mode="Computed_DocIcon_body">

    <xsl:param name="thisNode" select="." />
    <xsl:param name="folderUrlAdditionalQueryString" select="''"/>
    <xsl:choose>
      <xsl:when test="$thisNode/@FSObjType='1'">
        <xsl:variable name="alttext">
          <xsl:choose>
            <xsl:when test="starts-with($thisNode/@ContentTypeId, &quot;0x0120D5&quot;)">
              <xsl:value-of select="$thisNode/../@itemname_documentset"/>: <xsl:value-of select="$thisNode/@FileLeafRef"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$thisNode/../@listformtitle_folder"/>: <xsl:value-of select="$thisNode/@FileLeafRef"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="mapico" select="$thisNode/@HTML_x0020_File_x0020_Type.File_x0020_Type.mapico"/>
        <xsl:variable name="folderIconPath">
          <xsl:call-template name="GetFolderIconSourcePath">
            <xsl:with-param name="thisNode" select="$thisNode"/>
          </xsl:call-template>
        </xsl:variable>
        <!-- This is a folder -->
        <xsl:choose>
          <xsl:when test="$RecursiveView='1'">
            <img border="0" alt="{$alttext}" src="{$folderIconPath}" />
            <xsl:choose>
              <xsl:when test="$thisNode/@IconOverlay != ''">
                <img src="/_layouts/images/{$thisNode/@IconOverlay.mapoly}" class="ms-vb-icon-overlay" alt="" title="" />
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:variable name="FolderCTID">
              <xsl:value-of select="$PagePathFinal" />RootFolder=<xsl:value-of select="$thisNode/@FileRef.urlencode" /><xsl:value-of select="$ShowWebPart"/>&amp;FolderCTID=<xsl:value-of select="$thisNode/@ContentTypeId" />&amp;View=<xsl:value-of select="$View"/><xsl:value-of select="$folderUrlAdditionalQueryString"/>
            </xsl:variable>
            <a href="{$FolderCTID}" onmousedown ="VerifyFolderHref(this, event, '{$thisNode/@File_x0020_Type.url}','{$thisNode/@File_x0020_Type.progid}','{$XmlDefinition/List/@DefaultItemOpen}', '{$thisNode/@HTML_x0020_File_x0020_Type.File_x0020_Type.mapcon}', '{$thisNode/@HTML_x0020_File_x0020_Type}', '{$thisNode/@serverurl.progid}')"
               onclick="return HandleFolder(this,event,&quot;{$PagePathFinal}RootFolder=&quot; + escapeProperly(&quot;{$thisNode/@FileRef}&quot;) + '{$ShowWebPart}&amp;FolderCTID={$thisNode/@ContentTypeId}&amp;View={$View}{$folderUrlAdditionalQueryString}','TRUE','FALSE','{$thisNode/@File_x0020_Type.url}','{$thisNode/@File_x0020_Type.progid}','{$XmlDefinition/List/@DefaultItemOpen}','{$thisNode/@HTML_x0020_File_x0020_Type.File_x0020_Type.mapcon}','{$thisNode/@HTML_x0020_File_x0020_Type}','{$thisNode/@serverurl.progid}','{$thisNode/@CheckoutUser.id}','{$Userid}','{$XmlDefinition/List/@ForceCheckout}','{$thisNode/@IsCheckedoutToLocal}','{$thisNode/@PermMask}');">
              <img border="0" alt="{$alttext}" title="{$alttext}" src="{$folderIconPath}" />
              <xsl:choose>
                <xsl:when test="$thisNode/@IconOverlay != ''">
                  <img src="/_layouts/images/{$thisNode/@IconOverlay.mapoly}" class="ms-vb-icon-overlay" alt="" title="" />
                </xsl:when>
              </xsl:choose>
            </a>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$IsDocLib">
            <!-- warning: this code has optimization in webpart. Change it must change the webpart code too!-->
            <xsl:choose>
              <xsl:when test="not ($thisNode/@IconOverlay) or $thisNode/@IconOverlay =''">
                <xsl:choose>
                  <xsl:when test="not ($thisNode/@CheckoutUser.id) or $thisNode/@CheckoutUser.id =''">
                    <img border="0" alt="{$thisNode/@FileLeafRef}" title="{$thisNode/@FileLeafRef}" src="/_layouts/images/{$thisNode/@HTML_x0020_File_x0020_Type.File_x0020_Type.mapico}">
                      <xsl:attribute name="onClick">
                        window.location='<xsl:value-of select="$thisNode/@FileRef"/>'
                      </xsl:attribute>
                    </img>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:variable name="alttext">
                      <xsl:value-of select="$thisNode/@FileLeafRef"/><xsl:text disable-output-escaping="yes" ddwrt:nbsp-preserve="yes">&#10;</xsl:text><xsl:value-of select="$thisNode/../@managecheckedoutfiles_header_checkedoutby"/>: <xsl:value-of select="$thisNode/@CheckoutUser.title"/>
                    </xsl:variable>
                    <img border="0" alt="{$alttext}" title="{$alttext}" src="/_layouts/images/{$thisNode/@HTML_x0020_File_x0020_Type.File_x0020_Type.mapico}" >
                      <xsl:attribute name="onClick">
                        window.location='<xsl:value-of select="$thisNode/@FileRef"/>'
                      </xsl:attribute>
                    </img>
                    <img src="/_layouts/images/checkoutoverlay.gif" class="ms-vb-icon-overlay" alt="{$alttext}" title="{$alttext}" />
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise >
                <img border="0" alt="{$thisNode/@FileLeafRef}" title="{$thisNode/@FileLeafRef}" src="/_layouts/images/{$thisNode/@IconOverlay.mapico}" />
                <img src="/_layouts/images/{$thisNode/@IconOverlay.mapoly}" class="ms-vb-icon-overlay" alt="" title="" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <img border="0" src="/_layouts/images/{$thisNode/@HTML_x0020_File_x0020_Type.File_x0020_Type.mapico}">
              <xsl:attribute name="alt">
                <xsl:value-of select="$thisNode/@Title"/>
              </xsl:attribute>
              <xsl:attribute name="title">
                <xsl:value-of select="$thisNode/@Title"/>
              </xsl:attribute>
            </img>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template >
  <xsl:template match="FieldRef[@Name = 'isbn']" mode="Text_body">

    <xsl:param name="thisNode" select="." />

    <span style="background-color:lightgreen;font-weight:bold">

      Hey1<xsl:value-of select="$thisNode/@*[name()=current()/@Name]" />

    </span>

  </xsl:template >

</xsl:stylesheet>
