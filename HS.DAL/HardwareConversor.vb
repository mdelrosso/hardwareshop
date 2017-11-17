Imports HS.BE

Public Class HardwareConversor
    Implements IConversor(Of BE.HardwareDTO)

    Public Function Convertir(row As DataRow) As HardwareDTO Implements IConversor(Of HardwareDTO).Convertir
        Dim HardwareDTO As HardwareDTO = New HardwareDTO
        HardwareDTO.Id = Convert.ToInt32(row("Hard_Id"))
        HardwareDTO.Descripcion = Convert.ToString(row("Hard_Descripcion"))
        HardwareDTO.Precio = Convert.ToDouble(row("Hard_Precio"))
        HardwareDTO.Eliminado = Convert.ToBoolean(row("Hard_Eliminado"))
        HardwareDTO.DigitoHorizontal = Convert.ToInt32(row("DigitoHorizontal"))
        Return HardwareDTO
    End Function

    Public Function Convertir(reader As IDataReader) As HardwareDTO Implements IConversor(Of HardwareDTO).Convertir
        Dim HardwareDTO As HardwareDTO = New HardwareDTO
        HardwareDTO.Id = Convert.ToInt32(reader("Hard_Id"))
        HardwareDTO.Descripcion = Convert.ToString(reader("Hard_Descripcion"))
        HardwareDTO.Precio = Convert.ToDouble(reader("Hard_Precio"))
        HardwareDTO.Eliminado = Convert.ToBoolean(reader("Hard_Eliminado"))
        HardwareDTO.DigitoHorizontal = Convert.ToInt32(reader("DigitoHorizontal"))
        Return HardwareDTO
    End Function
End Class
