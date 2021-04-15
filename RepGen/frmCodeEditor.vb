Public Class frmCodeEditor

    Public ControlID As Integer
    Public SaveField As String
 
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
 
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub frmCodeEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        If SaveField = "Text" Then
            Me.RichTextBox1.Text = frmMain.catControlAttributes(frmMain.currControl).OnLoad
        Else
            Me.RichTextBox1.Text = frmMain.catControlAttributes(frmMain.currControl).DataBlock
        End If

        ControlID = frmMain.currControl

    End Sub

    Private Sub FileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileToolStripMenuItem.Click

        If SaveField = "Text" Then
            frmMain.catControlAttributes(ControlID).OnLoad = Me.RichTextBox1.Text
        Else
            frmMain.catControlAttributes(ControlID).DataBlock = Me.RichTextBox1.Text
        End If

        frmMain.catControlAttributes(ControlID).Tag = "edit"
        frmMain.SaveControlChanges()
        Me.Close()

    End Sub

    Private Sub ExecuteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExecuteToolStripMenuItem.Click
        frmMain.RunScript(Me.RichTextBox1.Text)
    End Sub
End Class