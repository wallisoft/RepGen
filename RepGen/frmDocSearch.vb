Imports System.Windows.Forms
Imports System.Data
Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions

Public Class frmDocSearch


    Private arrUserID() As String
    Public DocID As Integer = -1
    Public ClientID As Integer = -1
    Public EditSelected As Boolean = False

    Private Sub Dialog1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        SetDateRange()
        loadForm()
        LoadDocuments()

    End Sub


    Private Sub loadForm()

        Dim status As Integer = 0
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim count As Integer = 0
        Dim docCount As Integer = 0

        btnSelect.Enabled = False

        If txtClient.Text < 1 Then
            txtClient.Text = "All"
        End If

        cmbUsers.Text = "All"

        cmbDocType1.Items.Clear()
        cmbDocType2.Items.Clear()
        cmbDocType2.Items.Clear()

        cmbDocType1.Items.Add("All")
        cmbDocType1.Text = "All"
        cmbUsers.Items.Clear()

        tmpSQL = "SELECT DISTINCT DocumentTag FROM Documents WHERE ClientID > 0 "

        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()

        SQLcommand.CommandText = tmpSQL
        SQLreader = SQLcommand.ExecuteReader()

        While SQLreader.Read()
            cmbDocType1.Items.Add(SQLreader(0))
            cmbDocType2.Items.Add(SQLreader(0))
            cmbDocType3.Items.Add(SQLreader(0))
        End While

        tmpSQL = "SELECT UserID, Fname, Sname FROM User ORDER BY 2"

        SQLcommand = frmMain.connStr1.CreateCommand


        SQLcommand.CommandText = tmpSQL

        SQLreader = SQLcommand.ExecuteReader()

        count = 0

        While SQLreader.Read()
            cmbUsers.Items.Add(SQLreader(1).ToString.Trim & " " & SQLreader(2).ToString.Trim)

            ReDim Preserve arrUserID(count)
            arrUserID(count) = SQLreader(0)
            count += 1

        End While

        SQLcommand.Connection.Close()

        Dim vals() As String = {"View", "Edit", "Send To Share", "Send To Word", "Send To Folder", "Send To Zip File"}

        cmbAction.Items.Clear()

        For count = 0 To 5
            cmbAction.Items.Add(vals(count))
        Next

       
    End Sub

    Private Sub LoadDocuments()

        Dim status As Integer = 0
        Dim tmpSQL As String
        Dim tmpName As String = ""
        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim count As Integer = 0
        Dim docCount As Integer = 0
        Dim tmpBool1 As String = ""
        Dim tmpBool2 As String = ""

        If txtClient.Text.Trim.Length = 0 Then
            txtClient.Text = "All"
        End If


        If cmbContainsBool1.Text.Trim.ToUpper = "NOT" Then tmpBool1 = "NOT"
        If cmbContainsBool2.Text.Trim.ToUpper = "NOT" Then tmpBool2 = "NOT"


        Try
            ListView1.Items.Clear()
        Catch ex As Exception
        End Try


        tmpSQL = "SELECT DISTINCT Documents.DocumentID," & _
                " Documents.ClientID," & _
                " Documents.OwnerID, " & _
                " Documents.DocumentTag," & _
                " Documents.DateAdded," & _
                " Documents.DateLastEdit," & _
                " Documents.DocText " & _
                " FROM Documents " & _
                " WHERE Documents.DateAdded BETWEEN " & frmMain.appVars.DateToDays(CDate(DateTimePicker1.Text)) & _
                " AND " & frmMain.appVars.DateToDays(CDate(DateTimePicker2.Text))

        If txtClient.Text.Trim <> "All" Then tmpSQL += " AND Documents.ClientID = " & txtClient.Text

        If cmbUsers.SelectedIndex > -1 Then tmpSQL += " AND Documents.OwnerID = " & arrUserID(cmbUsers.SelectedIndex)


        If cmbDocType1.Text.Trim.Length > 0 And cmbDocType1.Text <> "All" Then tmpSQL += " AND ( Documents.DocumentTag = '" & cmbDocType1.Text & "' "
        If cmbDocType2.Text.Trim.Length > 0 Then tmpSQL += " OR Documents.DocumentTag = '" & cmbDocType2.Text & "' "
        If cmbDocType3.Text.Trim.Length > 0 Then tmpSQL += " OR Documents.DocumentTag = '" & cmbDocType3.Text & "' "
        If cmbDocType1.Text.Trim.Length > 0 And cmbDocType1.Text <> "All" Then tmpSQL += ") "

        If txtContains1.TextLength > 0 Then tmpSQL += " AND Documents.DocText LIKE '%" & txtContains1.Text & "%' "
        If txtContains2.TextLength > 0 Then tmpSQL += tmpBool1 & " Documents.DocText LIKE '%" & txtContains2.Text & "%' "
        If txtContains3.TextLength > 0 Then tmpSQL += tmpBool2 & " Documents.DocText LIKE '%" & txtContains3.Text & "%' "

        tmpSQL += " ORDER BY Documents.DocumentID "


        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()


        SQLcommand.CommandText = tmpSQL

        SQLreader = SQLcommand.ExecuteReader()

        While SQLreader.Read()

            tmpName = SQLreader("OwnerID")

            For count1 = 0 To arrUserID.Count - 1
                If arrUserID(count1) = SQLreader("OwnerID") Then
                    tmpName = cmbUsers.Items(count1)
                End If
            Next

            ListView1.Items.Add("") 'checkbox column
            ListView1.Items(count).SubItems.Add(SQLreader("DocumentTag").ToString)
            ListView1.Items(count).SubItems.Add(frmMain.appVars.DaysToString(SQLreader("DateAdded").ToString))
            ListView1.Items(count).SubItems.Add(tmpName)

            ListView1.Items(count).Tag = SQLreader("DocumentID")

            count += 1

        End While

        ListView1.Columns(0).Width = 20
        ListView1.Columns(1).Width = 100
        ListView1.Columns(2).Width = 100
        ListView1.Columns(3).Width = 160


        SQLcommand.Connection.Close()


    End Sub

    Private Sub SetDateRange()

        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim tmpSQL As String

        DateTimePicker2.Text = Date.Today

        tmpSQL = "SELECT MIN(DateAdded) FROM Documents"

        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()

        SQLcommand.CommandText = tmpSQL
        SQLreader = SQLcommand.ExecuteReader()

        If SQLreader.Read() Then
            Try
                DateTimePicker1.Text = frmMain.appVars.DaysToString(SQLreader(0).ToString)
            Catch ex As Exception
            End Try
        End If


        SQLcommand.Connection.Close()


    End Sub


    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        LoadDocuments()

    End Sub

    Private Function GetCurrRtf()

        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim tmpSQL As String
        Dim index As Integer
        Dim tmpRtf As String = ""


        If ListView1.SelectedItems.Count > 0 Then

            index = ListView1.SelectedIndices(0)

            DocID = ListView1.Items(index).Tag

            tmpSQL = "SELECT DocText FROM Documents WHERE DocumentID = " & ListView1.Items(index).Tag

            SQLcommand = frmMain.connStr1.CreateCommand
            SQLcommand.Connection.Open()

            SQLcommand.CommandText = tmpSQL
            SQLreader = SQLcommand.ExecuteReader()

            If SQLreader.Read() Then
                tmpRtf = SQLreader(0).ToString.Replace("\\", "\")
            End If

            SQLcommand.Connection.Close()

        End If

        Return tmpRtf

    End Function

    Private Function GetClientID(ByVal tmpDocID As Integer)

        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim tmpSQL As String
        Dim index As Integer
        Dim tmpClientID As Integer = -1


        If ListView1.SelectedItems.Count > 0 Then

            index = ListView1.SelectedIndices(0)

            DocID = ListView1.Items(index).Tag

            tmpSQL = "SELECT ClientID FROM Documents WHERE DocumentID = " & ListView1.Items(index).Tag

            SQLcommand = frmMain.connStr1.CreateCommand
            SQLcommand.Connection.Open()

            SQLcommand.CommandText = tmpSQL
            SQLreader = SQLcommand.ExecuteReader()

            If SQLreader.Read() Then
                tmpClientID = SQLreader(0)
            End If

            SQLcommand.Connection.Close()
        End If

        Return tmpClientID

    End Function

    Private Sub ViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem.Click

        Dim tmpfrm1 As New frmDocViewer
        Dim tmpRtf As String

        tmpRtf = GetCurrRtf()

        tmpfrm1.RichTextBox1.Rtf = tmpRtf
        tmpfrm1.ShowInTaskbar = True
        tmpfrm1.Show()

    End Sub

    Private Sub WordToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WordToolStripMenuItem.Click

        Dim frmWord1 As New frmWord
        Dim tmpRtf As String

        tmpRtf = GetCurrRtf()

        Clipboard.Clear()
        Clipboard.SetText(tmpRtf, TextDataFormat.Rtf)

        frmWord1.ShowDialog()

    End Sub

    Private Sub EditToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditToolStripMenuItem.Click

        frmMain.LoadDocument(DocID)
        frmMain.currClientID = ClientID

        frmMain.cmbReportTemplate.Text = ""

        Me.Close()

    End Sub


    Private Sub ListView1_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick

        If e.Column = 0 Then

            For count = 0 To ListView1.Items.Count - 1

                If ListView1.Items(count).Checked = False Then
                    ListView1.Items(count).Checked = True
                Else
                    ListView1.Items(count).Checked = False
                End If
            Next

        End If

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click

        Me.Close()

    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click

        ClearForm()

    End Sub

    Private Sub ClearForm()

        SetDateRange()

        txtClient.Text = "All"

        cmbUsers.Text = "All"

        txtContains1.Text = ""
        txtContains2.Text = ""
        txtContains3.Text = ""

        cmbDocType1.Text = ""
        cmbDocType2.Text = ""
        cmbDocType3.Text = ""

        cmbContainsBool1.Text = ""
        cmbContainsBool2.Text = ""

        ListView1.Items.Clear()

    End Sub

    Private Sub ListView1_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles ListView1.ItemChecked

        Dim status As Boolean = False

        For count = 0 To ListView1.Items.Count - 1
            If ListView1.Items(count).Checked = True Then
                DocID = ListView1.Items(count).Tag
                status = True
            End If
        Next

        If status = True Then
            btnSelect.Enabled = True
        Else
            btnSelect.Enabled = False
        End If

    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

        If ListView1.SelectedItems.Count > 0 Then
            DocID = ListView1.Items(ListView1.SelectedIndices(0)).Tag
        End If

    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click

        If cmbAction.SelectedIndex = 0 Then SendToView()
        If cmbAction.SelectedIndex = 1 Then SendToEdit()
        If cmbAction.SelectedIndex = 2 Then SendToSharePoint()
        If cmbAction.SelectedIndex = 3 Then MergeToWord()
        If cmbAction.SelectedIndex = 4 Then CreateFiles()
        If cmbAction.SelectedIndex = 5 Then SendToZip()

    End Sub

    Private Sub SendToEdit()

        Dim arrVals(1) As String
        Dim tmpRtf As String


        For count = 0 To ListView1.Items.Count - 1

            If ListView1.Items(count).Selected Or ListView1.Items(count).Checked Then

                arrVals = GetDocument(ListView1.Items(count).Tag)
                tmpRtf = GetCurrRtf()
                frmMain.LoadDocument(DocID)
                frmMain.currClientID = ClientID
                frmMain.cmbReportTemplate.Text = ListView1.Items(count).SubItems(1).Text

                Me.Close()

            End If
        Next

    End Sub


    Private Sub SendToView()

        Dim arrVals(1) As String
        Dim tmpfrm1 As New frmDocViewer
        Dim tmpRtf As String


        For count = 0 To ListView1.Items.Count - 1

            If ListView1.Items(count).Selected = True Or ListView1.Items(count).Checked Then

                arrVals = GetDocument(ListView1.Items(count).Tag)
                tmpRtf = GetCurrRtf()

                tmpfrm1.RichTextBox1.Rtf = tmpRtf
                tmpfrm1.ShowInTaskbar = True
                tmpfrm1.Show()

                If EditSelected = True Then

                    SendToEdit()
                    Me.Close()

                End If

                Exit For

            End If
        Next

    End Sub


    Private Sub SendToSharePoint()

        Dim dirName As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\GPS\Temp\" & Date.Today.Ticks
        Dim SaveFileDialog1 As SaveFileDialog = New SaveFileDialog
        Dim strCwd As String = Environment.CurrentDirectory
        Dim allFiles As String = ""
        Dim runStr As String = ""
        Dim tmpPath As String = ".\tmp\" & Date.Now & "\"


        If frmMain.appVars.RegCode = "" Then
            MsgBox("Please Register To Enable")
            Exit Sub
        End If

        Directory.CreateDirectory(tmpPath)

        SendToFolder(dirName)

        For Each fileName As String In Directory.GetFiles(dirName)
            allFiles += " " & fileName.Substring(fileName.LastIndexOf("\") + 1)
        Next

        For Each fileName As String In Directory.GetFiles(dirName)
            File.Delete(fileName)
        Next

        Directory.Delete(dirName)


    End Sub


    Private Sub MergeToWord()

        Dim arrVals(1) As String
        Dim tmpRtf As String = ""
        Dim frmWord1 As New frmWord
        Dim tmpBox1 As New RichTextBox
        Dim tmpBox2 As New RichTextBox

        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim tmpSQL As String
        Dim tmpArr(1) As String
        Dim tmpPageBreak As String = ""


        tmpSQL = "SELECT AppDocs.DocText FROM AppDocs WHERE DocumentTAG = 'PageBreak'"


        SQLcommand = frmMain.connStr.CreateCommand

        SQLcommand.CommandText = tmpSQL
        SQLreader = SQLcommand.ExecuteReader()

        If SQLreader.Read() Then
            tmpPageBreak = SQLreader(0)
        End If

        For count = 0 To ListView1.Items.Count - 1

            If ListView1.Items(count).Checked Or ListView1.Items(count).Selected Then

                arrVals = GetDocument(ListView1.Items(count).Tag)
                tmpBox1.Rtf = arrVals(0)
                tmpBox1.SelectAll()
                tmpBox1.Copy()
                tmpBox2.Paste()

                tmpBox1.Rtf = tmpPageBreak
                tmpBox1.SelectAll()
                tmpBox1.Copy()
                tmpBox2.Paste()

            End If
        Next

        tmpBox2.SelectAll()
        tmpBox2.Copy()

        frmWord1.ShowDialog()

    End Sub

    Private Sub CreateFiles()

        Dim dirName As String = ""
        Dim SaveFileDialog1 As SaveFileDialog = New SaveFileDialog
        Dim fileName As String


        SaveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        SaveFileDialog1.FileName = "Client DocType Date .rtf"


        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

            fileName = SaveFileDialog1.FileName

            dirName = SaveFileDialog1.FileName.ToString.Substring(0, SaveFileDialog1.FileName.ToString.LastIndexOf("\"))
            SendToFolder(dirName)

            Shell("explorer.exe " & dirName, AppWinStyle.NormalFocus)

        End If



    End Sub

    Private Sub SendToZip()

        Dim dirName As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\GPS\Temp\" & Date.Today.Ticks
        Dim SaveFileDialog1 As SaveFileDialog = New SaveFileDialog
        Dim strCwd As String = Environment.CurrentDirectory
        Dim allFiles As String = ""
        Dim runStr As String = ""


        SaveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        SaveFileDialog1.FileName = "ClientDocs.zip"


        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

            Directory.CreateDirectory(dirName)

            SendToFolder(dirName)

            For Each fileName As String In Directory.GetFiles(dirName)
                allFiles += " " & fileName.Substring(fileName.LastIndexOf("\") + 1)
            Next

            File.Copy(System.IO.Path.GetDirectoryName(Application.ExecutablePath()) & "\7za.exe", dirName & "\7za.exe", True)

            Directory.SetCurrentDirectory(dirName)

            runStr = "7za.exe a """ & SaveFileDialog1.FileName & """ " & allFiles
            Shell(runStr, AppWinStyle.Hide, True)

            Directory.SetCurrentDirectory(strCwd)

            For Each fileName As String In Directory.GetFiles(dirName)
                File.Delete(fileName)
            Next
            Directory.Delete(dirName)

        End If

    End Sub


    Private Sub SendToFolder(ByVal dirName As String)

        Dim status As Boolean = False
        Dim strCwd As String = Environment.CurrentDirectory
        Dim SaveFileDialog1 As SaveFileDialog = New SaveFileDialog
        Dim fileName As String = ""
        Dim count As Integer = 0
        Dim arrVals(1) As String
        Dim objSW As StreamWriter

        For count = 0 To ListView1.Items.Count - 1

            If ListView1.Items(count).Checked Or ListView1.Items(count).Selected Then

                arrVals = GetDocument(ListView1.Items(count).Tag)

                fileName = dirName & "\" & arrVals(1)

                objSW = New StreamWriter(fileName)
                objSW.Write(arrVals(0))
                objSW.Close()

            End If
        Next

        Environment.CurrentDirectory = strCwd

    End Sub



    Private Function GetDocument(ByVal tmpDocID As Integer)

        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim tmpSQL As String
        Dim tmpArr(1) As String
        Dim tmpDate As String = ""
        Dim tmpClientID As Integer
        Dim tmpDocTag As String = ""

        frmMain.connStr.close()


        tmpSQL = "SELECT Documents.ClientID,Documents.DocText,Documents.DocumentTag,Documents.DateAdded FROM Documents WHERE DocumentID = " & tmpDocID

        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()

        SQLcommand.CommandText = tmpSQL
        SQLreader = SQLcommand.ExecuteReader()

        If SQLreader.Read() Then
            tmpClientID = SQLreader("ClientID")
            tmpDocTag = SQLreader("DocumentTag")

            tmpDate = frmMain.appVars.DaysToString(SQLreader("DateAdded")).ToString.Replace("/", "-")
            tmpArr(0) = SQLreader("DocText").ToString.Replace("\\", "\")
        End If

        SQLreader.Close()

        tmpSQL = "SELECT Client.Fname, Client.Sname FROM Client WHERE ClientID = " & tmpClientID

        SQLcommand.CommandText = tmpSQL
        SQLreader = SQLcommand.ExecuteReader()

        If SQLreader.Read() Then
            tmpArr(1) = SQLreader("Sname") & "," & SQLreader("Fname") & "_" & tmpDocTag & "_" & tmpDate & ".rtf"
        End If

        SQLcommand.Connection.Close()

        frmMain.connStr.open()

        Return tmpArr

    End Function


    Private Sub ListView1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick

        Dim tmpfrm1 As New frmDocViewer
        Dim tmpRtf As String

        tmpRtf = GetCurrRtf()

        tmpfrm1.RichTextBox1.Rtf = tmpRtf
        tmpfrm1.ShowInTaskbar = True
        tmpfrm1.Show()

        Try 'undo dbl click checkbox
            If ListView1.Items(ListView1.SelectedIndices(0)).Checked = True Then
                ListView1.Items(ListView1.SelectedIndices(0)).Checked = False
            Else
                ListView1.Items(ListView1.SelectedIndices(0)).Checked = True
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub cmbAction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAction.SelectedIndexChanged

        Dim status As Boolean = False

        For count = 0 To ListView1.Items.Count - 1
            If ListView1.Items(count).Checked = True Or ListView1.SelectedItems.Count > 0 Then
                status = True
            End If
        Next


        If status = False Then
            If cmbAction.SelectedIndex > -1 Then MsgBox("No Document Selected")
            cmbAction.SelectedIndex = -1
            btnExit.Focus()
        Else
            If cmbAction.SelectedIndex = 0 Then SendToView()
            If cmbAction.SelectedIndex = 1 Then SendToEdit()
            If cmbAction.SelectedIndex = 2 Then SendToSharePoint()
            If cmbAction.SelectedIndex = 3 Then MergeToWord()
            If cmbAction.SelectedIndex = 4 Then CreateFiles()
            If cmbAction.SelectedIndex = 5 Then SendToZip()
        End If

    End Sub
End Class
