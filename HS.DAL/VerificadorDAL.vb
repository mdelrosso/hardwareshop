Imports System.Data
Imports HS.BE
Public Interface IVerificadorDAL
    Inherits IMapeador(Of VerificadorDTO), IVerificator(Of VerificadorDTO)

End Interface
Public Class VerificadorDAL
    Implements IVerificadorDAL

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
    Private _conversor As IConversor(Of BE.VerificadorDTO) = Nothing
    Public Function Alta(ByRef value As VerificadorDTO) As Boolean Implements IMapeador(Of VerificadorDTO).Alta
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("INSERT INTO DIGITOVERIFICADOR VALUES(@digitovertical,@tablanombre, 0) SET @identity=@@Identity", CommandType.Text)
        Try
            Me.Wrapper.AgregarParametro(comando, "@tablanombre", value.TablaNombre)
            ' Calculo el nuevo digito horizontal
            Me.Wrapper.AgregarParametro(comando, "@digitovertical", value.DigitoVertical)
            Dim paramRet As IDataParameter = Me.Wrapper.AgregarParametro(comando, "@identity", 0, DbType.Int32, ParameterDirection.Output)

            resultado = Me._wrapper.EjecutarConsulta(comando)

            ' asignar el Id devuelto por la consulta al objeto
            If (resultado > 0) Then
                value.Id = CType(paramRet.Value, Integer)

                ' Calculo el nuevo digito horizontal
                value.DigitoHorizontal = CalcularDVH(value)
                Modificacion(value)

                ' CASO PARTICULAR PARA TDIGITOVERIFICAR. Una vez que inserto el nuevo registro, tengo que modificar el campo DIGITOVERTICAL del registro propio en la tabla 
                ActualizarDVVPropio()
            End If

        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return (resultado > 0)
    End Function

    Public Function Baja(ByRef value As VerificadorDTO) As Boolean Implements IMapeador(Of VerificadorDTO).Baja
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("DELETE FROM DIGITOVERIFICADOR WHERE iddigitoverificador=@id", CommandType.Text)
        Try
            Me.Wrapper.AgregarParametro(comando, "@id", value.Id)

            resultado = Me._wrapper.EjecutarConsulta(comando)

            If (resultado > 0) Then

                ' CASO PARTICULAR PARA TDIGITOVERIFICAR. Una vez que borro un registro, tengo que modificar el campo DIGITOVERTICAL del registro propio en la tabla 
                ActualizarDVVPropio()
            End If
        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return (resultado > 0)
    End Function

    Public Function Consulta(ByRef filtro As VerificadorDTO) As VerificadorDTO Implements IMapeador(Of VerificadorDTO).Consulta
        Dim lista As List(Of BE.VerificadorDTO) = Me.ConsultaRango(filtro, Nothing)
        If Not lista Is Nothing AndAlso lista.Count > 0 Then
            ' retornar solo el primer objeto que cumpla con el filtro
            Return lista(0)
        Else
            Return Nothing
        End If
    End Function

    Public Function ConsultaRango(ByRef filtroDesde As VerificadorDTO, ByRef filtroHasta As VerificadorDTO) As List(Of VerificadorDTO) Implements IMapeador(Of VerificadorDTO).ConsultaRango
        Dim lista As List(Of BE.VerificadorDTO) = New List(Of BE.VerificadorDTO)

        Dim comando As IDbCommand = Me.Wrapper.CrearComando("SELECT * FROM DIGITOVERIFICADOR WHERE (iddigitoverificador=@id OR @id IS NULL) AND (tablanombre=@tablanombre OR @tablanombre IS NULL)", CommandType.Text)
        Try
            If Not filtroDesde Is Nothing AndAlso filtroDesde.Id > 0 Then
                Me.Wrapper.AgregarParametro(comando, "@id", filtroDesde.Id)
            Else
                Me.Wrapper.AgregarParametro(comando, "@id", DBNull.Value)
            End If

            If Not filtroDesde Is Nothing AndAlso Not String.IsNullOrWhiteSpace(filtroDesde.TablaNombre) Then
                Me.Wrapper.AgregarParametro(comando, "@tablanombre", filtroDesde.TablaNombre)
            Else
                Me.Wrapper.AgregarParametro(comando, "@tablanombre", DBNull.Value)
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
        Return lista
    End Function

    Public Function Modificacion(ByRef value As VerificadorDTO) As Boolean Implements IMapeador(Of VerificadorDTO).Modificacion
        Dim resultado As Integer = 0
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("UPDATE DIGITOVERIFICADOR SET DIGITOVERTICAL=@digitovertical, DIGITOHORIZONTAL=@digitohorizontal, TABLANOMBRE=@nombretabla WHERE iddigitoverificador=@id", CommandType.Text)
        Try
            Me.Wrapper.AgregarParametro(comando, "@nombretabla", value.TablaNombre)
            Me.Wrapper.AgregarParametro(comando, "@digitovertical", value.DigitoVertical)
            Me.Wrapper.AgregarParametro(comando, "@id", value.Id)

            ' Calculo el nuevo digito horizontal
            value.DigitoHorizontal = CalcularDVH(value)
            Me.Wrapper.AgregarParametro(comando, "@digitohorizontal", value.DigitoHorizontal)

            ' ejecutar el comando/consulta SQL en el origen de datos
            resultado = Me._wrapper.EjecutarConsulta(comando)

        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return (resultado > 0)
    End Function

    Public Property Conversor As IConversor(Of VerificadorDTO) Implements IMapeador(Of VerificadorDTO).Conversor
        Get
            If Me._conversor Is Nothing Then
                Me._conversor = New VerificadorConversor()
            End If
            Return Me._conversor
        End Get
        Set(ByVal value As IConversor(Of VerificadorDTO))
            Me._conversor = value
        End Set
    End Property

    Public Property Wrapper As IComando Implements IMapeador(Of VerificadorDTO).Wrapper
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

    Private Sub ActualizarDVH(value As VerificadorDTO) Implements IVerificator(Of VerificadorDTO).ActualizarDVH
        value.DigitoHorizontal = CalcularDVH(value)
        Modificacion(value)
    End Sub

    Public Sub ActualizarDVHTabla() Implements IVerificator(Of VerificadorDTO).ActualizarDVHTabla
        Dim listaDTO As List(Of VerificadorDTO) = ConsultaRango(Nothing, Nothing)
        For Each objDTO As VerificadorDTO In listaDTO
            ActualizarDVH(objDTO)
        Next
    End Sub

    Private Function CalcularDVH(ByRef value As VerificadorDTO) As Integer Implements IVerificator(Of VerificadorDTO).CalcularDVH
        Dim DVH As Integer = 0
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.Id), 0)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.DigitoVertical), 1)
        DVH += DBUtils.CalcularDigitoVerificador(CStr(value.TablaNombre), 2)
        Return DVH
    End Function

    Private Function VerificarDVH(value As VerificadorDTO) As Boolean Implements IVerificator(Of VerificadorDTO).VerificarDVH
        If (value.DigitoHorizontal <> CalcularDVH(value)) Then
            Return False
        End If
        Return True
    End Function

    Public Function VerificarDVHTabla() As Boolean Implements IVerificator(Of VerificadorDTO).VerificarDVHTabla
        Dim listaDTO As List(Of VerificadorDTO) = ConsultaRango(Nothing, Nothing)
        For Each objDTO As VerificadorDTO In listaDTO
            If (Not VerificarDVH(objDTO)) Then
                Throw New Exception("Verificacion Digito Horizontal en tabla DIGITOVERIFICADOR, id:" + CStr(objDTO.Id) + " Fallido")
            End If
        Next
        Return True
    End Function

    Private Sub ActualizarDVVPropio()
        ActualizarDVV("DIGITOVERIFICADOR", "IDDIGITOVERIFICADOR")
    End Sub
    Public Shared Sub ActualizarDVV(sNombreTabla As String, nombreCampoId As String)
        Dim propioObj As VerificadorDTO = New VerificadorDTO()
        propioObj.TablaNombre = sNombreTabla
        Dim objDAO As VerificadorDAL = New VerificadorDAL
        propioObj = objDAO.Consulta(propioObj)
        If (propioObj Is Nothing) Then
            propioObj = New VerificadorDTO()
            propioObj.TablaNombre = sNombreTabla
            propioObj.DigitoVertical = objDAO.CalcularDVV(sNombreTabla, nombreCampoId)
            objDAO.Alta(propioObj)
        Else
            propioObj.DigitoVertical = objDAO.CalcularDVV(sNombreTabla, nombreCampoId)
            objDAO.Modificacion(propioObj)
        End If
    End Sub

    Private Function CalcularDVV(nombreTabla As String, nombreCampoId As String) As Integer
        Dim lista As List(Of Integer) = New List(Of Integer)
        Dim comando As IDbCommand = Me.Wrapper.CrearComando("select " + nombreCampoId + " FROM " + nombreTabla, CommandType.Text)
        Dim DVV As Integer = 0

        Try
            Using reader As IDataReader = Me.Wrapper.ConsultarReader(comando)
                Dim i As Integer = 0
                Do While reader.Read()
                    Dim id As Integer = (Convert.ToInt32(reader(nombreCampoId)))
                    DVV += DBUtils.CalcularDigitoVerificador(CStr(id), i)
                    i += 1
                Loop

            End Using
        Catch
            Throw
        Finally
            Me.Wrapper.CerrarConexion(comando)
        End Try
        Return DVV
    End Function

    Public Function VerificarDVV(sNombreTabla As String, nombreCampoId As String) As Boolean
        Dim propioObj As VerificadorDTO = New VerificadorDTO()
        propioObj.TablaNombre = sNombreTabla
        propioObj = Consulta(propioObj)

        If (propioObj Is Nothing) Then
            Return False
        ElseIf propioObj.DigitoVertical <> CalcularDVV(sNombreTabla, nombreCampoId) Then
            Throw New Exception("Verificacion Digito VERTICAL en tabla " + sNombreTabla + " Fallido")
        End If

        Return True
    End Function

End Class
