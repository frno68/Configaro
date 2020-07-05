Imports System.Xml
Imports System.IO

Public Class Answer

    Public Property Question As Question
    Public Property XmlNode As XmlNode
    Public Property Edit As Boolean = False
    Private _ID As String = "-1"
    Public ReadOnly Property ID As String
        Get
            Return _ID
        End Get
    End Property
    Private _IHaveInfoToBeShown As Boolean = False
    Public ReadOnly Property IHaveInfoToBeShown As Boolean
        Get
            Return _IHaveInfoToBeShown
        End Get
    End Property
    Public Property Proceed_With As String = ""
    Public Property Selected As String = "False"
    Public Property Description As String = ""

    Public Sub New(p_Question As Question, p_ID As String, p_Edit As Boolean)

        Me.Question = p_Question
        init(p_ID, p_Edit)

    End Sub

    Private Sub init(p_ID As String, p_Edit As Boolean)
        If p_ID = "-1" Then
            Me.Edit = p_Edit
            Me.XmlNode = Me.Question.Protocol.XmlDocument.CreateElement("Answer")
            Me._ID = p_ID
            Me.Selected = False
            Me.Description = "New Answer"
        Else
            Me.Edit = p_Edit
            Me.XmlNode = Me.Question.Protocol.XmlDocument.SelectSingleNode("/Protocol/Question/Answer[@ID=" & p_ID & "]")
            Me._ID = getValueFromAttribute(Me.XmlNode.Attributes("ID"))
            Me.Proceed_With = getValueFromAttribute(Me.XmlNode.Attributes("Proceed_With"))
            Me.Selected = getValueFromAttribute(Me.XmlNode.Attributes("Selected"))
            Me.Description = getValueFromAttribute(Me.XmlNode.Attributes("Description"))
        End If
    End Sub

    Public Sub CommitChanges()
        Dim m_XMLDocumentProtocol As XmlDocument = Me.Question.Protocol.XmlDocument
        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "ID", Me.ID)
        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "Proceed_With", Me.Proceed_With)
        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "Selected", Me.Selected)
        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "Description", Me.Description)

        If Me.ID = "-1" Then
            'Set the new ID by finding the highest value and add 1 to it
            Dim m_XmlNodes = m_XMLDocumentProtocol.SelectNodes("//Protocol/Question/Answer")
            Dim m_ID As Integer = -1
            For Each m_XmlNode In m_XmlNodes
                m_ID = IIf(CInt(getValueFromAttribute(m_XmlNode.Attributes("ID"))) > m_ID, CInt(getValueFromAttribute(m_XmlNode.Attributes("ID"))), m_ID)
            Next
            Me._ID = CStr(CInt(m_ID) + 1)
        End If
        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "ID", Me.ID)
        Me.Question.CommitChanges()

    End Sub

    Public Function ToXmlDocument() As XmlDocument
        Dim m_XMLDocument As New XmlDocument
        Dim m_XMLDocumentToReturn As New XmlDocument
        Dim m_HTMLStringToReturn As String = ""

        Dim m_XmlDeclaration As XmlDeclaration = m_XMLDocumentToReturn.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XMLDocumentToReturn.AppendChild(m_XmlDeclaration)

        Dim m_XMLRootNode As XmlNode = m_XMLDocumentToReturn.CreateElement("Object")
        m_XMLDocumentToReturn.AppendChild(m_XMLRootNode)

        m_XMLRootNode.InnerXml = Me.XmlNode.OuterXml

        Return m_XMLDocumentToReturn

    End Function

    Public Function ToHtml() As String

        Dim m_StringWriter As New StringWriter
        Dim m_HtmlTextWriter As New HtmlTextWriter(m_StringWriter)
        If Me.Edit Then
            m_HtmlTextWriter.AddAttribute("type", "checkbox")
        Else
            m_HtmlTextWriter.AddAttribute("type", "radio")
        End If
        m_HtmlTextWriter.AddAttribute("QuestionID", Me.Question.ID)
        m_HtmlTextWriter.AddAttribute("AnswerID", Me.ID)
        m_HtmlTextWriter.AddAttribute("name", "radio-choice-" & Me.Question.ID)
        m_HtmlTextWriter.AddAttribute("id", "Answer-" & Me.ID)
        m_HtmlTextWriter.AddAttribute("value", Me.ID)
        If Me.Selected <> "" Then
            If CBool(Me.Selected) Then m_HtmlTextWriter.AddAttribute("checked", "checked")
        End If
        If Me.Question.IHaveInfoToBeShown And Not Me.Question.Edit Then
            m_HtmlTextWriter.AddAttribute("style", "display: none")
        End If
        m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Input)
        m_HtmlTextWriter.RenderEndTag()

        m_HtmlTextWriter.AddAttribute("for", "Answer-" & Me.ID)
        m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Label)

        m_HtmlTextWriter.Write(Me.Description)

        If Me.Edit Then
            m_HtmlTextWriter.Write("&nbsp;" & "{" & "&nbsp;" & "Id=" & Me.ID & "&nbsp;" & "}")
        End If

        m_HtmlTextWriter.RenderEndTag()

        Return m_StringWriter.ToString

    End Function

End Class
