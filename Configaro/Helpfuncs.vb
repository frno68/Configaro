Imports Microsoft.VisualBasic
Imports System.Xml

Public Module Helpfuncs

    Public Function getValueFromAttribute(ByVal p_XmlAttribute As XmlAttribute) As String

        Dim myReturnValue As String
        If Not p_XmlAttribute Is Nothing Then
            myReturnValue = p_XmlAttribute.Value
        Else
            myReturnValue = ""
        End If

        Return myReturnValue

    End Function

    Public Sub setValueForAttribute(ByRef p_XmlDocument As XmlDocument, ByRef p_XmlAttributeCollection As XmlAttributeCollection, ByVal p_StrAttribute As String, ByVal p_StrValue As String)
        If p_StrValue = "" Then
            ' Inga tomma attribut
            p_XmlAttributeCollection.Remove(p_XmlAttributeCollection(p_StrAttribute))
        Else
            Dim myAttribute As XmlAttribute = p_XmlAttributeCollection(p_StrAttribute)
            If Not myAttribute Is Nothing Then
                p_XmlAttributeCollection(p_StrAttribute).Value = p_StrValue
            Else
                myAttribute = p_XmlDocument.CreateAttribute(p_StrAttribute)
                myAttribute.Value = p_StrValue
                p_XmlAttributeCollection.Append(myAttribute)
            End If
        End If

    End Sub

End Module

