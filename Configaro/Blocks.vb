Imports System.Xml
Imports System.IO
Public Class Blocks

    Private Property FileInfos As FileInfo() = Nothing
    Private Property Path As String = ""
    Private Property ProjectID As String = "-1"

    Public Sub New()
        Me.Path = HttpContext.Current.Server.MapPath("_Blocks")
        FileInfos = New DirectoryInfo(Me.Path).GetFiles
    End Sub

    Public Sub New(p_ProjectID As String)
        Me.ProjectID = p_ProjectID
        Me.Path = New ProjectSettings(p_ProjectID).DirectoryInfo.FullName & "/_Blocks"
        FileInfos = New DirectoryInfo(Me.Path).GetFiles
    End Sub

    Public Function ToHtml() As String

        Dim m_Block As Block = Nothing
        Dim m_StringWriter As New StringWriter
        Dim m_HtmlTextWriter As New HtmlTextWriter(m_StringWriter)

        For Each m_FileInfo As FileInfo In Me.FileInfos

            m_Block = New Block(m_FileInfo.Name)

            m_HtmlTextWriter.AddAttribute("data-role", "collapsible")
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div)

            m_HtmlTextWriter.AddAttribute("BlockID", m_FileInfo.Name)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.H4)

            m_HtmlTextWriter.AddAttribute("href", "/")
            m_HtmlTextWriter.AddAttribute("BlockId", m_Block.BlockID)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.A)
            m_HtmlTextWriter.WriteLine(m_Block.BlockID)
            m_HtmlTextWriter.RenderEndTag()

            m_HtmlTextWriter.RenderEndTag()

            m_HtmlTextWriter.AddAttribute("for", m_Block.BlockID & "_North")
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Label)
            m_HtmlTextWriter.WriteLine("North")
            m_HtmlTextWriter.RenderEndTag()
            m_HtmlTextWriter.AddAttribute("id", m_Block.BlockID & "_North")
            m_HtmlTextWriter.AddAttribute("type", "text")
            m_HtmlTextWriter.AddAttribute("value", m_Block.North)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Input)
            m_HtmlTextWriter.RenderEndTag()

            m_HtmlTextWriter.AddAttribute("for", m_Block.BlockID & "_East")
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Label)
            m_HtmlTextWriter.WriteLine("East")
            m_HtmlTextWriter.RenderEndTag()
            m_HtmlTextWriter.AddAttribute("id", m_Block.BlockID & "_East")
            m_HtmlTextWriter.AddAttribute("type", "text")
            m_HtmlTextWriter.AddAttribute("value", m_Block.East)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Input)
            m_HtmlTextWriter.RenderEndTag()

            m_HtmlTextWriter.AddAttribute("for", m_Block.BlockID & "_South")
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Label)
            m_HtmlTextWriter.WriteLine("South")
            m_HtmlTextWriter.RenderEndTag()
            m_HtmlTextWriter.AddAttribute("id", m_Block.BlockID & "_South")
            m_HtmlTextWriter.AddAttribute("type", "text")
            m_HtmlTextWriter.AddAttribute("value", m_Block.South)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Input)
            m_HtmlTextWriter.RenderEndTag()

            m_HtmlTextWriter.AddAttribute("for", m_Block.BlockID & "_West")
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Label)
            m_HtmlTextWriter.WriteLine("West")
            m_HtmlTextWriter.RenderEndTag()
            m_HtmlTextWriter.AddAttribute("id", m_Block.BlockID & "_West")
            m_HtmlTextWriter.AddAttribute("type", "text")
            m_HtmlTextWriter.AddAttribute("value", m_Block.West)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Input)
            m_HtmlTextWriter.RenderEndTag()

            m_HtmlTextWriter.RenderEndTag()
        Next

        Return m_StringWriter.ToString
    End Function

    Public Function ToXMLDocument() As XmlDocument

        Dim m_XMLDocument As New XmlDocument
        Dim m_XMLDocumentToReturn As New XmlDocument
        Dim m_HTMLStringToReturn As String = ""
        Dim m_XMLNode As XmlNode
        Dim m_XmlDeclaration As XmlDeclaration = m_XMLDocumentToReturn.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XMLDocumentToReturn.AppendChild(m_XmlDeclaration)

        Dim m_XMLRootNode As XmlNode = m_XMLDocumentToReturn.CreateElement("Object")
        m_XMLDocumentToReturn.AppendChild(m_XMLRootNode)

        For Each m_FileInfo As FileInfo In Me.FileInfos

            m_XMLDocument.Load(m_FileInfo.FullName)
            m_XMLDocument = New Protocol(Me.ProjectID).Analyze(m_XMLDocument)

            m_XMLNode = m_XMLDocumentToReturn.CreateElement("Representation")

            Dim m_XMLNodeRepresentation As XmlNode = m_XMLDocument.SelectSingleNode("/Object/Representation")

            If Not m_XMLNodeRepresentation Is Nothing Then
                m_XMLNode.InnerXml = "<![CDATA[" & m_XMLNodeRepresentation.InnerXml & "]]>"
            Else
                m_XMLNode.InnerXml = "<![CDATA[" & "" & "]]>"
            End If

            m_XMLRootNode.AppendChild(m_XMLNode)

        Next

        Return m_XMLDocumentToReturn

    End Function

End Class
