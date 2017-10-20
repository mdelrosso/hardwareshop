﻿Imports System.Data
Imports HS.BE

Public Interface IFamiliaPermisoDAL
    Inherits IMapeador(Of PermisoDTO)

    ''' <summary>
    ''' Permiso padre de todos los permisos que se van a agregar, borrar o consultar.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property PermisoPadre As PermisoDTO
End Interface

Public Class FamiliaPermisoDAL
    Implements IFamiliaPermisoDAL


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
    Private _conversor As IConversor(Of BE.PermisoDTO) = Nothing
    ''' <summary>
    ''' Permiso padre de todos los pemisos que se van a agregar, borrar o consultar
    ''' </summary>
    ''' <remarks></remarks>
    Private _permisoPadre As PermisoDTO = Nothing

    Public Property Conversor As IConversor(Of BE.PermisoDTO) Implements IFamiliaPermisoDAL.Conversor
        Get
            If Me._conversor Is Nothing Then Me._conversor = New PermisoConversor()
            Return Me._conversor
        End Get
        Set(ByVal value As IConversor(Of BE.PermisoDTO))
            Me._conversor = value
        End Set
    End Property

    Public Property Wrapper As IComando Implements IFamiliaPermisoDAL.Wrapper
        Get
            If Me._wrapper Is Nothing Then
                ' obtener el wrapper por defecto
                Me._wrapper = ComandoFactory.CrearComando("Default")
            End If
            Return Me._wrapper
        End Get
        Set(ByVal value As IComando)
            Me._wrapper = value
        End Set
    End Property

    ''' <summary>
    ''' Permiso padre de todos los permisos que se van a agregar, borrar o consultar.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PermisoPadre As PermisoDTO Implements IFamiliaPermisoDAL.PermisoPadre
        Get
            If Me._permisoPadre Is Nothing Then Throw New ArgumentNullException("No se especificó el permiso padre.")
            Return Me._permisoPadre
        End Get
        Set(ByVal value As PermisoDTO)
            Me._permisoPadre = value
        End Set
    End Property

    Public Function Alta(ByRef value As BE.PermisoDTO) As Boolean Implements IFamiliaPermisoDAL.Alta
        Dim resultado As Integer = 0
        ' crear el objeto comando que vamos a usar para realizar la accion en el origen de datos (NOTA: se recomienda usar PROCEDIMIENTOS ALMACENADOS)
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("HS_PERMISO_FAMILIA_AGREGAR", CommandType.StoredProcedure)
        Try
            ' agregar los parametros necesarios para poder ejecutar la consulta
            Me.Wrapper.AgregarParametro(comando, "@padreId", Me.PermisoPadre.Id)
            Me.Wrapper.AgregarParametro(comando, "@hijoId", value.Id)

            ' ejecutar el comando/consulta SQL en el origen de datos
            resultado = Me._wrapper.EjecutarConsulta(comando)
        Catch
            ' si se produjo una excepcion, devolverla al objeto que invocó al metodo (sin encapsularla)
            Throw
        Finally
            ' independientemente de si la accion fue exitosa o se produzco una excepcion
            ' cerrar la conexion
            Me.Wrapper.CerrarConexion(comando)
        End Try
        ' este metodo retornará true si hubo registros afectados en el origen de datos
        Return (resultado > 0)
    End Function

    Public Function Baja(ByRef value As BE.PermisoDTO) As Boolean Implements IFamiliaPermisoDAL.Baja
        Dim resultado As Integer = 0
        ' crear el objeto comando que vamos a usar para realizar la accion en el origen de datos (NOTA: se recomienda usar PROCEDIMIENTOS ALMACENADOS)
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("HS_PERMISO_FAMILIA_ELIMINAR", CommandType.StoredProcedure)
        Try
            ' agregar los parametros necesarios para poder ejecutar la consulta
            Me.Wrapper.AgregarParametro(comando, "@padreId", Me.PermisoPadre.Id)
            Me.Wrapper.AgregarParametro(comando, "@hijoId", value.Id)

            ' ejecutar el comando/consulta SQL en el origen de datos
            resultado = Me._wrapper.EjecutarConsulta(comando)
        Catch
            ' si se produjo una excepcion, devolverla al objeto que invocó al metodo (sin encapsularla)
            Throw
        Finally
            ' independientemente de si la accion fue exitosa o se produzco una excepcion
            ' cerrar la conexion
            Me.Wrapper.CerrarConexion(comando)
        End Try
        ' este metodo retornará true si hubo registros afectados en el origen de datos
        Return (resultado > 0)

    End Function

    Public Function Consulta(ByRef filtro As BE.PermisoDTO) As BE.PermisoDTO Implements IMapeador(Of BE.PermisoDTO).Consulta
        Dim lista As List(Of PermisoDTO) = Me.ConsultaRango(filtro, Nothing)
        If lista.Count > 0 Then
            ' retornar solo el primer objeto que cumpla con el filtro
            Return lista(0)
        Else
            Return Nothing
        End If
    End Function

    Public Function ConsultaRango(ByRef filtroDesde As BE.PermisoDTO, ByRef filtroHasta As BE.PermisoDTO) As System.Collections.Generic.List(Of BE.PermisoDTO) Implements IFamiliaPermisoDAL.ConsultaRango
        Dim lista As List(Of BE.PermisoDTO) = New List(Of BE.PermisoDTO)

        ' crear el objeto comando que vamos a usar para realizar la accion en el origen de datos (NOTA: se recomienda usar PROCEDIMIENTOS ALMACENADOS)
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("HS_PERMISO_FAMILIA_LISTAR", CommandType.StoredProcedure)
        Try
            ' agregar los parametros necesarios para poder ejecutar la consulta

            If Not filtroHasta Is Nothing AndAlso filtroHasta.Id > 0 Then
                ' buscar los padres del hijo especificado
                Me.Wrapper.AgregarParametro(comando, "@padreid", DBNull.Value)
                Me.Wrapper.AgregarParametro(comando, "@hijoId", filtroHasta.Id)
            Else
                ' siempre buscar por el Id del padre
                Me.Wrapper.AgregarParametro(comando, "@padreid", Me.PermisoPadre.Id)
                ' solo buscar por Id, si se especifico filtrodesde y el Id en el filtroDesde es mayor que cero
                If Not filtroDesde Is Nothing AndAlso filtroDesde.Id > 0 Then
                    Me.Wrapper.AgregarParametro(comando, "@hijoId", filtroDesde.Id)
                Else
                    Me.Wrapper.AgregarParametro(comando, "@hijoId", DBNull.Value)
                End If
            End If

            ' ejecutar el comando/consulta SQL en el origen de datos
            ' la instruccion Usuing nos garantiza que el objeto reader va a ser cerrado luego de ser consumido
            Using reader As IDataReader = Me.Wrapper.ConsultarReader(comando)

                ' recorrer el IDataReader obtenido de la base de datos y convertirlo a un objeto entidad
                Do While reader.Read()
                    ' delegarle la responsabilidad de convertir un IDataReader al objeto Conversor
                    lista.Add(Me.Conversor.Convertir(reader))
                Loop

            End Using

        Catch
            ' si se produjo una excepcion, devolverla al objeto que invocó al metodo (sin encapsularla)
            Throw
        Finally
            ' independientemente de si la accion fue exitosa o se produzco una excepcion
            ' cerrar la conexion
            Me.Wrapper.CerrarConexion(comando)
        End Try

        ' este metodo retornará la lista con todas las entidades convertidas que
        ' se obtuvieron del origen de datos
        Return lista
    End Function

    Public Function Modificacion(ByRef value As BE.PermisoDTO) As Boolean Implements IFamiliaPermisoDAL.Modificacion
        Throw New NotImplementedException("No se puede realizar una modificacion para el permiso.")
    End Function


End Class