Imports System.Data
Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Management


Public Class clsAppRegistry

    Public Function getKeyValue(ByVal keyStr As String)

        Dim keyValue As String = ""
        Dim tmpSQL As String

        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader


        tmpSQL = "SELECT Value FROM Settings WHERE KeyName = '" & keyStr & "'"

        SQLcommand = frmMain.connStr.CreateCommand
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
        SQLcommand = frmMain.connStr.CreateCommand
        SQLcommand.CommandText = tmpSQL

        Try
            SQLcommand.ExecuteNonQuery()
        Catch ex As Exception
            status = -1
        End Try



        tmpSQL = "INSERT INTO Settings VALUES(NULL,'" & keyStr & "','" & keyVal & "',0)"

        SQLcommand = frmMain.connStr.CreateCommand
        SQLcommand.CommandText = tmpSQL

        Try
            SQLcommand.ExecuteNonQuery()
        Catch ex As Exception
            status = -1
        End Try

        Return status

    End Function


End Class
