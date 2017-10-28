Imports HS.BE
Public Class DBUtils

    Public Shared Function GetPropertyValue(ByVal obj As Object, ByVal PropName As String) As Object
        Dim objType As Type = obj.GetType()
        Dim pInfo As System.Reflection.PropertyInfo = objType.GetProperty(PropName)
        Dim PropValue As Object = pInfo.GetValue(obj, Reflection.BindingFlags.GetProperty, Nothing, Nothing, Nothing)
        Return PropValue
    End Function

    Public Shared Function CalcularDigitoVerificador(ByRef sCadena As String, Optional startIndex As Integer = 0) As Integer
        Dim i As Integer = 1
        Dim DV As Integer = startIndex + 1
        For Each sChar As Char In sCadena
            DV += (AscW(sChar)) * i
            i += 1
        Next
        Return DV
    End Function

End Class

