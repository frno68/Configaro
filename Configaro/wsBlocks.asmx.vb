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
Public Class wsBlocks
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetBlocks() As XmlDocument

        Dim m_XmlDocument As New XmlDocument
        Dim m_XmlDeclaration As XmlDeclaration = m_XmlDocument.CreateXmlDeclaration("1.0", Nothing, Nothing)
        m_XmlDocument.AppendChild(m_XmlDeclaration)

        Dim m_XmlRoot As XmlElement = m_XmlDocument.CreateElement("Root")
        m_XmlDocument.AppendChild(m_XmlRoot)

        Dim m_XmlElement As XmlElement = m_XmlDocument.CreateElement("Blocks")
        m_XmlRoot.AppendChild(m_XmlElement)

        m_XmlElement.InnerText = "" & New Blocks().ToHtml

        Return m_XmlDocument

    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)> _
    Public Function GetBlock(p_BlockID As String) As XmlDocument

        Dim m_Block As New Block(UriLocal.UnescapeDataString(p_BlockID))
        Return m_Block.ToXmlDocument

    End Function

    <WebMethod()> _
    Public Sub SaveBlock(p_BlockId As String, _
                                p_Expression As String, _
                                p_North As String, _
                                p_East As String, _
                                p_South As String, _
                                p_West As String, _
                                p_Representation As String, _
                                p_Content As String, _
                                p_Summary As String)

        Dim m_Block As New Block(UriLocal.UnescapeDataString(p_BlockId))
        m_Block.Expression = UriLocal.UnescapeDataString(p_Expression)
        m_Block.North = UriLocal.UnescapeDataString(p_North)
        m_Block.East = UriLocal.UnescapeDataString(p_East)
        m_Block.South = UriLocal.UnescapeDataString(p_South)
        m_Block.West = UriLocal.UnescapeDataString(p_West)
        m_Block.Representation = UriLocal.UnescapeDataString(p_Representation)
        m_Block.Content = UriLocal.UnescapeDataString(p_Content)
        m_Block.Summary = UriLocal.UnescapeDataString(p_Summary)
        m_Block.CommitChanges()


    End Sub

    <WebMethod()> _
    Public Sub ExecuteCommand(p_BlockID As String, _
                              p_Command As String)

        Dim m_Block As New Block(p_BlockID)
        m_Block.ExecuteCommand(p_Command)

    End Sub

End Class