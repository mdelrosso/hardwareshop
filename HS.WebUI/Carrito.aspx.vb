Imports HS.BE
Imports HS.BLL
Imports System.Data
Public Class Carrito
    Inherits System.Web.UI.Page

    Private _vista As HardwareVista = New HardwareVista()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Valido si el usuario posee permiso para acceder a esta página.
            Dim autenticacionVista As AutenticacionVista = New AutenticacionVista()
            Dim usuarioActual = autenticacionVista.UsuarioActual
            If Not autenticacionVista.UsuarioPoseePermiso(usuarioActual, "USUARIOFINAL") Then
                'Si no lo tiene se redirecciona a página de inicio.
                Me.Response.Redirect("~/Default.aspx")
            End If

            lblMensaje.Text = String.Empty

            If Not Page.IsPostBack Then
                Me.LlenarGrilla()
            End If

        Catch ex As Exception
            lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
        End Try
    End Sub

    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Me.Response.Redirect("Default.aspx")
    End Sub

    Private Sub LlenarGrilla()
        Me._vista.LlenarGrilla(grillaHardware)
    End Sub

    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        Dim index As Integer = grillaHardware.SelectedRow.RowIndex
        Dim name As String = grillaHardware.SelectedRow.Cells(0).Text
        Dim country As String = grillaHardware.SelectedRow.Cells(1).Text
        Dim message As String = "Row Index: " & index & "\nName: " & name + "\nCountry: " & country
        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('" + message + "');", True)
    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(grillaHardware, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub

End Class