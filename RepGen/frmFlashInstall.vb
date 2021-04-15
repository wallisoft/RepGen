Imports System.Data
Imports System.Data.Odbc
Imports System.Data.Sql
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public Class frmFlashInstall

    Private connStr = New OdbcConnection


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInstall.Click

        Dim fromStr As String
        Dim toStr As String


        fromStr = Application.ExecutablePath()
        toStr = ComboBox1.Text & "Assessment Navigation Tool 1.1.exe"

        Try
            File.Copy(fromStr, toStr, True)

            File.Copy(frmMain.MyDocs & "\7za.exe", ComboBox1.Text & "7za.exe", True)

            File.Copy(frmMain.MyDocs & "\Dict.mdb", ComboBox1.Text & "Dict.mdb", True)

            File.Copy(frmMain.MyDocs & "\Data.s3db", ComboBox1.Text & "Data.s3db", True)

            File.Copy(frmMain.MyDocs & "\Client.s3db", ComboBox1.Text & "Client.s3db", True)

            File.Copy(frmMain.MyDocs & "\sqliteodbc.exe", ComboBox1.Text & "sqliteodbc.exe", True)

            updateLicense(ComboBox1.Text & "Data.s3db")

            MsgBox("ANT Successfully Installed On Drive " & ComboBox1.Text, MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox("Failed To Copy ANT To " & ComboBox1.Text, MsgBoxStyle.Critical)
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub updateLicense(ByVal dbName As String)

        Dim tmpConnStr = "DRIVER=SQLite3 ODBC Driver;Database=" & dbName & ";LongNames=0;Timeout=1000;NoTXN=0;SyncPragma=NORMAL;StepAPI=0;"
        Dim status As Boolean = False

        connStr = New OdbcConnection(tmpConnStr)
        connStr.open()

        Try
            setKeyValue("ClientLocation", ".\Client.s3db")
            setKeyValue("ClientConnStr", "DRIVER=SQLite3 ODBC Driver;Database=.\Client.s3db;LongNames=0;Timeout=1000;NoTXN=0;SyncPragma=NORMAL;StepAPI=0;")
            setKeyValue("ClientDbName", ".\Client.s3db")
            setKeyValue("ClientDbServer", "LocalHost")
            setKeyValue("DName", "")
            setKeyValue("Lock", "")
            setKeyValue("TrialStart", "")
        Catch ex As Exception
            status = True
        End Try

        'Lock = Asc(GetVolumeSerialNumber(DriveLetter)) & Asc(Today)
        'frmMain.appRegistry.setKeyValue("DName", serialNo)
        'frmMain.appRegistry.setKeyValue("TrialStart", TrialStart)
        'frmMain.appRegistry.setKeyValue("Lock", Lock)


    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        btnInstall.Enabled = True

    End Sub

    Private Sub frmFlashInstall_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        btnInstall.Enabled = False

        Dim allDrives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
        Dim d As IO.DriveInfo
        For Each d In allDrives
            'If d.IsReady = True AndAlso d.DriveType = IO.DriveType.Removable Then #flash not always shown removable
            If d.IsReady = True Then
                ComboBox1.Items.Add(d.Name)
            End If
        Next

    End Sub

    Public Function getKeyValue(ByVal keyStr As String)

        Dim keyValue As String = ""
        Dim tmpSQL As String

        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader


        tmpSQL = "SELECT Value FROM Settings WHERE KeyName = '" & keyStr & "'"

        SQLcommand = connStr.CreateCommand
        SQLcommand.CommandText = tmpSQL


        SQLreader = SQLcommand.ExecuteReader()

        If SQLreader.Read() Then

            Try
                keyValue = SQLreader(0).ToString
            Catch ex As Exception
            End Try
        End If

        Return keyValue

    End Function

    Public Function setKeyValue(ByVal keyStr As String, ByVal keyVal As String)

        Dim status As Integer = 0
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand


        tmpSQL = "DELETE  FROM Settings WHERE KeyName = '" & keyStr & "'"
        SQLcommand = connStr.CreateCommand
        SQLcommand.CommandText = tmpSQL

        Try
            SQLcommand.ExecuteNonQuery()
        Catch ex As Exception
            status = -1
        End Try



        tmpSQL = "INSERT INTO Settings VALUES(NULL,'" & keyStr & "','" & keyVal & "',0)"

        SQLcommand = connStr.CreateCommand
        SQLcommand.CommandText = tmpSQL

        Try
            SQLcommand.ExecuteNonQuery()
        Catch ex As Exception
            status = -1
        End Try

        Return status

    End Function


End Class
