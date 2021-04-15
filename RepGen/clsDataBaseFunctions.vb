Imports System.Data
Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions


Public Class clsDataBaseFunctions

    Dim OpenFileDialog1 As New OpenFileDialog
    Dim SaveFileDialog1 As New SaveFileDialog



    Public Sub SetupDatabase()

        Dim ClientConnStr As String
        Dim ClientDbType As String
        Dim ClientDbName As String
        Dim ClientDbServer As String
        Dim ClientDbUser As String
        Dim ClientDbPassword As String



        Try
            frmMain.connStr.Open()
        Catch ex As Exception

            Dim tmpStr = """" & frmMain.MyDocs & "\sqliteodbc.exe"""
            tmpStr = tmpStr.Replace("\", "\\")

            If MsgBox("Press OK To Install SQLite ODBC Database Driver", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Try
                    Dim Proc As New Process
                    Proc.StartInfo.FileName = tmpStr
                    Proc.Start()
                    Proc.WaitForExit()
                Catch ex1 As Exception
                    MsgBox("File Not Found. Please Re-install Application.", MsgBoxStyle.Critical)
                    End
                End Try
            Else
                End
            End If

            Try
                frmMain.connStr.Open()
            Catch ex1 As Exception
                MsgBox("Error Opening Database. Please Re-Install")
                End
            End Try
        End Try


        ClientConnStr = frmMain.appRegistry.getKeyValue("ClientConnStr")
        ClientDbType = frmMain.appRegistry.getKeyValue("ClientDbType")
        ClientDbName = frmMain.appRegistry.getKeyValue("ClientDbName")
        ClientDbServer = frmMain.appRegistry.getKeyValue("ClientDbServer")
        ClientDbUser = frmMain.appRegistry.getKeyValue("ClientDbUser")
        ClientDbPassword = frmMain.appRegistry.getKeyValue("ClientDbPassword")

        If ClientConnStr.Trim.Length = 0 Then
            ClientConnStr = "Driver={SQLite3 ODBC Driver};Database=" & System.IO.Path.GetDirectoryName(Application.ExecutablePath()) & "\Client.s3db;LongNames=0;Timeout=1000;NoTXN=0;SyncPragma=NORMAL;StepAPI=0;"
            frmMain.appRegistry.setKeyValue("ClientConnStr", ClientConnStr)
        End If


        If CheckValidDatabase(ClientConnStr, True) = False Then
            MsgBox("Database Not Available - Setting To Local")
            ClientConnStr = "Driver={SQLite3 ODBC Driver};Database=" & System.IO.Path.GetDirectoryName(Application.ExecutablePath()) & "\Client.s3db;LongNames=0;Timeout=1000;NoTXN=0;SyncPragma=NORMAL;StepAPI=0;"
        End If

        frmMain.connStr1 = New OdbcConnection(ClientConnStr)

        frmMain.ClientConnStr = ClientConnStr
        frmMain.ClientDbType = ClientDbType
        frmMain.ClientDbName = ClientDbName
        frmMain.ClientDbServer = ClientDbServer
        frmMain.ClientDbUser = ClientDbUser
        frmMain.ClientDbPassword = ClientDbPassword

        If ClientDbServer.Trim.Length = 0 Then ClientDbServer = "Localhost"
        If ClientDbName.Trim.Length = 0 Then ClientDbName = ".\Client.s3db"


        frmMain.ToolStripStatusLabel1.Text = "Database Server : " & ClientDbServer & "    Database Name : " & ClientDbName


    End Sub



    Public Sub DB_Backup()

        Dim strCwd As String = Environment.CurrentDirectory

        SaveFileDialog1.FileName = "ANT_Database.bak"
        SaveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)

        If Not SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel Then

            FileCopy(strCwd + "\Client.s3db", SaveFileDialog1.FileName)

            If File.Exists(SaveFileDialog1.FileName) Then 'Add file comparison code here!
                MsgBox("Database Backup Completed")
            Else
                MsgBox("Database Backup Failed")
            End If
        Else
            MsgBox("Database Backup Aborted")
        End If

        Environment.CurrentDirectory = strCwd

    End Sub

    Public Sub DB_Restore()

        Dim strCwd As String = Environment.CurrentDirectory

        If MsgBox("This Will Overwrite Current Database - Continue ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.ShowDialog()

            If File.Exists(OpenFileDialog1.FileName) Then

                Environment.CurrentDirectory = strCwd
                FileCopy(frmMain.ClientConnStr, "Database.bak")
                FileCopy(OpenFileDialog1.FileName, frmMain.ClientConnStr)

                If CheckValidDatabase(frmMain.ClientConnStr, False) Then
                    MsgBox("Database Restored Successfully")
                Else
                    FileCopy("Database.bak", frmMain.ClientConnStr)
                    MsgBox("Database Restore Failed")
                End If
            Else
                MsgBox("Database Restore Failed")
            End If

        End If

    End Sub

    Public Sub DB_UndoRestore()

        If File.Exists("Database.bak") Then

            FileCopy(frmMain.ClientConnStr, "Database.bak1")

            If MsgBox("This Will Overwrite Current Database - Continue ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                FileCopy("Database.bak", frmMain.ClientConnStr)

                If CheckValidDatabase(frmMain.ClientConnStr, False) Then
                    MsgBox("Database Restored Successfully")
                Else
                    MsgBox("Database Corrupted. Restore Failed")
                    FileCopy("Database.bak1", frmMain.ClientConnStr)
                End If

            End If
        Else
            MsgBox("Database Not Found")
        End If

    End Sub

    Public Function CheckValidDatabase(ByVal testConnStr As String, ByVal runSilent As Boolean)

        Dim status As Integer = 0
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim connStr As OdbcConnection
        Dim count As Integer = 0
        Dim clientCount As Integer = 0
        Dim docCount As Integer = 0

        Try
            connStr = New OdbcConnection(testConnStr)
            connStr.Open()

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

            status = 1

        Catch ex As Exception

            If runSilent = False Then
                MsgBox("Database Corrupted. Aborting Restore")
                status = 0
            End If

        End Try

        If status = 1 And runSilent = False Then

            If Not MsgBox("Database Contains " & clientCount & " Clients And " & docCount & " Documents. Continue ? ", MsgBoxStyle.YesNo) _
                = MsgBoxResult.Yes Then
                status = 1
            End If

        End If

        Return status

    End Function

    Function ChangeCurrentDatabase()

        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()

        If File.Exists(OpenFileDialog1.FileName) Then

            If CheckValidDatabase(OpenFileDialog1.FileName, False) Then

                frmMain.connStr1 = New OdbcConnection("Driver=SQLite ODBC Driver;Database=" & OpenFileDialog1.FileName)
                frmMain.connStr1.Open()

                frmMain.appRegistry.setKeyValue("ClientLocation", OpenFileDialog1.FileName)
                frmMain.ToolStripStatusLabel1.Text = "Database : " & OpenFileDialog1.FileName

                frmMain.ClientConnStr = OpenFileDialog1.FileName

                MsgBox("Database Location Reset Successfully")
            End If
        End If


        Return 0

    End Function

    Public Sub SetLocalDatabase()

        Dim tmpConnStr As String = "Driver={SQLite3 ODBC Driver};Database=.\Client.s3db;LongNames=0;Timeout=1000;NoTXN=0;SyncPragma=NORMAL;StepAPI=0;"

        If File.Exists(".\Client.s3db") Then

            If CheckValidDatabase(tmpConnStr, False) Then

                frmMain.connStr1 = New OdbcConnection(tmpConnStr)

                frmMain.appRegistry.setKeyValue("ClientLocation", ".\Client.s3db")
                frmMain.appRegistry.setKeyValue("ClientConnStr", tmpConnStr)
                frmMain.appRegistry.setKeyValue("ClientDbName", ".\Client.s3db")
                frmMain.appRegistry.setKeyValue("ClientDbServer", "LocalHost")

                frmMain.ClientConnStr = OpenFileDialog1.FileName
                frmMain.ToolStripStatusLabel1.Text = "Database Server : LocalHost    Database Name : .\Client.s3db"

                MsgBox("Database Location Reset Successfully")
            End If
        Else
            MsgBox("Local Database Not Found")
        End If
    End Sub


    Public Sub openODBC()

        Dim tmpFrm As New frmChooseOdbc
        tmpFrm.ShowDialog()

        frmMain.ToolStripStatusLabel1.Text = "Database Server : " & frmMain.ClientDbServer & "    Database Name : " & frmMain.ClientDbName

    End Sub

 
End Class
