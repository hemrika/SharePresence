﻿<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>

	<xsl:template name="GetUserIDs">
		<xsl:param name="People"/>
		<xsl:param name="Separator" select="';'"/>
		<xsl:choose>
			<xsl:when test="contains($People, '&lt;tr&gt;')">
				<!-- Get the next UserID -->
				<xsl:call-template name="GetUserID">
					<xsl:with-param name="Person" select="$People"/>
				</xsl:call-template>
				<!-- Add the separator if this isn't the last value -->
				<xsl:if test="string-length(substring-after(substring-after($People, '&lt;tr&gt;'), '&lt;tr&gt;')) &gt; 0 ">
					<xsl:value-of select="$Separator"/>
				</xsl:if>
				<xsl:call-template name="GetUserIDs">
					<xsl:with-param name="People" select="substring-after($People, '&lt;/tr&gt;')"/>
					<xsl:with-param name="Separator" select="$Separator"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<!-- Get the last UserID -->
				<xsl:call-template name="GetUserID">
					<xsl:with-param name="Person" select="$People"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>