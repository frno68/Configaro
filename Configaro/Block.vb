Imports System.Xml
Imports System.IO

Public Class Block
    Public Property FileInfos As FileInfo() = New DirectoryInfo(HttpContext.Current.Server.MapPath("_Blocks")).GetFiles
    Public Property XmlDocument As New XmlDocument
    Public Property BlockID As String = ""
    Public Property Expression As String = ""
    Public Property North As String = ""
    Public Property East As String = ""
    Public Property South As String = ""
    Public Property West As String = ""

    Public Property Representation As String = ""
    Public Property Content As String = ""
    Public Property Summary As String = ""

    Public Sub New(p_BlockID As String)

        Me.BlockID = p_BlockID

        For Each m_FileInfo As FileInfo In Me.FileInfos
            If m_FileInfo.Name = Me.BlockID Then
                Me.XmlDocument.Load(m_FileInfo.FullName)
                Me.Expression = getValueFromAttribute(Me.XmlDocument.SelectSingleNode("/Object").Attributes("expression"))
                Me.North = getValueFromAttribute(Me.XmlDocument.SelectSingleNode("/Object").Attributes("north"))
                Me.East = getValueFromAttribute(Me.XmlDocument.SelectSingleNode("/Object").Attributes("east"))
                Me.South = getValueFromAttribute(Me.XmlDocument.SelectSingleNode("/Object").Attributes("south"))
                Me.West = getValueFromAttribute(Me.XmlDocument.SelectSingleNode("/Object").Attributes("west"))
                Me.Representation = Me.XmlDocument.SelectSingleNode("/Object/Representation").InnerXml
                Me.Content = Me.XmlDocument.SelectSingleNode("/Object/Content").InnerXml
                Me.Summary = Me.XmlDocument.SelectSingleNode("/Object/Summary").InnerXml
                Exit For
            End If
        Next
    End Sub

    Public Function ToXmlDocument() As XmlDocument
        Return Me.XmlDocument
    End Function

    Public Sub CommitChanges()

        Me.XmlDocument = New XmlDocument

        Dim m_XmlDeclaration As XmlDeclaration = Me.XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        Me.XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XMLNodeRoot As XmlNode = Me.XmlDocument.CreateElement("Object")
        Me.XmlDocument.AppendChild(m_XMLNodeRoot)
        setValueForAttribute(Me.XmlDocument, m_XMLNodeRoot.Attributes, "id", Me.BlockID)
        setValueForAttribute(Me.XmlDocument, m_XMLNodeRoot.Attributes, "expression", Me.Expression)
        setValueForAttribute(Me.XmlDocument, m_XMLNodeRoot.Attributes, "north", Me.North)
        setValueForAttribute(Me.XmlDocument, m_XMLNodeRoot.Attributes, "east", Me.East)
        setValueForAttribute(Me.XmlDocument, m_XMLNodeRoot.Attributes, "south", Me.South)
        setValueForAttribute(Me.XmlDocument, m_XMLNodeRoot.Attributes, "west", Me.West)

        Dim m_XmlElement As XmlElement

        m_XmlElement = Me.XmlDocument.CreateElement("Representation")
        m_XMLNodeRoot.AppendChild(m_XmlElement)
        If Me.Representation = "" Then Me.Representation = "<div></div>"
        m_XmlElement.InnerXml = Me.Representation

        For Each m_XmlNode In m_XmlElement.SelectNodes("*")
            setValueForAttribute(Me.XmlDocument, m_XmlNode.Attributes, "id", Me.BlockID)
            setValueForAttribute(Me.XmlDocument, m_XmlNode.Attributes, "class", "item") 'Used for marking item as building block
        Next

        m_XmlElement = Me.XmlDocument.CreateElement("Content")
        m_XMLNodeRoot.AppendChild(m_XmlElement)
        If Me.Content = "" Then Me.Content = "<div></div>"

        m_XmlElement.InnerXml = Me.Content
        setValueForAttribute(Me.XmlDocument, m_XmlElement.Attributes, "id", Me.BlockID) 'Has to be there for outputgeneration

        m_XmlElement = Me.XmlDocument.CreateElement("Summary")
        m_XMLNodeRoot.AppendChild(m_XmlElement)
        If Me.Summary = "" Then Me.Summary = "<div></div>"

        m_XmlElement.InnerXml = Me.Summary
        setValueForAttribute(Me.XmlDocument, m_XmlElement.Attributes, "id", Me.BlockID) 'Has to be there for outputgeneration

        Me.XmlDocument.Save(HttpContext.Current.Server.MapPath("_Blocks") & "/" & Me.BlockID & ".xml")

    End Sub

    Public Sub ExecuteCommand(p_Command As String)

        Select Case p_Command.ToLower
            Case "delete"
                For Each m_FileInfo As FileInfo In Me.FileInfos
                    If m_FileInfo.Name = Me.BlockID Then
                        m_FileInfo.Delete()
                        Exit For
                    End If
                Next
            Case "add"
                Me.BlockID = "NewBlock"
                Me.Representation = "<div>New Representation</div>"
                Me.Content = "<div>New Content</div>"
                Me.Summary = "<div>New Summary</div>"
                Me.CommitChanges()
        End Select

    End Sub
End Class
