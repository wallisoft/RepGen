Imports System.Data.Odbc
Imports System.Drawing

Public Class clsReportFunctions

    Public oldRtf As String
    Private arrParts() As String
    Private fromPos As Integer
    Private toPos As Integer


    Public Sub AddToReport()

        Dim status As Integer = 0
        Dim repTxt As String = ""
        Dim count As Integer = 0
        Dim curLine As String = ""
        Dim str As String()
        Dim tmpStr As String


        'tmpStr = frmMain.catControlAttributes(frmMain.currControl).Action.Replace(vbNewLine, "¬") 'odd workaround but works fine :)
        'str = tmpStr.Split("¬") #MYSQL#


        tmpStr = frmMain.catControlAttributes(frmMain.currControl).Action
        str = tmpStr.Split(vbCrLf)



        Dim oldData As System.Windows.Forms.DataObject = System.Windows.Forms.Clipboard.GetDataObject() 'backup clipboard

        For Each currLine In str

            If currLine.Trim.Length > 0 Then

                Try
                    arrParts = Split(currLine, "|")
                Catch ex As Exception
                    status = 1
                    Exit For
                End Try

                ReDim Preserve arrParts(20) 'arbitary number > current necessary


                If arrParts.Length = 0 Then Exit For
                If arrParts(0).ToString.Trim.Length = 0 Or arrParts(2).ToString.Trim.Length = 0 Then Exit For 'main category
                If arrParts(1) = Nothing Then arrParts(1) = "" 'sub category
                If arrParts(2) = Nothing Then Exit For 'text to insert
                If arrParts(3) = Nothing Then arrParts(3) = "Regular" 'font style
                If arrParts(4) = Nothing Then arrParts(4) = -1 'number of adjacent CRLF to search for 
                If arrParts(5) = Nothing Then arrParts(5) = "" 'prepend text - sentence starter, or expanded first time action
                If arrParts(6) = Nothing Then arrParts(6) = -1 'sub category parent
                If arrParts(7) = Nothing Then arrParts(7) = "insert" 'insert or delete 
                If arrParts(8) = Nothing Then arrParts(8) = "" 'allow new lines in previously added text - should be ok for all controls #TODO#
                If arrParts(9) = Nothing Then arrParts(9) = "" 'append text



                If arrParts(0).Trim = "#IMPORT#" Then
                    procImport()
                Else
                    procInsert()
                End If

            End If
        Next


        Try
            Clipboard.SetDataObject(oldData, True) 'restore clipboard
        Catch ex As Exception
        End Try

    End Sub

    Private Sub procImport()

        Dim tmpRtb As RichTextBox = New RichTextBox
        Dim FromPos As Integer
        Dim ToPos As Integer
        Dim srcFromStr As String
        Dim srcToStr As String
        Dim tarFromStr As String
        Dim tarToStr As String
        Dim srcDocTag As String
        Dim tmpSql As String
        Dim count As Integer

        System.Windows.Forms.Clipboard.Clear()

        For count = 0 To 4
            If arrParts(count).Trim.Length = 0 Then
                Exit Sub
            End If
        Next


        srcDocTag = arrParts(1)
        srcFromStr = arrParts(2)
        srcToStr = arrParts(3)
        tarFromStr = arrParts(4)
        tarToStr = arrParts(5)


        tmpSql = "SELECT DocText, DocumentID " & _
                "FROM  Documents " & _
                "WHERE DocumentTag = '" & srcDocTag.Trim & "' " & _
                "AND   ClientID = " & frmMain.currClientID & " " & _
                "ORDER BY 2 DESC"


        Dim SQLcommand As OdbcCommand

        SQLcommand = frmMain.connStr1.CreateCommand
        SQLcommand.Connection.Open()



        SQLcommand.CommandText = tmpSql

        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        If SQLreader.Read() Then
            tmpRtb.Rtf = SQLreader(0).ToString.Replace("\\", "\")
        End If


        FromPos = tmpRtb.Text.IndexOf(srcFromStr)

        If FromPos > 0 Then
            FromPos += srcFromStr.Length
        End If

        ToPos = tmpRtb.Text.IndexOf(srcToStr)

        If FromPos > -1 And ToPos > -1 Then
            tmpRtb.Select(FromPos, (ToPos - FromPos) - 1)
            tmpRtb.Copy()
        End If

        FromPos = frmMain.rtbDefault.Text.IndexOf(tarFromStr)

        If FromPos > 0 Then
            FromPos += tarFromStr.Length
        End If

        ToPos = frmMain.rtbDefault.Text.IndexOf(tarToStr)

        If FromPos > -1 And ToPos > -1 Then
            frmMain.rtbDefault.Select(FromPos, (ToPos - FromPos) - 1)
            frmMain.rtbDefault.Paste()
        End If

        SQLcommand.Connection.Close()


        frmMain.Refresh()

    End Sub

    Private Sub procInsert()

        Dim repPos As Integer
        Dim insTxt As String
        Dim count As Integer
        Dim count1 As Integer
        Dim count2 As Integer
        Dim status As Integer


        If arrParts(1).ToString.Trim.Length > 0 Then

            arrParts(1) = ProcessRepVals(arrParts(1))

            If frmMain.rtbDefault.Text.IndexOf(arrParts(1)) = -1 Then

                repPos = frmMain.rtbDefault.Text.IndexOf(arrParts(0))

                If repPos = -1 Then
                    status = 1
                Else
                    repPos += arrParts(0).Length

                    If arrParts(6) > -1 Then
                        repPos = insertSubCat(arrParts(6), repPos, arrParts(1))
                    End If

                    If arrParts(7).ToUpper = "Y" Then
                        For count = repPos To frmMain.rtbDefault.TextLength - 1

                            frmMain.rtbDefault.Select(count, 1)

                            Dim a As String = frmMain.rtbDefault.SelectedText

                            If frmMain.rtbDefault.SelectionFont.Bold = True And frmMain.rtbDefault.SelectedText.Trim <> "" Then
                                repPos = count - 2
                                frmMain.rtbDefault.Select(0, 0)
                                Exit For
                            End If
                        Next
                    End If


                    frmMain.rtbDefault.SelectionStart = repPos
                    frmMain.rtbDefault.SelectionFont = New Font("Times New Roman", 12, FontStyle.Bold)
                    frmMain.rtbDefault.SelectedText = vbCrLf & vbCrLf & arrParts(1)

                End If
            End If
        End If

        'set prev insert to black
        frmMain.rtbDefault.Select(0, frmMain.rtbDefault.TextLength)
        frmMain.rtbDefault.SelectionColor = Color.Black
        frmMain.rtbDefault.Select(0, 0)


        If arrParts(1).Trim.Length > 0 Then
            arrParts(0) = arrParts(1).Trim  'now subheading inserted treat as main heading
        Else
            arrParts(0) = arrParts(0).Trim
        End If


        If frmMain.catControlAttributes(frmMain.currControl).counter = 0 Then
            insTxt = arrParts(5).ToString.Trim & " " & ProcessRepVals(arrParts(2))
        Else
            insTxt = ProcessRepVals(arrParts(2))
        End If

        repPos = frmMain.rtbDefault.Text.IndexOf(arrParts(0))

        If repPos = -1 Then
            status = -1
        Else
            repPos += arrParts(0).Length

            For count = 1 To arrParts(4) 'find reqd number of adjacent newlines
                count1 = frmMain.rtbDefault.Text.ToString.ToString _
                    .IndexOf(ControlChars.Lf.ToString() & ControlChars.Lf.ToString(), repPos)

                repPos = count1
            Next
        End If


        '#sw find next bold text and count back 1 newline. set this as ins pos if ?

        If arrParts(7).ToUpper = "Y" Then
            For count = repPos To frmMain.rtbDefault.TextLength - 1

                frmMain.rtbDefault.Select(count, 1)

                Dim a As String = frmMain.rtbDefault.SelectedText

                If frmMain.rtbDefault.SelectionFont.Bold = True And frmMain.rtbDefault.SelectedText.Trim <> "" Then
                    repPos = count - 2
                    frmMain.rtbDefault.Select(0, 0)
                    Exit For
                End If
            Next
        End If


        If status > -1 Then

            If arrParts(5).Length > 0 And frmMain.catControlAttributes(frmMain.currControl).counter = 1 Then
                insTxt = arrParts(5) & insTxt
            End If

            'uppercase first letter and prepend space.
            If insTxt.Trim.Length > 0 Then
                insTxt = " " & insTxt.Substring(0, 1).ToUpper & insTxt.Substring(1)
            End If


            frmMain.rtbDefault.SelectionStart = repPos

            Dim fnt As Font = frmMain.fntCurrentFont

            If arrParts(3).Substring(0, 1).ToUpper = "B" Then
                frmMain.rtbDefault.SelectionFont = New Font(fnt.Name, fnt.Size, FontStyle.Bold)
            ElseIf arrParts(3).Substring(0, 1).ToUpper = "U" Then
                frmMain.rtbDefault.SelectionFont = New Font(fnt.Name, fnt.Size, FontStyle.Underline)
            ElseIf arrParts(3).Substring(0, 1).ToUpper = "I" Then
                frmMain.rtbDefault.SelectionFont = New Font(fnt.Name, fnt.Size, FontStyle.Italic)
            ElseIf arrParts(3).Substring(0, 1).ToUpper = "S" Then
                frmMain.rtbDefault.SelectionFont = New Font(fnt.Name, fnt.Size, FontStyle.Strikeout)
            Else
                frmMain.rtbDefault.SelectionFont = New Font(fnt.Name, fnt.Size, FontStyle.Regular)
            End If


            frmMain.rtbDefault.SelectionColor = Color.Green
            frmMain.rtbDefault.SelectedText = insTxt


            If insTxt.Contains("#SIGNATURE#") Then
                Dim img As Image
                Try
                    repPos = frmMain.rtbDefault.Text.IndexOf("#SIGNATURE#")
                    frmMain.rtbDefault.Select(repPos, 11)
                    img = Image.FromFile(frmMain.appVars.User.sigFile)
                    Clipboard.SetImage(img)
                    frmMain.rtbDefault.Paste()
                Catch ex As Exception
                End Try
            End If

            If insTxt.Contains("#SUPSIGNATURE#") Then
                Dim img As Image
                Try
                    repPos = frmMain.rtbDefault.Text.IndexOf("#SUPSIGNATURE#")
                    frmMain.rtbDefault.Select(repPos, 14)
                    img = Image.FromFile(frmMain.appVars.User.sigFile1)
                    Clipboard.SetImage(img)
                    frmMain.rtbDefault.Paste()
                Catch ex As Exception
                End Try
            End If

            frmMain.rtbDefault.Text.Replace("#SIGNATURE#", "").Replace("#SUPSIGNATURE#", "")


            'set repPos line number to center of rich text box

            count = 0
            count1 = 0
            count2 = 0

            For Each line As String In frmMain.rtbDefault.Lines

                count += (line.Length / 110) + 1
                count1 += line.Length


                If count1 > repPos Then
                    Exit For
                End If
            Next

            count -= 14
            count1 = 0
            count2 = 0


            For Each line As String In frmMain.rtbDefault.Lines

                count2 += (line.Length / 110) + 1
                count1 += line.Length

                If count2 >= count Then
                    Exit For
                End If
            Next

            If count1 > 0 Then
                repPos = count1
            Else
                repPos = 0
            End If

            frmMain.rtbDefault.Select(repPos, 0)
            frmMain.rtbDefault.ScrollToCaret()

            frmMain.oldReportCursorPos = repPos

        End If
        status = 0


    End Sub


    Public Function insertSubCat(ByVal parent As Integer, ByVal repPos As Integer, ByVal subCat As String)

        Dim nextSub As String = ""
        Dim count As Integer = 0
        Dim parentControl As Integer = -1
        Dim count1 As Integer = 0
        Dim catControlAttributes As frmMain.strAttributes
        Dim repPos1 As Integer = repPos
        Dim currCat As String = ""
        Dim currCatPos As Integer = -1
        Dim nextCat As String = ""
        Dim nextCatPos As Integer = -1
        Dim tmpTxt As String = ""
        Dim tmpTxt1 As String = ""
        Dim tmpStr As String = ""

        subCat = subCat.Substring(0, subCat.Length - 1).Trim  'cut ':' from end

        For Each catControlAttributes In frmMain.catControlAttributes

            If catControlAttributes.ControlID = parent Then
                parentControl = count
                Exit For
            End If
            count += 1
        Next

        tmpTxt = frmMain.rtbDefault.Text.Substring(repPos)
        tmpTxt1 = tmpTxt

        repPos1 = repPos

        While True

            'cut next sub category

            tmpTxt = tmpTxt.Substring(tmpTxt.IndexOf(vbLf) + 1)

            nextSub = tmpTxt.Substring(0, tmpTxt.IndexOf(":")).Trim

            repPos = frmMain.rtbDefault.Text.IndexOf(nextSub & ":")

            tmpTxt = tmpTxt.Substring(nextSub.Length)

            'get position of sub category in parent

            count = 0
            nextCatPos = -1
            currCatPos = -1

            If parentControl > -1 Then

                For Each tmpStr In frmMain.catControls(parentControl).items

                    If tmpStr = nextSub Then
                        nextCatPos = count
                    End If

                    If tmpStr = subCat Then
                        currCatPos = count
                    End If

                    count += 1
                Next
            Else
                Exit While
            End If


            If nextCatPos = -1 Then 'next subcat not found in parent so insert here
                Exit While
            End If

            If nextCatPos > currCatPos Then 'next subcat after curr subcat so insert here
                Exit While
            End If

        End While

        Return repPos - 2

    End Function


    Public Function ProcessRepVals(ByVal repVals As String)

        Dim tmpStr As String
        Dim tmpStr1 As String
        Dim tmpStr2 As String
        Dim tmpPos As Integer
        Dim tmpID As Integer
        Dim selText As String
        Dim tmpChild As Integer

        tmpStr = repVals

        While True

            tmpPos = tmpStr.IndexOf("#VAL")

            If Not tmpPos > -1 Then
                Exit While
            End If

            tmpStr1 = tmpStr.Substring(tmpPos + 4, 3)
            tmpStr2 = tmpStr.Substring(tmpPos, 8)

            tmpID = tmpStr1
            tmpStr = tmpStr.Substring(tmpPos + 3)

            selText = ""


            tmpChild = frmMain.GetControlID(tmpID)

            If tmpChild > -1 Then
                If frmMain.catControlAttributes(tmpChild).Type = 5 Then
                    For Each item As String In frmMain.catControls(tmpChild).SelectedItems
                        selText += item.Replace("'", "''") & vbCrLf
                    Next
                Else
                    selText = frmMain.catControls(tmpChild).Text.Replace("'", "''")
                End If
            End If

            repVals = repVals.Replace(tmpStr2, selText.Trim)

        End While

        repVals = FormatGenderRules(repVals)

        repVals = repVals.Replace("#CRLF#", vbCrLf)

        repVals = repVals.Replace("#UserFN#", frmMain.appVars.User.Fname)
        repVals = repVals.Replace("#UserSN#", frmMain.appVars.User.Sname)
        repVals = repVals.Replace("#UserTitle#", frmMain.appVars.User.Title)
        repVals = repVals.Replace("#UserQuals#", frmMain.appVars.User.Quals)

        repVals = repVals.Replace("#SupText#", frmMain.appVars.User.supText)

        repVals = repVals.Replace("#Date#", Date.Today.ToString.Substring(0, Date.Today.ToString.IndexOf(" ")))

        repVals = repVals.Replace("#Time#", Now.ToString("t"))

        Try
            repVals = repVals.Replace("#Age#", Math.Floor(DateDiff(DateInterval.Month, _
                Convert.ToDateTime(frmMain.txtDOB.Text.ToString), Date.Today) / 12))
        Catch ex As Exception

        End Try


        repVals = repVals.Replace("#Title#", frmMain.cmbTitle.Text.ToString)
        repVals = repVals.Replace("#Fname#", frmMain.txtFname.Text.ToString)
        repVals = repVals.Replace("#Sname#", frmMain.txtLname.Text.ToString)
        repVals = repVals.Replace("#DOB#", frmMain.txtDOB.Text.ToString)
        repVals = repVals.Replace("#SSN#", frmMain.txtSSN.Text.ToString)
        repVals = repVals.Replace("#Ref#", frmMain.txtRefNo.Text.ToString)
        repVals = repVals.Replace("#Sex#", frmMain.cmbSex.Text.ToString)
        repVals = repVals.Replace("#Ethnicity#", frmMain.cmbEthnicity.Text.ToString)
        repVals = repVals.Replace("#City#", frmMain.txtCity.Text.ToString)
        repVals = repVals.Replace("#State#", frmMain.txtState.Text.ToString)
        repVals = repVals.Replace("#Zip#", frmMain.txtZip.Text.ToString)
        repVals = repVals.Replace("#HomePhone#", frmMain.txtHomePhone.Text.ToString)
        repVals = repVals.Replace("#WorkPhone#", frmMain.txtWorkPhone.Text.ToString)
        repVals = repVals.Replace("#CellPhone#", frmMain.txtCellPhone.Text.ToString)
        repVals = repVals.Replace("#Email#", frmMain.txtEmail.Text.ToString)
        repVals = repVals.Replace("#Template#", frmMain.cmbTemplate.Text.ToString)
        repVals = repVals.Replace("#Notes#", frmMain.txtNotes.Text.ToString)

        repVals = repVals.Replace("#Count#", frmMain.catControlAttributes(frmMain.currControl).counter)
        'If frmMain.currControl > 0 Then

        repVals = repVals.Replace("#ClientID#", frmMain.currClient.ClientID)

        Return repVals

    End Function


    Public Function FormatGenderRules(ByVal strAnswer As String)

        Dim arrCols() As String
        Dim count As Integer = 0
        Dim tmpStr As String = ""
        Dim tmpStr1 As String = " "


        If strAnswer.Contains("//") Or strAnswer.Contains("||") Or strAnswer.Contains("\\") Then

            arrCols = strAnswer.Split(" ")

            For count = 0 To arrCols.Length - 1
                tmpStr = arrCols(count)

                If tmpStr.Contains("//") Then
                    If frmMain.cmbSex.Text.Trim.ToLower = "male" Then
                        tmpStr = tmpStr.Substring(0, tmpStr.IndexOf("/"))
                    ElseIf frmMain.cmbSex.Text.Trim.ToLower = "female" Then
                        tmpStr = tmpStr.Substring(tmpStr.IndexOf("/") + 2)
                    Else
                        tmpStr = tmpStr.Replace("//", "/")
                    End If

                    arrCols(count) = tmpStr

                ElseIf tmpStr.Contains("\\") Then

                    If frmMain.cmbSex.Text.Trim.ToLower = "male" Then
                        tmpStr = tmpStr.Substring(0, tmpStr.IndexOf("\"))
                    ElseIf frmMain.cmbSex.Text.Trim.ToLower = "female" Then
                        tmpStr = tmpStr.Substring(tmpStr.IndexOf("\") + 2)
                    Else
                        tmpStr = tmpStr.Replace("\\", "\")
                    End If

                    arrCols(count) = tmpStr

                ElseIf tmpStr.Contains("||") Then

                    If frmMain.cmbSex.Text.Trim.ToLower = "male" Then
                        tmpStr = tmpStr.Substring(0, tmpStr.IndexOf("|"))
                    ElseIf frmMain.cmbSex.Text.Trim.ToLower = "female" Then
                        tmpStr = tmpStr.Substring(tmpStr.IndexOf("|") + 2)
                    Else
                        tmpStr = tmpStr.Replace("||", "|")
                    End If

                    arrCols(count) = tmpStr

                End If

                tmpStr1 = tmpStr1 + " " + arrCols(count)
            Next

            strAnswer = tmpStr1.Trim
        End If

        Return strAnswer

    End Function


End Class
