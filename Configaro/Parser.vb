Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml
Imports System.Text.RegularExpressions
Imports Eval3

Public Class Parser

    Public Property Evaluator As New Evaluator
    Public Property XmlDocumentProtocol As XmlDocument

    Public Sub New(p_XmlDocumentProtocol As XmlDocument)
        Me.XmlDocumentProtocol = p_XmlDocumentProtocol
        Parse()
    End Sub

    Public Function Analyze(ByVal p_XMLDocument As XmlDocument) As XmlDocument

        Dim xNewA As XmlNode
        Dim xA As XmlAttribute

        For Each xN As XmlNode In p_XMLDocument.ChildNodes()
            'M�ste kolla om det finns n�gra attribut �ver huvudtaget
            If Not xN.Attributes Is Nothing Then
                xA = xN.Attributes("expression")
                If Not xA Is Nothing Then
                    'Vi hittade ett criteria som vi ska analysera
                    If Not AnalyzeExpression(Me.XmlDocumentProtocol, xA.Value) Then
                        'Vi ska ta bort denna nod och underliggande
                        xNewA = p_XMLDocument.CreateAttribute("Command")
                        xNewA.Value = "Delete"
                        xN.Attributes.Append(xNewA)
                    Else
                        'Vi ska l�mna denna och forts�tta analysera barnen    
                        If xN.HasChildNodes Then analyzeChildNodesOf(xN, p_XMLDocument)
                    End If
                Else
                    'Noden inneh�ll inget criteria s� vi analyserar barnen
                    If xN.HasChildNodes Then analyzeChildNodesOf(xN, p_XMLDocument)
                End If
            Else
                'Noden inneh�ll inget criteria s� vi analyserar barnen
                If xN.HasChildNodes Then analyzeChildNodesOf(xN, p_XMLDocument)
            End If
        Next

        Return removeUnvantedNodesFrom(p_XMLDocument)

    End Function

    Private Sub Parse()

        Dim m_XMLNodeP As XmlNode = Me.XmlDocumentProtocol.SelectSingleNode("//Protocol")
        Dim m_XmlNodesQ As XmlNodeList = m_XMLNodeP.ChildNodes
        Dim m_XMLNodesA As XmlNodeList

        Dim m_Proceed_With As String = ""
        Dim m_LinkedQuestionAnswersCounter As Integer

        For Each m_XmlNodeQ As XmlNode In m_XmlNodesQ

            'Kolla f�rst om vi h�ller p� med ett direkthopp fr�n fr�gan
            If m_Proceed_With <> "" Then
                If m_Proceed_With <> getValueFromAttribute(m_XmlNodeQ.Attributes("SortOrder")) Then
                    'S� l�nge vi inte n�tt fram till fr�gan vi vill
                    setValueForAttribute(Me.XmlDocumentProtocol, m_XmlNodeQ.Attributes, "Display", "False")
                    'Nollst�ll alla svar f�r denna fr�ga
                    m_XMLNodesA = m_XmlNodeQ.ChildNodes
                    For Each m_XMLNodeA As XmlNode In m_XMLNodesA
                        m_XMLNodeA.Attributes.RemoveNamedItem("Selected")
                    Next
                Else
                    'H�r landade vi p� den aktuella fr�gan och st�nger av direkthoppet
                    m_XmlNodeQ.Attributes.RemoveNamedItem("Display")
                    m_Proceed_With = ""
                End If
            Else
                m_XmlNodeQ.Attributes.RemoveNamedItem("Display")
            End If

            If m_Proceed_With = "" Then
                m_LinkedQuestionAnswersCounter = -1

                Dim strQuestionType As String = getValueFromAttribute(m_XmlNodeQ.Attributes("QuestionType"))
                If strQuestionType <> "" Then
                    'Lite specialhantering
                    Select Case strQuestionType
                        Case "Linked_Choice"
                            'Denna returnerar -1 om det inte gick av n�gon anliedning ... 
                            '... i dessa fall skiter vi i hanteringen och tolkar denna som en vanlig fr�ga
                            m_LinkedQuestionAnswersCounter = getSelectedAnswerForLinked_Choice(Me.XmlDocumentProtocol, m_XmlNodeQ)
                        Case "Linked_Value"
                            'Not implemented
                    End Select
                End If

                'Vi h�ller inte p� med n�got. D� kollar vi om vi ska p�b�rja ett
                'H�r ska vi kolla om vi ska p�b�rja ett direkthopp
                m_Proceed_With = getValueFromAttribute(m_XmlNodeQ.Attributes("Proceed_With"))

                m_XMLNodesA = m_XmlNodeQ.ChildNodes

                If m_LinkedQuestionAnswersCounter <> -1 Then
                    'H�r vet vi vilket svar som ska vara selekterat eftersom
                    setAnswerIndexSelected(m_LinkedQuestionAnswersCounter, m_XMLNodesA)
                Else
                    If Not selectedAnswerExists(m_XMLNodesA) Then
                        'Det fanns inget svarsalternativ med Selected True.
                        'Vi s�tter det f�rsta svaret som selected
                        setValueForAttribute(Me.XmlDocumentProtocol, m_XMLNodesA(0).Attributes, "Selected", "True")
                    End If
                End If

                If m_Proceed_With = "" Then
                    For Each m_XMLNodeA As XmlNode In m_XMLNodesA
                        If getValueFromAttribute(m_XMLNodeA.Attributes("Selected")) = "True" Then
                            If getValueFromAttribute(m_XMLNodeA.Attributes("Proceed_With")) <> "" Then
                                m_Proceed_With = getValueFromAttribute(m_XMLNodeA.Attributes("Proceed_With"))
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If
        Next
    End Sub

    Private Function selectedAnswerExists(p_XMLNodesA As XmlNodeList) As Boolean

        'There are no answers to check, lets return True
        If p_XMLNodesA.Count = 0 Then Return True

        For Each m_XMLNodeA As XmlNode In p_XMLNodesA
            If getValueFromAttribute(m_XMLNodeA.Attributes("Selected")) = "True" Then
                Return True
            End If
        Next

        Return False

    End Function

    Private Sub setAnswerIndexSelected(p_Index As Integer, p_XMLNodesA As XmlNodeList)
        Dim i As Integer = 0
        For Each m_XMLNodeA As XmlNode In p_XMLNodesA
            If i = p_Index Then
                setValueForAttribute(Me.XmlDocumentProtocol, m_XMLNodeA.Attributes, "Selected", "True")
            Else
                m_XMLNodeA.Attributes.RemoveNamedItem("Selected")
            End If
            i = i + 1
        Next

    End Sub

    Private Function removeUnvantedNodesFrom(ByVal pDataDocument As XmlDocument) As XmlDocument

        Dim m_XMLNodeList As XmlNodeList
        Dim m_XMLNode As XmlNode
        Dim m_XMLAttribute As XmlAttribute

        Dim i As Integer 'R�kning m�ste ske bakl�nges eftersom vi ska ta bort information
        m_XMLNodeList = pDataDocument.ChildNodes
        For i = m_XMLNodeList.Count - 1 To 0 Step -1
            m_XMLNode = m_XMLNodeList(i)
            Select Case m_XMLNode.NodeType
                Case XmlNodeType.Element
                    m_XMLAttribute = m_XMLNode.Attributes("Command")
                    If Not m_XMLAttribute Is Nothing Then
                        If m_XMLAttribute.Value = "Delete" Then
                            pDataDocument.RemoveChild(m_XMLNode)
                        End If
                    End If
                    m_XMLAttribute = m_XMLNode.Attributes("expression")
                    If Not m_XMLAttribute Is Nothing Then
                        m_XMLNode.Attributes.Remove(m_XMLAttribute)
                    End If
                    removeUnvantedNodesFromChild(m_XMLNode)
                Case Else
            End Select
        Next

        Return pDataDocument

    End Function

    Private Sub removeUnvantedNodesFromChild(ByRef pXMLNode As XmlNode)
        Dim m_XMLNode As XmlNode
        Dim m_XMLAttribute As XmlAttribute

        Dim i As Integer 'R�kning m�ste ske bakl�nges eftersom vi ska ta bort information
        If pXMLNode.HasChildNodes Then
            For i = pXMLNode.ChildNodes.Count - 1 To 0 Step -1
                m_XMLNode = pXMLNode.ChildNodes(i)
                Select Case m_XMLNode.NodeType
                    Case XmlNodeType.Element
                        m_XMLAttribute = m_XMLNode.Attributes("Command")
                        If Not m_XMLAttribute Is Nothing Then
                            If m_XMLAttribute.Value = "Delete" Then
                                pXMLNode.RemoveChild(m_XMLNode)
                            End If
                        End If

                        m_XMLAttribute = m_XMLNode.Attributes("expression")
                        If Not m_XMLAttribute Is Nothing Then
                            m_XMLNode.Attributes.Remove(m_XMLAttribute)
                        End If

                        removeUnvantedNodesFromChild(m_XMLNode)
                    Case Else
                End Select
            Next
        End If

    End Sub

    Private Sub analyzeChildNodesOf(ByVal p_XMLNode As XmlNode, ByVal p_XMLDocument As XmlDocument)

        Dim m_XMLAttribute As XmlAttribute
        Dim m_XMLAttributeNew As XmlAttribute

        For Each m_XMLNode In p_XMLNode.ChildNodes
            If Not m_XMLNode.Attributes Is Nothing Then
                m_XMLAttribute = m_XMLNode.Attributes("expression")
                If Not m_XMLAttribute Is Nothing Then
                    'Vi hittade ett criteria som vi ska analysera
                    If Not AnalyzeExpression(Me.XmlDocumentProtocol, m_XMLAttribute.Value) Then
                        'Vi ska ta bort denna nod och underliggande
                        m_XMLAttributeNew = p_XMLDocument.CreateAttribute("Command")
                        m_XMLAttributeNew.Value = "Delete"
                        m_XMLNode.Attributes.Append(m_XMLAttributeNew)
                    Else
                        'Vi ska l�mna denna och forts�tta analysera barnen    
                        If m_XMLNode.HasChildNodes Then analyzeChildNodesOf(m_XMLNode, p_XMLDocument)
                    End If
                Else
                    'Noden inneh�ll inget criteria s� vi analyserar barnen
                    If m_XMLNode.HasChildNodes Then analyzeChildNodesOf(m_XMLNode, p_XMLDocument)
                End If
            Else
                'Noden inneh�ll inget criteria s� vi analyserar barnen
                If m_XMLNode.HasChildNodes Then analyzeChildNodesOf(m_XMLNode, p_XMLDocument)
            End If
        Next

    End Sub

    Private Function AnalyzeExpression(ByVal p_XMLDocument As XmlDocument, ByVal p_Expression As String) As Boolean

        Dim m_Regex As New Regex("-?\d+")
        Dim m_Match As Match
        Dim m_Expression As String = p_Expression

        If Trim(m_Expression) = "" Then
            Return True 'Default s� �r tomma v�rden sanna
        End If
        m_Match = m_Regex.Match(m_Expression) 'S�k efter numeriska v�rden
        While m_Match.Value <> ""
            m_Expression = Replace(m_Expression, m_Match.Value.Trim, IsThisOneOfMyAnswers(m_Match.Value.Trim, p_XMLDocument).ToString.ToLower, Count:=1)
            m_Match = m_Regex.Match(m_Expression)
        End While

        Return CBool(Evaluator.Parse(m_Expression).value)

    End Function

    Private Function IsThisOneOfMyAnswers(ByVal p_MatchedValue As String, ByVal p_XMLDocument As XmlDocument) As Boolean

        'G�r en select mot svaren f�r att kolla ... 
        Dim m_XMLNodeA As XmlNode = p_XMLDocument.SelectSingleNode("//Protocol/Question/Answer[@ID='" & p_MatchedValue & "' and @Selected='True']")

        'Om vi f�r en nod som retur s� har vi en tr�ff dvs True
        Return IIf(Not m_XMLNodeA Is Nothing, True, False)

    End Function

    Private Function getSelectedAnswerForLinked_Choice(ByRef p_XMLDocument As XmlDocument, ByVal p_XmlNodeQ As XmlNode) As Integer

        Dim m_LinkedQuestionAnswersCounter As Integer = -1
        Dim m_FoundLinkedQuestionAnswer As Boolean = False

        'Vi kollar om valet p� denna fr�ga redan �r f�rutbest�mt av n�gon annan fr�ga

        'Idt p� den fr�gan vi ska h�mta f�rvalet fr�n.
        Dim m_LinkedQuestion As Integer = 0 + getValueFromAttribute(p_XmlNodeQ.Attributes("LinkedQuestion"))
        'H�mta ut aktuell fr�ga 
        Dim m_XMLNodeQ As XmlNode = p_XMLDocument.SelectSingleNode("//Protocol/Question[@ID='" & m_LinkedQuestion & "']")
        'Och s�k igenom svaren efter ordningsnumret p� den som �r selekterad
        If Not m_XMLNodeQ Is Nothing Then
            Dim myXMLNodesA As XmlNodeList = m_XMLNodeQ.ChildNodes

            'For myLinkedQuestionAnswersCounter = 0 To drLinkedQuestionsAnswers.Length - 1
            m_LinkedQuestionAnswersCounter = 0
            For Each m_XMLNodeA As XmlNode In myXMLNodesA
                If getValueFromAttribute(m_XMLNodeA.Attributes("Selected")) = "True" Then
                    'Vi har hittat den och r�knaren har det v�rdet vi �r intresserad av
                    m_FoundLinkedQuestionAnswer = True
                    Exit For
                    'End If
                End If
                m_LinkedQuestionAnswersCounter = m_LinkedQuestionAnswersCounter + 1
            Next
        End If
        If Not m_FoundLinkedQuestionAnswer Then
            'Vi letade igenom alla svaren men inget var valt ... troligen n�t fel ... 
            '... vi tar inte hand om fr�getypen allts�
            m_LinkedQuestionAnswersCounter = -1
        End If

        Return m_LinkedQuestionAnswersCounter

    End Function

End Class
