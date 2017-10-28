Imports System.Data
Imports HS.BE

Public Class BackupDAL

    ''' <summary>
    ''' objeto que encapsula la funcionalidad de acceso, persistencia y lectura
    ''' de datos en el origen de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Private _wrapper As IComando = Nothing
    ''' <summary>
    ''' conversor a BE de los datos devueltos por la consulta SQL.
    ''' </summary>
    ''' <remarks></remarks>
    Private _conversor As IConversor(Of BE.BackupDTO) = Nothing

    Const DBNAME As String = "HS"

    Public Function RealizarBackup(nombreBackup As String, ByRef filenames As List(Of String)) As Boolean
        Dim resultado As Integer = 0
        Dim cmdStr As String = "BACKUP DATABASE " + DBNAME + " TO "
        Dim paramcount As Integer = 1
        For Each filename As String In filenames
            cmdStr += "DISK = @filename" + CStr(paramcount) + ","
            paramcount += 1
        Next
        cmdStr = cmdStr.Trim().Remove(cmdStr.Length - 1)
        cmdStr += " WITH FORMAT,NAME=@name"
        Dim comando As IDbCommand = Me.Wrapper.CrearComando(cmdStr, CommandType.Text)

        Try
            Me.Wrapper.AgregarParametro(comando, "@name", nombreBackup)

            paramcount = 1
            For Each filename As String In filenames
                Me.Wrapper.AgregarParametro(comando, "@filename" + CStr(paramcount), filename)
                paramcount += 1
            Next

            resultado = Me._wrapper.EjecutarConsulta(comando)
        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return (resultado > 0)
    End Function

    Public Function RealizarRestore(ByRef filenames As List(Of String)) As Boolean
        Dim resultado As Integer = 0
        Dim cmdStr As String = "USE [MASTER] ; ALTER DATABASE " + DBNAME + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE ; RESTORE DATABASE " + DBNAME + " FROM "
        Dim paramcount As Integer = 1
        For Each filename As String In filenames
            cmdStr += "DISK = @filename" + CStr(paramcount) + ","
            paramcount += 1
        Next
        cmdStr = cmdStr.Trim().Remove(cmdStr.Length - 1)
        cmdStr += " ;ALTER DATABASE " + DBNAME + " SET MULTI_USER"

        Dim comando As IDbCommand = Me.Wrapper.CrearComando(cmdStr, CommandType.Text)

        Try
            paramcount = 1
            For Each filename As String In filenames
                Me.Wrapper.AgregarParametro(comando, "@filename" + CStr(paramcount), filename)
                paramcount += 1
            Next
            resultado = Me._wrapper.EjecutarConsulta(comando)
        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return (resultado > 0)
    End Function

    Public Function BackupExistentes() As System.Collections.Generic.List(Of BE.BackupDTO)
        Dim lista As List(Of BE.BackupDTO) = New List(Of BE.BackupDTO)
        Dim sCmd As String = "  SELECT   msdb.dbo.backupset.media_set_id,msdb.dbo.backupset.name AS backupset_name,msdb.dbo.backupset.backup_finish_date, "
        sCmd += " cast(round(msdb.dbo.backupset.backup_size/1048576,2) as decimal(18,2)) as backup_size_mb,  "
        sCmd += " msdb.dbo.backupmediafamily.physical_device_name, "
        sCmd += " msdb.dbo.backupset.description"
        sCmd += " FROM msdb.dbo.backupmediafamily"
        sCmd += " INNER JOIN msdb.dbo.backupset ON msdb.dbo.backupmediafamily.media_set_id = msdb.dbo.backupset.media_set_id  "
        sCmd += " WHERE  msdb..backupset.type = 'D' "
        sCmd += " and msdb..backupset.database_name = '" + DBNAME + "' "
        sCmd += " order by backup_finish_date"
        Dim comando As IDbCommand = Me.Wrapper.CrearComando(sCmd, CommandType.Text)
        Try
            Using reader As IDataReader = Me.Wrapper.ConsultarReader(comando)
                Do While reader.Read()
                    lista.Add(Me.Conversor.Convertir(reader))
                Loop
            End Using

        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return lista
    End Function

    Function BackupSize() As Double
        Dim fileSize As Double = -1
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("select backupSize = convert(numeric(10,2), round(fileproperty( a.name,'SpaceUsed')/128.,2)) from sysfiles a where a.name = '" + DBNAME + "'", CommandType.Text)
        Try
            Using reader As IDataReader = Me.Wrapper.ConsultarReader(comando)

                Do While reader.Read()
                    fileSize = (Convert.ToDouble(reader("backupSize")))
                Loop

            End Using

        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return fileSize
    End Function

    Public Property Conversor As IConversor(Of BackupDTO)
        Get
            If Me._conversor Is Nothing Then
                ' obtener el conversor por defecto para esta entidad
                Me._conversor = New BackupConversor()
            End If
            Return Me._conversor
        End Get
        Set(ByVal value As IConversor(Of BackupDTO))
            Me._conversor = value
        End Set
    End Property


    Public Property Wrapper As IComando
        Get
            If Me._wrapper Is Nothing Then
                 Me._wrapper = ComandoFactory.CrearComando("Default")
            End If
            Return Me._wrapper
        End Get
        Set(ByVal value As IComando)
            Me._wrapper = value
        End Set
    End Property

End Class

