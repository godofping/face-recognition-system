Imports System.Data.SqlClient

Public Class Admin
    Dim adminid As Integer
    Dim username As String
    Dim password As String

    Public Property _Adminid As Integer
        Get
            Return adminid
        End Get
        Set(value As Integer)
            adminid = value
        End Set
    End Property

    Public Property _Username As String
        Get
            Return username
        End Get
        Set(value As String)
            username = value
        End Set
    End Property

    Public Property _Password As String
        Get
            Return password
        End Get
        Set(value As String)
            password = value
        End Set
    End Property

    Public Function Login(ByVal admin As Admin) As DataTable
        Using cmd = New SqlCommand()
            cmd.CommandText = "select * from admin where username = @username and password = @password"
            cmd.Parameters.AddWithValue("@username", admin._Username)
            cmd.Parameters.AddWithValue("@password", admin._Password)
            Return Helper.executeQuery(cmd)
        End Using
    End Function
End Class
