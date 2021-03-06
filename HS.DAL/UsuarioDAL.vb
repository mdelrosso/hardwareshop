﻿Imports System.Data
Imports HS.DAL
Imports HS.BE

Public Interface IUsuarioDAL
    Inherits IMapeador(Of UsuarioDTO), IVerificator(Of UsuarioDTO)
    Function BloquearCuenta(ByRef value As BE.UsuarioDTO) As Boolean
End Interface

Public Class UsuarioDAL
    Implements IUsuarioDAL
    ''' <summary>
    ''' objeto que encapsula la funcionalidad de acceso, persistencia y lectura
    ''' de datos en el origen de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Private _wrapper As IComando = Nothing
    ''' <summary>
    ''' conversor a entidades de los datos devueltos por la consulta SQL.
    ''' </summary>
    ''' <remarks></remarks>
    Private _conversor As IConversor(Of BE.UsuarioDTO) = Nothing
    Public Function Alta(ByRef value As BE.UsuarioDTO) As Boolean Implements IUsuarioDAL.Alta
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("INSERT INTO USUARIO VALUES(@nombre, @clave, @email, @bloqueado, 0,0) SET @identity=@@Identity", CommandType.Text)
        Try
            Me.Wrapper.AgregarParametro(comando, "@nombre", value.Nombre)
            Me.Wrapper.AgregarParametro(comando, "@clave", value.Clave)
            Me.Wrapper.AgregarParametro(comando, "@email", value.Email)
            Me.Wrapper.AgregarParametro(comando, "@bloqueado", value.Bloqueado)

            Dim paramRet As IDataParameter = Me.Wrapper.AgregarParametro(comando, "@identity", 0, DbType.Int32, ParameterDirection.Output)

            resultado = Me._wrapper.EjecutarConsulta(comando)

            ' asignar el Id devuelto por la consulta al objeto
            If (resultado > 0) Then
                value.Id = CType(paramRet.Value, Integer)

                ' Calculo el nuevo digito horizontal
                value.DigitoHorizontal = CalcularDVH(value)
                Modificacion(value)
                VerificadorDAL.ActualizarDVV("USUARIO", "USU_ID")
            End If

        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
       Return (resultado > 0)
    End Function

    Public Function Baja(ByRef value As BE.UsuarioDTO) As Boolean Implements IUsuarioDAL.Baja
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("UPDATE USUARIO SET Usu_Eliminado=@eliminado WHERE Usu_Id=@id", CommandType.Text)
        Try
            Me.Wrapper.AgregarParametro(comando, "@id", value.Id)
            Me.Wrapper.AgregarParametro(comando, "@eliminado", value.Eliminado)

            resultado = Me._wrapper.EjecutarConsulta(comando)
        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return (resultado > 0)
    End Function

    Public Function Consulta(ByRef filtro As BE.UsuarioDTO) As BE.UsuarioDTO Implements IUsuarioDAL.Consulta
        Dim lista As List(Of BE.UsuarioDTO) = Me.ConsultaRango(filtro, Nothing)
        If Not lista Is Nothing AndAlso lista.Count > 0 Then
            Return lista(0)
        Else
            Return Nothing
        End If
    End Function

    Public Function BloquearCuenta(ByRef value As BE.UsuarioDTO) As Boolean Implements IUsuarioDAL.BloquearCuenta
        value.Bloqueado = True
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("UPDATE USUARIO SET Usu_Bloqueado=@bloqueado WHERE Usu_Id=@id", CommandType.Text)
        Try
            Me.Wrapper.AgregarParametro(comando, "@id", value.Id)
            Me.Wrapper.AgregarParametro(comando, "@bloqueado", value.Eliminado)
            resultado = Me._wrapper.EjecutarConsulta(comando)
        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return (resultado > 0)
    End Function

    Public Function ConsultaRango(ByRef filtroDesde As BE.UsuarioDTO, ByRef filtroHasta As BE.UsuarioDTO) As System.Collections.Generic.List(Of BE.UsuarioDTO) Implements IUsuarioDAL.ConsultaRango
        Dim lista As List(Of BE.UsuarioDTO) = New List(Of BE.UsuarioDTO)

        Dim comando As IDbCommand = Me.Wrapper.CrearComando("SELECT * FROM USUARIO WHERE (Usu_nombre=@nombre OR @nombre IS NULL) AND (Usu_Id=@id OR @id IS NULL) ORDER BY Usu_Nombre", CommandType.Text)
        Try
           If Not filtroDesde Is Nothing AndAlso filtroDesde.Id > 0 Then
                Me.Wrapper.AgregarParametro(comando, "@id", filtroDesde.Id)
            Else
                Me.Wrapper.AgregarParametro(comando, "@id", DBNull.Value)
            End If
            If Not filtroDesde Is Nothing AndAlso Not String.IsNullOrEmpty(filtroDesde.Nombre) Then
                Me.Wrapper.AgregarParametro(comando, "@nombre", filtroDesde.Nombre)
            Else
                Me.Wrapper.AgregarParametro(comando, "@nombre", DBNull.Value)
            End If
            Using reader As IDataReader = Me.Wrapper.ConsultarReader(comando)
                ' recorrer el IDataReader obtenido de la base de datos y convertirlo a un objeto entidad
                Do While reader.Read()
                    lista.Add(Me.Conversor.Convertir(reader))
                Loop
            End Using
        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return lista
    End Function

    Public Function Modificacion(ByRef value As BE.UsuarioDTO) As Boolean Implements IUsuarioDAL.Modificacion
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("UPDATE USUARIO SET Usu_Nombre=@nombre, DIGITOHORIZONTAL=@digitohorizontal, Usu_Clave=@clave, Usu_Email=@email, Usu_Bloqueado=@bloqueado, Usu_Eliminado=@eliminado WHERE Usu_Id=@id", CommandType.Text)
        Try
            Me.Wrapper.AgregarParametro(comando, "@nombre", value.Nombre)
            Me.Wrapper.AgregarParametro(comando, "@clave", value.Clave)
            Me.Wrapper.AgregarParametro(comando, "@email", value.Email)
            Me.Wrapper.AgregarParametro(comando, "@bloqueado", value.Bloqueado)
            Me.Wrapper.AgregarParametro(comando, "@eliminado", value.Eliminado)
            Me.Wrapper.AgregarParametro(comando, "@id", value.Id)

            value.DigitoHorizontal = CalcularDVH(value)
            Me.Wrapper.AgregarParametro(comando, "@digitohorizontal", value.DigitoHorizontal)

            resultado = Me._wrapper.EjecutarConsulta(comando)

        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return (resultado > 0)
    End Function

    Public Property Conversor As IConversor(Of BE.UsuarioDTO) Implements IUsuarioDAL.Conversor
        Get
            If Me._conversor Is Nothing Then
                Me._conversor = New UsuarioConversor()
            End If
            Return Me._conversor
        End Get
        Set(ByVal value As IConversor(Of BE.UsuarioDTO))
            Me._conversor = value
        End Set
    End Property

    Public Property Wrapper As IComando Implements IUsuarioDAL.Wrapper
        Get
            If Me._wrapper Is Nothing Then
                Me._wrapper = ComandoFactory.CrearComando("Default")
            End If
            Return Me._wrapper
        End Get
        Set(ByVal value As IComando)
            Me._wrapper = value
        End Set
    End Property

    Private Sub ActualizarDVH(value As UsuarioDTO) Implements IVerificator(Of UsuarioDTO).ActualizarDVH
        value.DigitoHorizontal = CalcularDVH(value)
        Modificacion(value)
    End Sub

    Public Sub ActualizarDVHTabla() Implements IVerificator(Of UsuarioDTO).ActualizarDVHTabla
        Dim listaDTO As List(Of UsuarioDTO) = ConsultaRango(Nothing, Nothing)
        For Each objDTO As UsuarioDTO In listaDTO
            ActualizarDVH(objDTO)
        Next

    End Sub

    Private Function CalcularDVH(ByRef value As UsuarioDTO) As Integer Implements IVerificator(Of UsuarioDTO).CalcularDVH
        Dim DVH As Integer = 0
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Id), 0)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Nombre), 1)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Clave), 2)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Email), 3)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Bloqueado), 4)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Eliminado), 5)
        Return DVH
    End Function

    Private Function VerificarDVH(value As UsuarioDTO) As Boolean Implements IVerificator(Of UsuarioDTO).VerificarDVH
        If (value.DigitoHorizontal <> CalcularDVH(value)) Then
            Return False
        End If
        Return True
    End Function

    Public Function VerificarDVHTabla() As Boolean Implements IVerificator(Of UsuarioDTO).VerificarDVHTabla
        Dim listaDTO As List(Of UsuarioDTO) = ConsultaRango(Nothing, Nothing)
        For Each objDTO As UsuarioDTO In listaDTO
            If (Not VerificarDVH(objDTO)) Then
                Throw New Exception("Verificacion Digito Horizontal en tabla USUARIO, id:" + CStr(objDTO.Id) + " Fallido")
            End If
        Next
        Return True
    End Function
End Class
