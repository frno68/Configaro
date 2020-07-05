Imports System.Xml
Imports System.IO
Public Class Summary

    Private Property FileInfos As FileInfo() = Nothing
    Private Property Path As String = ""
    Private Property ProjectID As String = ""

    Public Sub New(p_ProjectID As String)
        Me.ProjectID = p_ProjectID
        Me.Path = New ProjectSettings(Me.ProjectID).DirectoryInfo.FullName
        FileInfos = New DirectoryInfo(Me.Path & "\_Blocks").GetFiles
    End Sub

    Public Function ToXMLDocument() As XmlDocument
        Dim m_XMLDocument As New XmlDocument
        Dim m_XMLDocumentToReturn As New XmlDocument
        Dim m_HTMLStringToReturn As String = ""
        Dim m_XMLNode As XmlNode
        Dim m_XMLNodeSummary As XmlNode
        Dim m_XMLAttribute As XmlAttribute
        Dim m_XmlDeclaration As XmlDeclaration = m_XMLDocumentToReturn.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XMLDocumentToReturn.AppendChild(m_XmlDeclaration)

        Dim m_XMLRootNode As XmlNode = m_XMLDocumentToReturn.CreateElement("Object")
        m_XMLDocumentToReturn.AppendChild(m_XMLRootNode)

        Dim m_Protocol As New Protocol(Me.ProjectID)

        For Each m_FileInfo As FileInfo In Me.FileInfos

            m_XMLDocument.Load(m_FileInfo.FullName)
            m_XMLDocument = m_Protocol.Analyze(m_XMLDocument)

            m_XMLNode = m_XMLDocumentToReturn.CreateElement("Summary")
            m_XMLAttribute = m_XMLDocumentToReturn.CreateAttribute("Id")
            m_XMLNodeSummary = m_XMLDocument.SelectSingleNode("/Object/Summary")
            If Not m_XMLNodeSummary Is Nothing Then
                m_XMLAttribute.Value = m_XMLNodeSummary.Attributes("id").Value
                m_XMLNode.Attributes.Append(m_XMLAttribute)
                m_XMLNode.InnerXml = "<![CDATA[" & m_XMLNodeSummary.InnerXml & "]]>"
            End If
            m_XMLRootNode.AppendChild(m_XMLNode)

        Next

        Return m_XMLDocumentToReturn

    End Function

End Class
