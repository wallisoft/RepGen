Imports System.Data
Imports System.Data.Odbc
Imports System.Data.Sql
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public Class frmActionScript

    Public appVars As clsVariables
    Public currTextBox As TextBox
    Public currSelStart As Integer
    Public currSelEnd As Integer




    Private Sub frmDataBlock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        setupCmb()

        txtSQL.Text = frmMain.catControlAttributes(appVars.ControlID).DataBlock
        txtChSql.Text = frmMain.catControlAttributes(appVars.ControlID).Action
        cmbChild.Text = frmMain.catControlAttributes(appVars.ControlID).Child.ToString

        txtControlID.Text = "ControlID : " & frmMain.catControlAttributes(appVars.ControlID).ControlID

    End Sub

    Private Sub setupCmb()

        Dim SQLcommand As OdbcCommand
        SQLcommand = frmMain.connStr.CreateCommand
        SQLcommand.CommandText = "SELECT DISTINCT Tag FROM DataBlocks ORDER BY 1 ASC"
        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        cmbChooseDb.Items.Add("NONE")

        While SQLreader.Read()
            cmbChooseDb.Items.Add(SQLreader(0))
        End While

        For count = 0 To frmMain.catControls.Length - 1
            cmbChild.Items.Add(frmMain.catControlAttributes(count).ControlID)
        Next

        cmbVariable.Items.Add("#CRLF#")
        cmbVariable.Items.Add("#UserFN#")
        cmbVariable.Items.Add("#UserSN#")
        cmbVariable.Items.Add("#Date#")
        cmbVariable.Items.Add("#Time#")
        cmbVariable.Items.Add("#Age#")
        cmbVariable.Items.Add("#Title#")
        cmbVariable.Items.Add("#Fname#")
        cmbVariable.Items.Add("#Sname#")
        cmbVariable.Items.Add("#DOB#")
        cmbVariable.Items.Add("#SSN#")
        cmbVariable.Items.Add("#Ref#")
        cmbVariable.Items.Add("#Sex#")
        cmbVariable.Items.Add("#Ethnicity#")
        cmbVariable.Items.Add("#City#")
        cmbVariable.Items.Add("#State#")
        cmbVariable.Items.Add("#Zip#")
        cmbVariable.Items.Add("#HomePhone#")
        cmbVariable.Items.Add("#WorkPhone#")
        cmbVariable.Items.Add("#CellPhone#")
        cmbVariable.Items.Add("#Email#")
        cmbVariable.Items.Add("#Template#")
        cmbVariable.Items.Add("#Notes#")
        cmbVariable.Items.Add("#VAL000#")
        cmbVariable.Items.Add("#ClientID#")



    End Sub

    Private Sub LoadControl()

        Dim intChild As Integer
        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader = Nothing
        SQLcommand = frmMain.connStr.CreateCommand
        Dim status As Integer = 0

        If cmbChild.Text.Length > 0 Then
            intChild = Convert.ToInt32(cmbChild.Text)
        Else
            intChild = -1
        End If

        Dim tmpVal As Integer = appVars.ControlID

        If txtSql.Text.Trim.Length > 0 Then

            SQLcommand.CommandText = frmMain.Report.ProcessRepVals(txtSql.Text)

            Try
                SQLreader = SQLcommand.ExecuteReader()
            Catch ex As Exception
                status = 1
            End Try


            If (frmMain.catControlAttributes(appVars.ControlID).Type = 5 Or _
            frmMain.catControlAttributes(appVars.ControlID).Type = 3) And status = 0 Then

                frmMain.catControls(appVars.ControlID).Items.Clear()
                While SQLreader.Read()
                    frmMain.catControls(appVars.ControlID).Items.Add(SQLreader(0))
                End While
            ElseIf frmMain.catControlAttributes(appVars.ControlID).Type = 2 And status = 0 Then
                frmMain.catControls(tmpVal).Text = ""
                While SQLreader.Read()
                    frmMain.catControls(appVars.ControlID).Text = frmMain.catControls(appVars.ControlID).Text & _
                    SQLreader(0) & " "
                End While
            ElseIf status = 0 Then
                frmMain.catControls(tmpVal).Text = ""
                While SQLreader.Read()
                    frmMain.catControls(appVars.ControlID).Text = frmMain.catControls(appVars.ControlID).Text & _
                    SQLreader(0) & vbCrLf
                End While
            End If



        End If

        If cmbVariable.Text.Length > 0 And frmMain.catControlAttributes(appVars.ControlID).Type < 4 Then
            frmMain.catControlAttributes(appVars.ControlID).ConText += cmbVariable.Text
            frmMain.catControls(appVars.ControlID).Text += cmbVariable.Text
        End If

        If cmbChild.Text = "" Then cmbChild.Text = -1

        frmMain.catControlAttributes(appVars.ControlID).DataBlock = txtSql.Text
        frmMain.catControlAttributes(appVars.ControlID).Action = txtChSql.Text
        frmMain.catControlAttributes(appVars.ControlID).Child = cmbChild.Text
        If frmMain.catControlAttributes(appVars.ControlID).Tag.Length = 0 Then
            frmMain.catControlAttributes(appVars.ControlID).Tag = "edit"

            If frmFormEditor.Visible = False Then
                frmMain.SaveControlChanges()
            End If
        End If

    End Sub


    Private Sub cmbChooseDb_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If cmbChooseDb.Text = "NONE" Then
            txtSql.Text = ""
            cmbChooseDb.Text = ""
        Else
            txtSql.Text = "SELECT DISTINCT Text1 From DataBlocks WHERE Tag = '" & cmbChooseDb.Text & "'" & _
            " ORDER BY Text1"
        End If

        cmbVariable.Text = ""

    End Sub


    Private Sub txtSQL_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)

        currTextBox = sender
        currSelStart = currTextBox.SelectionStart
        currSelEnd = currTextBox.SelectionLength

    End Sub


    Private Sub cmbVariable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)


        currTextBox.Focus()

        currTextBox.Text = currTextBox.Text.Substring(0, currSelStart) & cmbVariable.Text & _
            currTextBox.Text.Substring(currSelStart + currSelEnd)

        currTextBox.SelectionStart = currSelStart + currSelEnd

    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If appVars.ControlID > -1 Then
            LoadControl()
        End If

        appVars.ControlID = -1

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub SetFont(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFontChInsTxt.Click, btnFontChPreTxt.Click, btnFontChSub.Click, btnFontInsTxt.Click, btnFontPreTxt.Click, btnFontSub.Click

        Dim fnt As New FontDialog
        fnt.ShowDialog()

    End Sub

    Private Sub CleadData()

        Dim SQLcommand As OdbcCommand
        Dim SQLcommand1 As OdbcCommand
        SQLcommand = frmMain.connStr.CreateCommand
        SQLcommand1 = frmMain.connStr.CreateCommand

        Dim DataBlockID As Integer
        Dim tag As String
        Dim text1 As String
        Dim text2 As String
        Dim text3 As String
        Dim text4 As String
        Dim text5 As String


        SQLcommand.CommandText = "SELECT * FROM DataBlocks "

        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()
        Dim count As Integer = 0

        SQLcommand1.CommandText = "BEGIN"
        SQLcommand1.ExecuteNonQuery()


        While SQLreader.Read()

            DataBlockID = SQLreader(0)
            tag = SQLreader(1)
            text1 = SQLreader(2).ToString.Replace("""", "")
            text2 = SQLreader(3).ToString.Replace("""", "")
            text3 = SQLreader(4).ToString.Replace("""", "")
            text4 = SQLreader(5).ToString.Replace("""", "")
            text5 = SQLreader(6).ToString.Replace("""", "")


            SQLcommand1.CommandText = "update DataBlocks set Tag = '" & tag.Replace("'", "''") & "' , " & _
            " Text1 = '" & text1.Replace("'", "''") & "', " & _
            " Text2 = '" & text2.Replace("'", "''") & "', " & _
            " Text3 = '" & text3.Replace("'", "''") & "', " & _
            " Text4 = '" & text4.Replace("'", "''") & "', " & _
            " Text5 = '" & text5.Replace("'", "''") & "' " & _
            " where DataBlockID = " & DataBlockID

            SQLcommand1.ExecuteNonQuery()

            count += 1

            'Button1.Text = count

        End While

        SQLcommand1.CommandText = "COMMIT"
        SQLcommand1.ExecuteNonQuery()

    End Sub

    Private Sub btnDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefault.Click

        txtSql.Text = "SELECT Text2 FROM DataBlocks Where Tag = 'DefaultVals' AND Text1 = '" & _
            frmMain.catControlAttributes(appVars.ControlID).ControlID & "'"

    End Sub



    Private Sub btnData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnData.Click

        Dim frmdDataBlockMaint As frmDataBlockMaint = New frmDataBlockMaint

        frmdDataBlockMaint.appVars = Me.appVars
        frmdDataBlockMaint.defControl = frmMain.catControlAttributes(appVars.ControlID).ControlID
        frmdDataBlockMaint.defSQL = txtSql.Text
        frmdDataBlockMaint.defTag = "DefaultVals"
        frmdDataBlockMaint.defText1 = frmMain.catControlAttributes(appVars.ControlID).ControlID

        frmdDataBlockMaint.ShowDialog()


    End Sub
End Class
