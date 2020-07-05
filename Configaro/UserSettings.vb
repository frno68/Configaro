Imports System.Xml
Imports System.IO
Imports System.Web.HttpContext

Public Class UserSettings

    Public Property DirectoryInfo As DirectoryInfo = Nothing

    Private Property DirectoryInfoProjects As DirectoryInfo = New DirectoryInfo(HttpContext.Current.Server.MapPath("_Projects"))

    Public Sub New()

        Me.DirectoryInfo = Me.DirectoryInfoProjects.GetDirectories(Current.User.Identity.Name)(0)

    End Sub

End Class
