Imports HS.BE
Public Class BackupConversor
    Implements IConversor(Of BE.BackupDTO)


    Public Function Convertir(row As DataRow) As BackupDTO Implements IConversor(Of BackupDTO).Convertir
        Dim objDTO As BackupDTO = New BackupDTO
        objDTO.Id = Convert.ToInt32(row("media_set_id"))
        objDTO.Nombre = Convert.ToString(row("backupset_name"))
        objDTO.Fecha = Convert.ToDateTime(row("backup_finish_date"))
        objDTO.SizeMB = Convert.ToDouble(row("backup_size_mb"))
        objDTO.Path = Convert.ToString(row("physical_device_name"))
        objDTO.Descripcion = Convert.ToString(row("description"))
        Return objDTO
    End Function

    Public Function Convertir(reader As IDataReader) As BackupDTO Implements IConversor(Of BackupDTO).Convertir
        Dim objDTO As BackupDTO = New BackupDTO
        objDTO.Id = Convert.ToInt32(reader("media_set_id"))
        objDTO.Nombre = Convert.ToString(reader("backupset_name"))
        objDTO.Fecha = Convert.ToDateTime(reader("backup_finish_date"))
        objDTO.SizeMB = Convert.ToDouble(reader("backup_size_mb"))
        objDTO.Path = Convert.ToString(reader("physical_device_name"))
        objDTO.Descripcion = Convert.ToString(reader("description"))
        Return objDTO
    End Function
End Class
