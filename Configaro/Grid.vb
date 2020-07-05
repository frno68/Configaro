Imports System.Xml
Imports System.IO
Imports System.Configuration.ConfigurationManager
Public Class Grid

    Public Property Path As String = ""
    Private Property ProjectID As String = "-1"
    Private Property Edit As Boolean = False
    Public Property XmlDocument As New XmlDocument
    Public Property XmlNode As XmlNode

    Public Sub New(p_ProjectID As String)

        Me.ProjectID = p_ProjectID
        Me.Path = New ProjectSettings(Me.ProjectID).DirectoryInfo.FullName & "\grid.xml"
        Me.XmlDocument.Load(Me.Path)
        Me.XmlNode = Me.XmlDocument.SelectSingleNode("/Grid")

    End Sub

    Public Sub Save(p_strHtml As String)

        Me.XmlNode.InnerText = p_strHtml
        Me.CommitChanges()

    End Sub

    Public Sub CommitChanges()

        Me.XmlDocument.Save(Me.Path)

    End Sub



End Class
