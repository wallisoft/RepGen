Imports System.Data
Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions


Public Class clsUpdateAnt

    Public OpenFileDialog1 As New OpenFileDialog
    Public SaveFileDialog1 As New SaveFileDialog



    Public Sub UpdateAnt(ByVal fileName As String)

        Dim strCwd As String = Environment.CurrentDirectory
        Dim fName As String = Application.ExecutablePath()
        Dim path As String = System.IO.Path.GetDirectoryName(Application.ExecutablePath())
        Dim status As Boolean = False

        'check if ANT is running

        Try
            Dim npProc() As Process
            npProc = Process.GetProcesses
            For Each proc As Process In npProc
                If proc.ProcessName.ToString.Contains("Assessment Navigation Tool") Then
                    If Not proc.ProcessName.ToString.Contains("vshost") Then 'not visual studio
                        status = True
                    End If
                End If
            Next
        Catch ex As Exception
        End Try

        '#sw add check for installed version > 1.1.0.3


        If MsgBox("This Update Will Overwrite All DataBlocks (Client Data Is Unaffected). Continue? ", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If status = False Then

                Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(Application.ExecutablePath()))

                File.Delete("update.zip")

                FileCopy(fileName, path & "\update.zip")

                Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath())

                'backup databases

                Try
                    FileCopy("Client.s3db", "Client.s3db.update.bak")
                Catch ex As Exception
                End Try

                Try
                    FileCopy("Data.s3db", "Data.s3db.update.bak")
                Catch ex As Exception
                End Try

                System.Threading.Thread.Sleep(1000)

                '#sw Add progress bar and update information. #TODO# 

                Process.Start("7za.exe", "e -y update.zip").WaitForExit()
                File.Delete("update.zip")

            End If
        Else
            MsgBox("Update Cancelled")
            Exit Sub
        End If


        If status = True Then
            MsgBox("Update Failed - Check Assessment Navigation Tool not running", MsgBoxStyle.Critical)
        Else
            MsgBox("Update Succeded", MsgBoxStyle.Information)
        End If

    End Sub

End Class
