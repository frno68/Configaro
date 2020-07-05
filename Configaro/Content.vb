Imports System.Xml
Imports System.IO
Public Class Content

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
        Dim m_XMLNodeContent As XmlNode
        Dim m_XMLAttribute As XmlAttribute
        Dim m_XmlDeclaration As XmlDeclaration = m_XMLDocumentToReturn.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XMLDocumentToReturn.AppendChild(m_XmlDeclaration)

        Dim m_XMLRootNode As XmlNode = m_XMLDocumentToReturn.CreateElement("Object")
        m_XMLDocumentToReturn.AppendChild(m_XMLRootNode)

        Dim m_Protocol As New Protocol(Me.ProjectID)

        For Each m_FileInfo As FileInfo In Me.FileInfos

            m_XMLDocument.Load(m_FileInfo.FullName)
            m_XMLDocument = m_Protocol.Analyze(m_XMLDocument)

            m_XMLNode = m_XMLDocumentToReturn.CreateElement("SEK")
            m_XMLNodeContent = m_XMLDocument.SelectSingleNode("/Object/Content")
            If Not m_XMLNodeContent Is Nothing Then
                m_XMLAttribute = m_XMLDocumentToReturn.CreateAttribute("Id")
                m_XMLAttribute.Value = m_XMLNodeContent.Attributes("id").Value
                m_XMLNode.Attributes.Append(m_XMLAttribute)
                m_XMLNode.InnerText = getAmount(m_XMLNodeContent, "SEK")
            End If
            m_XMLRootNode.AppendChild(m_XMLNode)

            m_XMLNode = m_XMLDocumentToReturn.CreateElement("USD")
            If Not m_XMLNodeContent Is Nothing Then
                m_XMLAttribute = m_XMLDocumentToReturn.CreateAttribute("Id")
                m_XMLAttribute.Value = m_XMLNodeContent.Attributes("id").Value
                m_XMLNode.Attributes.Append(m_XMLAttribute)
                m_XMLNode.InnerText = getAmount(m_XMLNodeContent, "USD")
            End If
            m_XMLRootNode.AppendChild(m_XMLNode)

            m_XMLNode = m_XMLDocumentToReturn.CreateElement("EUR")
            If Not m_XMLNodeContent Is Nothing Then
                m_XMLAttribute = m_XMLDocumentToReturn.CreateAttribute("Id")
                m_XMLAttribute.Value = m_XMLNodeContent.Attributes("id").Value
                m_XMLNode.Attributes.Append(m_XMLAttribute)
                m_XMLNode.InnerText = getAmount(m_XMLNodeContent, "EUR")
            End If
            m_XMLRootNode.AppendChild(m_XMLNode)

            m_XMLNode = m_XMLDocumentToReturn.CreateElement("Content")
            If Not m_XMLNodeContent Is Nothing Then
                m_XMLAttribute = m_XMLDocumentToReturn.CreateAttribute("Id")
                m_XMLAttribute.Value = m_XMLNodeContent.Attributes("id").Value
                m_XMLNode.Attributes.Append(m_XMLAttribute)
                m_XMLNode.InnerXml = "<![CDATA[" & m_XMLNodeContent.InnerXml & "]]>"
            End If
            m_XMLRootNode.AppendChild(m_XMLNode)

        Next

        Return m_XMLDocumentToReturn

    End Function

    Private Function getAmount(ByRef p_XMLNode As XmlNode, p_Currency As String) As Long

        Dim m_XMLAttribute As XmlAttribute = Nothing
        Dim m_Value As Long = 0

        If p_XMLNode.NodeType <> XmlNodeType.Element Then
            Return 0
        End If

        If p_XMLNode.HasChildNodes = False Then
            If (p_XMLNode.Attributes.Count = 0) Then
                Return 0
            Else
                If p_XMLNode.Attributes("currency") Is Nothing Then
                    If p_XMLNode.Attributes("amount") Is Nothing Then
                        m_Value = 0
                        Return m_Value
                    Else
                        m_Value = 0
                        p_XMLNode.Attributes.RemoveNamedItem("amount")
                        Return m_Value
                    End If
                Else
                    If (p_XMLNode.Attributes("currency").Value = p_Currency) Then
                        If p_XMLNode.Attributes("amount") Is Nothing Then
                            m_Value = 0
                            p_XMLNode.Attributes.RemoveNamedItem("currency")
                            Return m_Value
                        Else
                            m_Value = CLng(p_XMLNode.Attributes("amount").Value)
                            p_XMLNode.Attributes.RemoveNamedItem("amount")
                            p_XMLNode.Attributes.RemoveNamedItem("currency")
                            Return m_Value
                        End If
                    Else
                        m_Value = 0
                        Return m_Value
                    End If
                End If
            End If
        Else
            If (p_XMLNode.Attributes.Count = 0) Then
                Dim m_Price As Long = 0
                For Each m_XmlNode In p_XMLNode.ChildNodes
                    m_Price = m_Price + getAmount(m_XmlNode, p_Currency)
                Next
                Return m_Price
            Else
                If (p_XMLNode.Attributes("currency") Is Nothing) Then
                    Dim m_Price As Long = 0
                    For Each m_XmlNode In p_XMLNode.ChildNodes
                        m_Price = m_Price + getAmount(m_XmlNode, p_Currency)
                    Next
                    Return m_Price
                Else
                    If (p_XMLNode.Attributes("currency").Value <> p_Currency) Then
                        m_Value = 0
                        Return m_Value
                    Else
                        If p_XMLNode.Attributes("amount") Is Nothing Then
                            m_Value = 0
                            p_XMLNode.Attributes.RemoveNamedItem("currency")
                            Return m_Value
                        Else
                            m_Value = CLng(p_XMLNode.Attributes("amount").Value)
                            p_XMLNode.Attributes.RemoveNamedItem("amount")
                            p_XMLNode.Attributes.RemoveNamedItem("currency")
                            Return m_Value
                        End If
                    End If
                End If
            End If
        End If



    End Function

End Class
