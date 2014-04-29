<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
  <xsl:output method="html" indent="no"/>
  <xsl:template name="MultiSelectValueCheck">
    <xsl:param name="MultiSelectColumn"/>
    <xsl:param name="CheckValue"/>
    <xsl:param name="Delimiter"/>
    <xsl:param name="ReturnTrue"/>
    <xsl:param name="ReturnFalse"/>
    <xsl:choose>
      <!-- Exact match -->
      <xsl:when test="$MultiSelectColumn = $CheckValue">
        <xsl:value-of select="$ReturnTrue"/>
      </xsl:when>
      <!-- First of multiple values match -->
      <xsl:when test="starts-with($MultiSelectColumn, concat($CheckValue, $Delimiter))">
        <xsl:value-of select="$ReturnTrue"/>
      </xsl:when>
      <!-- Match in the middle of multiple values -->
      <xsl:when test="contains($MultiSelectColumn, concat($Delimiter, $CheckValue, $Delimiter))">
        <xsl:value-of select="$ReturnTrue"/>
      </xsl:when>
      <!-- Last of multiple values match -->
      <xsl:when test="substring($MultiSelectColumn, string-length($MultiSelectColumn) - string-length(concat($Delimiter, $CheckValue)) + 1, string-length(concat($Delimiter, $CheckValue))) = concat($Delimiter, $CheckValue)">
        <xsl:value-of select="$ReturnTrue"/>
      </xsl:when>
      <!-- No match -->
      <xsl:otherwise>
        <xsl:value-of select="$ReturnFalse"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>