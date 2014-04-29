<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>

	<xsl:template name="MultiSelectDisplay"> 
		<xsl:param name="MultiSelectValue"/> 
		<xsl:param name="MultiSelectDelimiter"/> 
		<xsl:param name="MultiSelectSeparator"/> 
		<xsl:choose> 
			<xsl:when test="contains($MultiSelectValue, $MultiSelectDelimiter)"> 
				<xsl:value-of select="concat(substring-before($MultiSelectValue, $MultiSelectDelimiter), $MultiSelectSeparator)" disable-output-escaping="yes"/> 
				<xsl:call-template name="MultiSelectDisplay"> 
					<xsl:with-param name="MultiSelectValue" select="substring-after($MultiSelectValue, $MultiSelectDelimiter)"/> 
					<xsl:with-param name="MultiSelectDelimiter" select="$MultiSelectDelimiter"/> 
					<xsl:with-param name="MultiSelectSeparator" select="$MultiSelectSeparator"/> 
				</xsl:call-template> 
			</xsl:when> 
			<xsl:otherwise> 
				<xsl:value-of select="$MultiSelectValue" disable-output-escaping="yes"/> 
			</xsl:otherwise> 
		</xsl:choose> 
	</xsl:template> 

</xsl:stylesheet>