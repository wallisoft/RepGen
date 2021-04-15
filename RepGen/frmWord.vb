Imports System.Data
Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions


Public NotInheritable Class frmWord

    Private Sub frmWord_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim strCwd As String = Environment.CurrentDirectory
        Dim startStr As String

        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()

        If File.Exists(OpenFileDialog1.FileName) Then

            startStr = """" & OpenFileDialog1.FileName & """"

            Try
                Process.Start("winword.exe", startStr)
            Catch ex As Exception
                Try
                    Process.Start("wordpad.exe")
                Catch ex1 As Exception
                End Try
            End Try

            Me.Close()

        End If

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Try
            Process.Start("winword.exe")
        Catch ex As Exception
            Try
                Process.Start("wordpad.exe")
            Catch ex1 As Exception

            End Try
        End Try

        Me.Close()

    End Sub
End Class
