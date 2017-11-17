Imports HS.BE
Imports HS.BLL

Public Class HardwareVista
    Private _usuario As BE.UsuarioDTO
    Private _HardwareDTO As HardwareDTO

    Public Property Hardware() As HardwareDTO
        Get
            If _usuario Is Nothing Then _HardwareDTO = New HardwareDTO()
            Return _HardwareDTO
        End Get
        Set(ByVal value As HardwareDTO)
            _HardwareDTO = value
        End Set
    End Property

    Private _HardwareBLL As IHardwareBLL
    Public Property HardwareBLL() As IHardwareBLL
        Get
            If _HardwareBLL Is Nothing Then _HardwareBLL = New HardwareBLL()
            Return _HardwareBLL
        End Get
        Set(ByVal value As IHardwareBLL)
            _HardwareBLL = value
        End Set
    End Property

    Dim listaHardwareDTO As List(Of HardwareDTO) = New List(Of HardwareDTO)
    Public Sub LlenarGrilla(ByRef dataGrid As System.Web.UI.WebControls.GridView)
        Dim Lista As List(Of HardwareDTO) = Me.HardwareBLL.ConsultaRango(Nothing, Nothing)
        Dim listaHardwareDTo = New List(Of HardwareDTO)
        For Each obj As HardwareDTO In Lista
            If Not obj.Eliminado Then
                listaHardwareDTo.Add(obj)
            End If
        Next
        listaHardwareDTo.Sort(Function(x, y) y.Id.CompareTo(x.Id))
        dataGrid.DataSource = listaHardwareDTo
        dataGrid.DataBind()
    End Sub

End Class

