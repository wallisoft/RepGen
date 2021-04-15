<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtPrev1 = New System.Windows.Forms.TextBox
        Me.txtPrev2 = New System.Windows.Forms.TextBox
        Me.txtPrev3 = New System.Windows.Forms.TextBox
        Me.txtPrev4 = New System.Windows.Forms.TextBox
        Me.txtPrev5 = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtDataBlockTag = New System.Windows.Forms.TextBox
        Me.txtRow = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.chkTag = New System.Windows.Forms.CheckBox
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(277, 289)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Confirm"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Column 1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 142)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Column 2"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 178)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Column 3"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 213)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Column 4"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 249)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Column 5"
        '
        'txtPrev1
        '
        Me.txtPrev1.Location = New System.Drawing.Point(70, 104)
        Me.txtPrev1.Name = "txtPrev1"
        Me.txtPrev1.Size = New System.Drawing.Size(350, 20)
        Me.txtPrev1.TabIndex = 11
        '
        'txtPrev2
        '
        Me.txtPrev2.Location = New System.Drawing.Point(70, 139)
        Me.txtPrev2.Name = "txtPrev2"
        Me.txtPrev2.Size = New System.Drawing.Size(350, 20)
        Me.txtPrev2.TabIndex = 12
        '
        'txtPrev3
        '
        Me.txtPrev3.Location = New System.Drawing.Point(70, 175)
        Me.txtPrev3.Name = "txtPrev3"
        Me.txtPrev3.Size = New System.Drawing.Size(350, 20)
        Me.txtPrev3.TabIndex = 13
        '
        'txtPrev4
        '
        Me.txtPrev4.Location = New System.Drawing.Point(70, 210)
        Me.txtPrev4.Name = "txtPrev4"
        Me.txtPrev4.Size = New System.Drawing.Size(350, 20)
        Me.txtPrev4.TabIndex = 14
        '
        'txtPrev5
        '
        Me.txtPrev5.Location = New System.Drawing.Point(70, 246)
        Me.txtPrev5.Name = "txtPrev5"
        Me.txtPrev5.Size = New System.Drawing.Size(350, 20)
        Me.txtPrev5.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(67, 79)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Data Preview"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(29, 34)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 13)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "Name"
        '
        'txtDataBlockTag
        '
        Me.txtDataBlockTag.Location = New System.Drawing.Point(70, 30)
        Me.txtDataBlockTag.Name = "txtDataBlockTag"
        Me.txtDataBlockTag.Size = New System.Drawing.Size(100, 20)
        Me.txtDataBlockTag.TabIndex = 17
        '
        'txtRow
        '
        Me.txtRow.AutoSize = True
        Me.txtRow.Location = New System.Drawing.Point(24, 302)
        Me.txtRow.Name = "txtRow"
        Me.txtRow.Size = New System.Drawing.Size(42, 13)
        Me.txtRow.TabIndex = 19
        Me.txtRow.Text = "Row # "
        '
        'Button1
        '
        Me.Button1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Button1.Location = New System.Drawing.Point(172, 292)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(67, 23)
        Me.Button1.TabIndex = 20
        Me.Button1.Text = "Special"
        '
        'chkTag
        '
        Me.chkTag.AutoSize = True
        Me.chkTag.Location = New System.Drawing.Point(198, 33)
        Me.chkTag.Name = "chkTag"
        Me.chkTag.Size = New System.Drawing.Size(92, 17)
        Me.chkTag.TabIndex = 22
        Me.chkTag.Text = "Use Column 1"
        Me.chkTag.UseVisualStyleBackColor = True
        '
        'frmImport
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(435, 330)
        Me.Controls.Add(Me.chkTag)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtRow)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtDataBlockTag)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtPrev5)
        Me.Controls.Add(Me.txtPrev4)
        Me.Controls.Add(Me.txtPrev3)
        Me.Controls.Add(Me.txtPrev2)
        Me.Controls.Add(Me.txtPrev1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmImport"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Import DataBlock"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtPrev1 As System.Windows.Forms.TextBox
    Friend WithEvents txtPrev2 As System.Windows.Forms.TextBox
    Friend WithEvents txtPrev3 As System.Windows.Forms.TextBox
    Friend WithEvents txtPrev4 As System.Windows.Forms.TextBox
    Friend WithEvents txtPrev5 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtDataBlockTag As System.Windows.Forms.TextBox
    Friend WithEvents txtRow As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents chkTag As System.Windows.Forms.CheckBox

End Class
