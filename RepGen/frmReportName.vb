Imports System.Windows.Forms
Imports System.Data
Imports System.Data.Odbc
Imports System.Data.Sql
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions

Public Class frmReportName

    Public oldRepName As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim status As Integer = 0
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand
        SQLcommand = frmMain.connStr.CreateCommand

        If oldRepName = TextBox1.Text Then

            tmpSQL = "UPDATE AppDocs SET " & _
                    " DocText = '" & frmMain.rtbDefault.Rtf.Replace("'", "''") & "' " & _
                    " WHERE DocumentTag = '" & TextBox1.Text & "'"
            Try
                SQLcommand.CommandText = tmpSQL
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                status = 1
            End Try
        Else
            tmpSQL = "INSERT INTO AppDocs ( " & _
                    " DocumentTag, " & _
                    " DocText, DocType) " & _
                    " VALUES ( " & _
                    "'" & TextBox1.Text & "'," & _
                    "'" & frmMain.rtbDefault.Rtf.Replace("'", "''") & "',1)"
            Try
                SQLcommand.CommandText = tmpSQL
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                status = 1
            End Try
        End If

        If status = 0 Then
            MsgBox("Report Template Saved", MsgBoxStyle.Information)
        Else
            MsgBox("Could Not Save Report Template", MsgBoxStyle.Critical)
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub frmReportName_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        TextBox1.Text = frmMain.cmbReportTemplate.Text
        oldRepName = TextBox1.Text

    End Sub
End Class
