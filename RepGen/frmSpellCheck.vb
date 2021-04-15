Imports System.Windows.Forms

Public Class frmSpellCheck

    Private Rtbox As RichTextBox
    Private SChk As clsSpellCheck
    Private CurWord As String, CurLoc As String
    Private StartLoc As Integer, EndLoc As Integer, TotalLoc As Integer, cl As Integer
    Private Ended As Boolean = False
    Private ScheckDiff As Integer
    Private msgvar As Integer


    Private Function Initiate()
        cl = Rtbox.SelectionStart
        If Rtbox.SelectionLength = 0 Then
            StartLoc = 1
            EndLoc = Rtbox.TextLength
            TotalLoc = Rtbox.SelectionLength
        Else
            StartLoc = Rtbox.SelectionStart + 1
            EndLoc = Rtbox.SelectionStart + Rtbox.SelectionLength
            TotalLoc = Rtbox.SelectionLength
        End If
        CurLoc = StartLoc
    End Function

    Private Function FindNextNotFound()
        Word2.Text = ""
        If Ended Then
            Rtbox.SelectionLength = 0
            Rtbox.SelectionStart = cl
            msgvar += 1
            If msgvar = 1 Then MsgBox("   Spell Check Finished.")
            Me.Dispose()
            Exit Function
        End If
        Word1.Text = ""
        While True
            If Word1.Text <> "" Then Exit While
            CurWord = FindCurWord()
            If Ended = True Then Exit While
            ChkWord(CurWord)
        End While
        If Ended And Word1.Text = "" Then
            Rtbox.SelectionLength = 0
            Rtbox.SelectionStart = cl
            msgvar += 1
            If msgvar = 1 Then MsgBox("    Spell Check Finished.")
            Me.Dispose()
            Exit Function
        End If
    End Function

    Private Function ChkWord(ByVal Word As String)
        Dim S As String = ""
        Try
            S = SChk.ChangeAllList.Item(Word)
        Catch ex1 As System.NullReferenceException
        Catch ex2 As Exception
        End Try
        If S <> "" Then
            Word2.Text = S
            NotFound(Word)
            ChangeWord()
            FindNextNotFound()
            Exit Function
        End If
        If SChk.DicT.Tables("Words").Rows.Contains(Word) Then
            Word1.Text = ""
        Else
            If Len(Word) >= 3 And Word.Substring(Len(Word) - 1, 1).ToLower = "s" Then
                If SChk.DicT.Tables("Words").Rows.Contains(Word.Substring(0, Len(Word) - 1)) Then
                    Word1.Text = ""
                Else
                    NotFound(Word)
                End If
            Else
                NotFound(Word)
            End If
        End If
    End Function

    Private Function NotFound(ByVal Word)
        Word1.Text = Word
        Rtbox.SelectionStart = CurLoc - Len(Word) - 2
        Rtbox.SelectionLength = Len(Word)
        CurLoc = CurLoc + ScheckDiff
    End Function

    Private Function ChangeWord()
        Rtbox.SelectedText = Word2.Text
        ScheckDiff = Len(Word2.Text) - Len(Word1.Text)
        CurLoc += ScheckDiff
        EndLoc += ScheckDiff
        ScheckDiff = 0
    End Function

    Private Function FindCurWord() As String
        If Ended Then Exit Function
        Dim C As Char, Word As String = ""
        While True
            C = Mid(Rtbox.Text, CurLoc, 1)
            If Char.IsLetter(Chr(Asc(C))) Then
                CurLoc += 1
                If CurLoc >= EndLoc Then Ended = True : Exit While
                Word += C
            Else
                CurLoc += 1
                If CurLoc >= EndLoc Then Ended = True : Exit While
                If Len(Word) > 0 Then Exit While
            End If
        End While
        Return Word
    End Function

    Private Function AddToDictionary()
        Dim DR1 As DataRow, DR2 As DataRow
        DR1 = SChk.DicT.Tables("Words").NewRow
        DR2 = SChk.DicT.Tables("Custom").NewRow
        DR1.Item(0) = Word1.Text
        DR2.Item(0) = Word1.Text
        SChk.DicT.Tables("Words").Rows.Add(DR1)
        SChk.DicT.Tables("Custom").Rows.Add(DR2)
        SChk.Adp2.Update(SChk.DicT, "Custom")
    End Function

    Private Function AddToIgnoreAllList()
        Dim dr As DataRow = SChk.DicT.Tables("Words").NewRow
        dr.Item(0) = Word1.Text
        SChk.DicT.Tables("Words").Rows.Add(dr)
    End Function

    Private Function AddToIgnoreAllList(ByVal Word)
        Dim dr As DataRow = SChk.DicT.Tables("Words").NewRow
        dr.Item(0) = Word
        SChk.DicT.Tables("Words").Rows.Add(dr)
    End Function

    Private Function AddToChangeAllList()
        Try
            SChk.ChangeAllList.Add(Word1.Text, Word2.Text)
        Catch ex As ArgumentException
        End Try
    End Function

    Public Function setSP(ByRef RichTxtBox As RichTextBox, ByRef Sc As clsSpellCheck)
        Rtbox = RichTxtBox
        SChk = Sc
        Rtbox.HideSelection = False
    End Function

    Private Sub frMSpellCheck_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Initiate()
        FindNextNotFound()
    End Sub

    Private Sub btnIgnore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIgnore.Click
        FindNextNotFound()
    End Sub

    Private Sub btnIgnoreAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIgnoreAll.Click
        AddToIgnoreAllList()
        FindNextNotFound()
    End Sub

    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        ChangeWord()
        FindNextNotFound()
    End Sub

    Private Sub butChangeAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeAll.Click
        AddToChangeAllList()
        ChangeWord()
        FindNextNotFound()
    End Sub


    Private Sub btnAdd2Dict_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd2Dict.Click
        AddToDictionary()
        FindNextNotFound()
    End Sub

End Class
