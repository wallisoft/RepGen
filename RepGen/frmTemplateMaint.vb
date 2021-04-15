Imports System.Data
Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions

Public Class frmTemplateMaint

    Public connStr = frmMain.connStr

    Dim arrAllQuestions()() As String
    Dim arrTemplateQuestions()() As String
    Dim arrTemplateID() As Integer
    Dim arrCategory() As Integer

    Dim newTemplate As Boolean = False
    Dim newCategory As Boolean = False
    Dim oldCategory As String = ""
    Dim oldTemplate As String = ""
    Dim TemplateID As Integer
    Dim Template As Integer
    Dim TemplateName As String
    Dim Category As String

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub Templates_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        lvCategory.Columns.Add("", 134)
        lvQuestions.Columns.Add("", 188)
        lvTemplateQuestions.Columns.Add("", 188)

        cmbDataBlock.Enabled = False

        LoadAll()

    End Sub

    Sub LoadAll()

        Dim count As Integer
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader

        SQLcommand = connStr.CreateCommand

        cmbCategories.Items.Clear()
        cmbCategories.Text = ""
        cmbTemplate.Items.Clear()
        cmbTemplate.Text = ""
        cmbDataBlock.Items.Clear()
        cmbDataBlock.Text = ""


        'Get templates
        tmpSQL = "SELECT * FROM Templates "


        SQLcommand.CommandText = tmpSQL
        SQLreader = SQLcommand.ExecuteReader()

        count = 0
        While SQLreader.Read()
            ReDim Preserve arrTemplateID(count)
            cmbTemplate.Items.Add(SQLreader(1))
            arrTemplateID(count) = 2 ^ SQLreader(0)
            count += 1
        End While
        SQLreader.Close()


        'get DataBlocks
        tmpSQL = "SELECT DISTINCT Tag " & _
                " FROM   DataBlocks "

        SQLcommand.CommandText = tmpSQL
        SQLreader = SQLcommand.ExecuteReader()

        count = 0

        While SQLreader.Read()
            If SQLreader(0).ToString.Trim.Length > 0 Then
                If SQLreader(0) <> "Menu" Then
                    cmbDataBlock.Items.Add(SQLreader(0))
                    count += 1
                End If
            End If

        End While
        SQLreader.Close()


    End Sub


    Private Sub LoadAllQuestions()

        Dim count As Integer = 0
        Dim count1 As Integer = 0
        Dim SQLreader As OdbcDataReader
        Dim oldCategory As String = ""
        Dim SQLcommand As OdbcCommand
        Dim CatID As Integer
        Dim oldText1 As String = ""
        Dim oldText2 As String = ""


        TemplateID = arrTemplateID(cmbTemplate.SelectedIndex)


        SQLcommand = connStr.CreateCommand

        'get categories

        cmbCategories.Items.Clear()

        SQLcommand.CommandText = "SELECT DISTINCT Text1 " & _
                                " FROM   DataBlocks " & _
                                " WHERE  Tag = '" & cmbDataBlock.Text & "' "

        SQLreader = SQLcommand.ExecuteReader()


        While SQLreader.Read()
            cmbCategories.Items.Add(SQLreader(0))
            count += 1
        End While

        ReDim arrAllQuestions(count)
        ReDim arrTemplateQuestions(count)

        SQLreader.Close()

        'get TemplateQuestions

        SQLcommand.CommandText = "SELECT DISTINCT Text1, Text2 " & _
                        " FROM   DataBlocks " & _
                        " WHERE  DataBlocks.Tag = '" & cmbDataBlock.Text & "' " & _
                        " AND Datablocks.Templates & " & TemplateID & " > 0 "

        SQLreader = SQLcommand.ExecuteReader()

        count = 0
        count1 = -1
        oldCategory = 0

        While SQLreader.Read()

            CatID = GetCategoryID(SQLreader(0))

            Try
                count = arrTemplateQuestions(CatID).Length
            Catch ex As Exception
                count = 0
            End Try

            ReDim Preserve arrTemplateQuestions(CatID)(count)

            If count = 0 Then
                lvCategory.Items.Add(SQLreader(0))
            End If

            arrTemplateQuestions(CatID)(count) = SQLreader(1).ToString

        End While

        SQLreader.Close()

        'get All Questions

        SQLcommand.CommandText = "SELECT DISTINCT Text1, Text2 " & _
                        " FROM   DataBlocks " & _
                        " WHERE  Tag = '" & cmbDataBlock.Text & "' "

        SQLreader = SQLcommand.ExecuteReader()

        count = 0
        count1 = -1
        oldCategory = 0
        oldText1 = ""
        oldText2 = ""

        While SQLreader.Read()

            CatID = GetCategoryID(SQLreader(0))
            Try
                count = arrAllQuestions(CatID).Length
            Catch ex As Exception
                count = 0
            End Try

            ReDim Preserve arrAllQuestions(CatID)(count)

            arrAllQuestions(CatID)(count) = SQLreader(1).ToString

        End While



    End Sub


    Private Sub lvCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles lvCategory.SelectedIndexChanged


        If lvCategory.SelectedItems.Count = 1 Then

            If oldCategory.ToString.Length > 0 Then
                SaveTemplateQuestionChanges()
                oldCategory = lvCategory.SelectedItems(0).Text
            End If

            Category = lvCategory.SelectedItems(0).Text
            lvQuestions.Items.Clear()
            lvTemplateQuestions.Items.Clear()

            LoadQuestions()

            oldCategory = Category
            lvCategory.Focus()

        End If

    End Sub

    Function SaveTemplateQuestionChanges()

        Dim count As Integer = 0
        Dim count1 As Integer = 0
        Dim CatID As Integer
        Dim strQuestion As String

        CatID = GetCategoryID(oldCategory)

        count = 0
        For Each lvItem As ListViewItem In lvTemplateQuestions.Items

            strQuestion = lvItem.Text

            ReDim Preserve arrTemplateQuestions(CatID)(count)

            arrTemplateQuestions(CatID)(count) = strQuestion

            count += 1
        Next

        Return 0

    End Function

    Private Sub cmbTemplate_SelectedIndexChanged(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cmbTemplate.SelectedIndexChanged

        If cmbTemplate.Text.Length > 0 Then
            oldTemplate = cmbTemplate.Text

            Template = getTemplateID(cmbTemplate.Text)
            TemplateName = cmbTemplate.Text

            cmbDataBlock.Enabled = True

            clearForm()

        End If

    End Sub

    Sub clearAll()

        cmbTemplate.Items.Clear()
        cmbTemplate.Text = ""
        cmbDataBlock.Items.Clear()
        cmbCategories.Items.Clear()

        lvCategory.Items.Clear()
        lvQuestions.Items.Clear()
        lvTemplateQuestions.Items.Clear()
        txtTemplateNew.Text = ""

    End Sub

    Sub clearForm()

        lvCategory.Items.Clear()
        lvQuestions.Items.Clear()
        lvTemplateQuestions.Items.Clear()
        txtTemplateNew.Text = ""

    End Sub

    Sub clearQuestions()

        lvQuestions.Items.Clear()
        lvTemplateQuestions.Items.Clear()

    End Sub


    Function LoadCategories()

        Return 0

    End Function


    Function LoadQuestions()

        Dim found As Boolean = False
        Dim tmpStr As String = ""
        Dim tmpStr1 As String = ""
        Dim CatID As Integer

        CatID = GetCategoryID(Category)


        lvQuestions.Items.Clear()

        'load template questions first to preserve order

        If Not arrTemplateQuestions(CatID) Is Nothing Then
            For Each tmpStr In arrTemplateQuestions(CatID)
                lvTemplateQuestions.Items.Add(tmpStr)
            Next
        End If



        If Not arrAllQuestions(CatID) Is Nothing Then

            For Each tmpStr In arrAllQuestions(CatID)

                For Each lvItem As ListViewItem In lvTemplateQuestions.Items

                    tmpStr1 = lvItem.Text
                    If tmpStr1 = tmpStr Then
                        found = True
                        Exit For
                    End If
                Next

                If found = False Then
                    lvQuestions.Items.Add(tmpStr)
                Else
                    found = False
                End If
            Next
        End If

        Return 0

    End Function


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnCancel.Click

        Me.Close()
    End Sub


    Private Sub cmbCategories_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles cmbCategories.SelectedIndexChanged

        Dim count As Integer
        Dim dupFound As Boolean = False

        If Not newCategory Then

            If lvCategory.Items.Count > 0 Then

                For count = 0 To lvCategory.Items.Count - 1
                    If lvCategory.Items.Item(count).Text = cmbCategories.Text Then
                        dupFound = True
                    End If
                Next

                If Not dupFound Then
                    lvCategory.Items.Add(cmbCategories.Text)
                Else
                    MsgBox("Category Already Selected")
                End If

            Else
                lvCategory.Items.Add(cmbCategories.Text)
            End If

        End If

        cmbCategories.SelectedItem = ""
        cmbCategories.Text = ""

    End Sub


    Private Sub btnCategoriesAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnCategoriesAll.Click

        Dim count As Integer

        lvCategory.Items.Clear()

        For count = 0 To cmbCategories.Items.Count - 1
            lvCategory.Items.Add(cmbCategories.Items(count))
        Next

    End Sub


    Private Sub btnQuestionsAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnQuestionsAll.Click

        Dim count As Integer

        If lvCategory.Items.Count > 0 Then
            For count = 0 To lvQuestions.Items.Count - 1
                lvQuestions.Items.Item(count).Selected = True
            Next
        End If
        lvQuestions.Refresh()
        lvQuestions.Focus()

    End Sub

    Private Sub BtnQuestionAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles BtnQuestionAdd.Click

        While lvQuestions.SelectedItems.Count > 0
            lvTemplateQuestions.Items.Add(lvQuestions.SelectedItems(0).Clone)
            lvQuestions.SelectedItems.Item(0).Remove()
        End While

    End Sub

    Private Sub lvQuestions_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles lvQuestions.DoubleClick

        If lvQuestions.SelectedItems.Count = 1 Then
            lvTemplateQuestions.Items.Add(lvQuestions.SelectedItems(0).Clone)
            lvQuestions.SelectedItems.Item(0).Remove()
        End If

    End Sub


    Private Sub btnTemplateQuestionDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnTemplateQuestionDelete.Click

        While lvTemplateQuestions.SelectedItems.Count > 0
            lvQuestions.Items.Add(lvTemplateQuestions.SelectedItems(0).Clone)
            lvTemplateQuestions.SelectedItems(0).Remove()
        End While

    End Sub


    Private Sub lvTemplateQuestions_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles lvTemplateQuestions.DoubleClick

        If lvTemplateQuestions.SelectedItems.Count = 1 Then
            lvQuestions.Items.Add(lvTemplateQuestions.SelectedItems(0).Clone)
            lvTemplateQuestions.SelectedItems(0).Remove()
        End If

    End Sub

    Private Sub MoveListViewItem(ByVal moveUp As Boolean, ByVal lv As ListView)

        Dim i As Integer
        Dim cache As String
        Dim selIdx As Integer

        If lv.SelectedItems.Count = 1 Then
            selIdx = lv.SelectedItems.Item(0).Index
            If moveUp Then
                ' ignore moveup of row(0)
                If selIdx = 0 Then
                    Exit Sub
                End If
                ' move the subitems for the previous row
                ' to cache so we can move the selected row up
                For i = 0 To lv.Items(selIdx).SubItems.Count - 1
                    cache = lv.Items(selIdx - 1).SubItems(i).Text

                    lv.Items(selIdx - 1).SubItems(i).Text = lv.Items(selIdx).SubItems(i).Text
                    lv.Items(selIdx).SubItems(i).Text = cache
                Next
                lv.Items(selIdx - 1).Selected = True
                lv.Items(selIdx).Selected = False
                lv.Refresh()
                lv.Focus()
            Else
                ' ignore move down of last row
                If selIdx = lv.Items.Count - 1 Then
                    Exit Sub
                End If
                ' move the subitems for the next row
                ' to cache so we can move the selected row down
                For i = 0 To lv.Items(selIdx).SubItems.Count - 1
                    cache = lv.Items(selIdx + 1).SubItems(i).Text
                    lv.Items(selIdx + 1).SubItems(i).Text = lv.Items(selIdx).SubItems(i).Text
                    lv.Items(selIdx).SubItems(i).Text = cache
                Next
                lv.Items(selIdx + 1).Selected = True
                lv.Items(selIdx).Selected = False
                lv.Refresh()
                lv.Focus()
            End If
        End If

    End Sub


    Private Sub btnCategoryUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnCategoryUp.Click

        Me.MoveListViewItem(True, lvCategory)
    End Sub

    Private Sub btnCategoryDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnCategoryDown.Click

        Me.MoveListViewItem(False, lvCategory)
    End Sub

    Private Sub btnTemplateQuestionUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnTemplateQuestionUp.Click

        Me.MoveListViewItem(True, lvTemplateQuestions)
    End Sub

    Private Sub btnTemplateQuestionDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnTemplateQuestionDown.Click

        Me.MoveListViewItem(False, lvTemplateQuestions)
    End Sub


    Public Function getTemplateID(ByVal Template As String)

        Dim count As Integer
        Dim TemplateID As Integer = -1
        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim tmpSQL As String
        Dim status As Integer = 0

        count = 0
        For Each Str As String In cmbTemplate.Items

            If Str.Trim = Template.Trim Then
                TemplateID = arrTemplateID(count)
                Exit For
            End If
            count += 1
        Next



        If TemplateID = -1 And Template.Trim.Length > 0 Then

            SQLcommand = connStr.CreateCommand

            tmpSQL = "INSERT INTO Templates (Template) " & _
                    "VALUES ('" & Template & "')"

            Try
                SQLcommand.CommandText = tmpSQL
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                status = 1
            End Try

            If status = 0 Then

                SQLcommand = connStr.CreateCommand
                SQLcommand.CommandText = "select last_insert_rowid()"

                SQLreader = SQLcommand.ExecuteReader()

                SQLreader.Read()
                TemplateID = 2 ^ SQLreader(0)


                If TemplateID < 1 Then
                    status = 1
                End If
            End If
        End If

        If status = 1 Then
            MsgBox("Failed To Add New Template", MsgBoxStyle.Critical)
        End If

        Return TemplateID

    End Function



    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnConfirm.Click

        Dim TemplateDataID As Integer = 0 '#sw

        If oldCategory.ToString.Trim.Length > 0 Then SaveTemplateQuestionChanges()


        If MsgBox("Save Changes - Continue ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            If txtTemplateNew.Text.Length > 0 Then
                TemplateDataID = getTemplateID(txtTemplateNew.Text)
            ElseIf cmbTemplate.Text.Length > 0 Then
                TemplateDataID = getTemplateID(cmbTemplate.Text)
            Else
                MsgBox("No Template Selected")
            End If

            If TemplateDataID > 0 Then
                saveChanges(TemplateDataID)
                clearAll()
                LoadAll()
            End If
        End If

    End Sub

    Function saveChanges(ByVal TemplateDataID As Integer)

        Dim count As Integer = 0
        Dim count1 As Integer = 0
        Dim CatID As Integer
        Dim SQLcommand As OdbcCommand

        SQLcommand = connStr.CreateCommand

        With SQLcommand

            count = 0

            .CommandText = "BEGIN"
            .ExecuteNonQuery()

            .CommandText = "  UPDATE DataBlocks " & _
                            " SET Templates = Templates &~ " & TemplateID & _
                            " WHERE  Tag = '" & cmbDataBlock.Text & "'"

            .ExecuteNonQuery()


            For Each lvItem As ListViewItem In lvCategory.Items

                CatID = GetCategoryID(lvItem.Text)

                For Each tmpStr As String In arrTemplateQuestions(CatID).ToString

                    .CommandText = "  UPDATE DataBlocks " & _
                                    " SET Templates = Templates | " & TemplateID & _
                                    " WHERE  Tag = '" & cmbDataBlock.Text & "'" & _
                                    " AND    Text1 = '" & lvItem.Text.Replace("'", "''") & "'" & _
                                    " AND    Text2 = '" & tmpStr.Replace("'", "''") & "'"


                    .ExecuteNonQuery()
                Next
            Next

            .CommandText = "COMMIT"
            .ExecuteNonQuery()

            .Dispose()

        End With

        Me.Close()

        Return 0

    End Function

    Private Function GetCategoryID(ByVal Cat As String)

        Dim count As Integer

        For count = 0 To cmbCategories.Items.Count - 1
            If cmbCategories.Items(count).ToString.Trim = Cat.Trim Then
                Exit For
            End If
        Next

        Return count

    End Function


    Private Sub btnFrmDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnFrmDelete.Click

        Dim count As Integer = 0
        Dim cmd As New OdbcCommand


        If TemplateID > 0 Then

            If MsgBox("Delete Template - Continue ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                With cmd
                    .Connection = connStr

                    .CommandText = "DELETE FROM Templates" & _
                    " WHERE Template = " & TemplateName
                    .Dispose()

                End With
            End If
        Else
            MsgBox("No Template Selected")
        End If

        clearAll()
        LoadAll()

    End Sub



    Private Sub BtnCategoryDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCategoryDel.Click

        While lvCategory.SelectedItems.Count > 0
            lvCategory.SelectedItems(0).Remove()
        End While
        clearQuestions()

    End Sub

    Private Sub lvCategory_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvCategory.KeyDown

        If e.KeyCode = Keys.Delete Then

            While lvCategory.SelectedItems.Count > 0
                lvCategory.SelectedItems(0).Remove()
            End While

            clearQuestions()
            lvQuestions.Focus()

        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim count As Integer

        If lvTemplateQuestions.Items.Count > 0 Then
            For count = 0 To lvTemplateQuestions.Items.Count - 1
                lvTemplateQuestions.Items.Item(count).Selected = True
            Next
        End If
        lvTemplateQuestions.Refresh()
        lvTemplateQuestions.Focus()
    End Sub


    Private Sub cmbDataBlock_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDataBlock.SelectedIndexChanged

        arrAllQuestions = Nothing
        arrTemplateQuestions = Nothing

        clearForm()
        LoadAllQuestions()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim TemplateID As Integer = 0
        Dim SQLcommand As OdbcCommand
        Dim tmpTxt As String = ""

        If MsgBox("Reset/Create Template ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then


            SQLcommand = connStr.CreateCommand

            If txtTemplateNew.Text.Trim.Length > 0 Then 'Add new Template
                tmpTxt = txtTemplateNew.Text
                TemplateID = getTemplateID(txtTemplateNew.Text)
                TemplateName = tmpTxt
            ElseIf cmbTemplate.SelectedIndex > -1 Then
                tmpTxt = cmbTemplate.Text
                TemplateName = tmpTxt
                TemplateID = getTemplateID(cmbTemplate.SelectedItem)
            End If


            SQLcommand.CommandText = "UPDATE DataBlocks " & _
                                    " SET Templates = Templates | " & TemplateID

            SQLcommand.ExecuteNonQuery()

            LoadAll()

            cmbTemplate.Text = tmpTxt

        End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim TemplateID As Integer = 0
        Dim SQLcommand As OdbcCommand

        SQLcommand = connStr.CreateCommand

        If cmbTemplate.SelectedIndex > -1 Then

            If MsgBox("Delete Template ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then


                TemplateID = getTemplateID(cmbTemplate.SelectedItem)

                SQLcommand.CommandText = "DELETE FROM Templates " & _
                                        " WHERE Template = '" & TemplateName & "'"

                SQLcommand.ExecuteNonQuery()

                SQLcommand.CommandText = "UPDATE DataBlocks " & _
                                        " SET Templates = Templates & " & TemplateID

                SQLcommand.ExecuteNonQuery()

                clearForm()
                LoadAll()

            End If

        End If

    End Sub

End Class
