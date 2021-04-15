<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFormEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFormEditor))
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lstProperty = New System.Windows.Forms.ListView()
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstNewControl = New System.Windows.Forms.ListView()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnTimer = New System.Windows.Forms.Button()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.newListBox = New System.Windows.Forms.ListBox()
        Me.newMultiLineTextBox = New System.Windows.Forms.ListBox()
        Me.newButton = New System.Windows.Forms.Button()
        Me.newLabel = New System.Windows.Forms.TextBox()
        Me.newCombo = New System.Windows.Forms.ComboBox()
        Me.newTextBox = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.btnLink = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnCopyPaste = New System.Windows.Forms.Button()
        Me.chkSnap = New System.Windows.Forms.CheckBox()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.lstProperty)
        Me.Panel4.Controls.Add(Me.lstNewControl)
        Me.Panel4.Location = New System.Drawing.Point(152, 27)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(217, 409)
        Me.Panel4.TabIndex = 30
        '
        'lstProperty
        '
        Me.lstProperty.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me.lstProperty.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstProperty.GridLines = True
        Me.lstProperty.Location = New System.Drawing.Point(-2, -1)
        Me.lstProperty.Name = "lstProperty"
        Me.lstProperty.Size = New System.Drawing.Size(89, 409)
        Me.lstProperty.TabIndex = 26
        Me.lstProperty.UseCompatibleStateImageBehavior = False
        Me.lstProperty.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Property"
        Me.ColumnHeader2.Width = 84
        '
        'lstNewControl
        '
        Me.lstNewControl.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3})
        Me.lstNewControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstNewControl.FullRowSelect = True
        Me.lstNewControl.GridLines = True
        Me.lstNewControl.LabelEdit = True
        Me.lstNewControl.LabelWrap = False
        Me.lstNewControl.Location = New System.Drawing.Point(84, -1)
        Me.lstNewControl.MultiSelect = False
        Me.lstNewControl.Name = "lstNewControl"
        Me.lstNewControl.Size = New System.Drawing.Size(133, 409)
        Me.lstNewControl.TabIndex = 27
        Me.lstNewControl.UseCompatibleStateImageBehavior = False
        Me.lstNewControl.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Value"
        Me.ColumnHeader3.Width = 125
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(177, 7)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(165, 16)
        Me.Label5.TabIndex = 28
        Me.Label5.Text = "Selected Control Attributes"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(17, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(111, 16)
        Me.Label4.TabIndex = 27
        Me.Label4.Text = " Add New Control"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.btnTimer)
        Me.Panel2.Controls.Add(Me.CheckBox1)
        Me.Panel2.Controls.Add(Me.RadioButton1)
        Me.Panel2.Controls.Add(Me.newListBox)
        Me.Panel2.Controls.Add(Me.newMultiLineTextBox)
        Me.Panel2.Controls.Add(Me.newButton)
        Me.Panel2.Controls.Add(Me.newLabel)
        Me.Panel2.Controls.Add(Me.newCombo)
        Me.Panel2.Controls.Add(Me.newTextBox)
        Me.Panel2.Location = New System.Drawing.Point(3, 27)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(142, 409)
        Me.Panel2.TabIndex = 26
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(39, 287)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 13)
        Me.Label3.TabIndex = 39
        Me.Label3.Text = "Transparent Circle"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label2.Location = New System.Drawing.Point(4, 275)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 37)
        Me.Label2.TabIndex = 38
        Me.Label2.Text = "O"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(80, 252)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 37
        Me.Label1.Text = "Timer"
        '
        'btnTimer
        '
        Me.btnTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTimer.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTimer.Location = New System.Drawing.Point(12, 243)
        Me.btnTimer.Name = "btnTimer"
        Me.btnTimer.Size = New System.Drawing.Size(59, 29)
        Me.btnTimer.TabIndex = 36
        Me.btnTimer.Text = "000.0"
        Me.btnTimer.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(109, 16)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox1.TabIndex = 35
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(83, 15)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton1.TabIndex = 34
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'newListBox
        '
        Me.newListBox.FormattingEnabled = True
        Me.newListBox.Items.AddRange(New Object() {"ListBox", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""})
        Me.newListBox.Location = New System.Drawing.Point(12, 180)
        Me.newListBox.Name = "newListBox"
        Me.newListBox.Size = New System.Drawing.Size(120, 56)
        Me.newListBox.TabIndex = 13
        '
        'newMultiLineTextBox
        '
        Me.newMultiLineTextBox.FormattingEnabled = True
        Me.newMultiLineTextBox.Items.AddRange(New Object() {"MultiLine TextBox", "", "", "", "", "", "", "", "", "", "", "", "", ""})
        Me.newMultiLineTextBox.Location = New System.Drawing.Point(10, 118)
        Me.newMultiLineTextBox.Name = "newMultiLineTextBox"
        Me.newMultiLineTextBox.Size = New System.Drawing.Size(120, 56)
        Me.newMultiLineTextBox.TabIndex = 12
        '
        'newButton
        '
        Me.newButton.Location = New System.Drawing.Point(15, 10)
        Me.newButton.Name = "newButton"
        Me.newButton.Size = New System.Drawing.Size(56, 23)
        Me.newButton.TabIndex = 7
        Me.newButton.Text = "Button"
        Me.newButton.UseVisualStyleBackColor = True
        '
        'newLabel
        '
        Me.newLabel.BackColor = System.Drawing.SystemColors.Control
        Me.newLabel.Location = New System.Drawing.Point(9, 39)
        Me.newLabel.Name = "newLabel"
        Me.newLabel.ReadOnly = True
        Me.newLabel.Size = New System.Drawing.Size(121, 20)
        Me.newLabel.TabIndex = 11
        Me.newLabel.Text = "Text"
        '
        'newCombo
        '
        Me.newCombo.FormattingEnabled = True
        Me.newCombo.Location = New System.Drawing.Point(9, 91)
        Me.newCombo.Name = "newCombo"
        Me.newCombo.Size = New System.Drawing.Size(121, 21)
        Me.newCombo.TabIndex = 10
        Me.newCombo.Text = "ComboBox"
        '
        'newTextBox
        '
        Me.newTextBox.Location = New System.Drawing.Point(9, 65)
        Me.newTextBox.Name = "newTextBox"
        Me.newTextBox.Size = New System.Drawing.Size(121, 20)
        Me.newTextBox.TabIndex = 9
        Me.newTextBox.Text = "TextBox"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button7)
        Me.Panel1.Controls.Add(Me.Button6)
        Me.Panel1.Controls.Add(Me.Button5)
        Me.Panel1.Controls.Add(Me.ComboBox1)
        Me.Panel1.Controls.Add(Me.btnLink)
        Me.Panel1.Controls.Add(Me.Button4)
        Me.Panel1.Controls.Add(Me.Button3)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.btnCopyPaste)
        Me.Panel1.Controls.Add(Me.chkSnap)
        Me.Panel1.Location = New System.Drawing.Point(3, 442)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(366, 100)
        Me.Panel1.TabIndex = 31
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.SystemColors.Control
        Me.Button7.Location = New System.Drawing.Point(197, 52)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(75, 23)
        Me.Button7.TabIndex = 50
        Me.Button7.Text = "AutoLink"
        Me.Button7.UseVisualStyleBackColor = False
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.SystemColors.Control
        Me.Button6.Location = New System.Drawing.Point(197, 24)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(75, 23)
        Me.Button6.TabIndex = 49
        Me.Button6.Text = "AutoArrange"
        Me.Button6.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Location = New System.Drawing.Point(303, 43)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(15, 16)
        Me.Button5.TabIndex = 48
        Me.Button5.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Select All", "Link Selected", "Copy ID's", "Delete Selected", "Paste Selected", "Show All", "Hide All"})
        Me.ComboBox1.Location = New System.Drawing.Point(13, 54)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(113, 21)
        Me.ComboBox1.TabIndex = 47
        Me.ComboBox1.Text = "Choose Action"
        '
        'btnLink
        '
        Me.btnLink.BackColor = System.Drawing.SystemColors.Control
        Me.btnLink.Location = New System.Drawing.Point(136, 53)
        Me.btnLink.Name = "btnLink"
        Me.btnLink.Size = New System.Drawing.Size(54, 23)
        Me.btnLink.TabIndex = 46
        Me.btnLink.Text = "Repeat"
        Me.btnLink.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button4.Location = New System.Drawing.Point(303, 58)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(15, 16)
        Me.Button4.TabIndex = 45
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.Location = New System.Drawing.Point(317, 43)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(15, 16)
        Me.Button3.TabIndex = 44
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Location = New System.Drawing.Point(289, 43)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(15, 16)
        Me.Button2.TabIndex = 43
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Location = New System.Drawing.Point(303, 28)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(15, 16)
        Me.Button1.TabIndex = 42
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnCopyPaste
        '
        Me.btnCopyPaste.BackColor = System.Drawing.SystemColors.Control
        Me.btnCopyPaste.Location = New System.Drawing.Point(136, 24)
        Me.btnCopyPaste.Name = "btnCopyPaste"
        Me.btnCopyPaste.Size = New System.Drawing.Size(55, 23)
        Me.btnCopyPaste.TabIndex = 41
        Me.btnCopyPaste.Text = "Copy"
        Me.btnCopyPaste.UseVisualStyleBackColor = False
        '
        'chkSnap
        '
        Me.chkSnap.AutoSize = True
        Me.chkSnap.Checked = True
        Me.chkSnap.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSnap.Location = New System.Drawing.Point(25, 26)
        Me.chkSnap.Name = "chkSnap"
        Me.chkSnap.Size = New System.Drawing.Size(89, 17)
        Me.chkSnap.TabIndex = 40
        Me.chkSnap.Text = "Snap To Grid"
        Me.chkSnap.UseVisualStyleBackColor = True
        '
        'frmFormEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(373, 544)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(990, 200)
        Me.MaximizeBox = False
        Me.Name = "frmFormEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Form Editor"
        Me.TopMost = True
        Me.Panel4.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents lstProperty As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lstNewControl As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents newListBox As System.Windows.Forms.ListBox
    Friend WithEvents newMultiLineTextBox As System.Windows.Forms.ListBox
    Friend WithEvents newButton As System.Windows.Forms.Button
    Friend WithEvents newLabel As System.Windows.Forms.TextBox
    Friend WithEvents newCombo As System.Windows.Forms.ComboBox
    Friend WithEvents newTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents btnTimer As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents btnLink As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnCopyPaste As System.Windows.Forms.Button
    Friend WithEvents chkSnap As System.Windows.Forms.CheckBox
    Friend WithEvents Button7 As System.Windows.Forms.Button
End Class
