Imports System.Drawing

Public Class clsControls

End Class

Public Class newLabel

    Inherits System.Windows.Forms.TextBox

    Public Sub New()

        Me.Size = New Point(100, 13)
        Me.Text = "New"
        Me.BorderStyle = BorderStyle.None
        Me.BackColor = Color.White
        Me.ForeColor = Control.DefaultForeColor


        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.MouseClick, AddressOf frmMain.catControls_MouseClick
        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub

End Class

Public Class newButton

    Inherits System.Windows.Forms.Button

    Public Sub New()

        Me.Size = New Point(56, 23)
        Me.Text = "Add"

        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.MouseClick, AddressOf frmMain.catControls_MouseClick
        Me.ContextMenuStrip = frmMain.ContextMenuStrip1


    End Sub

End Class


Public Class newTextBox

    Inherits System.Windows.Forms.TextBox

    Public Sub New()

        Me.Size = New Point(121, 20)

        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.MouseClick, AddressOf frmMain.catControls_MouseClick
        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub

End Class


Public Class newComboBox

    Inherits System.Windows.Forms.ComboBox

    Public Sub New()
        Me.Size = New Point(121, 20)

        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.SelectedIndexChanged, AddressOf frmMain.catControls_SelectedIndexChanged
        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub

End Class

Public Class newMultiLineTextBox

    Inherits System.Windows.Forms.TextBox

    Public Sub New()

        Me.Size = New Point(250, 100)
        Me.Multiline = True
        Me.Enabled = True
        Me.AllowDrop = True


        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.MouseClick, AddressOf frmMain.catControls_MouseClick
        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub

End Class


Public Class newListBox

    Inherits System.Windows.Forms.ListBox

    Public Sub New()

        Me.Size = New Point(250, 100)

        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.SelectedIndexChanged, AddressOf frmMain.catControls_SelectedIndexChanged

        AddHandler Me.DrawItem, AddressOf frmMain.ListBox_DrawItem
        Me.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed

        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub

End Class


Public Class newPanel

    Inherits System.Windows.Forms.Panel

    Public Sub New()

        AddHandler Me.MouseClick, AddressOf frmMain.catControls_MouseClick
        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub

End Class

Public Class newRichTextBox

    Inherits System.Windows.Forms.RichTextBox

    Public catID As Integer

    Public Sub New()

        Me.Size = frmMain.rtbDefault.Size
        Me.Location = frmMain.rtbDefault.Location

        Me.ContextMenuStrip = frmMain.ContextMenuStrip2


    End Sub
End Class

Public Class newControlTimer

    Inherits System.Windows.Forms.TextBox

    Public Sub New()

        Me.Size = New Point(45, 28)
        'Me.FlatStyle = FlatStyle.Flat
        Me.Text = "000.0"
        'Me.TextAlign = ContentAlignment.MiddleCenter 
        Me.Font = New Font("Microsoft Sans Serif", 12)
        Me.BorderStyle = BorderStyle.FixedSingle
        Me.BackColor = Color.LightGray

        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.MouseClick, AddressOf frmMain.catControls_MouseClick
        AddHandler Me.MouseDoubleClick, AddressOf frmMain.catControls_DoubleClick
        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub
End Class

Public Class newRadioButton

    Inherits System.Windows.Forms.RadioButton

    Public Sub New()

        Me.Checked = False
        Me.Text = ""

        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.MouseClick, AddressOf frmMain.catControls_MouseClick
        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub
End Class

Public Class newCheckBox

    Inherits System.Windows.Forms.CheckBox

    Public Sub New()

        Me.Checked = False
        Me.Text = ""

        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.MouseClick, AddressOf frmMain.catControls_MouseClick
        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub
End Class

Public Class newItemSelectedLabel

    Inherits System.Windows.Forms.Label

    Public Sub New()

        Dim tmpStr As String = frmMain.MyDocs & "\System Images\ItemSelected.png"

        'Me.Size = New Point(20, 20)
        'Me.Image = My.Resources.SelectedItemBox 'System.Drawing.Image.FromFile(tmpStr)

        Me.Text = Chr(161)
        Me.Size = New Point(27, 27)
        Me.Font = New Font("WingDings", 22, FontStyle.Regular)
        Me.BackColor = Color.Transparent
        Me.ForeColor = Color.Blue
        Me.TextAlign = ContentAlignment.BottomCenter


        AddHandler Me.MouseDown, AddressOf frmMain.catControls_MouseDown
        AddHandler Me.MouseUp, AddressOf frmMain.catControls_MouseUp
        AddHandler Me.MouseMove, AddressOf frmMain.catControls_MouseMove
        AddHandler Me.MouseClick, AddressOf frmMain.catControls_MouseClick

        AddHandler Me.TextChanged, AddressOf frmMain.ItemSelectedLabel_TextChanged

        Me.ContextMenuStrip = frmMain.ContextMenuStrip1

    End Sub
End Class





