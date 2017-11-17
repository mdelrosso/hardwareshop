Imports HS.BE
Imports HS.BLL

Public Class AdministracionUsuarios
    Inherits System.Web.UI.Page

    Private _vista As UsuarioVista = New UsuarioVista()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Valido si el usuario posee permiso para acceder a esta página.
            Dim autenticacionVista As AutenticacionVista = New AutenticacionVista()
            Dim usuarioActual = autenticacionVista.UsuarioActual
            If Not (autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMIN_USUARIOS") Or autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMINISTRACION")) Then
                'Si no lo tiene se redirecciona a página de inicio.
                Me.Response.Redirect("~/Default.aspx")
            End If

            lblMensaje.Text = String.Empty

            If Not Page.IsPostBack Then
                Me.LimpiarCampos()
                Me.LlenarGrilla()
            End If

        Catch ex As Exception
            lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
        End Try
    End Sub

    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Me.Response.Redirect("Default.aspx")
    End Sub

    'Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
    '    Try
    '        Me.LimpiarCampos()

    '        MultiView1.ActiveViewIndex = 1
    '    Catch ex As Exception
    '        lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
    '    End Try
    'End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            MultiView1.ActiveViewIndex = 0
        Catch ex As Exception
            lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
        End Try
    End Sub

    Protected Sub grillaUsuarios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grillaUsuarios.RowCommand
        Try
            Me.LimpiarCampos()

            Me._vista.ObtenerPorId(grillaUsuarios.Rows(Convert.ToInt32(e.CommandArgument)).Cells(1).Text)
            If Not Me._vista.Usuario Is Nothing Then
                Me.LlenarCampos(Me._vista.Usuario)

                MultiView1.ActiveViewIndex = 1
            Else
                lblMensaje.Text = "Usuario no encontrado"
            End If
        Catch ex As Exception
            lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Try
            Me.LimpiarCampos()
        Catch ex As Exception
            lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Try
            Dim resultado As Boolean = False
            If IsNumeric(lblId.Text) Then
                'modificar
                Me._vista.ObtenerPorId(lblId.Text)
                Me._vista.Usuario = Me.ObtenerCampos(Me._vista.Usuario)
                resultado = Me._vista.usuarioBLL.Modificacion(Me._vista.Usuario)
            Else
                'crear
                Me._vista.Usuario = Me.ObtenerCampos(Nothing)
                resultado = Me._vista.usuarioBLL.Alta(Me._vista.Usuario)
            End If

            If resultado Then
                lblMensaje.Text = "Agregado/Modificado con exito."
                Me.LimpiarCampos()
                Me.LlenarGrilla()
            Else
                lblMensaje.Text = "No se agregó/editó el usuario."
            End If
        Catch ex As Exception
            lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
        End Try
    End Sub

    'Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEliminar.Click
    '    Try
    '        If Me._vista.EliminarPorId(lblId.Text) Then
    '            lblMensaje.Text = "Usuario eliminado con exito"
    '            Me.LimpiarCampos()
    '            Me.LlenarGrilla()
    '        Else
    '            lblMensaje.Text = "Usuario no eliminado"
    '        End If

    '    Catch ex As Exception
    '        lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
    '    End Try
    'End Sub

    'Protected Sub btnRestaurar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRestaurar.Click
    '    Try
    '        If Me._vista.RestaurarPorId(lblId.Text) Then
    '            lblMensaje.Text = "Usuario restaurado con exito"
    '            Me.LimpiarCampos()
    '            Me.LlenarGrilla()
    '        Else
    '            lblMensaje.Text = "Usuario no restaurado"
    '        End If
    '    Catch ex As Exception
    '        lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
    '    End Try
    'End Sub

    Private Sub LimpiarCampos()
        MultiView1.ActiveViewIndex = 0

        lblId.Text = "(nuevo)"
        txtNombre.Text = String.Empty
        txtEmail.Text = String.Empty
        txtClave.Text = String.Empty
        txtReClave.Text = String.Empty
        chkBloqueado.Checked = False

        'btnEliminar.Visible = False
        'btnRestaurar.Visible = False
    End Sub

    Private Sub LlenarGrilla()
        'traer todos los usuarios
        Me._vista.LlenarGrilla(grillaUsuarios)
    End Sub

    Private Sub LlenarCampos(ByVal _usuario As UsuarioDTO)
        lblId.Text = _usuario.Id.ToString()
        txtNombre.Text = _usuario.Nombre
        txtEmail.Text = _usuario.Email
        txtClave.Text = _usuario.Clave
        txtReClave.Text = txtClave.Text
        chkBloqueado.Checked = _usuario.Bloqueado

        'btnEliminar.Visible = Not _usuario.Eliminado
        'btnRestaurar.Visible = _usuario.Eliminado
    End Sub

    Private Function ObtenerCampos(ByVal _usuario As UsuarioDTO) As UsuarioDTO
        If _usuario Is Nothing Then _usuario = New UsuarioDTO

        If IsNumeric(lblId.Text) Then
            _usuario.Id = Convert.ToInt32(lblId.Text)
        End If
        _usuario.Nombre = txtNombre.Text
        _usuario.Email = txtEmail.Text
        If Not String.IsNullOrEmpty(txtClave.Text) Then
            _usuario.Clave = Encrypter.EncriptarSHA512(txtClave.Text)
        End If
        _usuario.Bloqueado = chkBloqueado.Checked
        Return _usuario
    End Function

End Class