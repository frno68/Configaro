Public Class TableHandler
#Region "Contructors"

    ''' <summary>
    ''' Create a DataTableHandler object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(ByVal p_Table As WebControls.Table)
        Me.Table = p_Table
    End Sub

#End Region

#Region "Private Fields"

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Table As WebControls.Table

#End Region

#Region "Public Methods"


    Public Function ToHTMLTable() As String

        Dim objStringBuilder As New Text.StringBuilder
        Dim objStringWriter As New System.IO.StringWriter(objStringBuilder)
        Dim objHtmlTextWriter As HtmlTextWriter = New HtmlTextWriter(objStringWriter)

        Table.RenderControl(objHtmlTextWriter)

        'This is to remove all hexadecimal characters that destroys the asynchronous calls
        Dim returnValue As String = Text.RegularExpressions.Regex.Replace(objStringBuilder.ToString, "\p{C}+", "")

        Return returnValue

    End Function


#End Region

#Region "Private Methods"

    'If this section is not used initially, keep it for structural reasons, and fill it on demand.

#End Region

End Class
