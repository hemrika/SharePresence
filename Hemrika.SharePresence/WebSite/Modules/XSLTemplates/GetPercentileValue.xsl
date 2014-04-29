<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>

	<xsl:template name="GetPercentileValue">
		<xsl:param name="Rows"/>
		<xsl:param name="ColumnName"/>
		<xsl:param name="Percentile"/>
		<xsl:variable name="Pos" select="ceiling($Percentile div 100 * count($Rows))"/>
		<xsl:for-each select="$Rows">
			<xsl:sort select="@*[name()=$ColumnName]" order="ascending" data-type="number"/>
			<xsl:if test="position() = $Pos">
				<xsl:value-of select="@*[name()=$ColumnName]"/>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

</xsl:stylesheet>