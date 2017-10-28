Imports HS.BE
Imports hs.BLL

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblMensaje.Text = String.Empty
            welcomeLabel.Text = String.Empty

            Dim autenticacionVista As AutenticacionVista = New AutenticacionVista()
            Dim usuarioActual = autenticacionVista.UsuarioActual

            If Not usuarioActual Is Nothing Then

                welcomeLabel.Text = String.Format("<h1>¡Bienvenido {0} a <span>HARDWARE</span>SHOP!<h1>", usuarioActual.Nombre)
                btnIntegridadBD.Visible = autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMINISTRACION")

                If IntegridadBLL.VerificarIntegridadBD = Nothing Then
                    btnCarrito.Visible = autenticacionVista.UsuarioPoseePermiso(usuarioActual, "USUARIOFINAL")
                    btnCambioDePrecios.Visible = autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMIN_NEGOCIO")
                    btnBitacora.Visible = autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMINISTRACION")
                    btnAdministracionUsuarios.Visible = autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMIN_USUARIOS") Or autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMIN_PERFILES") Or autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMINISTRACION")
                    btnBackupYRestore.Visible = autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMINISTRACION")
                Else
                    If autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMINISTRACION") Then
                        Response.Redirect("~/IntegridadBD.aspx")
                    Else
                        lblMensaje.Text = "UPS :(  Parece haber problemas con el sistema. Nuestros administradores estan trabajando para solucionarlo. Muchas gracias"
                        lblMensaje.ForeColor = Drawing.Color.Red
                    End If
                End If
            Else
                welcomeLabel.Text = "<h1>¡Bienvenido a <span>HARDWARE</span>SHOP!<h1>"
            End If

        Catch ex As Exception
            lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
        End Try
    End Sub


    Protected Sub btnCambioDePrecios_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCambioDePrecios.Click
        Response.Redirect("~/CambioDePrecios.aspx")
    End Sub

    Protected Sub btnBitacora_Click(sender As Object, e As EventArgs) Handles btnBitacora.Click
        Response.Redirect("~/Bitacora.aspx")
    End Sub

    Protected Sub btnBackupYRestore_Click(sender As Object, e As EventArgs) Handles btnBackupYRestore.Click
        Response.Redirect("~/BackupRestore.aspx")
    End Sub

    Protected Sub btnCarrito_Click(sender As Object, e As EventArgs) Handles btnCarrito.Click
        Response.Redirect("~/Carrito.aspx")
    End Sub

    Protected Sub btnIntegridadBD_Click(sender As Object, e As EventArgs) Handles btnIntegridadBD.Click
        Response.Redirect("~/IntegridadBD.aspx")
    End Sub
    
End Class