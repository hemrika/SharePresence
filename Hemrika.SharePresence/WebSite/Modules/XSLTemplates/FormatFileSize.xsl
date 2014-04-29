<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>

	<xsl:template name="FormatFileSize">
		<xsl:param name="FileSize" />
		<xsl:choose>
			<xsl:when test="$FileSize &gt; 1099511627776">
				<xsl:value-of select="concat(format-number($FileSize div 1099511627776, '###.#'), 'TB')"/>
			</xsl:when>
			<xsl:when test="$FileSize &gt; 1073741824">
				<xsl:value-of select="concat(format-number($FileSize div 1073741824, '###.#'), 'GB')"/>
			</xsl:when>
			<xsl:when test="$FileSize &gt; 1048576">
				<xsl:value-of select="concat(format-number($FileSize div 1048576, '###.#'), 'MB')"/>
			</xsl:when>
			<xsl:when test="$FileSize &gt; 1024">
				<xsl:value-of select="concat(format-number($FileSize div 1024, '###.#'), 'KB')"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat(format-number($FileSize, '###.#'), 'Bytes')"/>	
			</xsl:otherwise>
		 </xsl:choose>
	</xsl:template>

</xsl:stylesheet>