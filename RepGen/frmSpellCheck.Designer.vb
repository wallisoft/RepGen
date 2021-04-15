<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSpellCheck
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
        Me.btnIgnore = New System.Windows.Forms.Button
        Me.btnIgnoreAll = New System.Windows.Forms.Button
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnChangeAll = New System.Windows.Forms.Button
        Me.btnAdd2Dict = New System.Windows.Forms.Button
        Me.Word1 = New System.Windows.Forms.TextBox
        Me.Word2 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnIgnore
        '
        Me.btnIgnore.Location = New System.Drawing.Point(150, 28)
        Me.btnIgnore.Name = "btnIgnore"
        Me.btnIgnore.Size = New System.Drawing.Size(75, 23)
        Me.btnIgnore.TabIndex = 3
        Me.btnIgnore.Text = "Ignore"
        Me.btnIgnore.UseVisualStyleBackColor = True
        '
        'btnIgnoreAll
        '
        Me.btnIgnoreAll.Location = New System.Drawing.Point(231, 28)
        Me.btnIgnoreAll.Name = "btnIgnoreAll"
        Me.btnIgnoreAll.Size = New System.Drawing.Size(75, 23)
        Me.btnIgnoreAll.TabIndex = 4
        Me.btnIgnoreAll.Text = "Ignore All"
        Me.btnIgnoreAll.UseVisualStyleBackColor = True
        '
        'btnChange
        '
        Me.btnChange.Location = New System.Drawing.Point(150, 113)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(75, 23)
        Me.btnChange.TabIndex = 5
        Me.btnChange.Text = "Change"
        Me.btnChange.UseVisualStyleBackColor = True
        '
        'btnChangeAll
        '
        Me.btnChangeAll.Location = New System.Drawing.Point(231, 113)
        Me.btnChangeAll.Name = "btnChangeAll"
        Me.btnChangeAll.Size = New System.Drawing.Size(75, 23)
        Me.btnChangeAll.TabIndex = 6
        Me.btnChangeAll.Text = "Change All"
        Me.btnChangeAll.UseVisualStyleBackColor = True
        '
        'btnAdd2Dict
        '
        Me.btnAdd2Dict.Location = New System.Drawing.Point(150, 57)
        Me.btnAdd2Dict.Name = "btnAdd2Dict"
        Me.btnAdd2Dict.Size = New System.Drawing.Size(73, 23)
        Me.btnAdd2Dict.TabIndex = 7
        Me.btnAdd2Dict.Text = "Add To Dict"
        Me.btnAdd2Dict.UseVisualStyleBackColor = True
        '
        'Word1
        '
        Me.Word1.Location = New System.Drawing.Point(8, 30)
        Me.Word1.Name = "Word1"
        Me.Word1.Size = New System.Drawing.Size(136, 20)
        Me.Word1.TabIndex = 8
        '
        'Word2
        '
        Me.Word2.Location = New System.Drawing.Point(8, 115)
        Me.Word2.Name = "Word2"
        Me.Word2.Size = New System.Drawing.Size(136, 20)
        Me.Word2.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Not Found"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 99)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Change To"
        '
        'frmSpellCheck
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(319, 159)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Word2)
        Me.Controls.Add(Me.Word1)
        Me.Controls.Add(Me.btnAdd2Dict)
        Me.Controls.Add(Me.btnChangeAll)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.btnIgnoreAll)
        Me.Controls.Add(Me.btnIgnore)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSpellCheck"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Spell Check"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnIgnore As System.Windows.Forms.Button
    Friend WithEvents btnIgnoreAll As System.Windows.Forms.Button
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents btnChangeAll As System.Windows.Forms.Button
    Friend WithEvents btnAdd2Dict As System.Windows.Forms.Button
    Friend WithEvents Word1 As System.Windows.Forms.TextBox
    Friend WithEvents Word2 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label

End Class
