Public Class VerificadorDTO
    Implements IVerificable

    Private _id As Integer
    Public Property Id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Private _tablanombre As String
    Public Property TablaNombre() As String
        Get
            Return _tablanombre
        End Get
        Set(ByVal value As String)
            _tablanombre = value
        End Set
    End Property


    Private _digitohorizontal As Integer
    Public Property DigitoHorizontal() As Integer Implements IVerificable.DigitoHorizontal
        Get
            Return _digitohorizontal
        End Get
        Set(ByVal value As Integer)
            _digitohorizontal = value
        End Set
    End Property

    Private _digitovertical As Integer
    Public Property DigitoVertical() As Integer
        Get
            Return _digitovertical
        End Get
        Set(ByVal value As Integer)
            _digitovertical = value
        End Set
    End Property

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not obj Is Nothing AndAlso TypeOf obj Is BitacoraDTO Then
            Return CType(obj, BitacoraDTO).Id.Equals(Me.Id)
        Else
            Return False
        End If
    End Function

    Public Overrides Function ToString() As String
        Return Me.Id.ToString
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return MyBase.GetHashCode()
    End Function

End Class
