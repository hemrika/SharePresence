﻿<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>

<xsl:template name="FixAmpersands">
	<xsl:param name="StringValue"/>

	<xsl:variable name="Ampersand" select="'&amp;'" />
	<xsl:choose>
		<xsl:when test="contains($StringValue, $Ampersand)">
			<xsl:value-of select="concat(substring-before($StringValue, $Ampersand), '&amp;amp;', substring-after($StringValue, $Ampersand))"/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="$StringValue"/>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

</xsl:stylesheet>