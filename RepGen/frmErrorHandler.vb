Imports System.Windows.Forms

Public Class frmDotnetWarning

    Public errorMsg As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.OK

        If frmMain.net35ok = True Then
            End
            Me.Close()
        Else
            End
            Me.Close()
        End If

    End Sub


    Private Sub frmDotnetWarning_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If frmMain.net35ok = False Then
            TextBox1.Text += "This Application Requires .Net 3.5"
            TextBox1.Text += vbCrLf + vbCrLf

            TextBox1.Text += "To install this FREE Microsoft Windows Add-On, please visit Windows Update in your Start Menu. Select Custom Installation and view available Software updates. Select the .Net 3.5 Framework and install. When complete, reboot your machine. After reboot, ANT should start normally."

            TextBox1.Text += vbCrLf + vbCrLf
        Else
            TextBox1.Text += "Assesment Navigation Tool has encountered an unexpected error." & vbCrLf & vbCrLf
            TextBox1.Text += "Please note exactly what you were doing when this error occurred and contact customer support."
            TextBox1.Text += vbCrLf & vbCrLf & vbCrLf

            TextBox1.Text += "Error  Detail:-" & vbCrLf
            TextBox1.Text += errorMsg
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click





    End Sub
End Class
