Imports System.Xml
Imports System.IO

Public Class Answers

    Public Property Question As Question
    Private _IHaveInfoToBeShown As Boolean = False

    Public ReadOnly Property IHaveInfoToBeShown As Boolean
        Get
            Return _IHaveInfoToBeShown
        End Get
    End Property

    Public Property Edit As Boolean = False

    Public Sub New(p_Question As Question, p_Edit As Boolean)
        Me.Question = p_Question
        Me.Edit = p_Edit
    End Sub

    Public Function ToHtml() As String
        Dim m_StringWriter As New StringWriter
        Dim m_HtmlTextWriter As New HtmlTextWriter(m_StringWriter)

        Dim m_Answer As Answer = Nothing
        For Each m_XmlNode As XmlNode In Me.Question.XmlNode.ChildNodes

            m_Answer = New Answer(Me.Question, getValueFromAttribute(m_XmlNode.Attributes("ID")), Me.Edit)
            m_HtmlTextWriter.Write(m_Answer.ToHtml)

        Next
        Return m_StringWriter.ToString
    End Function



End Class
