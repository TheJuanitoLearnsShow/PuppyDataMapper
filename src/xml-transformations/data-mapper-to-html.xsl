<?xml version="1.0"?>


<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:m="http://example/mapper"
>

  <xsl:template match="/">
    <html>
      <body>
        <h2><xsl:value-of select="/m:mapper/m:name" /> (<xsl:value-of
            select="/m:mapper/m:outputType" />)</h2>
        <p>
          <xsl:value-of select="/m:mapper/m:description" />
        </p>

        <h3>Inputs</h3>
        <table border="1">
          <tr>
            <th>Name</th>
            <th>System Type</th>
            <th>Reference Name</th>
          </tr>
          <xsl:for-each select="/m:mapper/m:inputs/m:input">
            <tr>
              <td>
                <xsl:value-of select="m:name" />
              </td>
              <td>
                <xsl:value-of select="m:type" />
              </td>
              <td>
                <xsl:value-of select="m:referenceName" />
              </td>
            </tr>
          </xsl:for-each>
        </table>


        <h3>Fields Mapping</h3>

        <table border="1">
          <tr>
            <th>Name</th>
            <th>Destination Field</th>
            <th>Notes</th>
            <th>Formula</th>
            <th>Inputs</th>
            <th>Example</th>
            <th>Output System Type</th>
          </tr>
          <xsl:for-each select="/m:mapper/m:maps/m:map">
            <tr>
              <td>
                <xsl:value-of select="m:name" />
              </td>
              <td>
                <xsl:value-of select="m:outputTo" />
              </td>
              <td>
                <xsl:value-of select="m:description" />
              </td>
              <td>
                <xsl:value-of select="m:formula" />
              </td>
              <td>
                <ol>

                  <xsl:for-each select="m:inputs/m:input">
                    <li>
                      <b><xsl:value-of select="m:source" /> -&gt; <xsl:value-of select="m:formula" />
                      </b> <p><xsl:value-of select="m:comments" /></p>
                      
                    </li>
                  </xsl:for-each>
                </ol>

              </td>
              <td>
                <xsl:value-of select="m:example" />
              </td>
              <td>
                <xsl:value-of select="m:outputType" />
              </td>
            </tr>
          </xsl:for-each>
        </table>

      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>