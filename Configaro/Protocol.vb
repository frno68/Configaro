Imports System.Xml
Imports System.IO
Imports System.Configuration.ConfigurationManager
Public Class Protocol

    Public Property Path As String = ""
    Private Property ProjectID As String = "-1"
    Private Property Edit As Boolean = False
    Public Property XmlDocument As New XmlDocument
    Public Property XmlNode As XmlNode

    Public Sub New()

        Me.Edit = True
        Me.Path = HttpContext.Current.Server.MapPath(AppSettings("TemplatePath"))
        Me.XmlDocument.Load(Me.Path)
        Me.XmlNode = Me.XmlDocument.SelectSingleNode("/Protocol")

    End Sub

    Public Sub New(p_ProjectID As String)

        Me.Edit = False
        Me.ProjectID = p_ProjectID
        Me.Path = New ProjectSettings(Me.ProjectID).DirectoryInfo.FullName & "\project.xml"
        Me.XmlDocument.Load(Me.Path)

        Me.XmlDocument = New Parser(Me.XmlDocument).XmlDocumentProtocol
        Me.CommitChanges()

        Me.XmlNode = Me.XmlDocument.SelectSingleNode("/Protocol")

    End Sub

    Public Sub SaveAnswer(p_QuestionID As Integer, p_Value As String)
        Dim m_XmlNodeQuestion As XmlNode = Me.XmlNode.SelectSingleNode("Question[@ID=" & p_QuestionID & "]")

        Select Case getValueFromAttribute(m_XmlNodeQuestion.Attributes("DisplayType"))
            Case "Check" 'This one allows multiple selection
                For Each m_XmlNodeAnswer In m_XmlNodeQuestion.ChildNodes
                    If getValueFromAttribute(m_XmlNodeAnswer.Attributes("ID")) = p_Value Then
                        If getValueFromAttribute(m_XmlNodeAnswer.Attributes("Selected")) = "True" Then
                            setValueForAttribute(Me.XmlDocument, m_XmlNodeAnswer.Attributes, "Selected", "")
                        Else
                            setValueForAttribute(Me.XmlDocument, m_XmlNodeAnswer.Attributes, "Selected", "True")
                        End If
                    Else
                        setValueForAttribute(Me.XmlDocument, m_XmlNodeAnswer.Attributes, "Selected", getValueFromAttribute(m_XmlNodeAnswer.Attributes("Selected")))
                    End If
                Next
            Case Else
                For Each m_XmlNodeAnswer In m_XmlNodeQuestion.ChildNodes
                    If getValueFromAttribute(m_XmlNodeAnswer.Attributes("ID")) = p_Value Then
                        setValueForAttribute(Me.XmlDocument, m_XmlNodeAnswer.Attributes, "Selected", "True")
                    Else
                        setValueForAttribute(Me.XmlDocument, m_XmlNodeAnswer.Attributes, "Selected", "")
                    End If
                Next
        End Select

        Me.CommitChanges()

    End Sub

    Public Sub ExecuteCommand(p_QuestionID As String, p_AnswerID As String, p_Command As String)
        Dim m_Question As New Question(Me, p_QuestionID, False)
        Dim m_Answer As New Answer(m_Question, p_AnswerID, False)
        Select Case p_Command.ToLower
            Case "add"
                If p_QuestionID = "-1" And p_AnswerID = "-1" Then
                    Dim m_QuestionToBeAdded As New Question(Me, "-1", False)
                    m_QuestionToBeAdded.SortOrder = "-1"
                    Me.XmlNode.AppendChild(m_QuestionToBeAdded.XmlNode)
                    m_QuestionToBeAdded.CommitChanges()
                ElseIf p_QuestionID <> "-1" And p_AnswerID = "-1" Then
                    Dim m_AnswerToBeAdded As New Answer(m_Question, "-1", False)
                    m_Question.XmlNode.AppendChild(m_AnswerToBeAdded.XmlNode)
                    m_AnswerToBeAdded.CommitChanges()
                End If
            Case "addbefore"
                If p_AnswerID <> "-1" Then
                    Dim m_AnswerToBeAdded As New Answer(m_Question, "-1", False)
                    m_Question.XmlNode.InsertBefore(m_AnswerToBeAdded.XmlNode, m_Answer.XmlNode)
                    m_AnswerToBeAdded.CommitChanges()
                ElseIf p_QuestionID <> "-1" Then
                    Dim m_QuestionToBeAdded As New Question(Me, "-1", False)
                    m_QuestionToBeAdded.SortOrder = m_Question.SortOrder - 5
                    Me.XmlNode.InsertBefore(m_QuestionToBeAdded.XmlNode, m_Question.XmlNode)
                    m_QuestionToBeAdded.CommitChanges()
                End If
            Case "addafter"
                If p_AnswerID <> "-1" Then
                    Dim m_AnswerToBeAdded As New Answer(m_Question, "-1", False)
                    m_Question.XmlNode.InsertAfter(m_AnswerToBeAdded.XmlNode, m_Answer.XmlNode)
                    m_AnswerToBeAdded.CommitChanges()
                ElseIf p_QuestionID <> "-1" Then
                    Dim m_QuestionToBeAdded As New Question(Me, "-1", False)
                    m_QuestionToBeAdded.SortOrder = m_Question.SortOrder + 5
                    Me.XmlNode.InsertAfter(m_QuestionToBeAdded.XmlNode, m_Question.XmlNode)
                    m_QuestionToBeAdded.CommitChanges()
                End If
            Case "movedown"
                If p_AnswerID <> "-1" Then
                    If Not m_Answer.XmlNode.NextSibling Is Nothing Then
                        m_Question.XmlNode.InsertAfter(m_Answer.XmlNode, m_Answer.XmlNode.NextSibling)
                    End If
                ElseIf p_QuestionID <> "-1" Then
                    If Not m_Question.XmlNode.NextSibling Is Nothing Then
                        Dim m_SortOrder As String = getValueFromAttribute(m_Question.XmlNode.Attributes("SortOrder"))
                        Dim m_SortOrderSibling As String = getValueFromAttribute(m_Question.XmlNode.NextSibling.Attributes("SortOrder"))

                        'Let the two questions swap sortorder
                        setValueForAttribute(Me.XmlDocument, m_Question.XmlNode.Attributes, "SortOrder", m_SortOrderSibling)
                        setValueForAttribute(Me.XmlDocument, m_Question.XmlNode.NextSibling.Attributes, "SortOrder", m_SortOrder)

                        'Make sure all references to each item also swaps Proceed_With
                        Dim m_XmlNodeList As XmlNodeList
                        m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question[@Proceed_With='" & m_SortOrderSibling & "']")
                        For Each m_XMLNode As XmlNode In m_XmlNodeList
                            setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", m_SortOrder)
                        Next
                        m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question/Answer[@Proceed_With='" & m_SortOrderSibling & "']")
                        For Each m_XMLNode As XmlNode In m_XmlNodeList
                            setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", m_SortOrder)
                        Next
                        m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question[@Proceed_With='" & m_SortOrder & "']")
                        For Each m_XMLNode As XmlNode In m_XmlNodeList
                            setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", m_SortOrderSibling)
                        Next
                        m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question/Answer[@Proceed_With='" & m_SortOrder & "']")
                        For Each m_XMLNode As XmlNode In m_XmlNodeList
                            setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", m_SortOrderSibling)
                        Next

                        Me.XmlNode.InsertAfter(m_Question.XmlNode, m_Question.XmlNode.NextSibling)
                    End If
                End If
            Case "moveup"
                If p_AnswerID <> "-1" Then
                    If Not m_Answer.XmlNode.PreviousSibling Is Nothing Then
                        m_Question.XmlNode.InsertBefore(m_Answer.XmlNode, m_Answer.XmlNode.PreviousSibling)
                    End If
                ElseIf p_QuestionID <> "-1" Then
                    If Not m_Question.XmlNode.PreviousSibling Is Nothing Then
                        Dim m_SortOrder As String = getValueFromAttribute(m_Question.XmlNode.Attributes("SortOrder"))
                        Dim m_SortOrderSibling As String = getValueFromAttribute(m_Question.XmlNode.PreviousSibling.Attributes("SortOrder"))

                        'Let the two questions swap sortorder
                        setValueForAttribute(Me.XmlDocument, m_Question.XmlNode.Attributes, "SortOrder", m_SortOrderSibling)
                        setValueForAttribute(Me.XmlDocument, m_Question.XmlNode.PreviousSibling.Attributes, "SortOrder", m_SortOrder)

                        'Make sure all references to each item also swaps Proceed_With
                        Dim m_XmlNodeList As XmlNodeList
                        m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question[@Proceed_With='" & m_SortOrderSibling & "']")
                        For Each m_XMLNode As XmlNode In m_XmlNodeList
                            setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", m_SortOrder)
                        Next
                        m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question/Answer[@Proceed_With='" & m_SortOrderSibling & "']")
                        For Each m_XMLNode As XmlNode In m_XmlNodeList
                            setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", m_SortOrder)
                        Next
                        m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question[@Proceed_With='" & m_SortOrder & "']")
                        For Each m_XMLNode As XmlNode In m_XmlNodeList
                            setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", m_SortOrderSibling)
                        Next
                        m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question/Answer[@Proceed_With='" & m_SortOrder & "']")
                        For Each m_XMLNode As XmlNode In m_XmlNodeList
                            setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", m_SortOrderSibling)
                        Next

                        Me.XmlNode.InsertBefore(m_Question.XmlNode, m_Question.XmlNode.PreviousSibling)
                    End If
                End If
            Case "delete"
                If p_AnswerID <> "-1" Then
                    If m_Question.XmlNode.ChildNodes.Count > 1 Then
                        m_Question.XmlNode.RemoveChild(m_Answer.XmlNode)
                    End If
                ElseIf p_QuestionID <> "-1" Then
                    Dim m_SortOrder As String = m_Question.SortOrder
                    Me.XmlNode.RemoveChild(m_Question.XmlNode)
                    'Clear all proceed from question to this question
                    Dim m_XmlNodeList As XmlNodeList
                    m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question[@Proceed_With='" & m_SortOrder & "']")
                    For Each m_XMLNode As XmlNode In m_XmlNodeList
                        setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", "")
                    Next
                    'Clear all proceed from answers to this question
                    m_XmlNodeList = Me.XmlDocument.SelectNodes("/Protocol/Question/Answer[@Proceed_With='" & m_SortOrder & "']")
                    For Each m_XMLNode As XmlNode In m_XmlNodeList
                        setValueForAttribute(Me.XmlDocument, m_XMLNode.Attributes, "Proceed_With", "")
                    Next

                End If

        End Select

        recalculateSortOrder()

        Me.CommitChanges()

    End Sub

    Public Sub CommitChanges()

        Me.XmlDocument.Save(Me.Path)

    End Sub

    Public Function Analyze(p_XMLDocument As XmlDocument) As XmlDocument
        Return New Parser(Me.XmlDocument).Analyze(p_XMLDocument)
    End Function

    Public Function ToHtml() As String

        Dim m_XMLNodeP As XmlNode = Me.XmlDocument.SelectSingleNode("//Protocol")
        Dim m_XmlNodeQs As XmlNodeList = m_XMLNodeP.ChildNodes
        Dim m_Question As Question = Nothing

        Dim m_StringWriter As New StringWriter
        Dim m_HtmlTextWriter As New HtmlTextWriter(m_StringWriter)

        For Each myXmlNodeQ As XmlNode In m_XmlNodeQs
            If getValueFromAttribute(myXmlNodeQ.Attributes("Display")) <> "False" Then
                m_Question = New Question(Me, myXmlNodeQ.Attributes("ID").Value, Me.Edit)
                m_HtmlTextWriter.Write(m_Question.ToHtml)
            End If
        Next
        Return m_StringWriter.ToString

    End Function

    Private Sub recalculateSortOrder()

        Dim m_XMLNodeP As XmlNode
        Dim m_XmlNodeListQ As XmlNodeList
        Dim m_XmlNodeListQToBeChanged As XmlNodeList
        Dim m_XmlNodeListAToBeChanged As XmlNodeList
        Dim m_XmlNodeQ As XmlNode
        Dim m_SortOrder As Integer = 0
        Dim m_SortOrderOld As Integer = 0
        m_XMLNodeP = Me.XmlDocument.SelectSingleNode("/Protocol")
        m_XmlNodeListQ = m_XMLNodeP.ChildNodes
        For i As Integer = m_XmlNodeListQ.Count - 1 To 0 Step -1

            m_SortOrder = 10 + i * 10 'This will end at 10
            m_XmlNodeQ = m_XmlNodeListQ(i)
            m_SortOrderOld = m_XmlNodeQ.Attributes("SortOrder").Value
            m_XmlNodeQ.Attributes("SortOrder").Value = m_SortOrder

            m_XmlNodeListAToBeChanged = Me.XmlDocument.SelectNodes("/Protocol/Question/Answer[@Proceed_With='" & m_SortOrderOld & "']")
            For Each m_XMLNodeAToBeChanged As XmlNode In m_XmlNodeListAToBeChanged
                setValueForAttribute(Me.XmlDocument, m_XMLNodeAToBeChanged.Attributes, "Proceed_With", m_SortOrder)
            Next

            m_XmlNodeListQToBeChanged = Me.XmlDocument.SelectNodes("/Protocol/Question[@Proceed_With='" & m_SortOrderOld & "']")
            For Each m_XMLNodeQToBeChanged As XmlNode In m_XmlNodeListQToBeChanged
                setValueForAttribute(Me.XmlDocument, m_XMLNodeQToBeChanged.Attributes, "Proceed_With", m_SortOrder)
            Next
        Next

    End Sub

End Class
