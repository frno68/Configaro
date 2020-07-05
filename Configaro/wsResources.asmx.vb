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
Public Class wsResources
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetResources() As XmlDocument

        Dim m_XmlDocument As New XmlDocument
        Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XmlRoot As XmlElement = m_XmlDocument.CreateElement("Root")
        m_XmlDocument.AppendChild(m_XmlRoot)

        Dim m_XmlElement As XmlElement = m_XmlDocument.CreateElement("Resources")
        m_XmlRoot.AppendChild(m_XmlElement)

        m_XmlElement.InnerText = "" & New Resources().ToHtmlList

        Return m_XmlDocument


    End Function

    <WebMethod()> _
    Public Sub ExecuteCommand(p_ResourceID As String, _
                              p_Command As String)

        Dim m_Resource As New Resource(UriLocal.UnescapeDataString(p_ResourceID))
        m_Resource.ExecuteCommand(p_Command)

    End Sub

End Class