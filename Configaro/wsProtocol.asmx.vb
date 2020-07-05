Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml
Imports System.Web.Script.Services

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class wsProtocol
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetProtocol() As XmlDocument

        Dim m_XmlDocument As New XmlDocument
        Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XmlRoot As XmlElement = m_XmlDocument.CreateElement("Root")
        m_XmlDocument.AppendChild(m_XmlRoot)

        Dim m_XmlElementProtocol As XmlElement = m_XmlDocument.CreateElement("Protocol")
        m_XmlRoot.AppendChild(m_XmlElementProtocol)

        Dim m_XmlElementPath As XmlElement = m_XmlDocument.CreateElement("Path")
        m_XmlRoot.AppendChild(m_XmlElementPath)

        Dim m_Protocol As New Protocol()

        m_XmlElementProtocol.InnerText = "" & m_Protocol.ToHtml
        m_XmlElementPath.InnerText = "" & m_Protocol.Path

        Return m_XmlDocument

    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetQuestion(p_Path As String, _
                                p_QuestionID As Integer) As XmlDocument

        Dim m_Question As New Question(UriLocal.UnescapeDataString(p_Path), p_QuestionID, True)
        Return m_Question.ToXmlDocument

    End Function

    <WebMethod()> _
    Public Sub SaveQuestion(p_Path As String, _
                            p_QuestionID As Integer, _
                            p_QuestionType As String, _
                            p_LinkedQuestion As String, _
                            p_Proceed_With As String, _
                            p_Description As String)

        Dim m_Question As New Question(UriLocal.UnescapeDataString(p_Path), p_QuestionID, True)
        m_Question.QuestionType = p_QuestionType
        m_Question.LinkedQuestion = p_LinkedQuestion
        m_Question.Proceed_With = p_Proceed_With
        m_Question.Description = UriLocal.UnescapeDataString(p_Description)
        m_Question.CommitChanges()

    End Sub

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetAnswer(p_Path As String, _
                              p_QuestionID As Integer, _
                              p_AnswerID As Integer) As XmlDocument

        Dim m_Question As New Question(UriLocal.UnescapeDataString(p_Path), p_QuestionID, True)
        Dim m_Answer As New Answer(m_Question, p_AnswerID, True)

        Return m_Answer.ToXmlDocument

    End Function

    <WebMethod()> _
    Public Sub SaveAnswer(p_Path As String, _
                            p_QuestionID As Integer, _
                            p_AnswerID As Integer, _
                            p_Proceed_With As String, _
                            p_Selected As Boolean, _
                            p_Description As String)

        Dim m_Question As New Question(UriLocal.UnescapeDataString(p_Path), p_QuestionID, True)
        Dim m_Answer As New Answer(m_Question, p_AnswerID, True)

        m_Answer.Proceed_With = p_Proceed_With
        m_Answer.Description = UriLocal.UnescapeDataString(p_Description)
        m_Answer.Selected = p_Selected.ToString
        m_Answer.CommitChanges()

    End Sub

    <WebMethod()> _
    Public Sub ExecuteCommand(p_Path As String, _
                              p_QuestionID As Integer, _
                              p_AnswerID As Integer, _
                              p_Command As String)

        Dim m_Protocol As New Protocol()
        m_Protocol.ExecuteCommand(p_QuestionID, p_AnswerID, p_Command)

    End Sub

End Class