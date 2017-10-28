Public Class BackupDTO

    Private _id As Integer
    Public Property Id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Private _fecha As Date
    Public Property Fecha() As Date
        Get
            Return _fecha
        End Get
        Set(ByVal value As Date)
            _fecha = value
        End Set
    End Property

    Private _path As String
    Public Property Path() As String
        Get
            Return _path
        End Get
        Set(ByVal value As String)
            _path = value
        End Set
    End Property


    Private _sizeMB As Double
    Public Property SizeMB() As Double
        Get
            Return _sizeMB
        End Get
        Set(ByVal value As Double)
            _sizeMB = value
        End Set
    End Property


    Private _descripcion As String
    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

    Private _nombre As String
    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property


    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not obj Is Nothing AndAlso TypeOf obj Is BackupDTO Then
            Return CType(obj, BackupDTO).Id.Equals(Me.Id)
        Else
            Return False
        End If
    End Function

    Public Overrides Function ToString() As String
        Return Me.Nombre
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return MyBase.GetHashCode()
    End Function
End Class

