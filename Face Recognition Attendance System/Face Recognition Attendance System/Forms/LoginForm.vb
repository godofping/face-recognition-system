Public Class LoginForm
    Dim admin As New Admin


    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        admin._Username = txtUsername.Text
        admin._Password = txtPassword.Text

        Dim dt As DataTable = admin.Login(admin)

        If dt.Rows.Count > 0 Then
            MsgBox("Welcome " & dt.Rows(0)("username"))
        Else
            MsgBox("Login Failed")
        End If

    End Sub
End Class
