Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public Class frmDataBlockMaint

    Public appVars As clsVariables
    Public RowDirty As Boolean = False
    Public LoadingValues As Boolean = True
    Public currSelected As Integer
    Public defControl = ""
    Public defSQL As String = ""
    Public defTag As String = ""
    Public defText1 As String = ""
    Public defText2 As String = ""


    Public Function InsertNewRow(ByVal rowid As Integer)

        Return 0

    End Function

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub frmDataBlockMaint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim SQLcommand As OdbcCommand
        SQLcommand = frmMain.connStr.CreateCommand

        SQLcommand.CommandText = "SELECT DISTINCT Tag FROM DataBlocks ORDER BY 1 DESC"
        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        While SQLreader.Read()
            cmbDataBlockTag.Items.Add(SQLreader(0))
        End While



        ComboBox1.Enabled = False
        ComboBox2.Enabled = False
        ComboBox3.Enabled = False
        ComboBox4.Enabled = False

        If defTag.Trim.Length > 0 Then
            cmbDataBlockTag.Text = defTag
        End If

        If defText1.Trim.Length > 0 Then
            ComboBox1.Text = defText1
        End If

        If defText2.Trim.Length > 0 Then
            ComboBox2.Text = defText2
        End If


    End Sub

    Private Sub SelectDataBlockValues()

        Dim count As Integer = 0
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader

        SQLcommand = frmMain.connStr.CreateCommand

        LoadingValues = True

        tmpSQL = "SELECT * From DataBlocks Where 1=1 "

        If cmbDataBlockTag.Text.Length > 0 Then
            tmpSQL = tmpSQL & "AND Tag = '" & cmbDataBlockTag.Text.Replace("'", "''") & "' "
        End If

        If ComboBox1.Text.Length > 0 Then
            tmpSQL = tmpSQL & "AND Text1 = '" & ComboBox1.Text.Replace("'", "''") & "' "
        End If

        If ComboBox2.Text.Length > 0 Then
            tmpSQL = tmpSQL & "AND Text2 = '" & ComboBox2.Text.Replace("'", "''") & "' "
        End If

        If ComboBox3.Text.Length > 0 Then
            tmpSQL = tmpSQL & "AND Text3 = '" & ComboBox3.Text.Replace("'", "''") & "' "
        End If

        If ComboBox4.Text.Length > 0 Then
            tmpSQL = tmpSQL & "AND Text4 = '" & ComboBox4.Text.Replace("'", "''") & "' "
        End If

        SQLcommand.CommandText = tmpSQL

        SQLreader = SQLcommand.ExecuteReader()

        DataGridView1.Rows.Clear()
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

        While SQLreader.Read()

            DataGridView1.Rows.Add()

            DataGridView1.Rows(count).Cells(0).Value = SQLreader(0)
            DataGridView1.Rows(count).Cells(1).Value = SQLreader(1)
            DataGridView1.Rows(count).Cells(2).Value = SQLreader(2)
            DataGridView1.Rows(count).Cells(3).Value = SQLreader(3)
            DataGridView1.Rows(count).Cells(4).Value = SQLreader(4)
            DataGridView1.Rows(count).Cells(5).Value = SQLreader(5)
            DataGridView1.Rows(count).Cells(6).Value = SQLreader(6)

            DataGridView1.Rows(count).Cells(0).ReadOnly = True

            count = count + 1

        End While



        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        LoadingValues = False

    End Sub

    Private Function UpdateRow(ByVal rowid As Integer)

        Dim count As Integer = 0
        Dim SQLcommand As OdbcCommand
        SQLcommand = frmMain.connStr.CreateCommand

        If DataGridView1.Rows(rowid).Cells(1).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(1).Value = ""
        If DataGridView1.Rows(rowid).Cells(2).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(2).Value = ""
        If DataGridView1.Rows(rowid).Cells(3).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(3).Value = ""
        If DataGridView1.Rows(rowid).Cells(4).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(4).Value = ""
        If DataGridView1.Rows(rowid).Cells(5).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(5).Value = ""
        If DataGridView1.Rows(rowid).Cells(6).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(6).Value = ""


        SQLcommand.CommandText = "UPDATE DataBlocks SET " & _
        " Tag = '" & DataGridView1.Rows(rowid).Cells(1).Value.ToString.Replace("'", "''") & "'," & _
        " Text1 = '" & DataGridView1.Rows(rowid).Cells(2).Value.ToString.Replace("'", "''") & "'," & _
        " Text2 = '" & DataGridView1.Rows(rowid).Cells(3).Value.ToString.Replace("'", "''") & "'," & _
        " Text3 = '" & DataGridView1.Rows(rowid).Cells(4).Value.ToString.Replace("'", "''") & "'," & _
        " Text4 = '" & DataGridView1.Rows(rowid).Cells(5).Value.ToString.Replace("'", "''") & "'," & _
        " Text5 = '" & DataGridView1.Rows(rowid).Cells(6).Value.ToString.Replace("'", "''") & "'" & _
        " WHERE DataBlockID = " & DataGridView1.Rows(rowid).Cells(0).Value.ToString
        SQLcommand.ExecuteNonQuery()



        Return 0

    End Function

    Private Function InsertRow(ByVal rowid As Integer)

        Dim count As Integer = 0
        Dim SQLcommand As OdbcCommand
        Dim NewRowID As Integer
        Dim tmpSQl As String = ""
        Dim tmpVals As String = ""
        Dim tmpCols As String = ""

        SQLcommand = frmMain.connStr.CreateCommand

        If DataGridView1.Rows(rowid).Cells(1).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(1).Value = ""
        If DataGridView1.Rows(rowid).Cells(2).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(2).Value = ""
        If DataGridView1.Rows(rowid).Cells(3).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(3).Value = ""
        If DataGridView1.Rows(rowid).Cells(4).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(4).Value = ""
        If DataGridView1.Rows(rowid).Cells(5).Value Is Nothing Then DataGridView1.Rows(rowid).Cells(5).Value = ""

        tmpCols = " INSERT INTO DataBlocks (Tag"
        tmpVals = " ) VALUES ('" & DataGridView1.Rows(rowid).Cells(1).Value.ToString.Replace("'", "''") & "'"

        'only try to insert column with a value - falls over on replace otherwise.

        If Not DataGridView1.Rows(rowid).Cells(2).Value Is Nothing Then
            tmpCols += ",Text1"
            tmpVals += ",'" & DataGridView1.Rows(rowid).Cells(2).Value.ToString.Replace("'", "''") & "'"
        End If

        If Not DataGridView1.Rows(rowid).Cells(3).Value Is Nothing Then
            tmpCols += ",Text2"
            tmpVals += ",'" & DataGridView1.Rows(rowid).Cells(3).Value.ToString.Replace("'", "''") & "'"
        End If

        If Not DataGridView1.Rows(rowid).Cells(4).Value Is Nothing Then
            tmpCols += ",Text3"
            tmpVals += ",'" & DataGridView1.Rows(rowid).Cells(4).Value.ToString.Replace("'", "''") & "'"
        End If

        If Not DataGridView1.Rows(rowid).Cells(5).Value Is Nothing Then
            tmpCols += ",Text4 "
            tmpVals += ",'" & DataGridView1.Rows(rowid).Cells(5).Value.ToString.Replace("'", "''") & "'"
        End If

        If Not DataGridView1.Rows(rowid).Cells(6).Value Is Nothing Then
            tmpCols += " ,Text5 "
            tmpVals += ",'" & DataGridView1.Rows(rowid).Cells(6).Value.ToString.Replace("'", "''") & "'"
        End If

        tmpCols += " ,Templates "
        tmpVals += ",65535"

        tmpSQl = tmpCols & tmpVals & ")"

        SQLcommand.CommandText = tmpSQl

        SQLcommand.ExecuteNonQuery()

        SQLcommand.CommandText = "select last_insert_rowid()"

        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        SQLreader.Read()
        NewRowID = SQLreader(0)


        Return NewRowID

    End Function

    Private Function DeleteRow(ByVal rowid As Integer)

        Dim count As Integer = 0
        Dim SQLcommand As OdbcCommand

        If DataGridView1.Rows(rowid).Cells(0).Value > 0 Then

            SQLcommand = frmMain.connStr.CreateCommand
            SQLcommand.CommandText = "DELETE FROM DataBlocks WHERE DataBlockID = " & DataGridView1.Rows(rowid).Cells(0).Value

            SQLcommand.ExecuteNonQuery()

        End If

        Return 0

    End Function


    Private Sub DataGridView1_RowLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.RowLeave

        Dim RowID As Integer

        DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit) 'force accept value of last edited cell

        If RowDirty = True Then

            RowDirty = False
            RowID = DataGridView1.Rows(e.RowIndex).Cells(0).Value

            If RowID > 0 Then
                UpdateRow(e.RowIndex)
            Else
                RowID = InsertRow(e.RowIndex)
                DataGridView1.Rows(e.RowIndex).Cells(0).Value = RowID
            End If
        End If

    End Sub


    Private Sub DataGridView1_CellBeginEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit

        RowDirty = True

    End Sub

    Private Sub DataGridView1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged

        Dim tmpControl As DataGridView = DirectCast(sender, DataGridView)

        If LoadingValues = False And tmpControl.SelectedRows.Count > 0 Then
            currSelected = tmpControl.SelectedRows(0).Index
        End If

    End Sub

    Private Sub DataGridView1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView1.KeyDown

        Dim tmpControl As DataGridView = DirectCast(sender, DataGridView)

        If tmpControl.SelectedRows.Count > 0 And e.KeyCode = Keys.Delete Then

            e.Handled = True

            For Each row In DataGridView1.SelectedRows

                If row.Index < DataGridView1.Rows.Count - 1 Then
                    DeleteRow(row.Index)
                    DataGridView1.Rows.Remove(row)
                End If
            Next

        End If

    End Sub


    Private Sub ComboBox_changed(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles ComboBox1.TextChanged, ComboBox2.TextChanged, ComboBox3.TextChanged, ComboBox4.TextChanged, _
        cmbDataBlockTag.TextChanged


        Dim SQLcommand As OdbcCommand
        Dim tmpSQL As String
        Dim SQLreader As OdbcDataReader
        SQLcommand = frmMain.connStr.CreateCommand


        If sender.Name = "cmbDataBlockTag" Then
            ComboBox1.Items.Clear()
            ComboBox1.Text = ""
            ComboBox2.Items.Clear()
            ComboBox2.Text = ""
            ComboBox2.Enabled = False
            ComboBox3.Items.Clear()
            ComboBox3.Text = ""
            ComboBox3.Enabled = False
            ComboBox4.Items.Clear()
            ComboBox4.Text = ""
            ComboBox4.Enabled = False

            tmpSQL = "SELECT DISTINCT Text1 FROM DataBlocks WHERE Tag = '" & cmbDataBlockTag.Text.Replace("'", "''") & "' "
            SQLcommand.CommandText = tmpSQL
            SQLreader = SQLcommand.ExecuteReader()

            ComboBox1.Enabled = True

            While SQLreader.Read()
                ComboBox1.Items.Add(SQLreader(0))
            End While
        End If

        If sender.Name = "ComboBox1" Then
            ComboBox2.Items.Clear()
            ComboBox2.Text = ""
            ComboBox3.Items.Clear()
            ComboBox3.Text = ""
            ComboBox3.Enabled = False
            ComboBox4.Items.Clear()
            ComboBox4.Text = ""
            ComboBox4.Enabled = False

            tmpSQL = "SELECT DISTINCT Text2 FROM DataBlocks WHERE Tag = '" & cmbDataBlockTag.Text.Replace("'", "''") & "' " & _
                "AND Text1 = '" & ComboBox1.Text.Replace("'", "''") & "' "
            SQLcommand.CommandText = tmpSQL
            SQLreader = SQLcommand.ExecuteReader()

            ComboBox2.Enabled = True

            While SQLreader.Read()
                ComboBox2.Items.Add(SQLreader(0))
            End While
        End If

        If sender.Name = "ComboBox2" Then
            ComboBox3.Items.Clear()
            ComboBox3.Text = ""
            ComboBox4.Items.Clear()
            ComboBox4.Text = ""
            ComboBox4.Enabled = False


            tmpSQL = "SELECT DISTINCT Text3 FROM DataBlocks WHERE Tag = '" & cmbDataBlockTag.Text.Replace("'", "'' ") & "' " & _
                "AND Text1 = '" & ComboBox1.Text.Replace("'", "''") & "' " & _
                "AND Text2 = '" & ComboBox2.Text.Replace("'", "''") & "' "

            SQLcommand.CommandText = tmpSQL
            SQLreader = SQLcommand.ExecuteReader()

            ComboBox3.Enabled = True

            While SQLreader.Read()
                ComboBox3.Items.Add(SQLreader(0))
            End While
        End If

        If sender.Name = "ComboBox3" Then
            ComboBox4.Items.Clear()
            ComboBox4.Text = ""

            tmpSQL = "SELECT DISTINCT Text4 FROM DataBlocks WHERE Tag = '" & cmbDataBlockTag.Text.Replace("'", "'' ") & "' " & _
                "AND Text1 = '" & ComboBox1.Text.Replace("'", "''") & "' " & _
                "AND Text2 = '" & ComboBox2.Text.Replace("'", "''") & "' " & _
                "AND Text3 = '" & ComboBox3.Text.Replace("'", "''") & "' "

            SQLcommand.CommandText = tmpSQL
            SQLreader = SQLcommand.ExecuteReader()

            ComboBox4.Enabled = True

            While SQLreader.Read()
                ComboBox4.Items.Add(SQLreader(0))
            End While
        End If



        SelectDataBlockValues()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If TextBox1.TextLength > 0 Then

            DataGridView1.Rows.Clear()

            DataGridView1.Rows.Add()

            DataGridView1.Rows(0).Cells(1).Value = TextBox1.Text
            DataGridView1.Rows(0).Cells(2).Value = ""
            DataGridView1.Rows(0).Cells(2).Selected = True

        End If

    End Sub


    Private Sub DataGridView1_RowEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.RowEnter

        Dim currIndex As Integer = DataGridView1.Rows.Count - 1
        Dim prevIndex As Integer = DataGridView1.Rows.Count - 2


    End Sub

    Private Sub DataGridView1_DefaultValuesNeeded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DataGridView1.DefaultValuesNeeded

        Dim currIndex As Integer = DataGridView1.Rows.Count - 1
        Dim prevIndex As Integer = DataGridView1.Rows.Count - 2

        If currIndex > 0 Then

            DataGridView1.Rows(currIndex).Cells(1).Value = DataGridView1.Rows(prevIndex).Cells(1).Value
            DataGridView1.Rows(currIndex).Cells(2).Value = DataGridView1.Rows(prevIndex).Cells(2).Value
            DataGridView1.Rows(currIndex).Cells(3).Value = DataGridView1.Rows(prevIndex).Cells(3).Value
            DataGridView1.Rows(currIndex).Cells(4).Value = DataGridView1.Rows(prevIndex).Cells(4).Value
            DataGridView1.Rows(currIndex).Cells(5).Value = DataGridView1.Rows(prevIndex).Cells(5).Value

        End If

    End Sub

End Class
