Imports HS.BE
Imports HS.BLL

Public Class IntegridadBD
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Valido si el usuario posee permiso para acceder a esta página.
            Dim autenticacionVista As AutenticacionVista = New AutenticacionVista()
            Dim usuarioActual = autenticacionVista.UsuarioActual
            If Not autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMINISTRACION") Then
                'Si no lo tiene se redirecciona a página de inicio.
                Me.Response.Redirect("~/Default.aspx")
            End If

            VerificarIntegridad(sender, e)
        Catch ex As Exception
            lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
        End Try
    End Sub

    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Me.Response.Redirect("~/Default.aspx")
    End Sub

    Protected Sub VerificarIntegridad(sender As Object, e As EventArgs) Handles btnVerificarIntegridad.Click
        Try
            Dim sMsg As String = ""
            sMsg = IntegridadBLL.VerificarIntegridadBD()
            If (sMsg = Nothing) Then
                lblMensaje.Text = "OK. Test de integridad finalizado con exito."
                lblMensaje.ForeColor = Drawing.Color.Blue
            Else
                lblMensaje.Text = sMsg
                lblMensaje.ForeColor = Drawing.Color.Red
            End If
        Catch ex As Exception
            lblMensaje.Text = "ERROR. Test de integridad finalizado con error. " + ErrorHandler.ObtenerMensajeDeError(ex)
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try

    End Sub

    Protected Sub RecalcularDigitosVerificadores(sender As Object, e As EventArgs) Handles btnRecalcularDigitosVerificadores.Click
        Try
            IntegridadBLL.RegenerarDigitosVerificadores()
            lblMensaje.Text = "OK. Digitos Verificadores regenerados con exito."
            lblMensaje.ForeColor = Drawing.Color.Blue
        Catch ex As Exception
            lblMensaje.Text = "ERROR. Regenericacion de digitos fallida. " + ErrorHandler.ObtenerMensajeDeError(ex)
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try
    End Sub
End Class