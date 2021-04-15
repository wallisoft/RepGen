Imports System.Data.Odbc
Imports System.IO


Public Class frmChooseOdbc

    Private connStr As OdbcConnection
    Private arrConStrings() As String
    Private loadFlag As Boolean = True


    Private Sub frmChooseOdbc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        loadFlag = True

        ShowCurrValues()
        GetDrivers()
        btnAccept.Enabled = True
        btnInstallDriver.Enabled = True

        loadFlag = False

    End Sub

    Private Sub ShowCurrValues()

        txtConnStr.Text = frmMain.ClientConnStr
        txtDbName.Text = frmMain.ClientDbName
        txtDomain.Text = frmMain.ClientDbServer
        txtPassword.Text = frmMain.ClientDbPassword
        txtUser.Text = frmMain.ClientDbUser
        cmbType.Text = frmMain.ClientDbType

    End Sub


    Private Sub GetDrivers()

        Dim status As Boolean = False
        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim tmpSQL As String
        Dim count As Integer = 0
        Dim selInt As Integer = -1
        Dim ClientDbType As String
        Dim tmpStr As String = frmMain.ClientDbType

        cmbType.Items.Clear()
        cmbType.Text = ""

        tmpSQL = "SELECT * FROM Settings where KeyName LIKE 'ODBC %'"

        SQLcommand = frmMain.connStr.CreateCommand
        SQLcommand.CommandText = tmpSQL

        SQLreader = SQLcommand.ExecuteReader()

        While SQLreader.Read

            ReDim Preserve arrConStrings(count)
            Try
                arrConStrings(count) = SQLreader("Value")
                ClientDbType = SQLreader("KeyName").ToString
                cmbType.Items.Add(ClientDbType)

                If ClientDbType.Trim = frmMain.ClientDbType.Trim Then
                    selInt = count
                End If
            Catch ex As Exception
                status = True
            End Try

            count += 1
        End While

        If selInt > -1 Then cmbType.SelectedIndex = selInt

    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click

        Dim tmpStr As String = ""
        Dim tmpSQL As String = ""
        Dim status As Integer = 0
        Dim SQLcommand As OdbcCommand = Nothing
        Dim SQLreader As OdbcDataReader = Nothing
        Dim connStr As OdbcConnection = Nothing
        Dim count As Integer = 0
        Dim clientCount As Integer = 0
        Dim docCount As Integer = 0

        If cmbType.SelectedIndex = -1 Then
            MsgBox("Please Select Server Type.")
            Exit Sub
        End If

        tmpStr = arrConStrings(cmbType.SelectedIndex)
        tmpStr = tmpStr.Replace("#SERVER#", txtDomain.Text.ToString)
        tmpStr = tmpStr.Replace("#DBNAME#", txtDbName.Text.ToString)
        tmpStr = tmpStr.Replace("#USER#", txtUser.Text.ToString)
        tmpStr = tmpStr.Replace("#PASSWORD#", txtPassword.Text.ToString)

        connStr = New OdbcConnection(tmpStr)

        Try
            connStr.Open()
        Catch ex As Exception
            status = 1 'login failed
        End Try

        If status = 0 Then
            Try
                tmpSQL = "SELECT COUNT(*) FROM Client"
                SQLcommand = connStr.CreateCommand
                SQLcommand.CommandText = tmpSQL
                SQLreader = SQLcommand.ExecuteReader()
                SQLreader.Read()
                clientCount = SQLreader(0)

                tmpSQL = "SELECT COUNT(*) FROM Documents"
                SQLcommand = connStr.CreateCommand
                SQLcommand.CommandText = tmpSQL
                SQLreader = SQLcommand.ExecuteReader()
                SQLreader.Read()
                docCount = SQLreader(0)

                status = False

            Catch ex As Exception
                status = 2 'select failed
            End Try
        End If


        connStr.Close()


        If status = 1 Then

            MsgBox("Could Not Connect To Server. Please Check Settings.")

        ElseIf status = 2 Then

            MsgBox("Client Tables Not Found. Click Setup To Automatically Install Tables")

        Else
            MsgBox("Selected Database Contains " & clientCount & " Clients And " & docCount & " Documents.")
            status = 0
        End If

        If status = 0 Then
            btnAccept.Enabled = True
        Else
            btnAccept.Enabled = False
        End If

    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        Dim tmpStr As String

        If cmbType.SelectedIndex = -1 Then
            MsgBox("Please Select Server Type.")
            Exit Sub
        End If

        tmpStr = arrConStrings(cmbType.SelectedIndex)
        tmpStr = tmpStr.Replace("#SERVER#", txtDomain.Text)
        tmpStr = tmpStr.Replace("#DBNAME#", txtDbName.Text)
        tmpStr = tmpStr.Replace("#USER#", txtUser.Text)
        tmpStr = tmpStr.Replace("#PASSWORD#", txtPassword.Text)

        frmMain.connStr1 = New OdbcConnection(tmpStr)

        frmMain.ClientConnStr = txtConnStr.Text
        frmMain.ClientDbName = txtDbName.Text
        frmMain.ClientDbServer = txtDomain.Text
        frmMain.ClientDbPassword = txtPassword.Text
        frmMain.ClientDbUser = txtUser.Text
        frmMain.ClientDbType = cmbType.Text

        frmMain.appRegistry.setKeyValue("ClientConnStr", tmpStr)
        frmMain.appRegistry.setKeyValue("ClientDbType", frmMain.ClientDbType)
        frmMain.appRegistry.setKeyValue("ClientDbName", frmMain.ClientDbName)
        frmMain.appRegistry.setKeyValue("ClientDbServer", frmMain.ClientDbServer)
        frmMain.appRegistry.setKeyValue("ClientDbUser", frmMain.ClientDbUser)
        frmMain.appRegistry.setKeyValue("ClientDbPassword", frmMain.ClientDbPassword)

        Me.Close()

    End Sub

    Private Sub btnInstallDriver_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInstallDriver.Click


        Dim tmpFrm As New frmDownloadLink
        Dim tmpIndex As Integer


        tmpIndex = cmbType.SelectedIndex

        If cmbType.Text.ToString.Trim = "ODBC mySQL" Then
            tmpFrm.LinkLabel1.Text = "mysql-connector-odbc-5.1.6-win32.msi"
            tmpFrm.lnkStr = "http://dev.mysql.com/get/Downloads/Connector-ODBC/5.1/mysql-connector-odbc-5.1.6-win32.msi/from/ftp://ftp.mirrorservice.org/sites/ftp.mysql.com/"
        End If

        tmpFrm.ShowDialog()

    End Sub


    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click

        Dim OpenFileDialog1 As New OpenFileDialog

        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()

        If File.Exists(OpenFileDialog1.FileName) Then

            txtDbName.Text = OpenFileDialog1.FileName

        End If

    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged


        If loadFlag = False Then
            If cmbType.Text.Trim <> "ODBC SQLite" And cmbType.Text.Trim <> "ODBC Access" And frmMain.appVars.RegCode = "" Then
                btnAccept.Enabled = False
                MsgBox("Client-Server Database Disabled. Please Register To Enable") 'wait for license business rules..
            Else
                btnAccept.Enabled = True
            End If
        End If

    End Sub
End Class