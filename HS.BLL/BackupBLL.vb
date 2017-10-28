Imports HS.BE
Imports HS.DAL
Public Class BackupBLL

    Private _dao As BackupDAL = Nothing

    Public Sub New(ByVal pDAO As BackupDAL)
        Me._dao = pDAO
    End Sub

    Public Sub New()
        Me._dao = New BackupDAL()
    End Sub

    Public Function RealizarBackup(nombreBackup As String, filename As String, Optional fragmentMB As Integer = -1) As Boolean
        Try

            Dim filenameList As New List(Of String)
            Dim backupSizeInMb As Double = _dao.BackupSize()
            If (fragmentMB > 0) Then
                Dim fileParts As Integer = CInt(Math.Truncate(backupSizeInMb / fragmentMB)) '1.21 / 1 = 1.21 ==> 1 || 2/1 = 2
                If (fileParts > 0) Then
                    If (backupSizeInMb Mod fragmentMB > 0) Then
                        fileParts += 1
                    End If
                    For index = 1 To fileParts
                        filenameList.Add(filename + "_part" + CStr(index))
                    Next
                Else
                    filenameList.Add(filename)
                End If
            Else
                filenameList.Add(filename)
            End If


            _dao.RealizarBackup(nombreBackup, filenameList)

            'Using zip As ZipFile = New ZipFile()
            '    zip.AddFile(filename)
            '    zip.Comment = "Backup creado a las " & System.DateTime.Now.ToString("G")
            '    If (fragmentMB > 0) Then
            '        zip.MaxOutputSegmentSize = fragmentMB * 1024 * 1024   '' 2mb
            '    End If
            '    zip.Save(filename + ".zip")
            ' End Using
        Catch ex As Exception
            Throw
        End Try
        Return True
    End Function



    Public Function RealizarRestore(filenameList As List(Of String)) As Boolean
        Try
            _dao.RealizarRestore(filenameList)
        Catch ex As Exception
            Throw
        End Try
        Return True
    End Function

    Function BackupLista() As List(Of BackupDTO)
        Return _dao.BackupExistentes()
    End Function


End Class
