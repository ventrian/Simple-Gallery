Imports System.IO
Imports System.Xml

Namespace Ventrian.SimpleGallery.Entities.MetaData

    Public Class XmpReader

#Region " Private Methods "

        Private Function GetDescription(ByVal objXmlDocument As XmlDocument) As String

            Dim objXmlNode As XmlNode = objXmlDocument.SelectSingleNode("/rdf:RDF/rdf:Description/dc:description/rdf:Alt", GetNamespace(objXmlDocument))

            If objXmlNode IsNot Nothing Then
                Return objXmlNode.ChildNodes(0).InnerText
            End If

            Return ""

        End Function

        Private Function GetNamespace(ByVal objXmlDocument As XmlDocument) As XmlNamespaceManager

            Dim objNamespaceManager As XmlNamespaceManager = New System.Xml.XmlNamespaceManager(objXmlDocument.NameTable)
            objNamespaceManager.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#")
            objNamespaceManager.AddNamespace("exif", "http://ns.adobe.com/exif/1.0/")
            objNamespaceManager.AddNamespace("x", "adobe:ns:meta/")
            objNamespaceManager.AddNamespace("xap", "http://ns.adobe.com/xap/1.0/")
            objNamespaceManager.AddNamespace("tiff", "http://ns.adobe.com/tiff/1.0/")
            objNamespaceManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/")

            Return objNamespaceManager

        End Function

        Private Function GetTags(ByVal objXmlDocument As XmlDocument) As String

            Dim objXmlNode As XmlNode = objXmlDocument.SelectSingleNode("/rdf:RDF/rdf:Description/dc:subject/rdf:Bag", GetNamespace(objXmlDocument))

            Dim tags As String = ""
            If objXmlNode IsNot Nothing Then
                For Each objXmlChildNode As XmlNode In objXmlNode
                    If (tags.Length = 0) Then
                        tags = objXmlChildNode.InnerText
                    Else
                        tags = tags & " " & objXmlChildNode.InnerText
                    End If
                Next
            End If

            Return tags

        End Function

        Private Function GetTitle(ByVal objXmlDocument As XmlDocument) As String

            Dim objXmlNode As XmlNode = objXmlDocument.SelectSingleNode("/rdf:RDF/rdf:Description/dc:title/rdf:Alt", GetNamespace(objXmlDocument))

            If objXmlNode IsNot Nothing Then
                Return objXmlNode.ChildNodes(0).InnerText
            End If

            Return ""

        End Function

        Private Function GetXmlDocument(ByVal xml As String) As XmlDocument

            Dim doc As New XmlDocument()
            doc.LoadXml(xml)
            Return doc

        End Function

        Private Function GetXmpXml(ByRef objStream As Stream) As String

            Dim beginCapture As String = "<rdf:RDF"
            Dim endCapture As String = "</rdf:RDF>"
            Dim collection As String = String.Empty
            Dim collecting As Boolean = False
            Dim matching As Boolean = False
            Dim collectionCount As Integer = 0

            objStream.Seek(0, SeekOrigin.Begin)

            Using sr As New System.IO.StreamReader(objStream)
                While Not sr.EndOfStream
                    Dim contents As Char = ChrW(sr.Read())

                    If Not matching AndAlso Not collecting AndAlso contents = "<"c Then
                        matching = True
                    End If

                    If matching Then
                        collection += contents

                        If collection.Contains(beginCapture) Then
                            'found the begin element we can stop matching and start collecting
                            matching = False
                            collecting = True
                        Else
                            If contents = beginCapture(collectionCount) Then
                                collectionCount = collectionCount + 1
                                'we are still looking, but on track to start collecting
                                Continue While
                            Else
                                'false start reset everything
                                collection = String.Empty
                                matching = False
                                collecting = False
                                collectionCount = 0

                            End If
                        End If
                    ElseIf collecting Then
                        collection += contents

                        If collection.Contains(endCapture) Then
                            'we are finished found the end of the XMP data
                            Exit While
                        End If
                    End If

                End While
            End Using

            Return collection

        End Function

#End Region

#Region " Public Methods "

        Public Sub ApplyAttributes(ByRef objPhoto As PhotoInfo, ByRef objStream As Stream)

            Dim xml As String = GetXmpXml(objStream)
            Dim objXmlDocument As XmlDocument = GetXmlDocument(xml)

            If Not (objXmlDocument Is Nothing) Then
                Dim title = GetTitle(objXmlDocument)
                If (title.trim() <> "") Then
                    objPhoto.Name = GetTitle(objXmlDocument)
                End If
                objPhoto.Description = GetDescription(objXmlDocument)
                objPhoto.Tags = GetTags(objXmlDocument)
            End If

        End Sub

#End Region

    End Class

End Namespace
