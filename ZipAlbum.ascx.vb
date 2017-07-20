Imports System.IO

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions

Imports Ventrian.SimpleGallery.Entities

Imports ICSharpCode.SharpZipLib.Checksums
Imports ICSharpCode.SharpZipLib.Zip

Namespace Ventrian.SimpleGallery

    Partial Public Class ZipAlbum
        Inherits SimpleGalleryBase

        Private Structure FileList

            Dim FullPath As String
            Dim FileName As String
            Dim Folder As String

        End Structure

#Region " Private Members "

        Private _albumID As Integer = Null.NullInteger
        Private _files As New List(Of FileList)

#End Region

#Region " Private Methods "

        Public Shared Sub AddFileToZip(ByRef ZipFile As ZipOutputStream, ByVal filePath As String, ByVal fileName As String, ByVal folder As String)
            Dim crc As Crc32 = New Crc32

            'Open File Stream
            Dim fs As FileStream = File.OpenRead(filePath)

            'Read file into byte array buffer
            Dim buffer As Byte()
            ReDim buffer(Convert.ToInt32(fs.Length) - 1)
            fs.Read(buffer, 0, buffer.Length)

            'Create Zip Entry
            Dim entry As ZipEntry = New ZipEntry(folder & fileName)
            entry.DateTime = DateTime.Now
            entry.Size = fs.Length
            fs.Close()
            crc.Reset()
            crc.Update(buffer)
            entry.Crc = crc.Value

            'Compress file and add to Zip file
            ZipFile.PutNextEntry(entry)
            ZipFile.Write(buffer, 0, buffer.Length)

        End Sub

        Private Function ExtractFileName(ByVal path As String) As String

            Dim extractPos As Integer = path.LastIndexOf("\") + 1
            Return path.Substring(extractPos, path.Length - extractPos).Replace("/", "_").Replace("..", ".")

        End Function

        Private Sub AddFiles(ByVal folder As String, ByVal objAlbum As AlbumInfo, ByVal logicalFolder As String)

            If (Directory.Exists(folder) = False) Then
                Return
            End If

            Dim objPhotoController As New PhotoController
            Dim objPhotos As ArrayList = objPhotoController.List(objAlbum.ModuleID, objAlbum.AlbumID, True, Null.NullInteger, False, Null.NullInteger, Null.NullString, "", Common.SortType.Name, Common.SortDirection.ASC)

            For Each objPhoto As PhotoInfo In objPhotos
                Dim path As String = folder
                If (path.EndsWith("\") = False) Then
                    path = path & "\"
                End If
                path = path & objPhoto.FileName

                If (File.Exists(path)) Then
                    Dim objFileList As New FileList()

                    objFileList.FullPath = path
                    objFileList.FileName = objPhoto.FileName
                    objFileList.Folder = logicalFolder

                    _files.Add(objFileList)
                End If
            Next

            If (GallerySettings.ZipIncludeSubFolders) Then

                Dim objAlbumController As New AlbumController
                Dim objChildAlbums As ArrayList = objAlbumController.List(Me.ModuleId, objAlbum.AlbumID, True, False, Common.AlbumSortType.Caption, Common.SortDirection.ASC)

                For Each objChildAlbum As AlbumInfo In objChildAlbums
                    AddFiles(PortalSettings.HomeDirectoryMapPath & objChildAlbum.HomeDirectory, objChildAlbum, logicalFolder & objChildAlbum.Caption & "\")
                Next

            End If

        End Sub


        Private Sub ReadQueryString()

            If (Request("AlbumID") <> "") Then
                Try
                    _albumID = Convert.ToInt32(Request("AlbumID"))
                Catch
                    Response.Redirect(NavigateURL(Me.TabId), True)
                End Try
            End If

            If (_albumID = Null.NullInteger) Then
                Response.Redirect(NavigateURL(Me.TabId), True)
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                If (GallerySettings.ZipEnabled = False) Then
                    Response.Redirect(NavigateURL(), True)
                End If

                ReadQueryString()

                Dim zipExists As Object = DataCache.GetCache("SG-Album-Zip-" & _albumID.ToString())
                If (zipExists IsNot Nothing) Then
                    Response.Redirect(zipExists.ToString(), True)
                End If

                Dim objAlbumController As New AlbumController()
                Dim objAlbum As AlbumInfo = objAlbumController.Get(_albumID)

                If (objAlbum IsNot Nothing) Then

                    Dim path As String = PortalSettings.HomeDirectoryMapPath & objAlbum.HomeDirectory.TrimEnd("\"c) & "\" & ExtractFileName(objAlbum.Caption) & ".zip"

                    If (File.Exists(path)) Then
                        File.Delete(path)
                    End If

                    Dim redirectPath = PortalSettings.HomeDirectory & objAlbum.HomeDirectory.TrimEnd("\"c) & "/" & ExtractFileName(objAlbum.Caption) & ".zip"

                    ' Build list of files.
                    AddFiles(PortalSettings.HomeDirectoryMapPath & objAlbum.HomeDirectory, objAlbum, "")

                    Dim CompressionLevel As Integer = 9

                    Dim zipFile As String = path

                    Dim strmZipFile As FileStream = Nothing
                    Try

                        strmZipFile = File.Create(zipFile)

                        Dim strmZipStream As ZipOutputStream = Nothing

                        Try

                            strmZipStream = New ZipOutputStream(strmZipFile)
                            strmZipStream.SetLevel(CompressionLevel)

                            For Each file As FileList In _files

                                AddFileToZip(strmZipStream, file.FullPath, file.FileName, file.Folder)

                            Next

                            DataCache.SetCache("SG-Album-Zip-" & _albumID.ToString(), redirectPath)

                        Catch ex As Exception
                            LogException(ex)
                        Finally
                            If Not strmZipStream Is Nothing Then
                                strmZipStream.Finish()
                                strmZipStream.Close()
                            End If
                        End Try

                        Response.Redirect(redirectPath, True)

                    Catch ex As Exception
                        LogException(ex)
                    Finally
                        If Not strmZipFile Is Nothing Then
                            strmZipFile.Close()
                        End If
                    End Try

                End If

                Response.Write(NavigateURL)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace