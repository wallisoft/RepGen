Imports System.Data
Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions


Public Class frmUserMaint

    Public UserID As Integer
    Public sigFile As String = ""
    Public sigFile1 As String = ""

    Private Sub frmUserMaint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ClearForm()

    End Sub

    Private Sub ClearForm()

        cmbLevel.Text = ""
        cmbLogin.Text = ""
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtNewLogin.Text = ""
        txtPassword.Text = ""
        txtQuals.Text = ""
        txtTitle.Text = ""
        UserID = -1

        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing

        TextBox1.Text = ""

        LoadUsers()

    End Sub


    Private Sub LoadUsers()

        Dim Count As Integer = 0

        Dim SQLcommand As OdbcCommand

        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()


        SQLcommand.CommandText = "SELECT Login " & _
                                " FROM   User "


        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        cmbLogin.Items.Clear()
        Count = 0

        While SQLreader.Read()
            cmbLogin.Items.Add(SQLreader(0))
            Count += 1
        End While

        SQLreader.Close()

        SQLcommand.Connection.Close()


    End Sub


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub cmbLogin_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLogin.SelectedIndexChanged

        Dim Count As Integer = 0

        Dim SQLcommand As OdbcCommand

        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()


        SQLcommand.CommandText = "SELECT * " & _
                                " FROM   User " & _
                                " WHERE  Login = '" & cmbLogin.Text & "'"


        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        SQLreader.Read()

        UserID = SQLreader(0).ToString
        cmbLevel.Text = SQLreader(1).ToString
        txtFirstName.Text = SQLreader(3).ToString
        txtLastName.Text = SQLreader(4).ToString
        txtTitle.Text = SQLreader(5).ToString
        txtQuals.Text = SQLreader(6).ToString
        txtPassword.Text = SQLreader(7).ToString
        sigFile = SQLreader(8).ToString
        sigFile1 = SQLreader(9).ToString
        TextBox1.Text = SQLreader(10).ToString

        SQLreader.Close()

        If File.Exists(sigFile) Then
            Try
                PictureBox1.ImageLocation = sigFile
            Catch ex As Exception
            End Try
        End If

        If File.Exists(sigFile1) Then
            Try
                PictureBox2.ImageLocation = sigFile1
            Catch ex As Exception
            End Try
        End If

        SQLcommand.Connection.Close()



    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim status As Integer = 0
        Dim SQLcommand As OdbcCommand

        If txtNewLogin.TextLength = 0 And cmbLogin.Text.Length = 0 Then
            MsgBox("Please Enter Login")
            Exit Sub
        ElseIf txtLastName.TextLength = 0 Then
            MsgBox("Please Enter Last Name")
            Exit Sub
        End If


        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()


        If txtNewLogin.Text.Length > 0 Then

            SQLcommand.CommandText = "INSERT INTO User (UserLevel,Login,Fname,Sname,Title,Quals,Password,Signature,SupSignature,SupSignatureText) " & _
                                    " VALUES ('" & _
                                    cmbLevel.Text & "','" & _
                                    txtNewLogin.Text & "','" & _
                                    txtFirstName.Text & "','" & _
                                    txtLastName.Text & "','" & _
                                    txtTitle.Text & "','" & _
                                    txtQuals.Text & "','" & _
                                    txtPassword.Text & "','" & _
                                    sigFile & "','" & _
                                    sigFile1 & "','" & _
                                    TextBox1.Text & "')"


            Try
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                status = 1
            End Try

        ElseIf cmbLogin.Text.Length > 0 Then

            SQLcommand.CommandText = "UPDATE User SET " & _
                         "UserLevel = '" & cmbLevel.Text & "'," & _
                         "Login = '" & cmbLogin.Text & "'," & _
                         "Fname = '" & txtFirstName.Text & "'," & _
                         "Sname = '" & txtLastName.Text & "'," & _
                         "Title = '" & txtTitle.Text & "'," & _
                         "Quals = '" & txtQuals.Text & "'," & _
                         "Password = '" & txtPassword.Text & "'," & _
                         "Signature = '" & sigFile & "'," & _
                         "SupSignature = '" & sigFile1 & "'," & _
                         "SupSignatureText = '" & TextBox1.Text & "' " & _
                         "WHERE UserID = " & UserID

            Try
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                status = 1
            End Try

        End If

        SQLcommand.Connection.Close()

        If status = 1 Then
            MsgBox("Failed To Update User Record", MsgBoxStyle.Critical)
        Else
            If UserID = frmMain.appVars.User.UserID Then

                frmMain.appVars.User.UserLevel = cmbLevel.Text
                frmMain.appVars.User.Login = cmbLogin.Text
                frmMain.appVars.User.Fname = txtFirstName.Text
                frmMain.appVars.User.Sname = txtLastName.Text
                frmMain.appVars.User.Title = txtTitle.Text
                frmMain.appVars.User.Quals = txtQuals.Text
                frmMain.appVars.User.Password = txtPassword.Text
                frmMain.appVars.User.sigFile = sigFile
                frmMain.appVars.User.sigFile1 = sigFile1
                frmMain.appVars.User.supText = TextBox1.Text
            End If

            MsgBox("User Record Updated", MsgBoxStyle.Information)
            ClearForm()
        End If





    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Dim status As Integer = 0
        Dim SQLcommand As OdbcCommand

        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()

        If UserID > -1 Then

            SQLcommand.CommandText = "DELETE " & _
                                    " FROM   User " & _
                                    " WHERE  UserID = " & UserID

            Try
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox("Failed To Delete User", MsgBoxStyle.Critical)
                status = 1
            Finally
                SQLcommand.Connection.Close()
            End Try

        Else
            MsgBox("No User Selected")
        End If

        ClearForm()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click, Button2.Click

        Dim openFileDialog1 As OpenFileDialog = New OpenFileDialog
        Dim toName As String


        openFileDialog1.InitialDirectory = Environment.CurrentDirectory & "\signatures"
        openFileDialog1.Title = "Select Signature Image File"
        openFileDialog1.FileName = "*.bmp"
        openFileDialog1.ShowDialog()


        If File.Exists(openFileDialog1.FileName) Then

            toName = System.IO.Path.GetDirectoryName(Application.ExecutablePath()) & "\signatures\"
            toName += System.IO.Path.GetFileName(openFileDialog1.FileName)

            Try
                If sender.Name = "Button1" Then
                    sigFile = toName
                Else
                    sigFile1 = toName
                End If

                If sender.Name = "Button1" Then
                    sigFile = toName
                    PictureBox1.ImageLocation = openFileDialog1.FileName
                Else
                    sigFile1 = toName
                    PictureBox2.ImageLocation = openFileDialog1.FileName
                End If

                File.Copy(openFileDialog1.FileName, toName, True)

            Catch ex As Exception
            End Try

        End If

    End Sub

End Class