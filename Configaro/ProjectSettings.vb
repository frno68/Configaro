Imports System.Xml
Imports System.IO
Imports System.Web.HttpContext

Public Class ProjectSettings

    Public Property DirectoryInfo As DirectoryInfo = Nothing

    Public Sub New(p_ProjectID As String)

        Me.DirectoryInfo = New DirectoryInfo(New UserSettings().DirectoryInfo.FullName & "\" & p_ProjectID)

    End Sub

End Class
