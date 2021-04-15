<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTemplateMaint
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTemplateMaint))
        Me.lblSelectTemplate = New System.Windows.Forms.Label
        Me.cmbTemplate = New System.Windows.Forms.ComboBox
        Me.lblTemplateName = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnConfirm = New System.Windows.Forms.Button
        Me.btnFrmDelete = New System.Windows.Forms.Button
        Me.cmbCategories = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnQuestionsAll = New System.Windows.Forms.Button
        Me.lvQuestions = New System.Windows.Forms.ListView
        Me.lvCategory = New System.Windows.Forms.ListView
        Me.BtnCategoryDel = New System.Windows.Forms.Button
        Me.btnCategoriesAll = New System.Windows.Forms.Button
        Me.lvTemplateQuestions = New System.Windows.Forms.ListView
        Me.txtTemplateNew = New System.Windows.Forms.TextBox
        Me.btnTemplateQuestionDelete = New System.Windows.Forms.Button
        Me.btnTemplateQuestionDown = New System.Windows.Forms.Button
        Me.btnTemplateQuestionUp = New System.Windows.Forms.Button
        Me.btnCategoryDown = New System.Windows.Forms.Button
        Me.btnCategoryUp = New System.Windows.Forms.Button
        Me.BtnQuestionAdd = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.cmbDataBlock = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblSelectTemplate
        '
        Me.lblSelectTemplate.AutoSize = True
        Me.lblSelectTemplate.Location = New System.Drawing.Point(16, 15)
        Me.lblSelectTemplate.Name = "lblSelectTemplate"
        Me.lblSelectTemplate.Size = New System.Drawing.Size(121, 13)
        Me.lblSelectTemplate.TabIndex = 0
        Me.lblSelectTemplate.Text = "Select Template To Edit"
        '
        'cmbTemplate
        '
        Me.cmbTemplate.FormattingEnabled = True
        Me.cmbTemplate.Location = New System.Drawing.Point(15, 31)
        Me.cmbTemplate.Name = "cmbTemplate"
        Me.cmbTemplate.Size = New System.Drawing.Size(152, 21)
        Me.cmbTemplate.TabIndex = 2
        '
        'lblTemplateName
        '
        Me.lblTemplateName.AutoSize = True
        Me.lblTemplateName.Location = New System.Drawing.Point(173, 15)
        Me.lblTemplateName.Name = "lblTemplateName"
        Me.lblTemplateName.Size = New System.Drawing.Size(110, 13)
        Me.lblTemplateName.TabIndex = 11
        Me.lblTemplateName.Text = "Create New Template"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(447, 485)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "Exit"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnConfirm
        '
        Me.btnConfirm.Location = New System.Drawing.Point(528, 485)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(75, 23)
        Me.btnConfirm.TabIndex = 14
        Me.btnConfirm.Text = "Confirm"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'btnFrmDelete
        '
        Me.btnFrmDelete.Location = New System.Drawing.Point(366, 485)
        Me.btnFrmDelete.Name = "btnFrmDelete"
        Me.btnFrmDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnFrmDelete.TabIndex = 66
        Me.btnFrmDelete.Text = "Delete"
        Me.btnFrmDelete.UseVisualStyleBackColor = True
        '
        'cmbCategories
        '
        Me.cmbCategories.FormattingEnabled = True
        Me.cmbCategories.Location = New System.Drawing.Point(15, 134)
        Me.cmbCategories.Name = "cmbCategories"
        Me.cmbCategories.Size = New System.Drawing.Size(155, 21)
        Me.cmbCategories.TabIndex = 68
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 118)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(123, 13)
        Me.Label2.TabIndex = 67
        Me.Label2.Text = "Select Category To  Add"
        '
        'btnQuestionsAll
        '
        Me.btnQuestionsAll.Location = New System.Drawing.Point(176, 431)
        Me.btnQuestionsAll.Name = "btnQuestionsAll"
        Me.btnQuestionsAll.Size = New System.Drawing.Size(37, 23)
        Me.btnQuestionsAll.TabIndex = 72
        Me.btnQuestionsAll.Text = "All"
        Me.btnQuestionsAll.UseVisualStyleBackColor = True
        '
        'lvQuestions
        '
        Me.lvQuestions.FullRowSelect = True
        Me.lvQuestions.GridLines = True
        Me.lvQuestions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvQuestions.LabelWrap = False
        Me.lvQuestions.Location = New System.Drawing.Point(176, 197)
        Me.lvQuestions.Name = "lvQuestions"
        Me.lvQuestions.ShowGroups = False
        Me.lvQuestions.Size = New System.Drawing.Size(210, 228)
        Me.lvQuestions.TabIndex = 85
        Me.lvQuestions.UseCompatibleStateImageBehavior = False
        Me.lvQuestions.View = System.Windows.Forms.View.Details
        '
        'lvCategory
        '
        Me.lvCategory.AllowDrop = True
        Me.lvCategory.FullRowSelect = True
        Me.lvCategory.GridLines = True
        Me.lvCategory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvCategory.LabelWrap = False
        Me.lvCategory.Location = New System.Drawing.Point(15, 197)
        Me.lvCategory.Name = "lvCategory"
        Me.lvCategory.ShowGroups = False
        Me.lvCategory.Size = New System.Drawing.Size(155, 228)
        Me.lvCategory.TabIndex = 98
        Me.lvCategory.UseCompatibleStateImageBehavior = False
        Me.lvCategory.View = System.Windows.Forms.View.Details
        '
        'BtnCategoryDel
        '
        Me.BtnCategoryDel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnCategoryDel.Location = New System.Drawing.Point(89, 431)
        Me.BtnCategoryDel.Name = "BtnCategoryDel"
        Me.BtnCategoryDel.Size = New System.Drawing.Size(40, 23)
        Me.BtnCategoryDel.TabIndex = 91
        Me.BtnCategoryDel.Text = "Clear"
        Me.BtnCategoryDel.UseVisualStyleBackColor = True
        '
        'btnCategoriesAll
        '
        Me.btnCategoriesAll.Location = New System.Drawing.Point(15, 431)
        Me.btnCategoriesAll.Name = "btnCategoriesAll"
        Me.btnCategoriesAll.Size = New System.Drawing.Size(37, 23)
        Me.btnCategoriesAll.TabIndex = 88
        Me.btnCategoriesAll.Text = "All"
        Me.btnCategoriesAll.UseVisualStyleBackColor = True
        '
        'lvTemplateQuestions
        '
        Me.lvTemplateQuestions.FullRowSelect = True
        Me.lvTemplateQuestions.GridLines = True
        Me.lvTemplateQuestions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvTemplateQuestions.LabelWrap = False
        Me.lvTemplateQuestions.Location = New System.Drawing.Point(392, 197)
        Me.lvTemplateQuestions.Name = "lvTemplateQuestions"
        Me.lvTemplateQuestions.Size = New System.Drawing.Size(210, 228)
        Me.lvTemplateQuestions.TabIndex = 96
        Me.lvTemplateQuestions.UseCompatibleStateImageBehavior = False
        Me.lvTemplateQuestions.View = System.Windows.Forms.View.Details
        '
        'txtTemplateNew
        '
        Me.txtTemplateNew.Location = New System.Drawing.Point(173, 31)
        Me.txtTemplateNew.Name = "txtTemplateNew"
        Me.txtTemplateNew.Size = New System.Drawing.Size(156, 20)
        Me.txtTemplateNew.TabIndex = 97
        '
        'btnTemplateQuestionDelete
        '
        Me.btnTemplateQuestionDelete.Image = CType(resources.GetObject("btnTemplateQuestionDelete.Image"), System.Drawing.Image)
        Me.btnTemplateQuestionDelete.Location = New System.Drawing.Point(392, 431)
        Me.btnTemplateQuestionDelete.Name = "btnTemplateQuestionDelete"
        Me.btnTemplateQuestionDelete.Size = New System.Drawing.Size(37, 23)
        Me.btnTemplateQuestionDelete.TabIndex = 95
        Me.btnTemplateQuestionDelete.UseVisualStyleBackColor = True
        '
        'btnTemplateQuestionDown
        '
        Me.btnTemplateQuestionDown.Image = CType(resources.GetObject("btnTemplateQuestionDown.Image"), System.Drawing.Image)
        Me.btnTemplateQuestionDown.Location = New System.Drawing.Point(447, 431)
        Me.btnTemplateQuestionDown.Name = "btnTemplateQuestionDown"
        Me.btnTemplateQuestionDown.Size = New System.Drawing.Size(20, 23)
        Me.btnTemplateQuestionDown.TabIndex = 94
        Me.btnTemplateQuestionDown.TabStop = False
        Me.btnTemplateQuestionDown.UseVisualStyleBackColor = True
        '
        'btnTemplateQuestionUp
        '
        Me.btnTemplateQuestionUp.Image = CType(resources.GetObject("btnTemplateQuestionUp.Image"), System.Drawing.Image)
        Me.btnTemplateQuestionUp.Location = New System.Drawing.Point(428, 431)
        Me.btnTemplateQuestionUp.Name = "btnTemplateQuestionUp"
        Me.btnTemplateQuestionUp.Size = New System.Drawing.Size(20, 23)
        Me.btnTemplateQuestionUp.TabIndex = 93
        Me.btnTemplateQuestionUp.TabStop = False
        Me.btnTemplateQuestionUp.UseVisualStyleBackColor = True
        '
        'btnCategoryDown
        '
        Me.btnCategoryDown.Image = CType(resources.GetObject("btnCategoryDown.Image"), System.Drawing.Image)
        Me.btnCategoryDown.Location = New System.Drawing.Point(70, 431)
        Me.btnCategoryDown.Name = "btnCategoryDown"
        Me.btnCategoryDown.Size = New System.Drawing.Size(20, 23)
        Me.btnCategoryDown.TabIndex = 90
        Me.btnCategoryDown.TabStop = False
        Me.btnCategoryDown.UseVisualStyleBackColor = True
        '
        'btnCategoryUp
        '
        Me.btnCategoryUp.Image = CType(resources.GetObject("btnCategoryUp.Image"), System.Drawing.Image)
        Me.btnCategoryUp.Location = New System.Drawing.Point(51, 431)
        Me.btnCategoryUp.Name = "btnCategoryUp"
        Me.btnCategoryUp.Size = New System.Drawing.Size(20, 23)
        Me.btnCategoryUp.TabIndex = 89
        Me.btnCategoryUp.TabStop = False
        Me.btnCategoryUp.UseVisualStyleBackColor = True
        '
        'BtnQuestionAdd
        '
        Me.BtnQuestionAdd.Image = CType(resources.GetObject("BtnQuestionAdd.Image"), System.Drawing.Image)
        Me.BtnQuestionAdd.Location = New System.Drawing.Point(212, 431)
        Me.BtnQuestionAdd.Name = "BtnQuestionAdd"
        Me.BtnQuestionAdd.Size = New System.Drawing.Size(37, 23)
        Me.BtnQuestionAdd.TabIndex = 87
        Me.BtnQuestionAdd.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(466, 431)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(37, 23)
        Me.Button1.TabIndex = 99
        Me.Button1.Text = "All"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cmbDataBlock
        '
        Me.cmbDataBlock.FormattingEnabled = True
        Me.cmbDataBlock.Location = New System.Drawing.Point(15, 81)
        Me.cmbDataBlock.Name = "cmbDataBlock"
        Me.cmbDataBlock.Size = New System.Drawing.Size(152, 21)
        Me.cmbDataBlock.TabIndex = 101
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 13)
        Me.Label1.TabIndex = 100
        Me.Label1.Text = "Select DataBlock"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 181)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(102, 13)
        Me.Label3.TabIndex = 102
        Me.Label3.Text = "Selected Categories"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(182, 181)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(123, 13)
        Me.Label4.TabIndex = 103
        Me.Label4.Text = "Select Question To  Add"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(398, 181)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(99, 13)
        Me.Label5.TabIndex = 104
        Me.Label5.Text = "Selected Questions"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(335, 29)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(79, 23)
        Me.Button2.TabIndex = 105
        Me.Button2.Text = "Create/Reset"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(418, 29)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(79, 23)
        Me.Button3.TabIndex = 106
        Me.Button3.Text = "Delete"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Templates
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(618, 520)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbDataBlock)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnTemplateQuestionDelete)
        Me.Controls.Add(Me.btnTemplateQuestionDown)
        Me.Controls.Add(Me.btnTemplateQuestionUp)
        Me.Controls.Add(Me.BtnCategoryDel)
        Me.Controls.Add(Me.btnCategoryDown)
        Me.Controls.Add(Me.btnCategoryUp)
        Me.Controls.Add(Me.btnCategoriesAll)
        Me.Controls.Add(Me.BtnQuestionAdd)
        Me.Controls.Add(Me.btnQuestionsAll)
        Me.Controls.Add(Me.cmbCategories)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnFrmDelete)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblTemplateName)
        Me.Controls.Add(Me.cmbTemplate)
        Me.Controls.Add(Me.lblSelectTemplate)
        Me.Controls.Add(Me.lvTemplateQuestions)
        Me.Controls.Add(Me.lvQuestions)
        Me.Controls.Add(Me.lvCategory)
        Me.Controls.Add(Me.txtTemplateNew)
        Me.Name = "Templates"
        Me.Text = "Template Maintenance"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSelectTemplate As System.Windows.Forms.Label
    Friend WithEvents cmbTemplate As System.Windows.Forms.ComboBox
    Friend WithEvents lblTemplateName As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnConfirm As System.Windows.Forms.Button
    Friend WithEvents btnFrmDelete As System.Windows.Forms.Button
    Friend WithEvents cmbCategories As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnQuestionsAll As System.Windows.Forms.Button
    Friend WithEvents lvQuestions As System.Windows.Forms.ListView
    Friend WithEvents lvCategory As System.Windows.Forms.ListView
    Friend WithEvents BtnQuestionAdd As System.Windows.Forms.Button
    Friend WithEvents BtnCategoryDel As System.Windows.Forms.Button
    Friend WithEvents btnCategoryDown As System.Windows.Forms.Button
    Friend WithEvents btnCategoryUp As System.Windows.Forms.Button
    Friend WithEvents btnCategoriesAll As System.Windows.Forms.Button
    Friend WithEvents btnTemplateQuestionDelete As System.Windows.Forms.Button
    Friend WithEvents btnTemplateQuestionDown As System.Windows.Forms.Button
    Friend WithEvents btnTemplateQuestionUp As System.Windows.Forms.Button
    Friend WithEvents lvTemplateQuestions As System.Windows.Forms.ListView
    Friend WithEvents txtTemplateNew As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cmbDataBlock As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
End Class
