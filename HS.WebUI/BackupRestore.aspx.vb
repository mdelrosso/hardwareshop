Imports HS.BE
Imports HS.BLL

Public Class BackupRestore
    Inherits System.Web.UI.Page

    Private _vistaUsuarios As UsuarioVista = New UsuarioVista()
    Private _vistaPermisos As PermisoVista = New PermisoVista()

    Private _vista As BitacoraVista = New BitacoraVista()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Valido si el usuario posee permiso para acceder a esta página.
            Dim autenticacionVista As AutenticacionVista = New AutenticacionVista()
            Dim usuarioActual = autenticacionVista.UsuarioActual
            If Not autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMINISTRACION") Then
                'Si no lo tiene se redirecciona a página de inicio.
                Me.Response.Redirect("~/Default.aspx")
            End If

            lblMensaje.Text = String.Empty
            If Not Page.IsPostBack Then
                Me.llenarGrilla()
            End If

        Catch ex As Exception
            lblMensaje.Text = ErrorHandler.ObtenerMensajeDeError(ex)
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try
    End Sub

    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Me.Response.Redirect("~/Default.aspx")
    End Sub


    Protected Sub btnRealizarBackup_Click(sender As Object, e As EventArgs) Handles btnRealizarBackup.Click
        If String.IsNullOrWhiteSpace(txtRutaBackup.Text) Then
            lblMensaje.Text = "Debe especificar una ruta destino para el backup"
            lblMensaje.ForeColor = Drawing.Color.Red
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtNombreBackup.Text) Then
            txtNombreBackup.Text = Date.Now.ToString
        End If


        Dim mBackup As BackupBLL = New BackupBLL()
        Dim tamFragment As Integer = -1
        If (Not String.IsNullOrWhiteSpace(txtTamanoBackup.Text)) Then
            tamFragment = CInt(txtTamanoBackup.Text)
        End If

        Try
            If (mBackup.RealizarBackup(txtNombreBackup.Text, txtRutaBackup.Text, tamFragment)) Then
                lblMensaje.Text = "Backup realizado con exito"
                lblMensaje.ForeColor = Drawing.Color.Blue
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error: " + ErrorHandler.ObtenerMensajeDeError(ex)
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try
        llenarGrilla()


    End Sub

    Private Sub llenarGrilla()
        Dim mBackup As BackupBLL = New BackupBLL
        grillaBackups.DataSource = mBackup.BackupLista()
        grillaBackups.DataBind()
    End Sub


    Protected Sub btnRealizarRestore_Click(sender As Object, e As EventArgs) Handles btnRealizarRestore.Click
        If String.IsNullOrWhiteSpace(txtBackupId.Text) Then
            lblMensaje.Text = "Debe especificar una grupo de backup a restaurar"
            lblMensaje.ForeColor = Drawing.Color.Red
            Exit Sub
        End If

        Dim mBackup As BackupBLL = New BackupBLL
        Dim lista As List(Of BackupDTO) = mBackup.BackupLista()
        Dim listaBackups = New List(Of String)
        For Each objDTO As BackupDTO In lista
            If (objDTO.Id = CInt(txtBackupId.Text)) Then
                listaBackups.Add(objDTO.Path)
            End If
        Next

        If (listaBackups.Count = 0) Then
            lblMensaje.Text = "Grupo de backup no existente. Por favor seleccione otro"
            lblMensaje.ForeColor = Drawing.Color.Red
            Exit Sub
        End If

        Dim cantFrag As Integer = 1
        Try
            If (mBackup.RealizarRestore(listaBackups)) Then
                lblMensaje.Text = "Restore realizado con exito"
                lblMensaje.ForeColor = Drawing.Color.Blue
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error: " + ErrorHandler.ObtenerMensajeDeError(ex)
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try

        llenarGrilla()

    End Sub
End Class