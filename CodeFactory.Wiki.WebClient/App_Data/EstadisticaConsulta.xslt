<?xml version="1.0"?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:DateTimeNow="ext:DateTimeNow">
  <xsl:output method="xml" indent="yes" encoding="utf-8"/>
  <xsl:param name="totalcount" xml:space="default"/>
  <xsl:template match="/">
    <xsl:processing-instruction xml:space="default" name="mso-application">progid="Excel.Sheet"</xsl:processing-instruction>
    <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
              xmlns:o="urn:schemas-microsoft-com:office:office"
              xmlns:x="urn:schemas-microsoft-com:office:excel"
              xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
              xmlns:html="http://www.w3.org/TR/REC-html40">
      <DocumentProperties xmlns="urn:schemas-microsoft-com:office:office">
        <Author>CodeFactory</Author>
        <LastAuthor>CodeFactory</LastAuthor>
        <Created>2009-06-04T18:11:49Z</Created>
        <Version>11.9999</Version>
      </DocumentProperties>
      <ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
        <WindowHeight>8190</WindowHeight>
        <WindowWidth>9435</WindowWidth>
        <WindowTopX>240</WindowTopX>
        <WindowTopY>30</WindowTopY>
        <ProtectStructure>False</ProtectStructure>
        <ProtectWindows>False</ProtectWindows>
      </ExcelWorkbook>
      <Styles>
        <Style ss:ID="Default" ss:Name="Normal">
          <Alignment ss:Vertical="Bottom"/>
          <Borders/>
          <Font/>
          <Interior/>
          <NumberFormat/>
          <Protection/>
        </Style>
        <Style ss:ID="s21">
          <Font ss:Size="8"/>
        </Style>
        <Style ss:ID="s23">
          <Alignment ss:Vertical="Bottom"/>
          <Font x:Family="Swiss" ss:Size="12" ss:Color="#FFFFFF" ss:Bold="1"/>
          <Interior ss:Color="#666699" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s24">
          <Font x:Family="Swiss" ss:Size="8" ss:Color="#808080" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s25">
          <Font ss:Size="8" ss:Color="#808080"/>
        </Style>
        <Style ss:ID="s26">
          <Alignment ss:Vertical="Bottom"/>
          <Font ss:Size="8" ss:Color="#808080"/>
        </Style>
        <Style ss:ID="s29">
          <Borders>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font x:Family="Swiss" ss:Size="8" ss:Color="#333399" ss:Bold="1"/>
          <Interior ss:Color="#33CCCC" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s31">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font x:Family="Swiss" ss:Size="8" ss:Color="#333399" ss:Bold="1"/>
          <Interior ss:Color="#33CCCC" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s34">
          <Borders>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font x:Family="Swiss" ss:Size="8" ss:Color="#333399" ss:Bold="1"/>
          <Interior ss:Color="#33CCCC" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s35">
          <Borders>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
        </Style>
        <Style ss:ID="s37">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
        </Style>
        <Style ss:ID="s38">
          <Borders>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s40">
          <Alignment ss:Vertical="Bottom"/>
          <Font ss:Size="8" ss:Color="#808080"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s42">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s43">
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
        </Style>
        <Style ss:ID="s45">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
        </Style>
        <Style ss:ID="s48">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
        </Style>
        <Style ss:ID="s49">
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s51">
          <Alignment ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s54">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s55">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Font ss:Size="8" ss:Color="#808080"/>
        </Style>
        <Style ss:ID="s58">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Font ss:Size="8" ss:Color="#808080"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s59">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
        </Style>
        <Style ss:ID="s62">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#0000FF"/>
          </Borders>
          <Font ss:Size="8" ss:Color="#808080"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
      </Styles>
      <Worksheet ss:Name="Reporte">
        <Table x:FullColumns="1" x:FullRows="1" ss:StyleID="s21" ss:DefaultRowHeight="11.25">
          <Column ss:StyleID="s21" ss:AutoFitWidth="0" ss:Width="60"/>
          <Column ss:StyleID="s21" ss:AutoFitWidth="0" ss:Width="67.5"/>
          <Column ss:StyleID="s21" ss:AutoFitWidth="0" ss:Width="60" ss:Span="1"/>
          <Column ss:Index="5" ss:StyleID="s21" ss:AutoFitWidth="0" ss:Width="67.5"/>
          <Row ss:AutoFitHeight="0" ss:Height="15.75">
            <Cell ss:MergeAcross="5" ss:StyleID="s23">
              <Data ss:Type="String">Reporte de Consultas</Data>
            </Cell>
          </Row>
          <Row ss:AutoFitHeight="0">
            <Cell ss:StyleID="s24">
              <Data ss:Type="String">Fecha inicio</Data>
            </Cell>
            <Cell ss:StyleID="s25">
              <Data ss:Type="String">
                <xsl:value-of select="EstadisticaConsulta/FechaInicio"/>
              </Data>
            </Cell>
            <Cell ss:Index="4" ss:StyleID="s24">
              <Data ss:Type="String">Fecha fin</Data>
            </Cell>
            <Cell ss:StyleID="s25">
              <Data ss:Type="String">
                <xsl:value-of select="EstadisticaConsulta/FechaFin"/>
              </Data>
            </Cell>
          </Row>
          <Row ss:AutoFitHeight="0"/>
          <Row ss:AutoFitHeight="0">
            <Cell ss:StyleID="s29">
              <Data ss:Type="String">Fecha</Data>
            </Cell>
            <Cell ss:MergeAcross="1" ss:StyleID="s31">
              <Data ss:Type="String">Título</Data>
            </Cell>
            <Cell ss:StyleID="s31">
              <Data ss:Type="String">Usuario</Data>
            </Cell>
            <Cell ss:StyleID="s31">
              <Data ss:Type="String">Tipo</Data>
            </Cell>
            <Cell ss:StyleID="s34">
              <Data ss:Type="String">Ocasiones</Data>
            </Cell>
          </Row>
          <xsl:for-each select="EstadisticaConsulta/Hits/StatisticTraceHit">
            <Row ss:AutoFitHeight="0">
              <Cell>
                <xsl:choose>
                  <xsl:when test="position() = $totalcount">
                    <xsl:choose>
                      <xsl:when test="position() mod 2 &gt; 0">
                        <xsl:attribute name="ss:StyleID">s49</xsl:attribute>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:attribute name="ss:StyleID">s43</xsl:attribute>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:when test="position() mod 2 &gt; 0">
                    <xsl:attribute name="ss:StyleID">s38</xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="ss:StyleID">s35</xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
                <Data ss:Type="String">
                  <xsl:value-of select="Date"/>
                </Data>
              </Cell>
              <Cell ss:MergeAcross="1">
                <xsl:choose>
                  <xsl:when test="position() = $totalcount">
                    <xsl:choose>
                      <xsl:when test="position() mod 2 &gt; 0">
                        <xsl:attribute name="ss:StyleID">s62</xsl:attribute>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:attribute name="ss:StyleID">s59</xsl:attribute>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:when test="position() mod 2 &gt; 0">
                    <xsl:attribute name="ss:StyleID">s58</xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="ss:StyleID">s55</xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
                <Data ss:Type="String">
                  <xsl:value-of select="Title"/>
                </Data>
              </Cell>
              <Cell>
                <xsl:choose>
                  <xsl:when test="position() = $totalcount">
                    <xsl:choose>
                      <xsl:when test="position() mod 2 &gt; 0">
                        <xsl:attribute name="ss:StyleID">s51</xsl:attribute>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:attribute name="ss:StyleID">s45</xsl:attribute>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:when test="position() mod 2 &gt; 0">
                    <xsl:attribute name="ss:StyleID">s40</xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="ss:StyleID">s26</xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
                <Data ss:Type="String">
                  <xsl:value-of select="UserName"/>
                </Data>
              </Cell>
              <Cell>
                <xsl:choose>
                  <xsl:when test="position() = $totalcount">
                    <xsl:choose>
                      <xsl:when test="position() mod 2 &gt; 0">
                        <xsl:attribute name="ss:StyleID">s51</xsl:attribute>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:attribute name="ss:StyleID">s45</xsl:attribute>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:when test="position() mod 2 &gt; 0">
                    <xsl:attribute name="ss:StyleID">s40</xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="ss:StyleID">s26</xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
                <Data ss:Type="String">
                  <xsl:value-of select="Type"/>
                </Data>
              </Cell>
              <Cell>
                <xsl:choose>
                  <xsl:when test="position() = $totalcount">
                    <xsl:choose>
                      <xsl:when test="position() mod 2 &gt; 0">
                        <xsl:attribute name="ss:StyleID">s54</xsl:attribute>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:attribute name="ss:StyleID">s48</xsl:attribute>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:when test="position() mod 2 &gt; 0">
                    <xsl:attribute name="ss:StyleID">s42</xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="ss:StyleID">s37</xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
                <Data ss:Type="Number">
                  <xsl:value-of select="Hits"/>
                </Data>
              </Cell>
            </Row>
          </xsl:for-each>
        </Table>
        <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
          <Unsynced/>
          <Print>
            <ValidPrinterInfo/>
            <HorizontalResolution>600</HorizontalResolution>
            <VerticalResolution>600</VerticalResolution>
          </Print>
          <Selected/>
          <Panes>
            <Pane>
              <Number>3</Number>
              <RangeSelection>R1C1:R1C6</RangeSelection>
            </Pane>
          </Panes>
          <ProtectObjects>False</ProtectObjects>
          <ProtectScenarios>False</ProtectScenarios>
        </WorksheetOptions>
      </Worksheet>
    </Workbook>
  </xsl:template>
</xsl:stylesheet>