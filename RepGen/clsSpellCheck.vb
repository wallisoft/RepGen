Imports System.Data.OleDb
Imports System.Collections


Public Class clsSpellCheck
    Public ConnStr As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ".\Dict.mdb"
    Public Conn As OleDbConnection
    Public Cmd As OleDbCommand
    Public Cmd2 As OleDbCommand
    Public Adp As OleDbDataAdapter
    Public Adp2 As OleDbDataAdapter
    Public CmB2 As OleDbCommandBuilder
    Public DicT As DataSet
    Public IgnoreAllList As New HashTable()
    Public ChangeAllList As New HashTable()

    Sub New()
        Conn = New OleDbConnection(ConnStr)
        Conn.Open()
        Cmd = New OleDbCommand("Select distinct word from Words", Conn)
        Cmd2 = New OleDbCommand("Select distinct word from Custom", Conn)
        Adp = New OleDbDataAdapter(Cmd)
        Adp2 = New OleDbDataAdapter(Cmd2)
        CmB2 = New OleDbCommandBuilder(Adp2)
        DicT = New DataSet("Dict")
        Adp.Fill(DicT, "Words")
        Adp2.Fill(DicT, "Custom")
        Dim DC(0) As DataColumn
        DC(0) = DicT.Tables("Words").Columns(0)
        DicT.Tables("Words").PrimaryKey = DC
        Dim Dr As DataRow
        For Each Dr In DicT.Tables("Custom").Rows
            Dim Dr2 As DataRow
            Dr2 = DicT.Tables("Words").NewRow
            Dr2.Item(0) = Dr.Item(0)
            Try
                DicT.Tables("Words").Rows.Add(Dr2)
            Catch ex As Exception
            End Try
            Dr2 = Nothing
        Next
    End Sub

End Class
