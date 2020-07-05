Imports System.Xml
Imports System.IO
Public Class UserAuthenticator

    Public Property DirectoryInfoUser As DirectoryInfo = Nothing
    Public Property DirectoryInfoProjects As DirectoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath("_Projects"))

    Private Property XmlDocument As New XmlDocument
    Private Property UserName As String = ""
    Private Property PassWord As String = ""

    Public ReadOnly Property Authenticated As Boolean
        Get
            Dim m_XmlNode As XmlNode = Me.XmlDocument.SelectSingleNode("/User")
            If getValueFromAttribute(m_XmlNode.Attributes("password")) = Me.PassWord Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public Sub New(p_UserName As String, p_PassWord As String)

        Me.UserName = p_UserName
        Me.PassWord = p_PassWord

        If Me.DirectoryInfoProjects.GetDirectories(Me.UserName).Length = 0 Then
            'New user. Lets create the projects folder and the user settings file
            CreateNewUser()
        End If

        Me.DirectoryInfoUser = Me.DirectoryInfoProjects.GetDirectories(Me.UserName)(0)
        Me.XmlDocument.Load(Me.DirectoryInfoUser.FullName & "/User.xml")

    End Sub

    Private Sub CreateNewUser()

        Me.XmlDocument = New XmlDocument

        Dim m_XmlDeclaration As XmlDeclaration = Me.XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        Me.XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XMLNodeRoot As XmlNode = Me.XmlDocument.CreateElement("User")
        Me.XmlDocument.AppendChild(m_XMLNodeRoot)
        setValueForAttribute(Me.XmlDocument, m_XMLNodeRoot.Attributes, "username", Me.UserName)
        setValueForAttribute(Me.XmlDocument, m_XMLNodeRoot.Attributes, "password", Me.PassWord)

        'create a folder with the users name
        Dim m_DirectoryInfo As DirectoryInfo = Me.DirectoryInfoProjects.CreateSubdirectory(Me.UserName)
        Me.XmlDocument.Save(m_DirectoryInfo.FullName & "/User.xml")

    End Sub

End Class
