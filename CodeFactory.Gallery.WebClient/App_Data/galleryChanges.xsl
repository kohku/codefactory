<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<html xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<title>Notification</title>
			</head>
			<body>
				La galería "<xsl:value-of select="Gallery/Title"/>" ha sido actualizada.<br />
				Fecha: <xsl:value-of select="Gallery/LastUpdated"/><br />
				Estatus: <xsl:value-of select="Gallery/Status"/>
				Link: http://localhost:1703/WebClient/<xsl:value-of select="Gallery/RelativeLink"/>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet> 

