Public NotInheritable Class frmAboutBox

    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the "Application" pane of the project 
        '    properties dialog (under the "Project" menu).
        Me.LabelProductName.Text = My.Application.Info.ProductName
        'Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        Me.LabelVersion.Text = "Release 1.1.0.4"
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName

        Me.TextBoxDescription.Text = "Registration Code : Trial " & vbCrLf
        Me.TextBoxDescription.Text += "Registered Users  : 1 " & vbCrLf
        Me.TextBoxDescription.Text += "Registration Type : Standard " & vbCrLf
        Me.TextBoxDescription.Text += "---" & vbCrLf
        Me.TextBoxDescription.Text += "Form Resolution   : " & frmMain.Width & "x" & frmMain.Height & vbCrLf
        Me.TextBoxDescription.Text += "Scrn Resolution   : " & Screen.PrimaryScreen.Bounds.Width & "x" & Screen.PrimaryScreen.Bounds.Height & vbCrLf

    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

End Class
