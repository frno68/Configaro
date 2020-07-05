Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports System.Xml

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class wsProjects
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetProjects() As XmlDocument

        Dim m_XmlDocument As New XmlDocument
        Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XmlRoot As XmlElement = m_XmlDocument.CreateElement("Root")
        m_XmlDocument.AppendChild(m_XmlRoot)

        Dim m_XmlElement As XmlElement = m_XmlDocument.CreateElement("Projects")
        m_XmlRoot.AppendChild(m_XmlElement)

        m_XmlElement.InnerText = "" & New Projects().ToHtml

        Return m_XmlDocument

    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetProject(p_ProjectID As String) As XmlDocument

        Dim m_Project As New Project(UriLocal.UnescapeDataString(p_ProjectID))
        Return m_Project.ToXmlDocument

    End Function

    <WebMethod()> _
    Public Sub SaveProject(p_ProjectId As String, _
                           p_ProjectIDNew As String)

        Dim m_Project As New Project(UriLocal.UnescapeDataString(p_ProjectId))
        m_Project.ProjectID = (UriLocal.UnescapeDataString(p_ProjectIDNew))
        m_Project.CommitChanges()


    End Sub

    <WebMethod()> _
    Public Sub ExecuteCommand(p_ProjectID As String, _
                              p_Command As String)

        Dim m_Project As New Project(UriLocal.UnescapeDataString(p_ProjectID))
        m_Project.ExecuteCommand(p_Command)

    End Sub


End Class