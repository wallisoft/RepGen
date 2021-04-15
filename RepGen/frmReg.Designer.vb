<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReg
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReg))
        Me.btnExit = New System.Windows.Forms.Button
        Me.chkTerms = New System.Windows.Forms.CheckBox
        Me.rtbEULA = New System.Windows.Forms.RichTextBox
        Me.lblEULA = New System.Windows.Forms.Label
        Me.btnCompleteRegistration = New System.Windows.Forms.Button
        Me.btnContinueTrial = New System.Windows.Forms.Button
        Me.txtKey = New System.Windows.Forms.TextBox
        Me.lblRegNumber = New System.Windows.Forms.Label
        Me.txtLock = New System.Windows.Forms.TextBox
        Me.lblCode = New System.Windows.Forms.Label
        Me.lblDays = New System.Windows.Forms.Label
        Me.lblDaysLeft = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(300, 420)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 0
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'chkTerms
        '
        Me.chkTerms.AutoSize = True
        Me.chkTerms.Location = New System.Drawing.Point(15, 276)
        Me.chkTerms.Name = "chkTerms"
        Me.chkTerms.Size = New System.Drawing.Size(150, 17)
        Me.chkTerms.TabIndex = 22
        Me.chkTerms.Text = "I agree to the terms above"
        Me.chkTerms.UseVisualStyleBackColor = True
        '
        'rtbEULA
        '
        Me.rtbEULA.Location = New System.Drawing.Point(12, 29)
        Me.rtbEULA.Name = "rtbEULA"
        Me.rtbEULA.Size = New System.Drawing.Size(363, 241)
        Me.rtbEULA.TabIndex = 21
        Me.rtbEULA.Text = resources.GetString("rtbEULA.Text")
        '
        'lblEULA
        '
        Me.lblEULA.AutoSize = True
        Me.lblEULA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEULA.Location = New System.Drawing.Point(12, 13)
        Me.lblEULA.Name = "lblEULA"
        Me.lblEULA.Size = New System.Drawing.Size(171, 13)
        Me.lblEULA.TabIndex = 20
        Me.lblEULA.Text = "End User License Agreement"
        '
        'btnCompleteRegistration
        '
        Me.btnCompleteRegistration.Enabled = False
        Me.btnCompleteRegistration.Location = New System.Drawing.Point(12, 420)
        Me.btnCompleteRegistration.Name = "btnCompleteRegistration"
        Me.btnCompleteRegistration.Size = New System.Drawing.Size(215, 23)
        Me.btnCompleteRegistration.TabIndex = 19
        Me.btnCompleteRegistration.Text = "Complete Registration"
        Me.btnCompleteRegistration.UseVisualStyleBackColor = True
        '
        'btnContinueTrial
        '
        Me.btnContinueTrial.Enabled = False
        Me.btnContinueTrial.Location = New System.Drawing.Point(12, 391)
        Me.btnContinueTrial.Name = "btnContinueTrial"
        Me.btnContinueTrial.Size = New System.Drawing.Size(215, 23)
        Me.btnContinueTrial.TabIndex = 18
        Me.btnContinueTrial.Text = "Continue Trial"
        Me.btnContinueTrial.UseVisualStyleBackColor = True
        '
        'txtKey
        '
        Me.txtKey.Enabled = False
        Me.txtKey.Location = New System.Drawing.Point(127, 365)
        Me.txtKey.Name = "txtKey"
        Me.txtKey.Size = New System.Drawing.Size(100, 20)
        Me.txtKey.TabIndex = 17
        '
        'lblRegNumber
        '
        Me.lblRegNumber.AutoSize = True
        Me.lblRegNumber.Location = New System.Drawing.Point(12, 368)
        Me.lblRegNumber.Name = "lblRegNumber"
        Me.lblRegNumber.Size = New System.Drawing.Size(109, 13)
        Me.lblRegNumber.TabIndex = 16
        Me.lblRegNumber.Text = "Registration Number: "
        '
        'txtLock
        '
        Me.txtLock.Location = New System.Drawing.Point(127, 339)
        Me.txtLock.Name = "txtLock"
        Me.txtLock.ReadOnly = True
        Me.txtLock.Size = New System.Drawing.Size(100, 20)
        Me.txtLock.TabIndex = 15
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.Location = New System.Drawing.Point(12, 342)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(80, 13)
        Me.lblCode.TabIndex = 14
        Me.lblCode.Text = "Program Code: "
        '
        'lblDays
        '
        Me.lblDays.AutoSize = True
        Me.lblDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDays.Location = New System.Drawing.Point(125, 323)
        Me.lblDays.Name = "lblDays"
        Me.lblDays.Size = New System.Drawing.Size(14, 13)
        Me.lblDays.TabIndex = 13
        Me.lblDays.Text = "0"
        '
        'lblDaysLeft
        '
        Me.lblDaysLeft.AutoSize = True
        Me.lblDaysLeft.Location = New System.Drawing.Point(12, 323)
        Me.lblDaysLeft.Name = "lblDaysLeft"
        Me.lblDaysLeft.Size = New System.Drawing.Size(93, 13)
        Me.lblDaysLeft.TabIndex = 12
        Me.lblDaysLeft.Text = "Days Left In Trial: "
        '
        'frmReg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(386, 465)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.chkTerms)
        Me.Controls.Add(Me.rtbEULA)
        Me.Controls.Add(Me.lblEULA)
        Me.Controls.Add(Me.btnCompleteRegistration)
        Me.Controls.Add(Me.btnContinueTrial)
        Me.Controls.Add(Me.txtKey)
        Me.Controls.Add(Me.lblRegNumber)
        Me.Controls.Add(Me.txtLock)
        Me.Controls.Add(Me.lblCode)
        Me.Controls.Add(Me.lblDays)
        Me.Controls.Add(Me.lblDaysLeft)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Registration"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents chkTerms As System.Windows.Forms.CheckBox
    Friend WithEvents rtbEULA As System.Windows.Forms.RichTextBox
    Friend WithEvents lblEULA As System.Windows.Forms.Label
    Friend WithEvents btnCompleteRegistration As System.Windows.Forms.Button
    Friend WithEvents btnContinueTrial As System.Windows.Forms.Button
    Friend WithEvents txtKey As System.Windows.Forms.TextBox
    Friend WithEvents lblRegNumber As System.Windows.Forms.Label
    Friend WithEvents txtLock As System.Windows.Forms.TextBox
    Friend WithEvents lblCode As System.Windows.Forms.Label
    Friend WithEvents lblDays As System.Windows.Forms.Label
    Friend WithEvents lblDaysLeft As System.Windows.Forms.Label

End Class
