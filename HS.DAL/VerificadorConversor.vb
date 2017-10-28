Imports HS.BE

Public Class VerificadorConversor
    Implements IConversor(Of BE.VerificadorDTO)

    Public Function Convertir(row As DataRow) As VerificadorDTO Implements IConversor(Of VerificadorDTO).Convertir
        Dim objDTO As VerificadorDTO = New VerificadorDTO
        objDTO.Id = Convert.ToInt32(row("IDDigitoVerificador"))
        objDTO.DigitoHorizontal = Convert.ToInt32(row("DigitoHorizontal"))
        objDTO.DigitoVertical = Convert.ToInt32(row("DigitoVertical"))
        objDTO.TablaNombre = Convert.ToString(row("TablaNombre"))
        Return objDTO
    End Function

    Public Function Convertir(reader As IDataReader) As VerificadorDTO Implements IConversor(Of VerificadorDTO).Convertir
        Dim objDTO As VerificadorDTO = New VerificadorDTO
        objDTO.Id = Convert.ToInt32(reader("IDDigitoVerificador"))
        objDTO.DigitoHorizontal = Convert.ToInt32(reader("DigitoHorizontal"))
        objDTO.DigitoVertical = Convert.ToInt32(reader("DigitoVertical"))
        objDTO.TablaNombre = Convert.ToString(reader("TablaNombre"))
        Return objDTO

    End Function
End Class
