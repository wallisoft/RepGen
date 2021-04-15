Imports System.Drawing

Public Class frmFormEditor

    Private Sub newLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newLabel.Click
        frmMain.AddNewControl(0)
    End Sub

    Private Sub newButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newButton.Click
        frmMain.AddNewControl(1)
    End Sub

    Private Sub newTextBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newTextBox.Click
        frmMain.AddNewControl(2)
    End Sub

    Private Sub newCombo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newCombo.Click
        frmMain.AddNewControl(3)
    End Sub

    Private Sub newMultiLineTextBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newMultiLineTextBox.Click
        frmMain.AddNewControl(4)
    End Sub

    Private Sub newListBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newListBox.Click
        frmMain.AddNewControl(5)
    End Sub

    Private Sub btnTimer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTimer.Click
        frmMain.AddNewControl(7)
    End Sub

    Private Sub CheckBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.Click
        frmMain.AddNewControl(8)
    End Sub

    Private Sub RadioButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.Click
        frmMain.AddNewControl(9)
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        frmMain.AddNewControl(11)
    End Sub

    Private Sub frmFormEditor_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        frmMain.SaveControlChanges()

        frmMain.Timer1.Enabled = True

        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Me.Hide()
        End If

    End Sub

    Private Sub lstNewControl_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstNewControl.SelectedIndexChanged

        frmMain.SetControlAttributes()

    End Sub

    Private Sub btnCopyPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyPaste.Click


        Dim count As Integer

        'For count = 0 To frmMain.catControlAttributes.Length - 1
        '    If frmMain.catControlAttributes(count).Type = 11 Then
        '        frmMain.catControlAttributes(count).Size = "{Width=27, Height=28}"
        '        frmMain.catControlAttributes(count).Tag = "edit"
        '    End If
        'Next

        'frmMain.SaveControlChanges()
        'Exit Sub



        If lstNewControl.Items(6).Text <> "" Then

            Dim tmpArr As String()

            ReDim tmpArr(lstNewControl.Items.Count - 1)

            For count = 2 To tmpArr.Length - 1
                tmpArr(count) = lstNewControl.Items(count).Text
            Next

            frmMain.AddNewControl(lstNewControl.Items(6).Text)

            For count = 2 To tmpArr.Length - 1
                lstNewControl.Items(count).Text = tmpArr(count)
            Next

            lstNewControl.Items(11).Text = "new"

            frmMain.SetControlAttributes()
            frmMain.catControls(frmMain.currControl).Focus()

        End If

    End Sub

    Public Sub MoveControl(ByVal MoveX As Int16, ByRef MoveY As Int16)

        Dim tmpControl As Control
        Dim tmpStr As String = ""
        Dim x As Int16
        Dim y As Int16
        Dim tmpPnt As New Point
        Dim tmpID As Integer

        If Button5.BackColor = Color.Red Then

            For Each tmpControl In frmMain.catPictureBoxes(frmMain.currPanel).Controls

                tmpID = tmpControl.Name.Substring(10)

                x = frmMain.catControls(tmpID).location.x + MoveX
                y = frmMain.catControls(tmpID).location.y + MoveY

                If x < 0 Then x = 0
                If y < 0 Then y = 0

                tmpPnt = frmMain.catControls(tmpID).Location

                tmpPnt.X = x
                tmpPnt.Y = y

                frmMain.catControls(tmpID).location = tmpPnt

                frmMain.catControlAttributes(tmpID).Tag = "edit"
                frmMain.catControlAttributes(tmpID).Location = tmpControl.Location.ToString

            Next

        Else

            x = frmMain.catControls(frmMain.currControl).location.x + MoveX
            y = frmMain.catControls(frmMain.currControl).location.y + MoveY

            If x < 0 Then x = 0
            If y < 0 Then y = 0

            tmpPnt = frmMain.catControls(frmMain.currControl).Location

            tmpPnt.X = x
            tmpPnt.Y = y

            frmMain.catControls(frmMain.currControl).location = tmpPnt
            frmMain.SetControlAttributePosition()

        End If

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        MoveControl(0, -1)

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        MoveControl(0, 1)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        MoveControl(-1, 0)

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        MoveControl(1, 0)

    End Sub



    Private Sub frmFormEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Button5.BackColor = Color.Green
        frmMain.Timer1.Enabled = False ' stop screen garbage when dragging

    End Sub


    Private Sub Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub lstProperty_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstProperty.Enter

    End Sub


    Private Sub lstProperty_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstProperty.SelectedIndexChanged

    End Sub

    Private Sub lstNewControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstNewControl.Click

        If lstNewControl.SelectedIndices(0) = 19 And frmMain.currControl > -1 Then

            Dim fnt As New FontDialog
            Dim cvt As New FontConverter

            fnt.Font = frmMain.catControls(frmMain.currControl).Font
            fnt.ShowDialog()
            frmMain.catControls(frmMain.currControl).Font = fnt.Font
            lstNewControl.SelectedItems(0).Text = cvt.ConvertToString(fnt.Font)

        End If

        lstNewControl.SelectedItems(0).BeginEdit()

    End Sub

    Private Sub LinkSelected(ByVal strSelected As String)

        Dim count As Integer
        Dim tmpID As Integer
        Dim tmpArr() As String = Split(strSelected, ",")

        Array.Sort(tmpArr) 'assumes left is 0 #needs work#

        For count = 0 To tmpArr.Length - 1

            tmpID = frmMain.GetControlID(tmpArr(count))

            frmMain.catControlAttributes(tmpID).Child = tmpArr(0).Trim
            frmMain.catControlAttributes(tmpID).LinkedItems = strSelected.Trim
            frmMain.catControlAttributes(tmpID).Items = count
            frmMain.catControlAttributes(tmpID).Tag = "edit"

        Next

    End Sub

    Private Sub CopyID()

        Dim tmpStr As String = ""
        Dim count As Integer
        Dim strSelected As String = frmMain.ToolStripStatusLabel2.Text.Substring(19)
        Dim tmpArr() As String = Split(strSelected, ",")


        For count = 0 To tmpArr.Length - 1
            tmpStr += " + cint(c" & tmpArr(count).Trim & ".Tag)"
        Next

        My.Computer.Clipboard.SetText(tmpStr)

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        ComboBox1_Action()

    End Sub

    Private Sub ComboBox1_Action()

        Select Case ComboBox1.Text

            Case "Link Selected"
                LinkSelected(frmMain.ToolStripStatusLabel2.Text.Substring(19))

            Case "Copy ID's"
                CopyID()

            Case "Select All"
                SelectAll()

            Case "Show All"
                ShowAll()

            Case "Hide All"
                HideAll()

        End Select


    End Sub

    Private Sub HideAll()

        For count = 0 To frmMain.catControls.Length - 1
            If frmMain.catControlAttributes(count).Type = 11 Then
                frmMain.catControls(count).Text = "" 'catControlAttributes(count).ConText
            End If
        Next

    End Sub

    Private Sub ShowAll()

        For count = 0 To frmMain.catControls.Length - 1
            If frmMain.catControlAttributes(count).Type = 11 Then
                frmMain.catControls(count).Text = Chr(161) 'catControlAttributes(count).ConText
            End If
        Next

    End Sub

    Private Sub SelectAll()

        Button5.BackColor = Color.Red


    End Sub

    Private Sub btnLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLink.Click

        ComboBox1_Action()

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        If Button5.BackColor = Color.Red Then
            Button5.BackColor = Color.Green
        Else
            Button5.BackColor = Color.Red
        End If

    End Sub

    Private Sub lstProperty_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstProperty.SizeChanged

    End Sub

    Private Sub lstProperty_TabIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstProperty.TabIndexChanged

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click

        Dim strLinked As String = ""

        For Each tmpControl As Control In frmMain.catPictureBoxes(frmMain.currPanel).Controls

            For Each tmpControl1 As Control In frmMain.catPictureBoxes(frmMain.currPanel).Controls

                If frmMain.catControlAttributes(tmpControl.Name.Substring(10)).Type = 11 _
                    And frmMain.catControlAttributes(tmpControl1.Name.Substring(10)).Type = 11 Then

                    If tmpControl.Location.X < tmpControl1.Location.X + 60 And tmpControl.Location.X > tmpControl1.Location.X - 60 _
                        And tmpControl.Location.Y < tmpControl1.Location.Y + 5 And tmpControl.Location.Y > tmpControl1.Location.Y - 5 Then

                        If tmpControl.Name <> tmpControl1.Name Then
                            strLinked += "," & frmMain.catControlAttributes(tmpControl1.Name.Substring(10)).ControlID
                        End If

                    End If
                End If



            Next

            If strLinked.Length > 0 Then

                strLinked = frmMain.catControlAttributes(tmpControl.Name.Substring(10)).ControlID & strLinked
                LinkSelected(strLinked)

                strLinked = ""

            End If
        Next

    End Sub


End Class

