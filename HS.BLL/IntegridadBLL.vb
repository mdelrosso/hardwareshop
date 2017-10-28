Imports HS.DAL
'Imports Lotenline.Web
Public Class IntegridadBLL
    Private Shared Function ObtenerMensajeDeError(ByVal ex As Exception) As String
        Dim mensaje As String = String.Format("<br /><small>{0}</small>", ex.Message)
        Return mensaje
    End Function

    Public Shared Function VerificarIntegridadBD() As String
        Dim mUsuario As UsuarioDAL = New UsuarioDAL
        Dim mVerificador As VerificadorDAL = New VerificadorDAL
        Dim mBitacora As BitacoraDAL = New BitacoraDAL
        Dim sMsg As String = Nothing

        Try
            mVerificador.VerificarDVV("USUARIO", "USU_ID")
        Catch ex As Exception
            sMsg += ObtenerMensajeDeError(ex)
        End Try

        Try
            mVerificador.VerificarDVV("BITACORA", "IDBITACORA")
        Catch ex As Exception
            sMsg += ObtenerMensajeDeError(ex)
        End Try

        Try
            mVerificador.VerificarDVV("DIGITOVERIFICADOR", "IDDIGITOVERIFICADOR")
        Catch ex As Exception
            sMsg += ObtenerMensajeDeError(ex)
        End Try

        Try
            mUsuario.VerificarDVHTabla()
        Catch ex As Exception
            sMsg += ObtenerMensajeDeError(ex)
        End Try

        Try
            mBitacora.VerificarDVHTabla()
        Catch ex As Exception
            sMsg += ObtenerMensajeDeError(ex)
        End Try

        Try
            mVerificador.VerificarDVHTabla()
        Catch ex As Exception
            sMsg += ObtenerMensajeDeError(ex)
        End Try

        Return sMsg
    End Function

    Public Shared Sub RegenerarDigitosVerificadores()
        Dim mUsuario As UsuarioDAL = New UsuarioDAL
        Dim mBitacora As BitacoraDAL = New BitacoraDAL
        Dim mVerificador As VerificadorDAL = New VerificadorDAL

        Try
            mUsuario.ActualizarDVHTabla()
            mBitacora.ActualizarDVHTabla()
            mVerificador.ActualizarDVHTabla()

            VerificadorDAL.ActualizarDVV("USUARIO", "USU_ID")
            VerificadorDAL.ActualizarDVV("BITACORA", "IDBITACORA")
            VerificadorDAL.ActualizarDVV("DIGITOVERIFICADOR", "IDDIGITOVERIFICADOR")
        Catch ex As Exception
            Throw
        End Try

    End Sub
End Class
