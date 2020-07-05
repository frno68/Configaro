Public Class UriLocal

    Public Shared Function UnescapeDataString(p_DataStringToUnescape As String) As String

        p_DataStringToUnescape = Uri.UnescapeDataString(p_DataStringToUnescape)

        p_DataStringToUnescape = p_DataStringToUnescape.Replace("%E5", "å")
        p_DataStringToUnescape = p_DataStringToUnescape.Replace("%E4", "ä")
        p_DataStringToUnescape = p_DataStringToUnescape.Replace("%F6", "ö")

        Return p_DataStringToUnescape

    End Function
End Class
