Public Class clsVariables

    Public Structure structUser
        Dim UserID As Integer
        Dim UserLevel As String
        Dim Login As String
        Dim Fname As String
        Dim Sname As String
        Dim Title As String
        Dim Quals As String
        Dim Password As String
        Dim sigFile As String
        Dim sigFile1 As String
        Dim supText As String
    End Structure


    Public AppClose As Boolean = False
    Public User As structUser
    Public ControlID As Integer
    Public UserFname As String
    Public UserSname As String
    Public RegistrationOK As Boolean
    Public RegCode As String = ""


    Public Function fmb(ByVal msg As String) As String  'system messagebox text centering  ##Add as overload

        Dim count As Integer

        count = Convert.ToInt32(msg.Length / 2)
        count = 21 - count
        If count < 0 Then count = 0
        msg = Space(count) & msg

        Return msg

    End Function

    Public Function DateToDays(ByVal tmpDate As Date)

        Dim dtStartDate As Date = "1/1/1900"
        Dim tsTimeSpan As TimeSpan
        Dim iNumberOfDays As Integer

        tsTimeSpan = tmpDate.Subtract(dtStartDate)

        iNumberOfDays = tsTimeSpan.Days

        Return iNumberOfDays

    End Function

    Public Function DaysToDate(ByVal iNumberOfDays As Integer)

        Dim dtStartDate As Date = "1/1/1900"
        Dim tmpDate As Date

        tmpDate = dtStartDate.AddDays(iNumberOfDays)

        Return tmpDate

    End Function

    Public Function DaysToString(ByVal iNumberOfDays As Integer)

        Dim dtStartDate As Date = "1/1/1900"
        Dim tmpStr As String
        Dim tmpArr() As String

        tmpStr = dtStartDate.AddDays(iNumberOfDays).ToString

        tmpArr = tmpStr.Split("/")

        If tmpArr(0).Trim.Length = 1 Then tmpArr(0) = "0" & tmpArr(0).Trim
        If tmpArr(1).Trim.Length = 1 Then tmpArr(1) = "0" & tmpArr(1).Trim

        tmpStr = tmpArr(0) & "/" & tmpArr(1) & "/" & tmpArr(2).Trim

        Return tmpStr.Substring(0, tmpStr.IndexOf(" ")) 'remove time element

    End Function



    Public Function GetFromDB(ByVal dbVal As Object)

        Dim retVal As String

        If IsDBNull(dbVal) Then
            retVal = ""
        Else
            retVal = dbVal.ToString
        End If

        Return retVal

    End Function

End Class
