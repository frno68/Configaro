Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml
Imports System.Web.Script.Services

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class wsGrid
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetRepresentation(p_ProjectID As String) As XmlDocument

        Return New Representation(UriLocal.UnescapeDataString(p_ProjectID)).ToXMLDocument

    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetContent(p_ProjectID As String) As XmlDocument

        Return New Content(UriLocal.UnescapeDataString(p_ProjectID)).ToXMLDocument

    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetSummary(p_ProjectID As String) As XmlDocument

        Return New Summary(UriLocal.UnescapeDataString(p_ProjectID)).ToXMLDocument

    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetProtocol(p_ProjectID As String) As XmlDocument

        Dim m_XmlDocument As New XmlDocument
        Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XmlRoot As XmlElement = m_XmlDocument.CreateElement("Root")
        m_XmlDocument.AppendChild(m_XmlRoot)

        Dim m_XmlElementProtocol As XmlElement = m_XmlDocument.CreateElement("Protocol")
        m_XmlRoot.AppendChild(m_XmlElementProtocol)

        Dim m_XmlElementPath As XmlElement = m_XmlDocument.CreateElement("Path")
        m_XmlRoot.AppendChild(m_XmlElementPath)

        Dim m_Protocol As New Protocol(UriLocal.UnescapeDataString(p_ProjectID))

        m_XmlElementProtocol.InnerText = "" & m_Protocol.ToHtml
        m_XmlElementPath.InnerText = "" & m_Protocol.Path

        Return m_XmlDocument

    End Function

    <WebMethod()> _
    Public Sub SaveAnswer(p_ProjectID As String, _
                          p_QuestionID As Integer, _
                          p_Value As String)

        Dim m_Protocol As New Protocol(UriLocal.UnescapeDataString(p_ProjectID))
        m_Protocol.SaveAnswer(p_QuestionID, p_Value)

    End Sub

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetGridEmpty() As XmlDocument
        Dim Grid As New Table
        Dim gridSize As Integer = 10
        'hidGridSize.Value = gridSize
        Dim Splitter As String = "•"
        Dim m_TR As TableRow
        Dim m_TD As TableCell

        For row As Integer = 0 To gridSize
            m_TR = New TableRow
            m_TR.TableSection = TableRowSection.TableBody
            m_TR.CssClass = "gridrow"
            For col As Integer = 0 To gridSize
                m_TD = New TableCell
                m_TD.ID = row & Splitter & col
                m_TD.CssClass = "gridcell"
                m_TR.Cells.Add(m_TD)
            Next
            Grid.Rows.Add(m_TR)
        Next
        Grid.CssClass = "grid"

        Dim m_XmlDocument As New XmlDocument
        Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XmlRoot As XmlElement = m_XmlDocument.CreateElement("Root")
        m_XmlDocument.AppendChild(m_XmlRoot)

        Dim m_XmlElementGrid As XmlElement = m_XmlDocument.CreateElement("Grid")
        m_XmlRoot.AppendChild(m_XmlElementGrid)

        m_XmlElementGrid.InnerText = "" & New TableHandler(Grid).ToHTMLTable

        Return m_XmlDocument


    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetGrid(p_ProjectID As String) As XmlDocument

        Dim m_XmlDocument As New XmlDocument
        Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XmlRoot As XmlElement = m_XmlDocument.CreateElement("Root")
        m_XmlDocument.AppendChild(m_XmlRoot)

        Dim m_XmlElementGrid As XmlElement = m_XmlDocument.CreateElement("Grid")
        m_XmlRoot.AppendChild(m_XmlElementGrid)

        Dim m_Grid As New Grid(UriLocal.UnescapeDataString(p_ProjectID))

        m_XmlElementGrid.InnerText = "" & m_Grid.XmlNode.InnerText

        Return m_XmlDocument

    End Function

    <WebMethod()> _
    Public Sub SaveGrid(p_ProjectID As String, _
                          p_strHtml As String)

        Dim m_Grid As New Grid(UriLocal.UnescapeDataString(p_ProjectID))
        m_Grid.Save(p_strHtml)

    End Sub


End Class