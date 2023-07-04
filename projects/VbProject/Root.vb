Public Class Root

    Public Property Id As Qowaiv.Uuid

    Public Shared Function Answer() As Integer
        Return New MathNet.Numerics.Random.MersenneTwister().Next()
    End Function

End Class
