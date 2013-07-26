<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:template match="/">
		<html xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<title>
					Wiki - Tienes una solicitud pendiente.
				</title>
			</head>
			<body>
				Título: <xsl:value-of select="WikiSerializable/Title"/><br/>
				<xsl:value-of select="WikiSerializable/Content"/>
			</body>
		</html>
    </xsl:template>
</xsl:stylesheet>
