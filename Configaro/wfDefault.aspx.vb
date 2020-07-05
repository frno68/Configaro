Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Xml
Imports System.Configuration.ConfigurationManager
Public Class wfDefault
    Inherits PageClass

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            hidPath.Value = AppSettings("TemplatePath")
        End If
    End Sub

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Shared Function Login(p_UserName As String, p_Password As String) As XmlDocument

        Dim m_XmlDocument As New XmlDocument
        If p_UserName.Trim.Length > 0 Then
            If New UserAuthenticator(p_UserName, p_Password).Authenticated Then
                FormsAuthentication.SetAuthCookie(p_UserName, False)

                Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
                m_XmlDocument.AppendChild(m_XmlDeclaration)
                Dim m_XMLNodeRoot As XmlNode = m_XmlDocument.CreateElement("IsAuthenticated")
                m_XMLNodeRoot.InnerXml = "true"
                m_XmlDocument.AppendChild(m_XMLNodeRoot)
            End If
        End If

        Return m_XmlDocument

    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Shared Function Logout() As XmlDocument

        FormsAuthentication.SignOut()
        Dim m_XmlDocument As New XmlDocument
        Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XmlDocument.AppendChild(m_XmlDeclaration)
        Dim m_XMLNodeRoot As XmlNode = m_XmlDocument.CreateElement("IsAuthenticated")
        m_XMLNodeRoot.InnerXml = "false"
        m_XmlDocument.AppendChild(m_XMLNodeRoot)

        Return m_XmlDocument

    End Function

    Private Sub Upload_Click(sender As Object, e As System.EventArgs) Handles Upload.Click
        FileUpload.SaveAs(HttpContext.Current.Server.MapPath("_Resources") & "/" & FileUpload.FileName)
    End Sub
End Class