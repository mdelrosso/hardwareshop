Imports System.Data
Imports HS.BE
Imports HS.DAL

Public Interface IHardwareDAL
    Inherits IMapeador(Of HardwareDTO), IVerificator(Of HardwareDTO)
End Interface

Public Class HardwareDAL
    Implements IHardwareDAL

    ''' <summary>
    ''' objeto que encapsula la funcionalidad de acceso, persistencia y lectura
    ''' de datos en el origen de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Private _wrapper As IComando = Nothing
    ''' <summary>
    ''' conversor a BE de los datos devueltos por la consulta SQL.
    ''' </summary>
    ''' <remarks></remarks>
    Private _conversor As IConversor(Of BE.HardwareDTO) = Nothing

    Public Function Alta(ByRef value As HardwareDTO) As Boolean Implements IMapeador(Of HardwareDTO).Alta
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("INSERT INTO Hardware VALUES(0, @descripcion,@precio,0,0) SET @identity=@@Identity", CommandType.Text)
        Try
            Me.Wrapper.AgregarParametro(comando, "@descripcion", value.Descripcion)
            Me.Wrapper.AgregarParametro(comando, "@precio", value.Precio)

            Dim paramRet As IDataParameter = Me.Wrapper.AgregarParametro(comando, "@identity", 0, DbType.Int32, ParameterDirection.Output)

            resultado = Me._wrapper.EjecutarConsulta(comando)

            ' asignar el Id devuelto por la consulta al objeto
            If (resultado > 0) Then
                value.Id = CType(paramRet.Value, Integer)

                ' Calculo el nuevo digito horizontal
                value.DigitoHorizontal = CalcularDVH(value)
                Modificacion(value)
                VerificadorDAL.ActualizarDVV("Hardware", "IDHardware")
            End If

        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        ' este metodo retornará true si hubo registros afectados en el origen de datos
        Return (resultado > 0)
    End Function

    Public Function Baja(ByRef value As HardwareDTO) As Boolean Implements IMapeador(Of HardwareDTO).Baja
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("UPDATE Hardware SET Hard_eliminado=@eliminado WHERE Hard_Id=@id", CommandType.Text)
        Try
            ' agregar los parametros necesarios para poder ejecutar la consulta
            Me.Wrapper.AgregarParametro(comando, "@id", value.Id)
            Me.Wrapper.AgregarParametro(comando, "@eliminado", value.Eliminado)

            ' ejecutar el comando/consulta SQL en el origen de datos
            resultado = Me._wrapper.EjecutarConsulta(comando)
        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        ' este metodo retornará true si hubo registros afectados en el origen de datos
        Return (resultado > 0)
    End Function

    Public Function Consulta(ByRef filtro As HardwareDTO) As HardwareDTO Implements IMapeador(Of HardwareDTO).Consulta
        Dim lista As List(Of BE.HardwareDTO) = Me.ConsultaRango(filtro, Nothing)
        If Not lista Is Nothing AndAlso lista.Count > 0 Then
            ' retornar solo el primer objeto que cumpla con el filtro
            Return lista(0)
        Else
            Return Nothing
        End If
    End Function

    Public Function ConsultaRango(ByRef filtroDesde As HardwareDTO, ByRef filtroHasta As HardwareDTO) As List(Of HardwareDTO) Implements IMapeador(Of HardwareDTO).ConsultaRango
        Dim lista As List(Of BE.HardwareDTO) = New List(Of BE.HardwareDTO)

        ' crear el objeto comando que vamos a usar para realizar la accion en el origen de datos (NOTA: se recomienda usar PROCEDIMIENTOS ALMACENADOS)
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("SELECT * FROM Hardware WHERE (Hard_Id=@id OR @id IS NULL)", CommandType.Text)
        Try
            ' agregar los parametros necesarios para poder ejecutar la consulta
            ' solo buscar por Id, si se especifico filtrodesde y el Id en el filtroDesde es mayor que cero
            If Not filtroDesde Is Nothing AndAlso filtroDesde.Id > 0 Then
                Me.Wrapper.AgregarParametro(comando, "@id", filtroDesde.Id)
            Else
                Me.Wrapper.AgregarParametro(comando, "@id", DBNull.Value)
            End If
            Using reader As IDataReader = Me.Wrapper.ConsultarReader(comando)

                Do While reader.Read()
                    lista.Add(Me.Conversor.Convertir(reader))
                Loop

            End Using

        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        ' este metodo retornará la lista con todas las BE convertidas que
        ' se obtuvieron del origen de datos
        Return lista
    End Function

    Public Function Modificacion(ByRef value As HardwareDTO) As Boolean Implements IMapeador(Of HardwareDTO).Modificacion
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("UPDATE Hardware SET Hard_descripcion=@descripcion, Hard_precio=@precio, DIGITOHORIZONTAL=@digitohorizontal, Hard_eliminado=@eliminado WHERE Hard_Id=@id", CommandType.Text)
        Try
            Me.Wrapper.AgregarParametro(comando, "@descripcion", value.Descripcion)
            Me.Wrapper.AgregarParametro(comando, "@precio", value.Precio)
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
        ' este metodo retornará true si hubo registros afectados en el origen de datos
        Return (resultado > 0)
    End Function

    Public Property Conversor As IConversor(Of HardwareDTO) Implements IMapeador(Of HardwareDTO).Conversor
        Get
            If Me._conversor Is Nothing Then
                Me._conversor = New HardwareConversor()
            End If
            Return Me._conversor
        End Get
        Set(ByVal value As IConversor(Of HardwareDTO))
            Me._conversor = value
        End Set
    End Property

    Public Property Wrapper As IComando Implements IMapeador(Of HardwareDTO).Wrapper
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


    Private Function CalcularDVH(ByRef value As HardwareDTO) As Integer Implements IVerificator(Of HardwareDTO).CalcularDVH
        Dim DVH As Integer = 0
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Id), 0)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Descripcion), 1)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Precio), 2)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Eliminado), 3)
        Return DVH
    End Function

    Private Sub ActualizarDVH(value As HardwareDTO) Implements IVerificator(Of HardwareDTO).ActualizarDVH
        value.DigitoHorizontal = CalcularDVH(value)
        Modificacion(value)
    End Sub

    Public Sub ActualizarDVHTabla() Implements IVerificator(Of HardwareDTO).ActualizarDVHTabla
        Dim listaDTO As List(Of HardwareDTO) = ConsultaRango(Nothing, Nothing)
        For Each objDTO As HardwareDTO In listaDTO
            ActualizarDVH(objDTO)
        Next
    End Sub

    Private Function VerificarDVH(value As HardwareDTO) As Boolean Implements IVerificator(Of HardwareDTO).VerificarDVH
        If (value.DigitoHorizontal <> CalcularDVH(value)) Then
            Return False
        End If
        Return True
    End Function

    Public Function VerificarDVHTabla() As Boolean Implements IVerificator(Of HardwareDTO).VerificarDVHTabla
        Dim listaDTO As List(Of HardwareDTO) = ConsultaRango(Nothing, Nothing)
        For Each objDTO As HardwareDTO In listaDTO
            If (Not VerificarDVH(objDTO)) Then
                Throw New Exception("Verificacion Digito Horizontal en tabla Hardware, id:" + CStr(objDTO.Id) + " Fallido")
            End If
        Next
        Return True
    End Function
End Class