Imports System.Windows.Forms
Imports System.IO
Imports System.Data.Odbc


Public Class frmPasteBox

    Public appVars As clsVariables = frmMain.appVars


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub LoadDoc()

        Dim strCwd As String = Environment.CurrentDirectory
        Dim tmpStr As String = ""
        Dim filename As String = ""
        Dim status As Integer = 0
        Dim count As Integer = 0
        Dim OpenFileDialog1 As New OpenFileDialog

        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        OpenFileDialog1.Title = "Select Report Template"
        OpenFileDialog1.FileName = "*.rtf"
        OpenFileDialog1.ShowDialog()

        If File.Exists(OpenFileDialog1.FileName) Then

            fileName = OpenFileDialog1.FileName

            Environment.CurrentDirectory = strCwd

            Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read)
            Dim sr As New StreamReader(fs)

            tmpStr = sr.ReadToEnd
            tmpStr = tmpStr.Replace("'", "''").Replace("\", "\\")

            Dim tmpSql As String
            Dim SQLcommand As OdbcCommand


            SQLcommand = frmMain.connStr.CreateCommand

            tmpSql = "INSERT INTO AppDocs (DocumentTag,DocText,DocType) VALUES ('" & TextBox2.Text & "','" & tmpStr & "',1)"

            'SQLcommand.Parameters.Add("RTF", OdbcType.Text).Value = tmpStr

            Try
                SQLcommand.CommandText = tmpSql
                SQLcommand.ExecuteNonQuery()

                frmMain.cmbReportTemplate.Items.Add(TextBox2.Text)

            Catch ex As Exception
                status = 1
            End Try


        Else
            status = 1
        End If

        If status = 1 Then
            MsgBox(appVars.fmb("Incorrect Document File. Insert Failed"))
        Else
            MsgBox(appVars.fmb("Document Loaded Successfully."))
            Me.Close()
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If TextBox2.Text.Trim.Length > 0 Then
            LoadDoc()
        Else
            MsgBox("Plase Enter Document Name")
        End If



    End Sub

 
End Class
