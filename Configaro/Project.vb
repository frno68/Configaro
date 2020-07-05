Imports System.Xml
Imports System.IO
Imports System.Web.HttpContext
Imports System.Configuration.ConfigurationManager
Public Class Project

    Private Property TemplatesFolder As String = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("TemplatesFolder"))
    Private Property TemplatePath As String = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("TemplatePath"))
    Private Property XmlDocument As New XmlDocument

    Dim _ProjectSettings As ProjectSettings = Nothing
    Private ReadOnly Property ProjectSettings As ProjectSettings
        Get
            If _ProjectSettings Is Nothing Then
                _ProjectSettings = New ProjectSettings(Me.ProjectID)
            End If
            Return _ProjectSettings
        End Get
    End Property

    Public Property ProjectID As String = ""
    Private Property OldProjectID As String = ""

    Public Sub New(p_ProjectID As String)

        Dim m_DirecoryInfo As DirectoryInfo = Nothing

        If p_ProjectID <> "-1" Then
            Me.ProjectID = p_ProjectID
        Else
            Dim m_Now As Date = Now
            Me.ProjectID = "New project" & "_" & m_Now.ToShortDateString.Replace("-", "_") & "_" & m_Now.ToLongTimeString.Replace(":", "_") & "_" & m_Now.Millisecond

            ProjectSettings.DirectoryInfo.Create()

            m_DirecoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath("_Blocks"))
            ProjectSettings.DirectoryInfo.CreateSubdirectory(m_DirecoryInfo.Name)
            For Each m_Fileinfo As FileInfo In m_DirecoryInfo.GetFiles
                m_Fileinfo.CopyTo(ProjectSettings.DirectoryInfo.GetDirectories(m_DirecoryInfo.Name)(0).FullName & "\" & m_Fileinfo.Name)
            Next

            m_DirecoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath("_Resources"))
            ProjectSettings.DirectoryInfo.CreateSubdirectory(m_DirecoryInfo.Name)
            For Each m_Fileinfo As FileInfo In m_DirecoryInfo.GetFiles
                m_Fileinfo.CopyTo(ProjectSettings.DirectoryInfo.GetDirectories(m_DirecoryInfo.Name)(0).FullName & "\" & m_Fileinfo.Name)
            Next

            m_DirecoryInfo = New DirectoryInfo(TemplatesFolder)
            For Each m_Fileinfo As FileInfo In m_DirecoryInfo.GetFiles
                If (TemplatesFolder & "\" & m_Fileinfo.Name).ToLower = TemplatePath.ToLower Then
                    'Use the active template as the project file
                    m_Fileinfo.CopyTo(ProjectSettings.DirectoryInfo.FullName & "\" & "project.xml")
                End If
            Next

            Dim m_XmlDocument As New XmlDocument

            'Create the grid.xml file
            Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
            m_XmlDocument.AppendChild(m_XmlDeclaration)
            Dim m_XMLNodeRoot As XmlNode = m_XmlDocument.CreateElement("Grid")
            m_XmlDocument.AppendChild(m_XMLNodeRoot)

            m_XmlDocument.Save(ProjectSettings.DirectoryInfo.FullName & "\" & "grid.xml")

        End If

        Me.OldProjectID = Me.ProjectID

    End Sub

    Public Function ToXmlDocument() As XmlDocument
        Me.XmlDocument = New XmlDocument

        Dim m_XmlDeclaration As XmlDeclaration = Me.XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        Me.XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XMLNodeRoot As XmlNode = Me.XmlDocument.CreateElement("Object")
        Me.XmlDocument.AppendChild(m_XMLNodeRoot)

        Dim m_XmlElement As XmlElement

        m_XmlElement = Me.XmlDocument.CreateElement("Project")
        m_XMLNodeRoot.AppendChild(m_XmlElement)
        setValueForAttribute(Me.XmlDocument, m_XmlElement.Attributes, "id", Me.ProjectID)

        Return Me.XmlDocument
    End Function

    Public Sub CommitChanges()
        Dim m_DirectoryInfoUser As DirectoryInfo = New UserSettings().DirectoryInfo
        If Me.OldProjectID <> Me.ProjectID Then
            Dim m_DirectoryInfo As New DirectoryInfo(m_DirectoryInfoUser.FullName & "\" & Me.OldProjectID)
            m_DirectoryInfo.MoveTo(m_DirectoryInfoUser.FullName & "\" & Me.ProjectID)
        End If
    End Sub

    Public Sub ExecuteCommand(p_Command As String)

        Dim m_DirectoryInfoUser As DirectoryInfo = New UserSettings().DirectoryInfo
        Select Case p_Command.ToLower
            Case "add"
                'Add is done on creation of the class
            Case "delete"
                Dim m_DirectoryInfo As New DirectoryInfo(m_DirectoryInfoUser.FullName & "\" & Me.ProjectID)
                m_DirectoryInfo.Delete(True)
        End Select

    End Sub


End Class
