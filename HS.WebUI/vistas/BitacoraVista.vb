Imports HS.BE
Imports HS.BLL

Public Class BitacoraVista
    Private _usuario As BE.UsuarioDTO
    Private _bitacoraDTO As BitacoraDTO

    Public Property Bitacora() As BitacoraDTO
        Get
            If _usuario Is Nothing Then _bitacoraDTO = New BitacoraDTO()
            Return _bitacoraDTO
        End Get
        Set(ByVal value As BitacoraDTO)
            _bitacoraDTO = value
        End Set
    End Property

    Private _BitacoraBLL As IBitacoraBLL
    Public Property BitacoraBLL() As IBitacoraBLL
        Get
            If _BitacoraBLL Is Nothing Then _BitacoraBLL = New BitacoraBLL()
            Return _BitacoraBLL
        End Get
        Set(ByVal value As IBitacoraBLL)
            _BitacoraBLL = value
        End Set
    End Property

    Dim listaBitacoraDTo As List(Of BitacoraDTO) = New List(Of BitacoraDTO)
    Public Sub LlenarGrilla(ByRef dataGrid As System.Web.UI.WebControls.GridView)
        'traer todos los logs de bitacoras para enlazarlo al DataSource del control
        Dim Lista As List(Of BitacoraDTO) = Me.BitacoraBLL.ConsultaRango(Nothing, Nothing)
        Dim listaBitacoraDTo = New List(Of BitacoraDTO)
        For Each obj As BitacoraDTO In Lista
            If Not obj.Eliminado Then
                listaBitacoraDTo.Add(obj)
            End If
        Next
        listaBitacoraDTo.Sort(Function(x, y) y.Fecha.CompareTo(x.Fecha))
        dataGrid.DataSource = listaBitacoraDTo
        dataGrid.DataBind()
    End Sub

End Class