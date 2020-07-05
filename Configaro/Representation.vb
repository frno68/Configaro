Imports System.Xml
Imports System.IO
Public Class Representation

    Private Property FileInfos As FileInfo() = Nothing
    Private Property Path As String = ""
    Private Property ProjectID As String = ""

    Public Sub New(p_ProjectID As String)
        Me.ProjectID = p_ProjectID
        Me.Path = New ProjectSettings(Me.ProjectID).DirectoryInfo.FullName
        FileInfos = New DirectoryInfo(Me.Path & "\_Blocks").GetFiles
    End Sub

    Public Sub New()
        Me.Path = HttpContext.Current.Server.MapPath("~")
        FileInfos = New DirectoryInfo(Me.Path & "\_Blocks").GetFiles
    End Sub

    Public Function ToXMLDocument() As XmlDocument

        Dim m_XMLDocument As New XmlDocument
        Dim m_XMLDocumentToReturn As New XmlDocument
        Dim m_HTMLStringToReturn As String = ""
        Dim m_XMLNode As XmlNode = Nothing
        Dim m_XmlDeclaration As XmlDeclaration = m_XMLDocumentToReturn.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XMLDocumentToReturn.AppendChild(m_XmlDeclaration)

        Dim m_XMLRootNode As XmlNode = m_XMLDocumentToReturn.CreateElement("Object")
        m_XMLDocumentToReturn.AppendChild(m_XMLRootNode)

        Dim m_Protocol As New Protocol(Me.ProjectID)

        Dim strNorth As String = ""
        Dim strEast As String = ""
        Dim strSouth As String = ""
        Dim strWest As String = ""
        For Each m_FileInfo As FileInfo In Me.FileInfos

            m_XMLDocument.Load(m_FileInfo.FullName)
            m_XMLDocument = m_Protocol.Analyze(m_XMLDocument)

            Dim m_XMLNodeRepresentation As XmlNode = m_XMLDocument.SelectSingleNode("/Object/Representation")
            If Not m_XMLNodeRepresentation Is Nothing Then
                m_XMLNode = m_XMLDocumentToReturn.CreateElement("Representation")
                m_XMLNode.InnerXml = m_XMLNodeRepresentation.InnerXml

                strNorth = getValueFromAttribute(m_XMLDocument.SelectSingleNode("/Object").Attributes("north"))
                strEast = getValueFromAttribute(m_XMLDocument.SelectSingleNode("/Object").Attributes("east"))
                strSouth = getValueFromAttribute(m_XMLDocument.SelectSingleNode("/Object").Attributes("south"))
                strWest = getValueFromAttribute(m_XMLDocument.SelectSingleNode("/Object").Attributes("west"))

                'Get the values for north, east, south, west and put them on the representation object top node
                setValueForAttribute(m_XMLDocumentToReturn, m_XMLNode.ChildNodes(0).Attributes, "north", strNorth)
                setValueForAttribute(m_XMLDocumentToReturn, m_XMLNode.ChildNodes(0).Attributes, "east", strEast)
                setValueForAttribute(m_XMLDocumentToReturn, m_XMLNode.ChildNodes(0).Attributes, "south", strSouth)
                setValueForAttribute(m_XMLDocumentToReturn, m_XMLNode.ChildNodes(0).Attributes, "west", strWest)
                m_XMLNode.InnerXml = "<![CDATA[" & m_XMLNode.InnerXml & "]]>"

                'Else
                '    m_XMLNode.InnerXml = "<![CDATA[" & "" & "]]>"
                m_XMLRootNode.AppendChild(m_XMLNode)
            End If
        Next

        Return m_XMLDocumentToReturn

    End Function

End Class
