Imports System.Windows.Forms
Imports System.Management

Public Class frmReg

    Private Lock As String = ""
    Private TrialStart As String = ""

    Private TrialPeriod As Integer


    Private Sub frmReg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim RightNow As Date = Today
        Dim DriveLetter As String = Application.StartupPath.Substring(0, 1)
        Dim serialNo As String = GetVolumeSerialNumber(DriveLetter)
        Dim DName As String = ""
        Dim RegCode As String = ""
        Dim TrialStartDays As String = frmMain.appVars.DateToDays(Today)


        Try
            TrialPeriod = frmMain.appRegistry.getKeyValue("TrialPeriod")
        Catch ex As Exception
            TrialPeriod = 15
        End Try

        DName = frmMain.appRegistry.getKeyValue("DName")
        TrialStart = frmMain.appRegistry.getKeyValue("TrialStart")
        Lock = frmMain.appRegistry.getKeyValue("Lock")
        RegCode = frmMain.appRegistry.getKeyValue("RegCode")


        If RegCode <> "" Then
            If DName = GetVolumeSerialNumber(DriveLetter) Then
                frmMain.appVars.RegistrationOK = True
                frmMain.appVars.RegCode = RegCode
            Else
                MessageBox.Show("This program may have been tampered with.  Please re-install.")
                frmMain.appVars.RegistrationOK = False
                frmMain.appVars.RegCode = ""
                Me.Close()
            End If
            Me.Close()
        Else

        End If


        If DName = "" Then
            Lock = Asc(GetVolumeSerialNumber(DriveLetter)) & Asc(Today)
            txtLock.Text = Lock
            btnExit.Focus()

            TrialStart = frmMain.appVars.DateToDays(Today)
            frmMain.appRegistry.setKeyValue("DName", serialNo)
            frmMain.appRegistry.setKeyValue("TrialStart", TrialStart)
            frmMain.appRegistry.setKeyValue("Lock", Lock)
        Else
            If Not DName = GetVolumeSerialNumber(DriveLetter) Then
                MessageBox.Show("This program may have been tampered with.  Please re-install.")
                frmMain.appVars.RegistrationOK = False
                Me.Close()
            End If
        End If


        Dim DaysLeft As TimeSpan = Now.Subtract(frmMain.appVars.DaysToDate(TrialStart))
        If TrialPeriod - Math.Truncate(DaysLeft.TotalDays) > 0 Then
            lblDays.Text = TrialPeriod - Math.Truncate(DaysLeft.TotalDays)
        Else
            lblDays.Text = 0
            btnContinueTrial.Text = "Close Program"
        End If

        txtLock.Text = Lock


    End Sub


    Private Sub btnContinueTrial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinueTrial.Click



        If TrialStart = "" Then TrialStart = Today

        Dim DaysLeft As TimeSpan = Now.Subtract(frmMain.appVars.DaysToDate(TrialStart))

        If TrialPeriod - Math.Truncate(DaysLeft.TotalDays) > 0 Then
            frmMain.appVars.RegistrationOK = True
            Me.Close()
        Else
            frmMain.appVars.RegistrationOK = False
            Me.Close()
        End If


    End Sub

    Private Sub btnCompleteRegistration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompleteRegistration.Click

        Dim intRegDays As Integer
        Dim errFlag As Boolean = False
        Dim strRegCode As String = ""

        Try
            intRegDays = txtKey.Text.Substring(0, 1) & txtKey.Text.Substring(2, 1) & txtKey.Text.Substring(4, 1)
            strRegCode = txtKey.Text.Substring(1, 1) & txtKey.Text.Substring(3, 1) & txtKey.Text.Substring(5)
        Catch ex As Exception
            errFlag = True
        End Try

        If strRegCode.Length > 1 And errFlag = False Then

            If CInt((Lock * 3) / 2) = strRegCode Then

                If intRegDays = 0 Then
                    frmMain.appRegistry.setKeyValue("RegCode", txtKey.Text)
                End If

                frmMain.appRegistry.setKeyValue("TrialPeriod", intRegDays)
                frmMain.appVars.RegistrationOK = True
                Me.Close()
            Else
                MessageBox.Show("Invalid Reg. Number")
            End If
        Else
            MessageBox.Show("Invalid Reg. Number")
        End If


    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click

        frmMain.appVars.RegistrationOK = False
        Me.Close()

    End Sub


    Private Sub chkTerms_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTerms.CheckedChanged

        If chkTerms.Checked Then
            txtKey.Enabled = True
            btnContinueTrial.Enabled = True
            btnCompleteRegistration.Enabled = True
        Else
            txtKey.Enabled = False
            btnContinueTrial.Enabled = False
            btnCompleteRegistration.Enabled = False
        End If
    End Sub


    Public Function GetVolumeSerialNumber(Optional ByVal strDriveLetter As String = "C")

        Dim disk As ManagementObject = New ManagementObject(String.Format("win32_logicaldisk.deviceid=""{0}:""", strDriveLetter))
        disk.Get()

        Return disk("VolumeSerialNumber").ToString()
    End Function


End Class
