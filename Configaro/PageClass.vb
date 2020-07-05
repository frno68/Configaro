Public Class PageClass
    Inherits Web.UI.Page

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RegisterIncludes()

    End Sub

    Private Sub RegisterIncludes()
        Dim objInclude As HtmlGenericControl
        Me.Page.Header.Controls.AddAt(0, GetJsHtmlGenericControl("Scripts/pagehandler.js"))
        objInclude = New HtmlGenericControl("script")
        objInclude.Attributes.Add("type", "text/javascript")
        objInclude.InnerHtml = GetJavaScriptForBaseURL()
        Me.Page.Header.Controls.AddAt(0, objInclude)
        Me.Page.Header.Controls.AddAt(0, GetJsHtmlGenericControl("Scripts/resources.js"))
        Me.Page.Header.Controls.AddAt(0, GetJsHtmlGenericControl("http://code.jquery.com/mobile/1.4.4/jquery.mobile-1.4.4.min.js"))
        Me.Page.Header.Controls.AddAt(0, GetJsHtmlGenericControl("Scripts/jquery/jquery.format.js"))
        'Me.Page.Header.Controls.AddAt(0, GetJsHtmlGenericControl("Scripts/jquery/jquery.ui.js"))
        Me.Page.Header.Controls.AddAt(0, GetJsHtmlGenericControl("http://code.jquery.com/jquery-1.11.1.min.js"))
        Me.Page.Header.Controls.AddAt(0, GetStylesHtmlGenericControl("Styles/styles.css"))
        Me.Page.Header.Controls.AddAt(0, GetStylesHtmlGenericControl("http://code.jquery.com/mobile/1.4.4/jquery.mobile-1.4.4.min.css"))
        '<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
        objInclude = New HtmlGenericControl("meta")
        objInclude.Attributes.Add("name", "viewport")
        objInclude.Attributes.Add("content", "width=device-width, initial-scale=1, maximum-scale=1")
        Me.Page.Header.Controls.AddAt(0, objInclude)
    End Sub

    Private Function GetJsHtmlGenericControl(ByVal p_strRelFilePath As String) As HtmlGenericControl

        Dim objInclude As HtmlGenericControl = New HtmlGenericControl("script")
        objInclude.Attributes.Add("type", "text/javascript")
        objInclude.Attributes.Add("language", "javascript")
        objInclude.Attributes.Add("src", ResolveUrl(p_strRelFilePath))

        Return objInclude

    End Function

    Private Function GetStylesHtmlGenericControl(ByVal p_strRelFilePath As String) As HtmlGenericControl

        Dim objInclude As HtmlGenericControl = New HtmlGenericControl("link")
        objInclude.Attributes.Add("type", "text/css")
        objInclude.Attributes.Add("rel", "stylesheet")
        objInclude.Attributes.Add("href", ResolveUrl(p_strRelFilePath))

        Return objInclude

    End Function

    Private Function GetJavaScriptForBaseURL() As String

        Dim strJavaScriptForBaseURL As String = "" & vbCrLf
        strJavaScriptForBaseURL = strJavaScriptForBaseURL & "var BaseUrl = '" & ResolveUrl("~") & "';" & vbCrLf
        Return strJavaScriptForBaseURL

    End Function

End Class
