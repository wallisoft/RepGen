Imports System.Windows.Forms
Imports System.Data.Odbc
Imports System.IO

Public Class frmLogin

    Private Sub frmLogin_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        Dim Loggedin As Boolean = False
        Dim fname As String = ""
        Dim sname As String = ""
        Dim status As Boolean = False
        Dim password As String = ""
        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader = Nothing


        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()


        SQLcommand.CommandText = "SELECT * " & _
                                " FROM   User " & _
                                " WHERE  Login = '" & txtUserName.Text & "'"

        Try
            SQLreader = SQLcommand.ExecuteReader()
        Catch ex As Exception

        End Try



        If SQLreader.Read() Then

            fname = frmMain.appVars.GetFromDB(SQLreader(3).ToString)
            sname = SQLreader(4).ToString
            password = SQLreader(7).ToString

            frmMain.appVars.User.UserID = SQLreader(0).ToString
            frmMain.appVars.User.UserLevel = SQLreader(1).ToString
            frmMain.appVars.User.Login = SQLreader(2).ToString
            frmMain.appVars.User.Fname = SQLreader(3).ToString
            frmMain.appVars.User.Sname = SQLreader(4).ToString
            frmMain.appVars.User.Title = SQLreader(5).ToString
            frmMain.appVars.User.Quals = SQLreader(6).ToString
            frmMain.appVars.User.Password = SQLreader(7).ToString
            frmMain.appVars.User.sigFile = SQLreader(8).ToString
            frmMain.appVars.User.sigFile1 = SQLreader(9).ToString
            frmMain.appVars.User.supText = SQLreader(10).ToString
        Else
            status = True
        End If

        SQLcommand.Connection.Close()

        If (txtPassword.Text.ToString.Trim = password.ToString.Trim) And status = False Then 'add lasname<>"" check

            frmMain.appVars.RegistrationOK = True
            Me.Close()

        Else
            MessageBox.Show("Invalid Username or Password")
            txtPassword.Text = ""
        End If

    End Sub

  
    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click

        frmMain.appVars.RegistrationOK = False
        Me.Close()

    End Sub

End Class
