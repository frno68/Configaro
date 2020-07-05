Imports System.Xml
Imports System.IO
Public Class Resources

    Public Property FileInfos As FileInfo() = New DirectoryInfo(HttpContext.Current.Server.MapPath("_Resources")).GetFiles

    Public Sub New()
    End Sub

    Public Function ToHtmlList() As String

        Dim m_StringWriter As New StringWriter
        Dim m_HtmlTextWriter As New HtmlTextWriter(m_StringWriter)

        'm_HtmlTextWriter.AddAttribute("data-role", "listview")
        'm_HtmlTextWriter.AddAttribute("data-inset", "true")
        'm_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Ul)

        m_HtmlTextWriter.AddAttribute("data-role", "controlgroup")
        m_HtmlTextWriter.AddAttribute("data-type", "horizontal")
        m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Fieldset)

        For Each m_FileInfo As FileInfo In Me.FileInfos

            m_HtmlTextWriter.AddAttribute("type", "checkbox")
            m_HtmlTextWriter.AddAttribute("ResourceID", m_FileInfo.Name)
            m_HtmlTextWriter.AddAttribute("name", "radio-choice-" & m_FileInfo.Name)
            m_HtmlTextWriter.AddAttribute("id", m_FileInfo.Name)
            m_HtmlTextWriter.AddAttribute("value", m_FileInfo.Name)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Input)
            m_HtmlTextWriter.RenderEndTag()

            m_HtmlTextWriter.AddAttribute("for", m_FileInfo.Name)
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Label)

            m_HtmlTextWriter.AddAttribute("src", "_Resources/" & m_FileInfo.Name)
            m_HtmlTextWriter.AddAttribute("class", "item ui-li-icon")
            m_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Img)
            m_HtmlTextWriter.RenderEndTag()
            m_HtmlTextWriter.Write(m_FileInfo.Name)

            m_HtmlTextWriter.RenderEndTag()

            'm_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Li)

            'm_HtmlTextWriter.AddAttribute("ResourceID", m_FileInfo.Name)
            'm_HtmlTextWriter.AddAttribute("href", "#")
            'm_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.A)

            'm_HtmlTextWriter.AddAttribute("src", "_Resources/" & m_FileInfo.Name)
            'm_HtmlTextWriter.AddAttribute("class", "item ui-li-icon")
            'm_HtmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Img)
            'm_HtmlTextWriter.RenderEndTag()

            'm_HtmlTextWriter.WriteLine(m_FileInfo.Name)

            'm_HtmlTextWriter.RenderEndTag()

            'm_HtmlTextWriter.RenderEndTag()
        Next
        'm_HtmlTextWriter.RenderEndTag()
        m_HtmlTextWriter.RenderEndTag()

        Return m_StringWriter.ToString
    End Function

End Class
