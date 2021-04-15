Imports System.Windows.Forms
Imports System.Data
Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions

Public Class frmImport

    Public appVars As New clsVariables
    Public fileName As String
    Dim oldRow As String = ""

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim fs As New FileStream(fileName, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim tmpStr As String
        Dim arrCols(10) As String
        Dim count As Integer = 0
        Dim status As Integer = 0
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand

        SQLcommand = frmMain.connStr.CreateCommand

        SQLcommand.CommandText = "BEGIN "
        SQLcommand.ExecuteNonQuery()

        While Not sr.EndOfStream

            count = count + 1

            tmpStr = sr.ReadLine

            arrCols = tmpStr.Split(vbTab)

            If arrCols.Length = 0 Then 'simple validation.. prob needs work #TODO#
                Continue While
            End If


            'code for < 5 values. don't need to be precise - allowed 10 elements.

            ReDim Preserve arrCols(arrCols.Length + 5)

            If arrCols(0) = Nothing Then arrCols(0) = ""
            If arrCols(1) = Nothing Then arrCols(1) = ""
            If arrCols(2) = Nothing Then arrCols(2) = ""
            If arrCols(3) = Nothing Then arrCols(3) = ""
            If arrCols(4) = Nothing Then arrCols(4) = ""
            If arrCols(5) = Nothing Then arrCols(5) = ""
            If arrCols(6) = Nothing Then arrCols(6) = "65535"

            If chkTag.Checked = False Then
                tmpSQL = " INSERT INTO DataBlocks (Tag,Text1,Text2,Text3,Text4,Text5,Templates) VALUES ('" & _
                txtDataBlockTag.Text.Replace("'", "''") & _
                "','" & arrCols(0).Replace("'", "''").Replace("""", "").Trim & _
                "','" & arrCols(1).Replace("'", "''").Replace("""", "").Trim & _
                "','" & arrCols(2).Replace("'", "''").Replace("""", "").Trim & _
                "','" & arrCols(3).Replace("'", "''").Replace("""", "").Trim & _
                "','" & arrCols(4).Replace("'", "''").Replace("""", "").Trim & _
                "',65535)"
            Else
                tmpSQL = " INSERT INTO DataBlocks (Tag,Text1,Text2,Text3,Text4,Text5,Templates ) VALUES (" & _
                "  '" & arrCols(0).Replace("'", "''").Replace("""", "").Trim & _
                "','" & arrCols(1).Replace("'", "''").Replace("""", "").Trim & _
                "','" & arrCols(2).Replace("'", "''").Replace("""", "").Trim & _
                "','" & arrCols(3).Replace("'", "''").Replace("""", "").Trim & _
                "','" & arrCols(4).Replace("'", "''").Replace("""", "").Trim & _
                "','" & arrCols(5).Replace("'", "''").Replace("""", "").Trim & _
                "'," & arrCols(6) & ")"
            End If

            SQLcommand.CommandText = tmpSQL
            SQLcommand.ExecuteNonQuery()

            txtRow.Text = "Row # " & count
            txtRow.Refresh()


        End While

        SQLcommand.CommandText = "COMMIT "
        SQLcommand.ExecuteNonQuery()

        MsgBox(appVars.fmb(count & " Rows Loaded"))

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub


    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmImport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Dim strCwd As String = Environment.CurrentDirectory
        Dim tmpStr As String = ""
        Dim strExt As String = ""
        Dim status As Integer = 0
        Dim count As Integer = 0
        Dim OpenFileDialog1 As New OpenFileDialog
        Dim arrCols(20) As String


        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        OpenFileDialog1.Title = "Select DataBlock File"
        OpenFileDialog1.FileName = "*.txt"
        OpenFileDialog1.ShowDialog()

        If File.Exists(OpenFileDialog1.FileName) Then

            fileName = OpenFileDialog1.FileName

            Environment.CurrentDirectory = strCwd

            Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read)
            Dim sr As New StreamReader(fs)

            tmpStr = sr.ReadLine

            arrCols = tmpStr.Split(vbTab)


            If arrCols.Length > 0 Then txtPrev1.Text = arrCols(0)
            If arrCols.Length > 1 Then txtPrev2.Text = arrCols(1)
            If arrCols.Length > 2 Then txtPrev3.Text = arrCols(2)
            If arrCols.Length > 3 Then txtPrev4.Text = arrCols(3)
            If arrCols.Length > 4 Then txtPrev5.Text = arrCols(4)


            If status = 1 Then
                MsgBox(appVars.fmb("Incorrect DataBlock File. Insert Failed"))
                Me.Close()
            End If

        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click


        'this uses the hidden button for custom loads needed by programmer.
        'steve wallis - wallisoft@gmail.com 27 may 2010


        Dim fs As New FileStream(fileName, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim tmpStr As String
        Dim arrCols(10) As String
        Dim count As Integer = 0
        Dim status As Integer = 0
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand

        Dim template As String = ""
        Dim templateID As Integer
        Dim tag As String = ""
        Dim t1 As String = ""
        Dim t2 As String = ""
        Dim t3 As String = ""
        Dim t4 As String = ""
        Dim t5 As String = ""


        If txtDataBlockTag.Text.Trim = "" And chkTag.Checked = False Then
            MsgBox("Please enter DataBlock Name")
            Exit Sub
        End If


        SQLcommand = frmMain.connStr.CreateCommand

        SQLcommand.CommandText = "BEGIN "
        SQLcommand.ExecuteNonQuery()


        While status = 0

            tag = ""
            t1 = ""
            t2 = ""
            t3 = ""
            t4 = ""
            t5 = ""
            template = ""


            tmpStr = sr.ReadLine

            If tmpStr = oldRow Then Continue While
            oldRow = tmpStr

            arrCols = tmpStr.Split(vbTab)

            ReDim Preserve arrCols(arrCols.Length + 7)

            If arrCols(0) Is Nothing Then arrCols(0) = ""
            If arrCols(1) Is Nothing Then arrCols(1) = ""
            If arrCols(2) Is Nothing Then arrCols(2) = ""
            If arrCols(3) Is Nothing Then arrCols(3) = ""
            If arrCols(4) Is Nothing Then arrCols(4) = ""
            If arrCols(5) Is Nothing Then arrCols(5) = ""
            If arrCols(6) Is Nothing Then arrCols(6) = ""
            If arrCols(7) Is Nothing Then arrCols(7) = ""

            If chkTag.Checked = True Then

                template = arrCols(0).Trim
                If template = "" Then template = "0/0"

                tag = arrCols(1).Replace("'", "''").Trim

                t1 = arrCols(2).Replace("'", "''").Trim
                t2 = arrCols(3).Replace("'", "''").Trim
                t3 = arrCols(4).Replace("'", "''").Trim
                t4 = arrCols(5).Replace("'", "''").Trim
                t5 = arrCols(6).Replace("'", "''").Trim

            Else
                template = "0/0"
                tag = txtDataBlockTag.Text
                t1 = arrCols(0).Replace("'", "''").Trim
                t2 = arrCols(1).Replace("'", "''").Trim
                t3 = arrCols(2).Replace("'", "''").Trim
                t4 = arrCols(3).Replace("'", "''").Trim
                t5 = arrCols(4).Replace("'", "''").Trim
            End If

            templateID = GetTemplateNumber(template)


            If tag.ToLower.Contains("axis1") Then

                Dim tmpStr1 As String
                Dim tmpStr2 As String

                tmpStr1 = t1.Substring(0, t1.IndexOf(" ")).Trim
                tmpStr2 = t1.Substring(t1.IndexOf(" ")).Trim

                If tmpStr1.Length = 5 Then tmpStr1 += "0"

                t1 = tmpStr1.Trim & " " & tmpStr2.Trim

            End If


            If tag.Trim <> "" Then

                count = count + 1

                tmpSQL = " INSERT INTO DataBlocks (Tag,Text1,Text2,Text3,Text4,Text5,Templates) VALUES (" & _
                "'" & tag.Trim & "'," & _
                "'" & t1.Trim & "'," & _
                "'" & t2.Trim & "'," & _
                "'" & t3.Trim & "'," & _
                "'" & t4.Trim & "'," & _
                "'" & t5.Trim & "'," & _
                " " & templateID & " )"

                SQLcommand.CommandText = tmpSQL
                SQLcommand.ExecuteNonQuery()

                txtRow.Text = "Row # " & count
                txtRow.Refresh()

            End If

            If sr.EndOfStream Then Exit While

        End While

        SQLcommand.CommandText = "COMMIT "
        SQLcommand.ExecuteNonQuery()

        MsgBox(appVars.fmb(count & " Rows Loaded"))

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Function GetTemplateNumber(ByVal template As String)

        Dim gender As String = ""
        Dim age As String = ""
        Dim tmpInt As Integer = 0

        If template.Contains("/") Then

            gender = template.Substring(0, template.IndexOf("/"))
            age = template.Substring(template.IndexOf("/") + 1)

            If age.Contains("0") Then tmpInt = 65535
            If age.Contains(1) Then tmpInt = tmpInt Xor 1 Xor 2
            If age.Contains(2) Then tmpInt = tmpInt Xor 16 Xor 32
            If age.Contains(3) Then tmpInt = tmpInt Xor 4 Xor 8
        End If

        If tmpInt = 0 Then
            template = 65535
        End If

        Return tmpInt

    End Function

End Class
