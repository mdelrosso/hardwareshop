Imports HS.BE
Imports HS.BLL

Public Class Hardware
    Inherits System.Web.UI.Page

    Private _vista As HardwareVista = New HardwareVista()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Valido si el usuario posee permiso para acceder a esta página.
            Dim autenticacionVista As AutenticacionVista = New AutenticacionVista()
            Dim usuarioActual = autenticacionVista.UsuarioActual
            If Not autenticacionVista.UsuarioPoseePermiso(usuarioActual, "ADMIN_NEGOCIO") Then
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

    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)

        Dim id As Integer = Int(txtId.Text)
        Dim descripcion As String = txtDescripcion.Text
        dim precio As Double = CDbl(Val(txtPrecio.Text))

        dim hbll as HardwareBLL = new HardwareBLL()
        Dim hardDto As HardwareDTO = New HardwareDTO()
        hardDto.Id = id
        hardDto.Descripcion = descripcion
        hardDto.Precio = precio
        hbll.Modificacion(hardDto)

        Dim message As String = "Producto guardado satisfactoriamente"
        Dim script As String = "<script type='text/javascript'> alert('" + message + "');</script>"

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "AlertBox", script)

        Response.AppendHeader("Refresh", "0;url="+HttpContext.Current.Request.Url.ToString())
    End Sub

    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        txtDescripcion.Text = row.Cells(1).Text
        txtPrecio.Text = row.Cells(2).Text
        txtId.Text = row.Cells(0).Text
        popup.Show()
    End Sub

End Class