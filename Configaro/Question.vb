Imports System.Xml
Imports System.IO
Public Class Question

    Public Property Protocol As Protocol = Nothing
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
    Public Property SortOrder As String = ""
    Public Property QuestionType As String = ""
    Public Property DisplayType As String = ""
    Public Property LinkedQuestion As String = ""
    Public Property Proceed_With As String = ""
    Public Property Description As String = ""

    Public Sub New(p_Protocol As Protocol, p_ID As String, p_Edit As Boolean)

        Me.Protocol = p_Protocol
        init(p_ID, p_Edit)

    End Sub

    Public Sub New(p_Path As String, p_ID As String, p_Edit As Boolean)

        Me.Protocol = New Protocol()
        init(p_ID, p_Edit)

    End Sub

    Private Sub init(p_ID As String, p_Edit As Boolean)
        If p_ID = "-1" Then
            Me.Edit = p_Edit
            Me.XmlNode = Me.Protocol.XmlDocument.CreateElement("Question")
            Me._ID = p_ID
            Me.Description = "New Question"
        Else
            Me.Edit = p_Edit
            Me.XmlNode = Me.Protocol.XmlDocument.SelectSingleNode("/Protocol/Question[@ID=" & p_ID & "]")
            Me._ID = getValueFromAttribute(Me.XmlNode.Attributes("ID"))
            Me.SortOrder = getValueFromAttribute(Me.XmlNode.Attributes("SortOrder"))
            Me.QuestionType = getValueFromAttribute(Me.XmlNode.Attributes("QuestionType"))
            Me.DisplayType = getValueFromAttribute(Me.XmlNode.Attributes("DisplayType"))
            Me.LinkedQuestion = getValueFromAttribute(Me.XmlNode.Attributes("LinkedQuestion"))
            Me.Proceed_With = getValueFromAttribute(Me.XmlNode.Attributes("Proceed_With"))
            Me.Description = getValueFromAttribute(Me.XmlNode.Attributes("Description"))
        End If
    End Sub

    Public Sub CommitChanges()
        Dim m_XMLDocumentProtocol As XmlDocument = Me.Protocol.XmlDocument
        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "ID", Me.ID)
        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "SortOrder", Me.SortOrder)
        If Not (Me.QuestionType <> "" And Me.LinkedQuestion <> "") Then
            setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "QuestionType", Me.QuestionType)
            setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "LinkedQuestion", Me.LinkedQuestion)
        Else
            Throw New DataException("QuestionType and LinkedQuestion has to be set at the same time")
        End If
        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "Proceed_With", Me.Proceed_With)
        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "Description", Me.Description)

        If Me.ID = "-1" Then
            'Set the new ID by finding the highest value and add 1 to it
            Dim m_XmlNodes = m_XMLDocumentProtocol.SelectNodes("//Protocol/Question")
            Dim m_ID As Integer = -1
            For Each m_XmlNode In m_XmlNodes
                m_ID = IIf(CInt(getValueFromAttribute(m_XmlNode.Attributes("ID"))) > m_ID, CInt(getValueFromAttribute(m_XmlNode.Attributes("ID"))), m_ID)
            Next
            Me._ID = CStr(CInt(m_ID) + 1)
        End If

        If Me.SortOrder = "" Then
            'Set the new SortOrder by finding the highest value and add 10 to it
            Dim m_XmlNodes = m_XMLDocumentProtocol.SelectNodes("//Protocol/Question")
            Dim m_SortOrder As Integer = -1
            Dim m_SortOrderString As String = ""
            For Each m_XmlNode In m_XmlNodes
                m_SortOrderString = getValueFromAttribute(m_XmlNode.Attributes("SortOrder"))
                m_SortOrder = IIf(CInt(m_SortOrderString) > m_SortOrder, CInt(m_SortOrderString), m_SortOrder)
            Next
            Me.SortOrder = CStr(CInt(m_SortOrder) + 10)
        End If

        setValueForAttribute(m_XMLDocumentProtocol, Me.XmlNode.Attributes, "ID", Me.ID)
        Me.Protocol.CommitChanges()
    End Sub

    Public Function ToHtml() As String

        Dim m_StringWriter As New StringWriter
        Dim m_HtmlTextWriter As New HtmlTextWriter(m_StringWriter)
        Dim m_Answer As Answer = Nothing

        m_HtmlTextWriter.AddAttribute("QuestionID", Me.ID)
        m_HtmlTextWriter.AddAttribute("data-role", "controlgroup")
        m_HtmlTextWriter.AddAttribute("data-type", "horizontal")
        m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Fieldset)
        m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Legend)

        If Me.Edit Then
            m_HtmlTextWriter.AddAttribute("type", "checkbox")
            m_HtmlTextWriter.AddAttribute("QuestionID", Me.ID)
            m_HtmlTextWriter.AddAttribute("AnswerID", "-1")
            m_HtmlTextWriter.AddAttribute("id", "Question-" & Me.ID)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Input)
            m_HtmlTextWriter.RenderEndTag()

            m_HtmlTextWriter.AddAttribute("for", "Question-" & Me.ID)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Label)
        End If

        m_HtmlTextWriter.Write(Me.Description)
        If Me.Edit Then
            m_HtmlTextWriter.Write("&nbsp;" & "{" & "&nbsp;" & "Id=" & Me.ID & "&nbsp;" & "Sortorder=" & Me.SortOrder & "&nbsp;" & "}")
            m_HtmlTextWriter.RenderEndTag()
        End If

        m_HtmlTextWriter.RenderEndTag()

        Select Case Me.QuestionType
            Case "Free_Text"
                m_HtmlTextWriter.AddAttribute("type", "text")
                m_HtmlTextWriter.AddAttribute("QuestionID", Me.ID)
                m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Input)
                m_HtmlTextWriter.RenderEndTag()
            Case Else
                Dim m_Answers As New Answers(Me, Me.Edit)
                m_HtmlTextWriter.Write(m_Answers.ToHtml)
                _IHaveInfoToBeShown = m_Answers.IHaveInfoToBeShown
        End Select

        m_HtmlTextWriter.RenderEndTag() 'Div

        Return m_StringWriter.ToString

    End Function

    Public Function ToXmlDocument() As XmlDocument

        Dim m_XMLDocumentToReturn As New XmlDocument
        Dim m_HTMLStringToReturn As String = ""

        Dim m_XmlDeclaration As XmlDeclaration = m_XMLDocumentToReturn.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XMLDocumentToReturn.AppendChild(m_XmlDeclaration)

        Dim m_XMLRootNode As XmlNode = m_XMLDocumentToReturn.CreateElement("Object")
        m_XMLDocumentToReturn.AppendChild(m_XMLRootNode)

        m_XMLRootNode.InnerXml = Me.XmlNode.OuterXml

        Return m_XMLDocumentToReturn

    End Function

End Class
