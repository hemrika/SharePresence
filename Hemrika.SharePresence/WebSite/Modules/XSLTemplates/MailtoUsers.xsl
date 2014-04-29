<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>

	<xsl:template name="MailtoUsers">
		<xsl:param name="People"/>
		<xsl:choose>
			<xsl:when test="contains($People, '&lt;tr&gt;')">
				<!-- Get the next User -->
				<xsl:call-template name="MailtoUser">
					<xsl:with-param name="Person" select="$People"/>
				</xsl:call-template>
				<!-- Add a separator if this isn't the last value -->
				<xsl:if test="string-length(substring-after(substring-after($People, '&lt;tr&gt;'), '&lt;tr&gt;')) &gt; 0 ">
					<xsl:text> </xsl:text>
				</xsl:if>
				<xsl:call-template name="MailtoUsers">
					<xsl:with-param name="People" select="substring-after($People, '&lt;/tr&gt;')"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<!-- Get the last UserID -->
				<xsl:call-template name="MailtoUser">
					<xsl:with-param name="Person" select="$People"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>