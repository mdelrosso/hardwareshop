Imports HS.BE
Imports HS.DAL

Public Interface IHardwareBLL
    Inherits ICRUD(Of HardwareDTO)
End Interface

''' <summary>
''' Gestiona el Hardware.
''' </summary>
Public Class HardwareBLL
    Implements IHardwareBLL

    ''' <summary>
    ''' objeto que se conectara al origen de datos para actualizarlo y consultarlo
    ''' </summary>
    Private _dal As IHardwareDAL = Nothing
    
    Public Sub New(ByVal pDAO As IHardwareDAL)
        Me._dal = pDAO
    End Sub

    Public Sub New()
        Me._dal = New HardwareDAL()
    End Sub

    ''' <summary>
    ''' Agrega un nuevo Hardware al sistema.
    ''' </summary>
    Public Function Alta(ByRef value As BE.HardwareDTO) As Boolean Implements IHardwareBLL.Alta
        Try
            Return Me._dal.Alta(value)
        Catch ex As Exception
            Throw New Exception("No se puede agregar.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Elimina un Hardware existente del sistema.
    ''' </summary>
    Public Function Baja(ByRef value As BE.HardwareDTO) As Boolean Implements IHardwareBLL.Baja
        Try
            Return Me._dal.Baja(value)
        Catch ex As Exception
            Throw New Exception("No se puede eliminar.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Retorna el primer Hardware que coincida con el filtro especificado.
    ''' </summary>
    Public Function Consulta(ByRef filtro As BE.HardwareDTO) As BE.HardwareDTO Implements IHardwareBLL.Consulta
        Try
            Return Me._dal.Consulta(filtro)
        Catch ex As Exception
            Throw New Exception("No se puede consultar.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Retorna todos los Hardwares que coincidan con el filtro especificado.
    ''' </summary>
    Public Function ConsultaRango(ByRef filtroDesde As BE.HardwareDTO, ByRef filtroHasta As BE.HardwareDTO) As System.Collections.Generic.List(Of BE.HardwareDTO) Implements IHardwareBLL.ConsultaRango
        Try
            Return Me._dal.ConsultaRango(filtroDesde, filtroHasta)
        Catch ex As Exception
            Throw New Exception("No se puede consultar por rango.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Modifica un Hardware existente del sistema.
    ''' </summary>
    Public Function Modificacion(ByRef value As BE.HardwareDTO) As Boolean Implements IHardwareBLL.Modificacion
        Try
            Return Me._dal.Modificacion(value)
        Catch ex As Exception
            Throw New Exception("No se puede modificar.", ex)
        End Try
    End Function


End Class
