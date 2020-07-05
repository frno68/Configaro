Imports System.Xml
Imports System.IO

Public Class Resource
    Public Property FileInfos As FileInfo() = New DirectoryInfo(HttpContext.Current.Server.MapPath("_Resources")).GetFiles
    Public Property ResourceID As String = ""

    Public Sub New(p_ResourceID As String)

        Me.ResourceID = p_ResourceID

    End Sub

    Public Sub ExecuteCommand(p_Command As String)

        Select Case p_Command.ToLower
            Case "delete"
                For Each m_FileInfo As FileInfo In Me.FileInfos
                    If m_FileInfo.Name = Me.ResourceID Then
                        m_FileInfo.Delete()
                        Exit For
                    End If
                Next
        End Select

    End Sub
End Class
