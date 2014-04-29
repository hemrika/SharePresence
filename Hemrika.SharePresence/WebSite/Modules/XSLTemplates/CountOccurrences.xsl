<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>

	<xsl:template name="CountOccurrences">
		<xsl:param name="withinString" />
		<xsl:param name="findString" />
		<xsl:param name="counter" select="0" />
		
		<xsl:choose>
			<xsl:when test="contains($withinString, $findString)">
				<xsl:call-template name="CountOccurrences">
					<xsl:with-param name="withinString" select="substring-after($withinString, $findString)" />
					<xsl:with-param name="findString" select="$findString" />
					<xsl:with-param name="counter" select="$counter + 1" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$counter" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>