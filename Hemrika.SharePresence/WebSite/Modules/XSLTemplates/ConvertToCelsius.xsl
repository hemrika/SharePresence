<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>

	<xsl:template name="ConvertToCelsius">
		<xsl:param name="paramTemp" />
		<xsl:param name="AddNotation" />
		<xsl:variable name="DegreesCelsius" select="format-number((($paramTemp - 32) div 9) * 5, &quot;#,##0.0;-#,##0.0&quot;)" />
		<xsl:choose> 
			<xsl:when test="$AddNotation = 'True'"> 
				<xsl:value-of select="concat($DegreesCelsius, '&#176;C')" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$DegreesCelsius" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>