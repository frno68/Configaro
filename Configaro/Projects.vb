Imports System.Xml
Imports System.IO
Imports System.Web.HttpContext
Public Class Projects

    Private Property DirectoryInfos As DirectoryInfo() = Nothing

    Public Sub New()
        DirectoryInfos = New DirectoryInfo(HttpContext.Current.Server.MapPath("_Projects")).GetDirectories(Current.User.Identity.Name)(0).GetDirectories
    End Sub

    Public Function ToHtml() As String

        Dim m_StringWriter As New StringWriter
        Dim m_HtmlTextWriter As New HtmlTextWriter(m_StringWriter)

        m_HtmlTextWriter.AddAttribute("data-role", "listview")
        m_HtmlTextWriter.AddAttribute("data-inset", "true")
        m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Ul)
        For Each m_DirectoryInfo As DirectoryInfo In Me.DirectoryInfos
            m_HtmlTextWriter.AddAttribute("ProjectID", m_DirectoryInfo.Name)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Li)
            m_HtmlTextWriter.AddAttribute("href", "/")
            m_HtmlTextWriter.AddAttribute("ProjectId", m_DirectoryInfo.Name)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.A)
            m_HtmlTextWriter.WriteLine(m_DirectoryInfo.Name)
            m_HtmlTextWriter.RenderEndTag()

            m_HtmlTextWriter.RenderEndTag()
        Next
        m_HtmlTextWriter.RenderEndTag()

        Return m_StringWriter.ToString
    End Function


End Class
