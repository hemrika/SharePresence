<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>

	<xsl:template name="FirstNWords">
		<xsl:param name="TextData"/>
		<xsl:param name="WordCount"/>
		<xsl:param name="MoreText"/>
		<xsl:choose>
			<xsl:when test="$WordCount &gt; 1 and (string-length(substring-before($TextData, ' ')) &gt; 0 or string-length(substring-before($TextData, '  ')) &gt; 0)">
				<xsl:value-of select="concat(substring-before($TextData, ' '), ' ')" disable-output-escaping="yes"/>
				<xsl:call-template name="FirstNWords">
					<xsl:with-param name="TextData" select="substring-after($TextData, ' ')"/>
					<xsl:with-param name="WordCount" select="$WordCount - 1"/>
					<xsl:with-param name="MoreText" select="$MoreText"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="(string-length(substring-before($TextData, ' ')) &gt; 0 or string-length(substring-before($TextData, '  ')) &gt; 0)">
				<xsl:value-of select="concat(substring-before($TextData, ' '), $MoreText)" disable-output-escaping="yes"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$TextData" disable-output-escaping="yes"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>