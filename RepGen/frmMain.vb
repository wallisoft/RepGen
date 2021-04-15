Imports Microsoft.Win32
Imports System.Data.Odbc
Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Management
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Threading
Imports System.Globalization



Public Class frmMain

    Dim currentDomain As AppDomain = AppDomain.CurrentDomain

    Public Structure strAttributes
        'databaase values
        Dim ControlID As Integer
        Dim Name As String
        Dim DataBlock As String
        Dim CatID As Integer
        Dim Child As Integer
        Dim ChildDataMode As String
        Dim Type As Integer
        Dim AllowEdit As Integer
        Dim Action As String
        Dim Location As String
        Dim Size As String
        Dim Tag As String
        Dim ConText As String
        Dim ScrollBars As String
        Dim SelectMode As String
        Dim SelectStyle As String
        Dim TabIndex As Integer
        Dim SaveField As Integer 'control.text saved in document searchfield. 1,2,3 int fields and 4,5,6 string 
        Dim Items As String
        Dim Font As String
        Dim LinkedItems
        Dim OnLoad As String

        'internal use only
        Dim SelectCount As Integer
        Dim FirstTimeAction As String
        Dim FirstTimeSelected As Boolean
        Dim DatabaseUpdate As String
        Dim PositionOrder As String
        Dim selecting As Boolean ' combo text_changed duplicate fix
        Dim counter As Integer
        Dim ParentIndex As Integer
        Dim Enabled As Boolean
        Dim Data As String
        Dim Selected As Boolean
    End Structure

    Public Structure strClient
        Dim ClientID As Integer
        Dim Title As String
        Dim Fname As String
        Dim Sname As String
        Dim DOB As Date
        Dim SSN As String
        Dim Ref As String
        Dim MaritalStatus As String
        Dim Sex As String
        Dim Ethnicity As String
        Dim Add1 As String
        Dim City As String
        Dim State As String
        Dim Zip As String
        Dim HomePhone As String
        Dim WorkPhone As String
        Dim CellPhone As String
        Dim Email As String
        Dim Template As String
        Dim Notes As String
    End Structure

    Public Structure strDocument
        Dim Searchfield1 As String
        Dim Searchfield2 As String
        Dim Searchfield3 As String
        Dim Searchfield4 As Decimal
        Dim Searchfield5 As Decimal
        Dim Searchfield6 As Decimal
    End Structure

    Public MyDocs As String

    'dotnet forces odbc setup here. reconfigured in clsDbFuncs if not local

    Public connStr = New OdbcConnection("DRIVER=SQLite3 ODBC Driver;Database=.\Data.s3db;LongNames=0;Timeout=1000;NoTXN=0;SyncPragma=NORMAL;StepAPI=0;")
    Public connStr1 = New OdbcConnection("DRIVER=SQLite3 ODBC Driver;Database=.\Client.s3db;LongNames=0;Timeout=1000;NoTXN=0;SyncPragma=NORMAL;StepAPI=0;")


    Public ClientConnStr As String = ""
    Public ClientDbType As String = ""
    Public ClientDbName As String = ""
    Public ClientDbServer As String = ""
    Public ClientDbUser As String = ""
    Public ClientDbPassword As String = ""


    Public Report As clsReportFunctions = New clsReportFunctions
    Public treeNodes() As Integer
    Public treeNodeDocument() As Integer
    Public treeNodeHeader() As Boolean
    Public PanelImages() As String

    Public catPanels() As Panel
    Public catPanelImages() As String
    Public catPictureBoxes() As PictureBox

    Public pnlBlank As Panel = New Panel
    Public catControls() As Object
    Public catDocument As RichTextBox = New RichTextBox
    Public Clients() As strClient
    Public catControlAttributes() As strAttributes
    Public arrTemplateID() As Integer
    Public arrDocuments() As Integer

    Public currPanel As Integer = 0
    Public currControl As Integer = -1
    Public oldControl As Integer
    Public currDocument As Integer = -1
    Public currDocumentVals As strDocument
    Public currClientID As Integer = -1
    Public currClient As strClient
    Public currClientCount As Integer = -1
    Public currAddButton As String
    Public currReportType As String

    Public appVars As New clsVariables
    Private FirstTime As Boolean = True

    Public isDragged As Boolean = False
    Public ptStartPosition As Point 'Capture the Starting coordinate of mouse. 

    Public frmWidth As Integer = Me.Width
    Public frmHeight As Integer = Me.Height

    Public currReportID As Integer = -1
    Public ReportSelected As Boolean = False
    Public FormLoad = True
    Public ClientSearch = True
    Public SelectedList As String = ""

    Public net35ok As Boolean = False

    Public contextMenuBuffer As String = ""
    Public TemplateLoaded As Boolean = False
    Public oldReport As String

    Public appRegistry As New clsAppRegistry

    Public oldMenuNode As TreeNode = New TreeNode
    Public oldDocComboText As String = ""

    Public oldReportCursorPos As Integer = 0
    Public defContBackCol As Color

    Public fntCurrentFont As Font
    Public SChecker As clsSpellCheck

    Public TotalControlCount As Integer = 0

    Public frmFormEditor As New frmFormEditor


    Public COLUMNWIDTH As Integer = 150
    Public PANELSIZE As Integer = 1 ' 0 std size, 1 full size (shrink logo, hide report and maximize form panel)

    Public btnHidden As New Button

    Public ControlScript As New MSScriptControl.ScriptControl
    Public cvt As New FontConverter
    Public ApplicationName As String



    ' Define a handler for unhandled exceptions.
    Private Sub MYExnHandler(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)

        Dim EX As Exception
        Dim frmNet As frmDotnetWarning = New frmDotnetWarning

        EX = e.ExceptionObject
        'Console.WriteLine(EX.StackTrace)

        frmNet.errorMsg = EX.StackTrace
        frmNet.ShowDialog()

    End Sub


    Private Sub MYThreadHandler(ByVal sender As Object, ByVal e As Threading.ThreadExceptionEventArgs)

        Dim frmNet As frmDotnetWarning = New frmDotnetWarning

        frmNet.errorMsg = e.Exception.StackTrace
        frmNet.ShowDialog()

    End Sub



    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

 
        net35ok = True
        ClientSearch = True
        FormLoad = True
        currClient.ClientID = 0

        Me.Hide()
        Me.Location = New Point(0, 0)
        Dim frmLoading As New frmLoading

        rtbDefault.ContextMenuStrip = ContextMenuStrip2


        Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Control Panel\International", "sShortDate", "MM/dd/yyyy")


        Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath))

        'check if database in cwd - if it is then running from flash drive so set mydocs to exe path

        If My.Computer.FileSystem.FileExists("Data.s3db") Or My.Computer.FileSystem.FileExists("Data1.s3db") Then
            MyDocs = Path.GetDirectoryName(Application.ExecutablePath)
        Else
            MyDocs = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Generated Psychological Solutions"
        End If

        If Process.GetCurrentProcess().MainModule.FileName.ToString.Contains("TestNav") Then
            ApplicationName = "TestNavigationTool.exe"
            PictureBox2.Image = Image.FromFile(MyDocs & "\System Images\Logo1.png")
            frmLoading.PictureBox2.Image = Image.FromFile(MyDocs & "\System Images\Logo1.png")
            Me.Text = "Test Navigation Tool"
            connStr = New OdbcConnection("DRIVER=SQLite3 ODBC Driver;Database=.\Data1.s3db;LongNames=0;Timeout=1000;NoTXN=0;SyncPragma=NORMAL;StepAPI=0;")
        Else
            ApplicationName = "AssessmentNavigationTool.exe"
            Me.Text = "Assessment Navigation Tool"
        End If

        SyncLocalDatabaseToolStripMenuItem.Enabled = False

        'if called with cmd line arg then update.ant dbl clicked

        Dim strAllArgs(My.Application.CommandLineArgs.Count - 1) As String
        Dim x As Integer = 0

        For Each arg As String In My.Application.CommandLineArgs
            Try
                strAllArgs(x) = arg
            Catch ex As Exception
            End Try
            x += 1
        Next

        If x > 0 Then

            Dim newUpdate As New clsUpdateAnt

            newUpdate.UpdateAnt(strAllArgs(0))

            Application.Exit()
            System.Threading.Thread.Sleep(500)  '?? walks straight past exit? #NEEDS WORK#
            Application.Exit()
            End

        Else 'sometimes ignores end statement so wrap in if/else #NEEDS WORK#

            'setup update.ant functionality

            Try
                FileCopy(Application.ExecutablePath, "update.exe") ' use the same executable 
            Catch ex As Exception
            End Try

            'set .ant file assosociation with this installations update.exe

            Dim tmpKeyTxt As String = My.Computer.Registry.ClassesRoot.GetValue("ant\shell\open\command")

            If tmpKeyTxt = Nothing Then
                Try
                    My.Computer.Registry.ClassesRoot.CreateSubKey(".ant").SetValue("", "ant", Microsoft.Win32.RegistryValueKind.String)
                    My.Computer.Registry.ClassesRoot.CreateSubKey("ant\shell\open\command").SetValue("", System.IO.Path.GetDirectoryName(Application.ExecutablePath()) & "\update.exe" & _
                       " ""%l"" ", Microsoft.Win32.RegistryValueKind.String)
                Catch ex As Exception
                    ' MsgBox("This application must be first run as Administrator for updates to automatically install.")
                End Try

            End If

            'set cwd to my documents/gps unless database in install dir (flash install)

            If Not File.Exists(".\Data.s3db") And Not File.Exists(".\Data1.s3db") Then
                Directory.SetCurrentDirectory(MyDocs)
            End If


            Dim clsDB As New clsDataBaseFunctions
            clsDB.SetupDatabase() 'setup 

            Dim regfrm As New frmReg
            regfrm.ShowDialog()

            If appVars.RegistrationOK = False Then
                Application.Exit()
                System.Threading.Thread.Sleep(500)  '?? walks straight past exit? #NEEDS WORK#
                Application.Exit()
                End 'force dirty exit
            End If


            Dim loginnfrm As New frmLogin

            loginnfrm.ShowDialog()

            If appVars.RegistrationOK = False Then
                Application.Exit()
                System.Threading.Thread.Sleep(500)
                Application.Exit()
                End
            End If

            If appVars.User.UserLevel = "Standard" Then
                Me.ToolsToolStripMenuItem.Visible = False
                'AddHandler currentDomain.UnhandledException, AddressOf MYExnHandler
                'AddHandler Application.ThreadException, AddressOf MYThreadHandler
            End If

            If appVars.User.UserLevel = "Administrator" Then
                Me.FormEditModeToolStripMenuItem.Enabled = False
                'AddHandler currentDomain.UnhandledException, AddressOf MYExnHandler
                'AddHandler Application.ThreadException, AddressOf MYThreadHandler
            End If

            'Me.WindowState = FormWindowState.Maximized
            'Me.Show()
            'Me.Refresh()

            frmLoading.Show()
            frmLoading.lblStatus.Text = "Loading Application Framework..."
            frmLoading.Refresh()
            frmLoading.Focus()

            Dim thread As System.Threading.Thread = New System.Threading.Thread(AddressOf InitApp)

            thread.Start() 'load spellcheck database in a new thread to speed up form load

            LoadAll()

            frmLoading.Close()


            defaultPanel.Visible = True
            defaultPanel.BringToFront()

            pnlDocuments.Hide()
            pnlSearch.Hide()

            rtbDefault.HideSelection = False
            rtbDefault.BringToFront()
            rtbDefault.Show()
            rtbDefault.WordWrap = False
            rtbDefault.ScrollBars = RichTextBoxScrollBars.ForcedBoth
            rtbDefault.RightMargin = 610 'rtbDefault.Width - 10

            ToolStripStatusLabel2.Text = "   UserControlID : "

            defContBackCol = btnRepBold.BackColor

            fntCurrentFont = New Font("Calibri", 12, FontStyle.Regular)
            txtFont.Text = fntCurrentFont.Name
            txtFontSize.Text = fntCurrentFont.Size

            'fix locale to US for now. needs to be in settings #TODO# needs date setting to mmddyyyy in settings

            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            FormLoad = False

            Me.WindowState = FormWindowState.Maximized

            SetPanelSize(0)
            SetApplicationLayout()

            frmMain_SizeChanged1()

            frmMainTreeView1.Select()
            currControl = -1

            MenuStripfrmMain.Select()

            Me.Show()

        End If

    End Sub

    Private Sub InitApp()

        SChecker = New clsSpellCheck

    End Sub

    Private Sub LoadAll()

        InitListView()
        LoadData()
        LoadCatDocuments()
        LoadMainMenu()
        LoadPanels()
        LoadAllControls()
        LoadVBScript()

        Timer1.Interval = 100
        Timer1.Enabled = True

        Try
            frmMainTreeView1.SelectedNode = frmMainTreeView1.Nodes(1) 'preselect first menu option if there
        Catch ex As Exception
        End Try

    End Sub

    Private Sub LoadVBScript()

        Dim count As Integer

        ControlScript.Language = "VBScript"

        For count = 0 To catControls.Length - 1
            ControlScript.AddObject("c" & catControlAttributes(count).ControlID.ToString.Trim, catControls(count), True)  'add all user controls to scripting object
            ControlScript.AddObject("ca" & catControlAttributes(count).ControlID.ToString.Trim, catControlAttributes(count), True)
        Next

    End Sub

    Private Sub InitListView()

        Dim vals() As String = {"Control ID", "Name", "DataBlock", "CatID", "Child", "ChildDataMode", "Type", _
                "Allow Edit", "Action", "Location", "Size", "Tag", "Text", "ScrollBars", "SelectMode", _
                "SelectStyle", "TabIndex", "SaveField", "Items", "Font", "Linked Items", "OnLoad"}

        frmFormEditor.lstProperty.Items.Clear()
        frmFormEditor.lstNewControl.Items.Clear()

        For count = 0 To 21
            frmFormEditor.lstProperty.Items.Add(vals(count))
            frmFormEditor.lstNewControl.Items.Add("")
        Next

    End Sub


    Private Sub LoadData()

        Dim SQLcommand As OdbcCommand
        Dim SQLreader As OdbcDataReader
        Dim Count As Integer = 0


        SQLcommand = connStr.CreateCommand

        SQLcommand.CommandText = "SELECT * FROM  Templates "

        SQLreader = SQLcommand.ExecuteReader()

        cmbTemplate.Items.Clear()
        Count = 0

        While SQLreader.Read()
            cmbTemplate.Items.Add(SQLreader(1))
            ReDim Preserve arrTemplateID(Count)
            arrTemplateID(Count) = 2 ^ SQLreader(0)
            Count += 1
        End While

        SQLreader.Close()

        SQLcommand.CommandText = "SELECT DataBlocks.DataBlockID, DataBlocks.Text1 " & _
                        " FROM   DataBlocks " & _
                        " WHERE DataBlocks.Tag = 'Title' "

        SQLreader = SQLcommand.ExecuteReader()

        cmbTitle.Items.Clear()

        While SQLreader.Read()
            cmbTitle.Items.Add(SQLreader(1))
        End While

        SQLreader.Close()

        SQLcommand.CommandText = "SELECT DataBlocks.DataBlockID, DataBlocks.Text1 " & _
                " FROM   DataBlocks " & _
                " WHERE DataBlocks.Tag = 'sex' "

        SQLreader = SQLcommand.ExecuteReader()

        cmbSex.Items.Clear()

        While SQLreader.Read()
            cmbSex.Items.Add(SQLreader(1))
        End While

        SQLreader.Close()

        SQLcommand.CommandText = "SELECT DataBlocks.DataBlockID, DataBlocks.Text1 " & _
        " FROM   DataBlocks " & _
        " WHERE DataBlocks.Tag = 'ethnicity' "

        SQLreader = SQLcommand.ExecuteReader()

        cmbEthnicity.Items.Clear()

        While SQLreader.Read()
            cmbEthnicity.Items.Add(SQLreader(1))
        End While

        SQLreader.Close()

        SQLcommand.CommandText = "SELECT DataBlocks.DataBlockID, DataBlocks.Text1 " & _
        " FROM   DataBlocks " & _
        " WHERE DataBlocks.Tag = 'Marital Status' "

        SQLreader = SQLcommand.ExecuteReader()

        cmbMaritalStatus.Items.Clear()

        While SQLreader.Read()
            cmbMaritalStatus.Items.Add(SQLreader(1))
        End While


    End Sub

    Function LoadMainMenu()

        Dim count As Integer = 0
        Dim count1 As Integer = 0
        Dim tmpStr As String = ""

        frmMainTreeView1.Nodes.Clear()

        'add inital space on menu

        frmMainTreeView1.Nodes.Add("", "")
        frmMainTreeView1.Nodes(0).Tag = ""

        frmMainTreeView1.Font = New Font(frmMainTreeView1.Font, FontStyle.Bold)


        Dim SQLcommand As OdbcCommand
        SQLcommand = connStr.CreateCommand

        SQLcommand.CommandText = "SELECT * " & _
                                " FROM   Menu " & _
                                " ORDER BY MenuPos "

        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        While SQLreader.Read()

            ReDim Preserve treeNodes(count1)
            ReDim Preserve treeNodeDocument(count1)
            ReDim Preserve treeNodeHeader(count1)
            ReDim Preserve PanelImages(count1)


            If SQLreader(2) = "0" Then

                frmMainTreeView1.Nodes.Add(SQLreader(0).ToString, SQLreader(1).ToString)
                frmMainTreeView1.Nodes(frmMainTreeView1.Nodes.Count - 1).Tag = SQLreader(3).ToString

                treeNodes(count1) = SQLreader(0) 'keeps an expanded node list for panels - collection better? #TODO#

                treeNodeDocument(count1) = count
                treeNodeHeader(count1) = True
                PanelImages(count1) = SQLreader(1).ToString

                If SQLreader(1).ToString.Length > 0 Then
                    Dim Item As New ToolStripMenuItem
                    Item.Text = SQLreader(1).ToString
                    Item.Tag = SQLreader(3)
                    Item.Font = New Font(frmMainTreeView1.Font, FontStyle.Bold)

                    MenuToolStripMenuItem.DropDownItems.Add(Item)
                    AddHandler Item.Click, New System.EventHandler(AddressOf MenuOptionClick)
                End If

                count += 1

            Else

                If SQLreader(1).ToString.Length > 0 Then

                    Dim Item As New ToolStripMenuItem
                    Item.Text = SQLreader(1).ToString
                    Item.Tag = SQLreader(3)

                    MenuToolStripMenuItem.DropDownItems.Add(Item)
                    AddHandler Item.Click, New System.EventHandler(AddressOf MenuOptionClick)
                End If

                frmMainTreeView1.Nodes(count).Nodes.Add(SQLreader(0).ToString, SQLreader(1).ToString)

                frmMainTreeView1.Nodes(count).Nodes(frmMainTreeView1.Nodes(frmMainTreeView1.Nodes.Count - 1) _
                    .Nodes.Count - 1).NodeFont = New Font(frmMainTreeView1.Font, FontStyle.Regular)

                frmMainTreeView1.Nodes(count).Nodes(frmMainTreeView1.Nodes(frmMainTreeView1.Nodes.Count - 1) _
                   .Nodes.Count - 1).Tag = SQLreader(3).ToString

                treeNodes(count1) = SQLreader(0).ToString
                treeNodeDocument(count1) = count - 1
                treeNodeHeader(count1) = False
                PanelImages(count1) = SQLreader(1).ToString

            End If

            count1 = count1 + 1

        End While


        frmMainTreeView1.Refresh()
        frmMainTreeView1.ExpandAll()

        Return 0

    End Function


    Private Sub MenuOptionClick(ByVal sender As Object, ByVal e As EventArgs)

        Dim itemClicked As New ToolStripMenuItem
        itemClicked = CType(sender, ToolStripMenuItem)
        ChangePanel(itemClicked.Tag)

    End Sub

    Private Sub LoadCatDocuments()

        Dim count As Integer = 0
        Dim SQLcommand As OdbcCommand

        SQLcommand = connStr.CreateCommand

        SQLcommand.CommandText = "SELECT DocumentTag " & _
                                " FROM   AppDocs " & _
                                " WHERE DocType = 1" & _
                                " ORDER BY DocumentTag ASC "

        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        cmbReportTemplate.Items.Clear()

        While SQLreader.Read()

            cmbReportTemplate.Items.Add(SQLreader(0))

            Dim Item As New ToolStripMenuItem
            Item.Text = SQLreader(0).ToString

            ReportsToolStripMenuItem.DropDownItems.Add(Item)
            AddHandler Item.Click, New System.EventHandler(AddressOf NewReportClick)

        End While

    End Sub

    Private Sub NewReportClick(ByVal sender As Object, ByVal e As EventArgs)

        NewReport(sender.Text)

    End Sub

    Private Sub LoadPanels()

        Dim count As Integer = 0
        Dim count1 As Integer = 0
        Dim ImageName As String = ""


        Try
            count1 = treeNodes.Length - 1
        Catch ex As Exception
            count1 = 0
        End Try

        ReDim Preserve catPanels(0)
        ReDim Preserve catPictureBoxes(0)

        catPictureBoxes(0) = New PictureBox
        ReDim Preserve catPanelImages(1)
        catPanelImages(0) = 0

        catPanels(0) = defaultPanel
        Panel1.Controls.Add(catPanels(0))


        For count = 1 To count1

            ReDim Preserve catPanels(count)
            ReDim Preserve catPictureBoxes(count)

            catPanels(count) = New Panel
            catPictureBoxes(count) = New PictureBox

            Panel1.Controls.Add(catPanels(count))

            With catPanels(count)
                .Name = "newPanel" & count
                .Location = defaultPanel.Location
                .Size = defaultPanel.Size
                .BorderStyle = defaultPanel.BorderStyle
                .Visible = True
                .Enabled = True
                .Tag = treeNodes(count).ToString
                .ContextMenuStrip = Me.ContextMenuStrip1

                .Controls.Add(catPictureBoxes(count)) 'needed for smooth scrolling
                .AutoScroll = True

            End With

            ImageName = MyDocs & "\Scanned Images\" & PanelImages(count).ToString.Trim & ".jpg"

            ReDim Preserve catPanelImages(count)

            If File.Exists(ImageName) Then

                catPictureBoxes(count).Image = System.Drawing.Image.FromFile(ImageName)
                catPictureBoxes(count).Size = catPictureBoxes(count).Image.Size
                catPanelImages(count) = ImageName

                catPictureBoxes(count).SendToBack()
                catPanels(count).Size = catPictureBoxes(count).Image.Size
            Else
                catPanelImages(count) = ""
            End If

        Next

        frmFormEditor.Visible = False
        catPictureBoxes(0).SendToBack()

    End Sub


    Private Sub LoadAllControls()

        Dim count As Integer = 0
        Dim count1 As Integer = 0
        Dim count2 As Integer = 0
        Dim type As Integer = 0
        Dim vals As String() = Nothing

        Dim SQLcommand As OdbcCommand
        SQLcommand = connStr.CreateCommand

        SQLcommand.CommandText = "SELECT * FROM Controls "

        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        While SQLreader.Read()

            For count2 = 0 To 21
                ReDim Preserve vals(count2)
                vals(count2) = SQLreader(count2).ToString
            Next

            LoadControl(vals)

            count1 += 1

        End While

        TotalControlCount = count1


    End Sub

    Private Function LoadControl(ByVal vals As String())

        Dim count As Integer
        Dim count1 As Integer
        Dim type As Integer
        Dim tmpControl As Control


        Try
            ReDim Preserve catControlAttributes(catControlAttributes.Length)
            ReDim Preserve catControls(catControls.Length)
        Catch ex As Exception
            ReDim Preserve catControlAttributes(0)
            ReDim Preserve catControls(0)
        End Try

        count = catControls.Length - 1
        currControl = catControls.Length - 1

        catControlAttributes(count).ControlID = vals(0)
        catControlAttributes(count).Name = "NewControl" & count
        catControlAttributes(count).DataBlock = vals(2).ToString
        catControlAttributes(count).CatID = vals(3)
        catControlAttributes(count).Child = vals(4)
        catControlAttributes(count).ChildDataMode = vals(5)
        catControlAttributes(count).Type = vals(6)
        catControlAttributes(count).AllowEdit = vals(7)
        catControlAttributes(count).Action = vals(8).ToString
        catControlAttributes(count).Location = vals(9)
        catControlAttributes(count).Size = vals(10)
        catControlAttributes(count).Tag = vals(11)
        catControlAttributes(count).ConText = vals(12).ToString
        catControlAttributes(count).ScrollBars = vals(13)
        catControlAttributes(count).SelectMode = vals(14)
        catControlAttributes(count).SelectStyle = vals(15)
        catControlAttributes(count).TabIndex = vals(16)
        catControlAttributes(count).SaveField = vals(17)
        catControlAttributes(count).Items = vals(18)
        catControlAttributes(count).Font = vals(19)
        catControlAttributes(count).LinkedItems = vals(20)
        catControlAttributes(count).OnLoad = vals(21)


        catControlAttributes(count).counter = 0
        catControlAttributes(count).PositionOrder = ""

        catControlAttributes(count).selecting = True 'stop controls being processed at buildtime


        type = vals(6)

        If type = 0 Then catControls(count) = New newLabel
        If type = 1 Then catControls(count) = New newButton
        If type = 2 Then catControls(count) = New newTextBox
        If type = 3 Then catControls(count) = New newComboBox
        If type = 4 Then catControls(count) = New newMultiLineTextBox
        If type = 5 Then catControls(count) = New newListBox
        If type = 6 Then catControls(count) = New Panel
        If type = 7 Then catControls(count) = New newControlTimer
        If type = 8 Then catControls(count) = New newCheckBox
        If type = 9 Then catControls(count) = New newRadioButton
        If type = 10 Then catControls(count) = New newRichTextBox
        If type = 11 Then catControls(count) = New newItemSelectedLabel


        tmpControl = DirectCast(catControls(count), Control)


        If catControlAttributes(count).Items.Length > 0 Then
            SetControlItems(count)
        End If


        With catControls(count) 'set attributes


            If catControlAttributes(count).Font.ToString.Length > 0 And type <> 11 Then
                .Font = cvt.ConvertFromString(catControlAttributes(count).Font)
            End If

            .Parent = catPictureBoxes(currPanel)
            'catPictureBoxes(currPanel).Refresh()

            .Name = catControlAttributes(count).Name
            .Location = New Point(convertXY("X", catControlAttributes(count).Location), _
                            convertXY("Y", catControlAttributes(count).Location))

            If type <> 11 Then
                .Text = catControlAttributes(count).ConText
                .Size = New Point(convertXY("X", catControlAttributes(count).Size), _
                                convertXY("Y", catControlAttributes(count).Size))
            End If

            If catControlAttributes(count).TabIndex > -1 Then
                .TabIndex = catControlAttributes(count).TabIndex
            End If

            If type = 11 Then
                '.Size = New Point(42, 37)
                '  .Text = "O"
                '.Font = New Font("Times New Roman", 24, FontStyle.Regular)
                '.TextAlign = ContentAlignment.MiddleCenter
                '.ForeColor = Color.Blue
                '.BackColor = Color.Transparent
                .Text = ""
            End If


            If catControlAttributes(count).Type = 4 Then
                If catControlAttributes(count).ScrollBars = "Horizontal" Then
                    .ScrollBars = ScrollBars.Horizontal
                ElseIf catControlAttributes(count).ScrollBars = "Vertical" Then
                    .ScrollBars = ScrollBars.Vertical
                ElseIf catControlAttributes(count).ScrollBars = "Both" Then
                    .ScrollBars = ScrollBars.Both
                End If
            End If

            If catControlAttributes(count).Type = 5 Then

                .ScrollAlwaysVisible = True
                .Enabled = True

                If catControlAttributes(count).ScrollBars = "Horizontal" Then
                    .MultiColumn = True
                ElseIf catControlAttributes(count).ScrollBars = "Vertical" Then
                    .MultiColumn = False
                End If

                If catControlAttributes(count).SelectMode = "Multi" Then
                    .SelectionMode = SelectionMode.MultiExtended
                ElseIf catControlAttributes(count).SelectMode = "Simple" Then
                    .SelectionMode = SelectionMode.MultiSimple
                ElseIf catControlAttributes(count).SelectMode = "Single" Then
                    .SelectionMode = SelectionMode.One
                End If

            End If

            If type = 2 Or type = 4 Then
                AddHandler tmpControl.TextChanged, AddressOf catControls_TextChanged
            End If

            If type > 1 And Not type = 3 Then
                '.BorderStyle = BorderStyle.FixedSingle
            End If

            If catControlAttributes(count).AllowEdit = 1 Then
                .readonly = True
            End If

        End With

        Dim colWidth As Integer = 20

        'find correct panel for control

        For count1 = 0 To catPanels.Length - 1
            If catPanels(count1).Tag = catControlAttributes(count).CatID.ToString.Trim Then
                Exit For
            End If
        Next

        'if no panel allocated assume default panel - needs work #TODO#

        If count1 = catPanels.Length Then
            defaultPanel.Controls.Add(catControls(count))
        Else
            If type = 0 And catPictureBoxes(count1).Image Is Nothing Then
                catControls(count).BackColor = DefaultBackColor
            End If
            catPictureBoxes(count1).Controls.Add(catControls(count))
            catPictureBoxes(count1).SendToBack()
        End If

        catControlAttributes(count).selecting = False
        catControlAttributes(count).TabIndex = catControls(count).TabIndex

        Return count

    End Function


    Private Sub NewClientToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewClientToolStripMenuItem.Click

        defaultPanel.Show()
        defaultPanel.Visible = True
        defaultPanel.BringToFront()
        pnlSearch.Hide()

        If currClient.ClientID > -1 Then
            SaveClient()
        End If

        btnDemoConfirm.Text = "Save"
        ClearClient()

        currClient.ClientID = -1

        TemplateLoaded = False
        cmbTemplate.Select()

    End Sub

    Private Sub FindClientToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindClientToolStripMenuItem.Click

        defaultPanel.Show()
        defaultPanel.Visible = True
        defaultPanel.BringToFront()
        pnlSearch.Hide()

        TemplateLoaded = False
        ClientSearch = True

        If currClient.ClientID > -1 Then
            SaveClient()
        End If

        ClearClient()

        btnDemoConfirm.Text = "Search"
        txtLname.Select()

    End Sub

    Private Sub DataBlockMaintenanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataBlockMaintenanceToolStripMenuItem.Click

        Dim frmdDataBlockMaint = New frmDataBlockMaint

        frmdDataBlockMaint.appVars = Me.appVars
        frmdDataBlockMaint.ShowDialog()

    End Sub

    Private Sub TableMaintenanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim frmdDataBlockMaint = New frmDataBlockMaint

        frmdDataBlockMaint.appVars = Me.appVars
        frmdDataBlockMaint.ShowDialog()

    End Sub


    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click

        Dim AboutFrm = New frmAboutBox
        AboutFrm.ShowDialog()

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click

        Me.Close()

    End Sub


    Function AddNewControl(ByVal type As Integer)

        Dim count As Integer = 0
        Dim tmpPnt As New Point

        Try
            count = catControls.Length
        Catch ex As Exception
            count = 0
        End Try

        ReDim Preserve catControls(count)
        ReDim Preserve catControlAttributes(count)

        If type = 0 Then catControls(count) = New newLabel
        If type = 1 Then catControls(count) = New newButton
        If type = 2 Then catControls(count) = New newTextBox
        If type = 3 Then catControls(count) = New newComboBox
        If type = 4 Then catControls(count) = New newMultiLineTextBox
        If type = 5 Then catControls(count) = New newListBox
        If type = 6 Then catControls(count) = New Panel
        If type = 7 Then catControls(count) = New newControlTimer
        If type = 8 Then catControls(count) = New newCheckBox
        If type = 9 Then catControls(count) = New newRadioButton
        If type = 10 Then catControls(count) = New newRichTextBox
        If type = 11 Then catControls(count) = New newItemSelectedLabel

        With catControls(count)

            tmpPnt.X = (catPanels(currPanel).AutoScrollPosition.X * -1)
            tmpPnt.Y = (catPanels(currPanel).AutoScrollPosition.Y * -1)

            .Location = tmpPnt

            .Name = "newControl" & count
            .Visible = True
            .Enabled = True

            If type = 11 Then
                .TextAlign = ContentAlignment.MiddleCenter
                .ForeColor = Color.Blue
                .BackColor = Color.Transparent
                catPictureBoxes(currPanel).Refresh()
            End If
        End With

        currControl = count
        catPictureBoxes(currPanel).Controls.Add(catControls(count))

        catPictureBoxes(currPanel).SendToBack()

        Dim tmpControl As Control = DirectCast(catControls(currControl), Control)

        frmFormEditor.lstNewControl.Items(0).Text = -1
        frmFormEditor.lstNewControl.Items(1).Text = tmpControl.Name
        frmFormEditor.lstNewControl.Items(2).Text = ""
        frmFormEditor.lstNewControl.Items(3).Text = catPanels(currPanel).Tag
        frmFormEditor.lstNewControl.Items(4).Text = -1
        frmFormEditor.lstNewControl.Items(5).Text = "Clear"
        frmFormEditor.lstNewControl.Items(6).Text = type
        frmFormEditor.lstNewControl.Items(7).Text = 0
        frmFormEditor.lstNewControl.Items(8).Text = ""
        frmFormEditor.lstNewControl.Items(9).Text = tmpControl.Location.ToString
        frmFormEditor.lstNewControl.Items(10).Text = tmpControl.Size.ToString
        frmFormEditor.lstNewControl.Items(11).Text = "new"
        frmFormEditor.lstNewControl.Items(12).Text = tmpControl.Text
        frmFormEditor.lstNewControl.Items(13).Text = "Vertical"
        frmFormEditor.lstNewControl.Items(14).Text = "Single"
        frmFormEditor.lstNewControl.Items(15).Text = "Default"
        frmFormEditor.lstNewControl.Items(16).Text = "-1"
        frmFormEditor.lstNewControl.Items(17).Text = "-1"
        frmFormEditor.lstNewControl.Items(18).Text = ""
        frmFormEditor.lstNewControl.Items(19).Text = cvt.ConvertToString(tmpControl.Font)
        frmFormEditor.lstNewControl.Items(20).Text = ""
        frmFormEditor.lstNewControl.Items(21).Text = ""


        SetControlAttributes()
        catControls(count).Focus()

        Return 0

    End Function


    Public Sub SaveControlChanges()

        Dim count As Integer = 0
        Dim count1 As Integer = 0
        Dim status As Integer = 0
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand

        SQLcommand = connStr.CreateCommand

        Try
            count1 = catControlAttributes.Length - 1 'safety check in case still none
        Catch ex As Exception
            count1 = -1
        End Try

        For count = 0 To count1

            If catControlAttributes(count).Tag = "edit" Then

                catControlAttributes(count).Tag = ""

                tmpSQL = "UPDATE Controls SET " & _
                    " Name = '" & catControlAttributes(count).Name & "' ," & _
                    " CatID = '" & catControlAttributes(count).CatID & "' ," & _
                    " Child = '" & catControlAttributes(count).Child & "' ," & _
                    " DataBlock = '" & catControlAttributes(count).DataBlock.Replace("'", "''") & "' ," & _
                    " ConText = '" & catControlAttributes(count).ConText.Replace("'", "''") & "' ," & _
                    " Action = '" & catControlAttributes(count).Action.Replace("'", "''") & "' ," & _
                    " ChildDataMode = '" & catControlAttributes(count).ChildDataMode & "' ," & _
                    " Type = '" & catControlAttributes(count).Type & "' ," & _
                    " AllowEdit = '" & catControlAttributes(count).AllowEdit & "' ," & _
                    " Location = '" & catControlAttributes(count).Location & "' ," & _
                    " Size = '" & catControlAttributes(count).Size & "' ," & _
                    " Tag = '" & catControlAttributes(count).Tag & "' ," & _
                    " ScrollBars = '" & catControlAttributes(count).ScrollBars & "' ," & _
                    " SelectMode = '" & catControlAttributes(count).SelectMode & "' ," & _
                    " SelectStyle = '" & catControlAttributes(count).SelectStyle & "' ," & _
                    " TabIndex = " & catControlAttributes(count).TabIndex & " ," & _
                    " SaveField = " & catControlAttributes(count).SaveField & " ," & _
                    " Items = '" & catControlAttributes(count).Items & "' ," & _
                    " Font = '" & catControlAttributes(count).Font & "' ," & _
                    " LinkedItems = '" & catControlAttributes(count).LinkedItems & "' ," & _
                    " OnLoad = '" & catControlAttributes(count).OnLoad & "' " & _
                    " WHERE ControlID = " & catControlAttributes(count).ControlID




                Try
                    SQLcommand.CommandText = tmpSQL
                    SQLcommand.ExecuteNonQuery()
                Catch ex As Exception
                    status = 1
                End Try

            ElseIf catControlAttributes(count).Tag = "new" Then

                catControlAttributes(count).Tag = ""

                tmpSQL = "INSERT INTO Controls ( " & _
                    " Name ," & _
                    " DataBlock ," & _
                    " CatID ," & _
                    " Child ," & _
                    " ChildDataMode ," & _
                    " Type ," & _
                    " AllowEdit ," & _
                    " Action ," & _
                    " Location ," & _
                    " Size ," & _
                    " Tag ," & _
                    " ConText ," & _
                    " ScrollBars ," & _
                    " SelectMode ," & _
                    " SelectStyle ," & _
                    " TabIndex ," & _
                    " SaveField ," & _
                    " Items ," & _
                    " Font ," & _
                    " LinkedItems ," & _
                    " OnLoad )" & _
                    " VALUES ( " & _
                    "'" & catControlAttributes(count).Name & "'," & _
                    "'" & catControlAttributes(count).DataBlock.Replace("'", "''") & "'," & _
                    "'" & catControlAttributes(count).CatID & "'," & _
                    "'" & catControlAttributes(count).Child & "'," & _
                    "'" & catControlAttributes(count).ChildDataMode & "'," & _
                    "'" & catControlAttributes(count).Type & "'," & _
                    "'" & catControlAttributes(count).AllowEdit & "'," & _
                    "'" & catControlAttributes(count).Action.Replace("'", "''") & "'," & _
                    "'" & catControlAttributes(count).Location & "'," & _
                    "'" & catControlAttributes(count).Size & "'," & _
                    "'" & catControlAttributes(count).Tag & "'," & _
                    "'" & catControlAttributes(count).ConText.Replace("'", "''") & "'," & _
                    "'" & catControlAttributes(count).ScrollBars & "'," & _
                    "'" & catControlAttributes(count).SelectMode & "'," & _
                    "'" & catControlAttributes(count).SelectStyle & "'," & _
                    "'" & catControlAttributes(count).TabIndex & "'," & _
                    "'" & catControlAttributes(count).SaveField & "'," & _
                    "'" & catControlAttributes(count).Items & "'," & _
                    "'" & catControlAttributes(count).Font & "'," & _
                    "'" & catControlAttributes(count).Items & "'," & _
                    "'" & catControlAttributes(count).OnLoad & "')"

                Try
                    SQLcommand.CommandText = tmpSQL
                    SQLcommand.ExecuteNonQuery()
                Catch ex As Exception
                    status = 1
                End Try


                If status = 0 Then

                    SQLcommand = connStr.CreateCommand

                    SQLcommand.CommandText = "select last_insert_rowid()"

                    Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

                    SQLreader.Read()
                    catControlAttributes(count).ControlID = SQLreader(0)
                    catControls(count).Name = "NewControl" & count
                    SQLreader.Close()

                    If catControlAttributes(count).ControlID < 1 Then
                        status = 1
                    End If
                End If
            End If
        Next

        If status = 1 Then
            MsgBox("Failed To Save All Control Data", MsgBoxStyle.Critical)
        End If

    End Sub

    Private Sub ChangePanel(ByVal index As Integer)

        If FormLoad = True Then Exit Sub

        If (cmbEthnicity.Text.Length = 0 Or cmbSex.Text.Length = 0 Or cmbTemplate.Text.Length = 0 _
            Or txtDOB.TextLength = 0 Or txtFname.TextLength = 0 Or txtLname.TextLength = 0) Then

            MsgBox("Please Complete Template, FirstName, LastName, DOB, Sex & Ethnicity")
            currPanel = 0
            Exit Sub
        End If

        catPanels(currPanel).SendToBack()

        If catPanelImages(index).Length > 2 And PANELSIZE = 0 Then ' contains scanned image so maximize panels
            SetPanelSize(1)
        ElseIf catPanelImages(index).Length < 3 And PANELSIZE = 1 Then
            SetPanelSize(0)
        End If

        catPanels(index).BringToFront()
        catPanels(index).Enabled = True
        catPanels(index).Visible = True
        catPanels(index).Show()
        catPanels(index).Focus()
        catPanels(index).Refresh()


        currPanel = index

        frmFormEditor.lstNewControl.Items(0).Text = ""
        frmFormEditor.lstNewControl.Items(1).Text = ""
        frmFormEditor.lstNewControl.Items(2).Text = ""
        frmFormEditor.lstNewControl.Items(3).Text = ""
        frmFormEditor.lstNewControl.Items(4).Text = ""
        frmFormEditor.lstNewControl.Items(5).Text = ""
        frmFormEditor.lstNewControl.Items(6).Text = ""
        frmFormEditor.lstNewControl.Items(7).Text = ""
        frmFormEditor.lstNewControl.Items(8).Text = ""
        frmFormEditor.lstNewControl.Items(9).Text = ""
        frmFormEditor.lstNewControl.Items(10).Text = ""
        frmFormEditor.lstNewControl.Items(11).Text = ""
        frmFormEditor.lstNewControl.Items(12).Text = ""
        frmFormEditor.lstNewControl.Items(13).Text = ""
        frmFormEditor.lstNewControl.Items(14).Text = ""
        frmFormEditor.lstNewControl.Items(15).Text = ""
        frmFormEditor.lstNewControl.Items(16).Text = ""
        frmFormEditor.lstNewControl.Items(17).Text = ""
        frmFormEditor.lstNewControl.Items(18).Text = ""
        frmFormEditor.lstNewControl.Items(19).Text = ""


    End Sub

    Private Sub frmMainTreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles frmMainTreeView1.AfterSelect


        If e.Node.Tag.ToString.Length > 0 Then

            Dim Index As Integer = Convert.ToInt32(e.Node.Tag)

            ChangePanel(Index)

            oldMenuNode.BackColor = Color.White
            e.Node.BackColor = Color.LightBlue
            oldMenuNode = e.Node


        End If

    End Sub


    Public Sub ShowControlAttributes()

        If currControl > -1 And frmFormEditor.Visible = True Then

            frmFormEditor.lstNewControl.Items(0).Text = catControlAttributes(currControl).ControlID
            frmFormEditor.lstNewControl.Items(1).Text = catControlAttributes(currControl).Name
            frmFormEditor.lstNewControl.Items(2).Text = catControlAttributes(currControl).DataBlock
            frmFormEditor.lstNewControl.Items(3).Text = catControlAttributes(currControl).CatID
            frmFormEditor.lstNewControl.Items(4).Text = catControlAttributes(currControl).Child
            frmFormEditor.lstNewControl.Items(5).Text = catControlAttributes(currControl).ChildDataMode
            frmFormEditor.lstNewControl.Items(6).Text = catControlAttributes(currControl).Type
            frmFormEditor.lstNewControl.Items(7).Text = catControlAttributes(currControl).AllowEdit
            frmFormEditor.lstNewControl.Items(8).Text = catControlAttributes(currControl).Action
            frmFormEditor.lstNewControl.Items(9).Text = catControlAttributes(currControl).Location
            frmFormEditor.lstNewControl.Items(10).Text = catControlAttributes(currControl).Size
            frmFormEditor.lstNewControl.Items(11).Text = catControlAttributes(currControl).Tag
            frmFormEditor.lstNewControl.Items(12).Text = catControlAttributes(currControl).ConText
            frmFormEditor.lstNewControl.Items(13).Text = catControlAttributes(currControl).ScrollBars
            frmFormEditor.lstNewControl.Items(14).Text = catControlAttributes(currControl).SelectMode
            frmFormEditor.lstNewControl.Items(15).Text = catControlAttributes(currControl).SelectStyle
            frmFormEditor.lstNewControl.Items(16).Text = catControlAttributes(currControl).TabIndex
            frmFormEditor.lstNewControl.Items(17).Text = catControlAttributes(currControl).SaveField
            frmFormEditor.lstNewControl.Items(18).Text = catControlAttributes(currControl).Items
            frmFormEditor.lstNewControl.Items(19).Text = catControlAttributes(currControl).Font
            frmFormEditor.lstNewControl.Items(20).Text = catControlAttributes(currControl).LinkedItems
            frmFormEditor.lstNewControl.Items(21).Text = catControlAttributes(currControl).OnLoad

            If catControlAttributes(currControl).Font.ToString.Length = 0 Then
                frmFormEditor.lstNewControl.Items(19).Text = cvt.ConvertToString(catControls(currControl).Font)
            End If

        End If

    End Sub

    Public Sub SetControlAttributes()

        If currControl > -1 And frmFormEditor.Visible = True Then

            Dim tmpControl As Control = DirectCast(catControls(currControl), Control)

            If frmFormEditor.lstNewControl.Items(3).Text.Length = 0 Then
                frmFormEditor.lstNewControl.Items(3).Text = -1
            End If

            catControlAttributes(currControl).ControlID = frmFormEditor.lstNewControl.Items(0).Text
            catControlAttributes(currControl).Name = frmFormEditor.lstNewControl.Items(1).Text
            catControlAttributes(currControl).DataBlock = frmFormEditor.lstNewControl.Items(2).Text
            catControlAttributes(currControl).CatID = frmFormEditor.lstNewControl.Items(3).Text
            catControlAttributes(currControl).Child = frmFormEditor.lstNewControl.Items(4).Text
            catControlAttributes(currControl).ChildDataMode = frmFormEditor.lstNewControl.Items(5).Text
            catControlAttributes(currControl).Type = frmFormEditor.lstNewControl.Items(6).Text
            catControlAttributes(currControl).AllowEdit = frmFormEditor.lstNewControl.Items(7).Text
            catControlAttributes(currControl).Action = frmFormEditor.lstNewControl.Items(8).Text
            catControlAttributes(currControl).Location = frmFormEditor.lstNewControl.Items(9).Text
            catControlAttributes(currControl).Size = frmFormEditor.lstNewControl.Items(10).Text
            catControlAttributes(currControl).Tag = frmFormEditor.lstNewControl.Items(11).Text
            catControlAttributes(currControl).ConText = frmFormEditor.lstNewControl.Items(12).Text
            catControlAttributes(currControl).ScrollBars = frmFormEditor.lstNewControl.Items(13).Text
            catControlAttributes(currControl).SelectMode = frmFormEditor.lstNewControl.Items(14).Text
            catControlAttributes(currControl).SelectStyle = frmFormEditor.lstNewControl.Items(15).Text
            catControlAttributes(currControl).TabIndex = Convert.ToInt16(frmFormEditor.lstNewControl.Items(16).Text)
            catControlAttributes(currControl).SaveField = Convert.ToInt16(frmFormEditor.lstNewControl.Items(17).Text)
            catControlAttributes(currControl).Items = frmFormEditor.lstNewControl.Items(18).Text
            catControlAttributes(currControl).Font = frmFormEditor.lstNewControl.Items(19).Text
            catControlAttributes(currControl).LinkedItems = frmFormEditor.lstNewControl.Items(20).Text
            catControlAttributes(currControl).OnLoad = frmFormEditor.lstNewControl.Items(21).Text



            With tmpControl 'set attributes

                .Name = catControlAttributes(currControl).Name

                '.Location = New Point(convertXY("X", catControlAttributes(currControl).Location) - (catPanels(currPanel).AutoScrollPosition.X * -1), _
                '                    convertXY("Y", catControlAttributes(currControl).Location) - (catPanels(currPanel).AutoScrollPosition.Y * -1)) 'swsw

                .Size = New Point(convertXY("X", catControlAttributes(currControl).Size), _
                                convertXY("Y", catControlAttributes(currControl).Size))


                If catControlAttributes(currControl).Type <> 11 Then
                    .Font = cvt.ConvertFromString(catControlAttributes(currControl).Font)
                    .Text = catControlAttributes(currControl).ConText
                End If

                If catControlAttributes(currControl).TabIndex > -1 Then
                    .TabIndex = catControlAttributes(currControl).TabIndex
                End If

            End With

            If catControlAttributes(currControl).Items.Length > 0 And catControlAttributes(currControl).Type <> 11 Then
                SetControlItems(currControl)
            End If


        End If

    End Sub


    Public Sub SetControlItems(ByVal index As Integer)

        Dim tmpArr() As String
        Dim count As Integer

        Try
            catControls(index).Items.Clear()

            tmpArr = catControlAttributes(index).Items.Split("|")

            For count = 0 To tmpArr.Length - 1
                catControls(index).Items.Add(tmpArr(count))
            Next

        Catch ex As Exception

        End Try



        '#swsw

    End Sub

    Public Sub ListBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs)

        Dim backCol As Color = Color.White
        Dim foreCol As Color = Color.Black
        Dim thisCurrControl As Integer = Convert.ToInt32(sender.Name.Substring(10))


        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            backCol = Color.LightBlue
            foreCol = Color.Black
        End If

        If sender.Items.Count > 0 Then

            e.DrawBackground()

            Dim itemText As String = sender.GetItemText(sender.Items(e.Index))

            If catControlAttributes(thisCurrControl).PositionOrder.IndexOf(sender _
                .GetItemText(sender.Items(e.Index))) > -1 _
                And catControlAttributes(thisCurrControl).SelectStyle = "Greyout" Then

                foreCol = Color.DarkBlue
            Else
                foreCol = Color.Black
            End If

            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then

                e.Graphics.FillRectangle(New SolidBrush(backCol), e.Bounds)

                Using b As New SolidBrush(foreCol)
                    e.Graphics.DrawString(sender.GetItemText(sender.Items(e.Index)), e.Font, b, e.Bounds)
                End Using
            Else

                Using b As New SolidBrush(foreCol)
                    e.Graphics.DrawString(sender.GetItemText(sender.Items(e.Index)), e.Font, b, e.Bounds)
                End Using
            End If

            e.DrawFocusRectangle()

        End If

    End Sub



    Public Sub catControls_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If catControlAttributes(currControl).selecting = False And FormLoad = False Then
            ControlItemSelected()
        End If

    End Sub


    Public Sub catControls_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'stop combo action script being called twice

        If catControlAttributes(currControl).selecting = False Then
            ControlItemSelected()
        End If

    End Sub

    Public Sub catControls_DoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)


        If catControlAttributes(currControl).Type = 7 Then

            catControls(currControl).Readonly = False
            catControls(currControl).Focus()

        End If

    End Sub


    Public Sub catControls_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        If frmFormEditor.Visible = False Then
            ControlItemSelected()
        End If

    End Sub

    Public Function getTemplateID(ByVal template As String)
        Dim TemplateID As Integer = -1
        Dim count As Integer

        If cmbTemplate.Items.Count > -1 Then

            For count = 0 To cmbTemplate.Items.Count - 1
                If cmbTemplate.Items.Item(count) = template Then
                    TemplateID = arrTemplateID(count)
                End If
            Next
        End If

        Return TemplateID

    End Function

    Public Sub ItemSelectedLabelClicked() 'toggle visible 

        If catControls(currControl).Text.ToString.Length > 0 Then
            catControls(currControl).Text = ""
        Else
            catControls(currControl).Text = Chr(161) 'catControlAttributes(currControl).ConText
        End If

    End Sub


    Public Sub ControlItemSelected()

        Dim childControl As Integer
        Dim tmpArr() As String
        Dim tmpSQL As String
        Dim tmpSQL1 As String
        Dim selText As String
        Dim colWidth As Integer = 30
        Dim tmpPos As Integer
        Dim tmpStr As String
        Dim tmpStr1 As String
        Dim tmpStr2 As String
        Dim tmpID As Integer
        Dim tmpChild As Integer
        Dim OldtmpChild As Integer
        Dim Status As Integer = 0
        Dim count As Integer
        Dim TemplateID As Integer
        Dim SQLreader As OdbcDataReader = Nothing
        Dim SQLcommand As OdbcCommand = Nothing
        Dim tmpConnstr As New OdbcConnection(ClientConnStr)



        If cmbTemplate.Text.Trim.Length > 0 Then
            TemplateID = getTemplateID(cmbTemplate.Text.Trim)
        Else
            TemplateID = -1
        End If

        If catControlAttributes(currControl).Type = 11 Then ItemSelectedLabelClicked()

        If frmFormEditor.Visible = True Then
            Exit Sub
        End If

        If catControlAttributes(currControl).Type = 7 Then

            If catControlAttributes(currControl).Enabled = True Then
                catControlAttributes(currControl).Enabled = False
            Else
                If catControls(currControl).Text <> "000.0" And catControlAttributes(currControl).Items = "up" Then
                    catControls(currControl).Text = "000.0"
                End If
                catControlAttributes(currControl).Data = DateTime.Now.Ticks
                catControlAttributes(currControl).Enabled = True
                Exit Sub
            End If
        End If

        tmpStr = catControlAttributes(currControl).DataBlock

        If tmpStr.Trim.StartsWith("<vbscript>") Then RunScript(tmpStr)

        catControlAttributes(currControl).counter += 1

        If catControlAttributes(currControl).Type <> 11 Then 'already using control.tag as a linked item value with newLabelSelectedItem (type11)

            catControls(currControl).Tag = catControlAttributes(currControl).counter 'so we can see in controls vbscript.

        End If

        'add selected item to list to allow for Greyout functionality


        If catControlAttributes(currControl).Type = 5 Then
            If catControls(currControl).SelectedItems.Count > 0 Then
                If Not catControlAttributes(currControl).PositionOrder.IndexOf("#" & catControls(currControl).SelectedItem.Trim & "#") > -1 Then

                    catControlAttributes(currControl).PositionOrder += "#" & catControls(currControl).SelectedItem.Trim & "#"
                End If
            End If
        End If

        tmpStr = ""


        Try
            If catControlAttributes(currControl).Type = 5 Then
                tmpStr = catControls(currControl).SelectedItem.ToString
            Else
                tmpStr = catControls(currControl).Text
            End If
        Catch ex As Exception
        End Try


        If catControlAttributes(currControl).Child = -2 Then
            If cmbReportTemplate.Text.Length > 0 Then

                Report.oldRtf = rtbDefault.Rtf

                Report.AddToReport()
                Exit Sub 'bad practice #NEEDS WORK#
            Else
                MsgBox("No Report Type Selected")
                Exit Sub
            End If
        End If

        Try 'catch empty selection
            If catControlAttributes(currControl).Type = 5 Then
                selText = catControls(currControl).SelectedItem.ToString
            Else
                selText = catControls(currControl).Text
            End If
        Catch ex As Exception
            selText = ""
        End Try


        If selText.Length < 4 Then ' allow for buttons with no text etc.
            selText = "TempText"
        End If


        If Not (frmFormEditor.Visible = False _
            And catControlAttributes(currControl).Action.Length > 0 _
            And selText.Trim.Length > 0 And catControlAttributes(currControl).Child > -1) Then

            Exit Sub
        End If

        childControl = GetControlID(catControlAttributes(currControl).Child)

        If childControl < 0 Then
            Exit Sub
        Else
            'catControlAttributes(childControl).PositionOrder = "" '#sw clear childs history - needs additional flag in catcontrols
        End If




        tmpSQL = catControlAttributes(currControl).Action
        tmpStr = tmpSQL

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

            Try
                tmpChild = GetControlID(tmpID)
                selText = catControls(tmpChild).SelectedItem.ToString
            Catch ex As Exception
                Try
                    selText = catControls(tmpChild).Text
                Catch ex1 As Exception
                    Exit While
                End Try
            End Try

            tmpSQL = tmpSQL.Replace(tmpStr2, selText.Trim)

            tmpSQL = Report.ProcessRepVals(tmpSQL)

        End While


        'add greyed out items to child if #SEL flag there 

        If tmpSQL.ToUpper.IndexOf("#SEL") > -1 Then

            tmpPos = tmpStr.IndexOf("#SEL")
            tmpID = GetControlID(tmpStr.Substring(tmpPos + 4, 3))
            tmpChild = GetControlID(catControlAttributes(currControl).Child)

            catControls(tmpChild).Items.Clear()

            tmpArr = Split(catControlAttributes(tmpID).PositionOrder, "##")

            For Each tmpStr In tmpArr

                tmpStr = tmpStr.Replace("#", "").Trim()

                catControls(tmpChild).Items.Add(tmpStr)

            Next
        End If


        If tmpSQL.ToUpper.IndexOf("SELECT") > -1 Then 'DataBlock select string

            If Not tmpSQL.ToUpper.Contains("FROM DOCUMENTS") Then

                selText = tmpSQL.Substring(0, tmpSQL.ToUpper.IndexOf("SELECT"))

                tmpSQL = tmpSQL.Substring(tmpSQL.ToUpper.IndexOf("SELECT"))

                If tmpSQL.ToUpper.Contains("ORDER BY") Then
                    tmpSQL1 = tmpSQL.Substring(0, tmpSQL.ToUpper.IndexOf("ORDER BY"))
                    tmpSQL1 += " AND Datablocks.Templates & " & TemplateID & " > 0 "
                    tmpSQL1 += " " & tmpSQL.Substring(tmpSQL.ToUpper.IndexOf("ORDER BY"))
                    tmpSQL = tmpSQL1
                Else
                    tmpSQL += " AND Datablocks.Templates & " & TemplateID & " > 0 "
                End If
            End If


            If tmpSQL.ToUpper.Contains("DOCUMENTS") Then
                connStr1.open()
                SQLcommand = connStr1.CreateCommand '#debug reqd #TODO#
                tmpSQL = Report.ProcessRepVals(tmpSQL)
                SQLcommand.CommandText = tmpSQL
            Else
                SQLcommand = connStr.CreateCommand
                SQLcommand.CommandText = tmpSQL
            End If



            Try
                Status = 0
                SQLreader = SQLcommand.ExecuteReader()

            Catch ex As Exception
                Status = 1
            End Try

            tmpChild = childControl
            OldtmpChild = currControl

            catControlAttributes(currControl).selecting = True


            While tmpChild > -1 'cascade clear children #TODO# add child insert type - Prepend, Append, Clear, Select

                If catControlAttributes(OldtmpChild).ChildDataMode = "Clear" Then
                    If catControlAttributes(tmpChild).Type = 5 Or _
                    catControlAttributes(tmpChild).Type = 3 Then
                        catControls(tmpChild).Items.Clear()
                    Else
                        catControls(tmpChild).Text = ""
                    End If
                End If

                If catControlAttributes(tmpChild).Child > -1 Then
                    OldtmpChild = tmpChild
                    tmpChild = GetControlID(catControlAttributes(tmpChild).Child)
                Else
                    Exit While
                End If

            End While

            If Status = 0 Then

                Dim oldTxt As String = ""

                While SQLreader.Read()

                    If SQLreader(0).ToString.Trim <> oldTxt.Trim Then  'force always unique #NEEDS WORK# ??

                        oldTxt = SQLreader(0).ToString.Trim

                        If catControlAttributes(childControl).Type < 5 Then
                            'catControls(childControl).Text = catControls(childControl).Text & selText & SQLreader(0) & vbCrLf & vbCrLf
                            catControls(childControl).Text = catControls(childControl).Text & SQLreader(0).ToString.Trim
                        Else
                            catControls(childControl).Items.Add(SQLreader(0).ToString.Trim)

                            If SQLreader(0).ToString.Trim.Length > colWidth Then
                                colWidth = SQLreader(0).ToString.Trim.Length
                            End If
                        End If
                    End If
                End While

                If catControlAttributes(childControl).Type = 5 Then
                    catControls(childControl).ColumnWidth = COLUMNWIDTH  '30 ' colWidth * 5 #sw
                End If

            End If


        ElseIf catControlAttributes(childControl).Type < 5 Then

            If catControlAttributes(currControl).ChildDataMode.Substring(0, 3).ToUpper = "APP" Then

                tmpStr1 = ""

                'allow for newlines, integer after "Append"

                If catControlAttributes(currControl).ChildDataMode.Length > 6 Then
                    tmpStr = catControlAttributes(currControl).ChildDataMode.Substring(6)

                    count = Convert.ToInt16(tmpStr)

                    If count > 0 Then
                        For count1 = 0 To count
                            tmpStr1 += vbCrLf
                        Next
                    End If
                End If

                catControls(childControl).Text = catControls(childControl).Text & tmpStr1 & tmpSQL

            ElseIf catControlAttributes(currControl).ChildDataMode.Substring(0, 3).ToUpper = "PRE" Then

                catControls(childControl).Text = tmpSQL & catControls(childControl).Text

            ElseIf catControlAttributes(currControl).ChildDataMode.Substring(0, 3).ToUpper = "CLE" Then

                catControls(childControl).Text = tmpSQL
            End If
        End If


        catControlAttributes(currControl).selecting = False


        'recursively call function to populate children if textbox or multiline textbox '#DISABLED#

        If catControlAttributes(currControl).Child > 0 Then

            tmpChild = GetControlID(catControlAttributes(currControl).Child)

            If catControlAttributes(tmpChild).Type = 2 Or _
                catControlAttributes(tmpChild).Type = 4 Then

                currControl = tmpChild 'watch recursion!!
                ControlItemSelected()

            End If
        End If

        Try
            connStr1.close()
        Catch ex As Exception
        End Try


    End Sub

    Private Function processDocSelect(ByVal tmpSQL As Integer)

        Return 1

    End Function


    Public Function GetControlID(ByVal child As Integer)

        Dim ChildID As Integer = -1

        For count = 0 To catControlAttributes.Length - 1

            If catControlAttributes(count).ControlID = child Then
                ChildID = count
                Exit For
            End If
        Next

        Return ChildID

    End Function




    Public Sub catControls_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)


        Dim tmpControl As Control = DirectCast(sender, Control)
        Dim MouseX As Integer
        Dim MouseY As Integer

        currControl = Convert.ToInt32(tmpControl.Name.Substring(10))

        If (My.Computer.Keyboard.CtrlKeyDown) Then
            ToolStripStatusLabel2.Text += "," & catControlAttributes(currControl).ControlID.ToString.PadLeft(3, "0")
        Else
            ToolStripStatusLabel2.Text = "   UserControlID : " & catControlAttributes(currControl).ControlID.ToString.PadLeft(3, "0")
        End If


        If frmFormEditor.Visible = True Then

            If Not catControlAttributes(currControl).Tag = "new" Then
                catControls(currControl).Tag = "edit"
                catControlAttributes(currControl).Tag = "edit"
            End If

        End If

        If e.Button = MouseButtons.Left And frmFormEditor.Visible = True Then

            isDragged = True

            MouseX = Cursor.Position.X
            MouseY = Cursor.Position.Y

            ptStartPosition.X = MouseX - 126 - tmpControl.Location.X
            ptStartPosition.Y = MouseY - 154 - tmpControl.Location.Y

        End If

        If e.Button = MouseButtons.Right Then
            If catControlAttributes(currControl).Type = 5 Then
                SaveTextToolStripMenuItem.Enabled = False
            Else
                SaveTextToolStripMenuItem.Enabled = True
            End If
        End If

        ShowControlAttributes()

    End Sub 'Once Mouse is Up Reset the Flag    


    Public Sub catControls_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)


        Dim tmpControl As Control = DirectCast(sender, Control)
        Dim ptPos As Point = tmpControl.Location

        If frmFormEditor.Visible = True And frmFormEditor.chkSnap.Checked = True Then

            If ptPos.X Mod 5 Then
                ptPos.X = ptPos.X - ptPos.X Mod 3
            End If

            If ptPos.Y Mod 5 Then
                ptPos.Y = ptPos.Y - ptPos.Y Mod 3
            End If

            tmpControl.Location = ptPos

            SetControlAttributePosition()

        End If

        isDragged = False

    End Sub 'InMousemove event perform the actual Transition of Control.    


    Public Sub catControls_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim tmpControl As Control = DirectCast(sender, Control)

        If isDragged Then

            Dim ptEndPosition As Point 'Set the New Transition point for control based on current mouse position and add appropriate offset to reduce flickering            

            ptEndPosition = tmpControl.PointToScreen(New Point(e.X, e.Y))

            ptEndPosition.Offset(-ptStartPosition.X - 126, -ptStartPosition.Y - 154) '?? works though?

            'add ctrl width/height #TODO#

            If ptEndPosition.X < 1 Then ptEndPosition.X = 1
            If ptEndPosition.Y < 1 Then ptEndPosition.Y = 1


            If ptEndPosition.X > catPanels(currPanel).Size.Width Then ptEndPosition.X = catPanels(currPanel).Size.Width
            'If ptEndPosition.Y > catPanels(currPanel).Size.Height Then ptEndPosition.Y = catPanels(currPanel).Size.Height

            tmpControl.Location = ptEndPosition

            'ptEndPosition = ptEndPosition + catPanels(currPanel).AutoScrollPosition  '#needed if control on panel rather than picturebox

            SetControlAttributePosition()

        End If

    End Sub

    Public Sub SetControlAttributePosition()

        Dim tmpPnt As New Point

        tmpPnt = catControls(currControl).location

        'tmpPnt.X = tmpPnt.X + (catPanels(currPanel).AutoScrollPosition.X * -1)
        'tmpPnt.Y = tmpPnt.Y + (catPanels(currPanel).AutoScrollPosition.Y * -1)

        frmFormEditor.lstNewControl.Items(9).Text = tmpPnt.ToString
        SetControlAttributes()

    End Sub


    Public Function convertXY(ByVal dimension As String, ByVal convStr As String)

        'cut out x or y coordinate from location or size string

        Dim retVal As Integer = -1

        Dim tmpStr As String = convStr.Replace("{Width=", "").Replace("Height=", "").Replace("}", "")
        tmpStr = tmpStr.Replace("{X=", "").Replace("Y=", "").Replace("}", "")

        If tmpStr.Trim.Length > 0 Then

            If dimension = "X" Then
                retVal = Convert.ToInt32(tmpStr.Substring(0, tmpStr.IndexOf(",")))
            Else
                retVal = Convert.ToInt32(tmpStr.Substring(tmpStr.IndexOf(",") + 1))
            End If

        End If

        Return retVal

    End Function

    Private Sub ImportDataBlockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportDataBlockToolStripMenuItem.Click

        Dim frmImport1 As New frmImport

        frmImport1.appVars = Me.appVars
        frmImport1.ShowDialog()

    End Sub


    Private Sub AttachDataBlockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AttachDataBlockToolStripMenuItem.Click

        Dim frmDataBlock1 As New frmActionScript

        If frmFormEditor.Visible = False Then

            appVars.ControlID = currControl

            frmDataBlock1.appVars = Me.appVars
            frmDataBlock1.ShowDialog()
        Else
            MsgBox("Not Allowed In FormEdit Mode")
        End If

    End Sub

    Private Sub SaveTextToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveTextToolStripMenuItem.Click

        Dim thisSQL As String = ""
        Dim repStr As String = ""
        Dim tmpTxt As String
        Dim sqlArray(100) As String
        Dim count As Integer

        If currControl = -1 Then
            Exit Sub
        End If

        For count = 0 To catControlAttributes.Length - 1

            If catControlAttributes(count).Child = catControlAttributes(currControl).ControlID Then

                thisSQL = catControlAttributes(count).Action
                Exit For
            End If
        Next

        While frmFormEditor.Visible = False

            If catControlAttributes(currControl).ConText.Trim.Length > 0 Then

                catControlAttributes(currControl).ConText = catControls(currControl).Text
                catControlAttributes(currControl).Tag = "edit"
                SaveControlChanges()
                MsgBox("Control Text Updated")

            ElseIf thisSQL.ToUpper.IndexOf("SELECT") > -1 Then

                sqlArray = Split(thisSQL)

                For count = 0 To sqlArray.Length - 1

                    If sqlArray(count).ToUpper = "FROM" Then
                        Exit For
                    End If
                Next

                If count < 2 Or count > 4 Then
                    Exit While
                End If

                tmpTxt = catControls(currControl).Text

                repStr = "UPDATE DataBlocks SET " & sqlArray(count - 1).ToString & " = '" & _
                    catControls(currControl).Text.ToString.Replace("'", "''") & "' WHERE "

                count = thisSQL.ToUpper.IndexOf("DISTINCT")

                If count > -1 Then
                    thisSQL = thisSQL.Substring(0, count) & " " & thisSQL.Substring(count + 8)
                End If

                thisSQL = thisSQL.Substring(thisSQL.ToUpper.IndexOf("WHERE") + 5)

                If thisSQL.IndexOf("ORDER") > -1 Then
                    thisSQL = thisSQL.Substring(0, thisSQL.ToUpper.IndexOf("ORDER"))
                End If

                thisSQL = repStr & thisSQL

                Dim tmpStr As String = thisSQL
                Dim tmpStr1 As String
                Dim tmpStr2 As String
                Dim tmpPos As Integer
                Dim tmpID As Integer
                Dim tmpChild As Integer
                Dim selText As String

                While True 'replace all control value placeholders in SQL #TODO# split into seperate func. 

                    tmpPos = tmpStr.IndexOf("#VAL")

                    If Not tmpPos > -1 Then
                        Exit While
                    End If

                    tmpStr1 = tmpStr.Substring(tmpPos + 4, 3)
                    tmpStr2 = tmpStr.Substring(tmpPos, 8)

                    tmpID = tmpStr1
                    tmpStr = tmpStr.Substring(tmpPos + 3)

                    selText = ""

                    Try
                        tmpChild = GetControlID(tmpID)
                        selText = catControls(tmpChild).SelectedItem.ToString.Replace("'", "''")
                    Catch ex As Exception
                        selText = catControls(tmpChild).Text.Replace("'", "''")
                    End Try

                    thisSQL = thisSQL.Replace(tmpStr2, selText.Trim)

                End While

                Dim SQLcommand As OdbcCommand
                SQLcommand = connStr.CreateCommand

                Try
                    SQLcommand.CommandText = thisSQL
                    SQLcommand.ExecuteNonQuery()
                    MsgBox("DataBlock Updated Successfully")
                Catch ex As Exception
                    MsgBox("DataBlock Update Failed")
                End Try


            End If
            Exit While
        End While

    End Sub


    Private Sub DocPasteBoxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocPasteBoxToolStripMenuItem.Click

        Dim frmDocPaste1 As New frmPasteBox
        frmDocPaste1.ShowDialog()

    End Sub

    Private Sub btnDemoConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDemoConfirm.Click


        If btnDemoConfirm.Text = "Search" Then

            ClientSearch = True
            SearchClient()

        ElseIf btnDemoConfirm.Text = "Select" Then

            SetCurrClient()
            LoadTemplateData()
            ClientSearch = False
            TemplateLoaded = True
            btnDemoConfirm.Text = "Save"

        ElseIf btnDemoConfirm.Text = "Save" Then

            If (cmbEthnicity.Text.Length = 0 Or cmbSex.Text.Length = 0 Or cmbTemplate.Text.Length = 0 _
                Or txtDOB.TextLength = 0 Or txtFname.TextLength = 0 Or txtLname.TextLength = 0) Then

                MsgBox("Please Complete Template, FirstName, LastName, DOB, Sex & Ethnicity")
            Else
                SaveClient()
                LoadTemplateData()
                MsgBox("Client Record Saved", MsgBoxStyle.Information)
            End If
        End If

        rtbDefault.Focus()


    End Sub

    Public Sub LoadClient()

        currClientID = Clients(currClientCount).ClientID

        currClient.ClientID = Clients(currClientCount).ClientID
        currClient.Add1 = txtAdd1.Text
        currClient.CellPhone = txtCellPhone.Text
        currClient.City = txtCity.Text
        currClient.DOB = txtDOB.Text
        currClient.Email = txtEmail.Text
        currClient.Ethnicity = cmbEthnicity.Text
        currClient.Fname = txtFname.Text
        currClient.HomePhone = txtHomePhone.Text
        currClient.Notes = txtNotes.Text
        currClient.Ref = txtRefNo.Text
        currClient.Sex = cmbSex.Text
        currClient.Sname = txtLname.Text
        currClient.SSN = txtSSN.Text
        currClient.State = txtState.Text
        currClient.Template = cmbTemplate.Text
        currClient.Title = cmbTitle.Text
        currClient.WorkPhone = txtWorkPhone.Text
        currClient.Zip = txtZip.Text

        pnlSearch.Hide()
        btnDemoConfirm.Text = "Save"


    End Sub

    Public Sub GetSearchFields() '#sw

        Try
            For count = 0 To catControlAttributes.Length - 1

                If catControlAttributes(count).SaveField = 1 Then currDocumentVals.Searchfield1 = catControls(count).text
                If catControlAttributes(count).SaveField = 2 Then currDocumentVals.Searchfield2 = catControls(count).text
                If catControlAttributes(count).SaveField = 3 Then currDocumentVals.Searchfield3 = catControls(count).text
                If catControlAttributes(count).SaveField = 4 Then currDocumentVals.Searchfield4 = catControls(count).text
                If catControlAttributes(count).SaveField = 5 Then currDocumentVals.Searchfield5 = catControls(count).text
                If catControlAttributes(count).SaveField = 6 Then currDocumentVals.Searchfield6 = catControls(count).text

            Next

        Catch ex As Exception
        End Try

    End Sub

    Private Function SaveClient()

        Dim status As Integer = 0
        Dim tmpSQL As String
        Dim SQLcommand As OdbcCommand
        Dim rtbDefault1 As New RichTextBox

        'set all text to black

        rtbDefault1.Rtf = rtbDefault.Rtf
        rtbDefault1.Select(0, rtbDefault1.TextLength)
        rtbDefault1.SelectionColor = Color.Black

        GetSearchFields()


        'split client functionality into client class #TODO#

        currClient.Add1 = txtAdd1.Text
        currClient.CellPhone = txtCellPhone.Text
        currClient.City = txtCity.Text

        If txtDOB.Text.Trim.Length > 1 Then currClient.DOB = DateValue(txtDOB.Text)

        currClient.Email = txtEmail.Text
        currClient.Ethnicity = cmbEthnicity.Text
        currClient.Fname = txtFname.Text
        currClient.HomePhone = txtHomePhone.Text
        currClient.MaritalStatus = cmbMaritalStatus.Text
        currClient.Notes = txtNotes.Text
        currClient.Ref = txtRefNo.Text
        currClient.Sex = cmbSex.Text
        currClient.Sname = txtLname.Text
        currClient.SSN = txtSSN.Text
        currClient.State = txtState.Text
        currClient.Template = cmbTemplate.Text
        currClient.Title = cmbTitle.Text
        currClient.WorkPhone = txtWorkPhone.Text
        currClient.Zip = txtZip.Text

        SQLcommand = connStr1.CreateCommand
        SQLcommand.Connection.Open()

        If currClient.ClientID > 0 Then

            tmpSQL = "UPDATE Client SET " & _
            "Add1 = '" & txtAdd1.Text.ToString & "' ," & _
            "CellPhone = '" & txtCellPhone.Text.ToString & "' ," & _
            "City = '" & txtCity.Text.ToString & "' ," & _
            "DOB = " & appVars.DateToDays(currClient.DOB) & " ," & _
            "Email = '" & txtEmail.Text.ToString & "' ," & _
            "Ethnicity = '" & cmbEthnicity.Text.ToString & "' ," & _
            "Fname = '" & txtFname.Text.ToString & "' ," & _
            "HomePhone = '" & txtHomePhone.Text.ToString & "' ," & _
            "MaritalStatus = '" & cmbMaritalStatus.Text.ToString & "' ," & _
            "Notes = '" & txtNotes.Text.ToString & "' ," & _
            "Ref = '" & txtRefNo.Text.ToString & "' ," & _
            "Sex = '" & cmbSex.Text.ToString & "' ," & _
            "Sname = '" & txtLname.Text.ToString & "' ," & _
            "SSN = '" & txtSSN.Text.ToString & "' ," & _
            "State = '" & txtState.Text.ToString & "' ," & _
            "Template = '" & cmbTemplate.Text.ToString & "' ," & _
            "Title = '" & cmbTitle.Text.ToString & "' ," & _
            "WorkPhone = '" & txtWorkPhone.Text.ToString & "' ," & _
            "Zip = '" & txtZip.Text.ToString & "' " & _
            "WHERE ClientID = " & currClient.ClientID

            Try
                SQLcommand.CommandText = tmpSQL
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                status = 1
            End Try

        ElseIf currClient.Sname.Trim.Length > 0 Then
            tmpSQL = "INSERT INTO Client ( " & _
            "Add1, " & _
            "CellPhone  ," & _
            "City, " & _
            "DOB," & _
            "Email, " & _
            "Ethnicity, " & _
            "Fname, " & _
            "HomePhone  ," & _
            "MaritalStatus ," & _
            "Notes, " & _
            "Ref  ," & _
            "Sex, " & _
            "Sname, " & _
            "SSN, " & _
            "State, " & _
            "Template, " & _
            "Title, " & _
            "WorkPhone, " & _
            "Zip " & _
            ") VALUES ( " & _
            "'" & currClient.Add1 & "'," & _
            "'" & currClient.CellPhone & "'," & _
            "'" & currClient.City & "'," & _
            "'" & appVars.DateToDays(currClient.DOB) & "'," & _
            "'" & currClient.Email & "'," & _
            "'" & currClient.Ethnicity & "'," & _
            "'" & currClient.Fname & "'," & _
            "'" & currClient.HomePhone & "'," & _
            "'" & currClient.MaritalStatus & "'," & _
            "'" & currClient.Notes & "'," & _
            "'" & currClient.Ref & "'," & _
            "'" & currClient.Sex & "'," & _
            "'" & currClient.Sname & "'," & _
            "'" & currClient.SSN & "'," & _
            "'" & currClient.State & "'," & _
            "'" & currClient.Template & "'," & _
            "'" & currClient.Title & "'," & _
            "'" & currClient.WorkPhone & "'," & _
            "'" & currClient.Zip & "')"

            Try
                SQLcommand.CommandText = tmpSQL
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                status = 1
            End Try

            If status = 0 Then

                SQLcommand = connStr1.CreateCommand

                SQLcommand.CommandText = "SELECT MAX(ClientID) FROM Client"

                Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

                If SQLreader.Read() Then
                    currClient.ClientID = SQLreader(0)
                End If


                If currClient.ClientID < 1 Then
                    status = 1
                Else
                    currClientID = currClient.ClientID
                End If
            End If

        End If

        Dim tmpRtf As String = rtbDefault.Text


        If currClient.ClientID > 0 And currReportID = -1 And rtbDefault.Text.Length > 0 _
        And oldReport <> rtbDefault.Rtf Then 'insert curr report

            tmpSQL = "INSERT INTO Documents ( " & _
            " ClientID, " & _
            " DocumentTag, " & _
            " DateAdded, " & _
            " DateLastEdit, " & _
            " DocText, " & _
            " Searchfield1, " & _
            " Searchfield2, " & _
            " Searchfield3, " & _
            " Searchfield4, " & _
            " Searchfield5, " & _
            " Searchfield6, " & _
            " OwnerID, " & _
            " LastEditID) " & _
            " VALUES ( " & _
            currClient.ClientID & "," & _
            "'" & currReportType & "'," & _
            " " & appVars.DateToDays(Now.Date) & " ," & _
            " " & appVars.DateToDays(Now.Date) & " ," & _
            "'" & rtbDefault1.Rtf.Replace("'", "''").Replace("\", "\\") & "'," & _
            "'" & currDocumentVals.Searchfield1 & "'," & _
            "'" & currDocumentVals.Searchfield2 & "'," & _
            "'" & currDocumentVals.Searchfield3 & "'," & _
            " " & currDocumentVals.Searchfield4 & " ," & _
            " " & currDocumentVals.Searchfield5 & " ," & _
            " " & currDocumentVals.Searchfield6 & " ," & _
            " " & appVars.User.UserID & " ," & _
            " " & appVars.User.UserID & " )"


            Try
                SQLcommand.CommandText = tmpSQL
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                status = 1
            End Try

            If status = 0 Then

                SQLcommand = connStr1.CreateCommand

                SQLcommand.CommandText = "SELECT MAX(ClientID) FROM Client"

                Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

                SQLreader.Read()
                currReportID = SQLreader(0)

                If currReportID < 1 Then
                    status = 1
                End If
            End If

        ElseIf currClient.ClientID > 0 And currReportID > -1 And rtbDefault.Text.Length > 0 Then 'update curr report

            tmpSQL = "UPDATE Documents SET " & _
            " DateLastEdit = '" & appVars.DateToDays(Now.Date) & "'," & _
            " DocText =  '" & rtbDefault1.Rtf.Replace("'", "''").Replace("\", "\\") & "'," & _
            " Searchfield1 = '" & currDocumentVals.Searchfield1 & "', " & _
            " Searchfield2 = '" & currDocumentVals.Searchfield2 & "', " & _
            " Searchfield3 = '" & currDocumentVals.Searchfield3 & "', " & _
            " Searchfield4 = " & currDocumentVals.Searchfield4 & ", " & _
            " Searchfield5 = " & currDocumentVals.Searchfield5 & ", " & _
            " Searchfield6 = " & currDocumentVals.Searchfield6 & ", " & _
            " LastEditID = " & appVars.User.UserID & _
            " WHERE DocumentID = " & currReportID & _
            " AND ClientID = " & currClient.ClientID


            Try
                SQLcommand.CommandText = tmpSQL
                SQLcommand.ExecuteNonQuery()
            Catch ex As Exception
                status = 1
            End Try

        End If

        If status = 1 Then
            MsgBox("Failed To Save Client Record", MsgBoxStyle.Critical)
        End If

        SQLcommand.Connection.Close()

        Return status


    End Function


    Private Sub SearchClient()

        Dim status As Integer = 0
        Dim tmpSQL As String = ""
        Dim SQLcommand As OdbcCommand
        Dim count As Integer = 0


        If txtAdd1.Text <> "" Then tmpSQL += " Add1 LIKE '" & txtAdd1.Text & "%'"
        If txtCellPhone.Text <> "" Then tmpSQL += " AND CellPhone LIKE '" & txtCellPhone.Text & "%'"
        If txtCity.Text <> "" Then tmpSQL += " AND City LIKE '" & txtCity.Text & "%'"

        If txtDOB.Text <> "" Then
            Try
                tmpSQL += " AND DOB = " & appVars.DateToDays(CDate(txtDOB.Text))
            Catch ex As Exception
            End Try
        End If


        If txtEmail.Text <> "" Then tmpSQL += " AND Email LIKE '" & txtEmail.Text & "%'"
        If cmbEthnicity.Text <> "" Then tmpSQL += " AND Ethnicity LIKE '" & cmbEthnicity.Text & "%'"
        If txtFname.Text <> "" Then tmpSQL += " AND Fname LIKE '" & txtFname.Text & "%'"
        If txtHomePhone.Text <> "" Then tmpSQL += " AND HomePhone LIKE '" & txtHomePhone.Text & "%'"
        If cmbMaritalStatus.Text <> "" Then tmpSQL += " AND MaritalStatus = '" & cmbMaritalStatus.Text & "'"
        If txtNotes.Text <> "" Then tmpSQL += " AND Notes LIKE '%" & txtNotes.Text & "%'"
        If txtRefNo.Text <> "" Then tmpSQL += " AND Ref LIKE '" & txtRefNo.Text & "%'"
        If cmbSex.Text <> "" Then tmpSQL += " AND Sex LIKE '" & cmbSex.Text & "%'"
        If txtLname.Text <> "" Then tmpSQL += " AND Sname LIKE '" & txtLname.Text & "%'"
        If txtSSN.Text <> "" Then tmpSQL += " AND SSN LIKE '" & txtSSN.Text & "%'"
        If txtState.Text <> "" Then tmpSQL += " AND State LIKE '" & txtState.Text & "%'"
        If cmbTemplate.Text <> "" Then tmpSQL += " AND Template LIKE '" & cmbTemplate.Text & "%'"
        If cmbTitle.Text <> "" Then tmpSQL += " AND Title LIKE '" & cmbTitle.Text & "%'"
        If txtWorkPhone.Text <> "" Then tmpSQL += " AND WorkPhone LIKE '" & txtWorkPhone.Text & "%'"
        If txtZip.Text <> "" Then tmpSQL += " AND Zip LIKE '" & txtZip.Text & "%'"


        If tmpSQL <> "" Then
            tmpSQL = "SELECT * FROM Client WHERE ClientID > 0 " & tmpSQL
        Else
            tmpSQL = "SELECT * FROM Client "
        End If


        SQLcommand = connStr1.CreateCommand
        SQLcommand.Connection.Open()

        SQLcommand.CommandText = tmpSQL

        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        While SQLreader.Read()

            ReDim Preserve Clients(count)

            Clients(count).ClientID = SQLreader("ClientID")
            Clients(count).Title = SQLreader("Title").ToString
            Clients(count).Fname = SQLreader("Fname").ToString
            Clients(count).Sname = SQLreader("Sname").ToString
            Clients(count).DOB = appVars.DaysToDate(SQLreader("DOB").ToString)
            Clients(count).SSN = SQLreader("SSN").ToString
            Clients(count).Ref = SQLreader("Ref").ToString
            Clients(count).MaritalStatus = SQLreader("MaritalStatus").ToString
            Clients(count).Sex = SQLreader("Sex").ToString
            Clients(count).Ethnicity = SQLreader("Ethnicity").ToString
            Clients(count).Add1 = SQLreader("Add1").ToString
            Clients(count).City = SQLreader("City").ToString
            Clients(count).State = SQLreader("State").ToString
            Clients(count).Zip = SQLreader("Zip").ToString
            Clients(count).HomePhone = SQLreader("HomePhone").ToString
            Clients(count).WorkPhone = SQLreader("WorkPhone").ToString
            Clients(count).CellPhone = SQLreader("CellPhone").ToString
            Clients(count).Email = SQLreader("Email").ToString
            Clients(count).Template = SQLreader("Template").ToString
            Clients(count).Notes = SQLreader("Notes").ToString

            count = count + 1

        End While


        If count > 0 Then
            currClientCount = 0
            btnDemoConfirm.Text = "Select"
            DisplayClient()
        Else
            MsgBox("No Matching Clients Found")
        End If

        SQLcommand.Connection.Close()


    End Sub


    Private Sub DisplayClient()

        Dim count As Integer = 0

        Try
            count = Clients.Length - 1
        Catch ex As Exception
            count = -1
        End Try

        pnlDocuments.SendToBack()
        pnlDocuments.Hide()

        pnlSearch.BringToFront()
        pnlSearch.Visible = True
        pnlSearch.Show()

        txtRecords.Text = "0/0"


        If (Not count = -1) And currClientCount <= count And currClientCount > -1 Then

            currClient = Clients(currClientCount)

            txtAdd1.Text = currClient.Add1
            txtCellPhone.Text = currClient.CellPhone
            txtCity.Text = currClient.City
            txtDOB.Text = currClient.DOB
            txtEmail.Text = currClient.Email
            cmbEthnicity.Text = currClient.Ethnicity
            txtFname.Text = currClient.Fname
            txtHomePhone.Text = currClient.HomePhone
            txtNotes.Text = currClient.Notes
            txtRefNo.Text = currClient.Ref
            cmbSex.Text = currClient.Sex
            txtLname.Text = currClient.Sname
            txtSSN.Text = currClient.SSN
            txtState.Text = currClient.State
            cmbTemplate.Text = currClient.Template
            cmbTitle.Text = currClient.Title
            txtWorkPhone.Text = currClient.WorkPhone
            txtZip.Text = currClient.Zip

            txtRecords.Text = Convert.ToString(currClientCount + 1).Trim & "/" & Convert.ToString(count + 1).Trim

        End If


    End Sub

    Public Sub SetCurrClient()

        currClientID = Clients(currClientCount).ClientID

        currClient.ClientID = Clients(currClientCount).ClientID
        currClient.Add1 = txtAdd1.Text
        currClient.CellPhone = txtCellPhone.Text
        currClient.City = txtCity.Text
        currClient.DOB = txtDOB.Text
        currClient.Email = txtEmail.Text
        currClient.Ethnicity = cmbEthnicity.Text
        currClient.Fname = txtFname.Text
        currClient.HomePhone = txtHomePhone.Text
        currClient.Notes = txtNotes.Text
        currClient.Ref = txtRefNo.Text
        currClient.Sex = cmbSex.Text
        currClient.Sname = txtLname.Text
        currClient.SSN = txtSSN.Text
        currClient.State = txtState.Text
        currClient.Template = cmbTemplate.Text
        currClient.Title = cmbTitle.Text
        currClient.WorkPhone = txtWorkPhone.Text
        currClient.Zip = txtZip.Text

        pnlSearch.Hide()
        btnDemoConfirm.Text = "Save"

    End Sub

    Private Sub GetClient(ByVal tmpClientID As Integer)

        Dim SQLcommand As OdbcCommand
        SQLcommand = connStr1.CreateCommand


        SQLcommand.Connection.Open()

        SQLcommand.CommandText = "SELECT * FROM Client WHERE ClientID = " & tmpClientID

        Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

        If SQLreader.Read() Then

            Try
                currClient.ClientID = tmpClientID
                currClientID = tmpClientID
                txtAdd1.Text = SQLreader("Add1")
                txtCellPhone.Text = SQLreader("CellPhone")
                txtCity.Text = SQLreader("City")
                txtDOB.Text = appVars.DaysToDate(SQLreader("DOB"))
                txtEmail.Text = SQLreader("Email")
                cmbEthnicity.Text = SQLreader("Ethnicity")
                txtFname.Text = SQLreader("Fname")
                txtHomePhone.Text = SQLreader("HomePhone")
                txtNotes.Text = SQLreader("Notes")
                txtRefNo.Text = SQLreader("Ref")
                cmbSex.Text = SQLreader("Sex")
                txtLname.Text = SQLreader("Sname")
                txtSSN.Text = SQLreader("SSN")
                txtState.Text = SQLreader("State")
                cmbTemplate.Text = SQLreader("Template")
                cmbTitle.Text = SQLreader("Title")
                txtWorkPhone.Text = SQLreader("WorkPhone")
                txtZip.Text = SQLreader("Zip")
            Catch ex As Exception
            End Try

            For count = 0 To cmbTemplate.Items.Count - 1
                If cmbTemplate.Text = cmbTemplate.Items(count) Then
                    cmbTemplate.SelectedIndex = count
                    Exit For
                End If
            Next

        End If

        SQLcommand.Connection.Close()


    End Sub


    Private Sub btnDemoCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        ClearClient()
        pnlSearch.Hide()
        btnDemoConfirm.Text = "Save"

    End Sub

    Private Sub btnDemoNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDemoNext.Click

        Dim count As Integer = 0

        Try
            count = Clients.Length - 1
        Catch ex As Exception
            count = -1
        End Try

        If Not count = -1 Then

            currClientCount = currClientCount + 1

            If currClientCount > count Then
                currClientCount = 0
            End If

            DisplayClient()

        End If


    End Sub

    Private Sub btnDemoPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDemoPrev.Click

        Dim count As Integer = 0

        Try
            count = Clients.Length - 1
        Catch ex As Exception
            count = -1
        End Try

        If Not count = -1 Then

            currClientCount = currClientCount - 1

            If currClientCount < 0 Then
                currClientCount = count
            End If

            DisplayClient()

        End If

    End Sub

    Private Sub NewReport(ByRef RepName As String)

        If ReportSelected = False Then


            If currClient.ClientID > -1 And rtbDefault.TextLength > 0 And oldReport <> rtbDefault.Rtf Then

                If MsgBox("Save Current Report ?", MsgBoxStyle.YesNo) = _
                MsgBoxResult.Yes Then

                    SaveClient()

                End If
            End If


            Dim SQLcommand As OdbcCommand
            SQLcommand = connStr.CreateCommand

            SQLcommand.CommandText = "SELECT AppDocs.DocText " & _
                                    " FROM   AppDocs " & _
                                    " WHERE  DocumentTag = '" & RepName & "' " & _
                                    " AND DocType = 1 "

            Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

            If SQLreader.Read() Then
                rtbDefault.Rtf = SQLreader(0).ToString.Replace("\\", "\")
                oldReport = rtbDefault.Rtf
            End If


            currReportID = -1

            rtbDefault.RightMargin = 610
            rtbDefault.Focus()

            currReportType = RepName
            cmbReportTemplate.Text = RepName

        End If

    End Sub

    Private Sub cmbReportTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReportTemplate.SelectedIndexChanged

        NewReport(cmbReportTemplate.Text)
 
    End Sub


    Private Sub ClearClient()

        pnlDocuments.Hide()
        pnlSearch.Hide()

        btnDemoConfirm.Text = "Save"
        cmbReportTemplate.Text = ""
        cmbTemplate.Focus()

        rtbDefault.BringToFront()
        'rtbDefault.Visible = True
        'rtbDefault.Show()
        'rtbDefault.WordWrap = False
        'rtbDefault.ScrollBars = RichTextBoxScrollBars.ForcedBoth
        'rtbDefault.RightMargin = rtbDefault.Width '#sw
        rtbDefault.Text = ""
        rtbDefault.Rtf = ""

        currDocumentVals.Searchfield1 = ""
        currDocumentVals.Searchfield2 = ""
        currDocumentVals.Searchfield3 = ""
        currDocumentVals.Searchfield4 = Nothing
        currDocumentVals.Searchfield5 = Nothing
        currDocumentVals.Searchfield6 = Nothing

        currReportID = -1
        currClientID = -1

        currClient.ClientID = -1
        currClient.Add1 = ""
        txtAdd1.Text = ""
        currClient.CellPhone = ""
        txtCellPhone.Text = ""
        currClient.City = ""
        txtCity.Text = ""
        currClient.DOB = Nothing
        txtDOB.Text = ""
        currClient.Email = ""
        txtEmail.Text = ""
        currClient.Ethnicity = ""
        cmbEthnicity.Text = ""
        currClient.Fname = ""
        txtFname.Text = ""
        currClient.HomePhone = ""
        txtHomePhone.Text = ""
        currClient.Notes = ""
        txtNotes.Text = ""
        currClient.Ref = ""
        txtRefNo.Text = ""
        currClient.Sex = ""
        cmbSex.Text = ""
        currClient.Sname = ""
        txtLname.Text = ""
        currClient.SSN = ""
        txtSSN.Text = ""
        currClient.State = ""
        txtState.Text = ""
        currClient.Template = ""
        cmbTemplate.Text = ""
        currClient.Title = ""
        cmbTitle.Text = ""
        currClient.WorkPhone = ""
        txtWorkPhone.Text = ""
        currClient.Zip = ""
        txtZip.Text = ""

        currClient.ClientID = -1
        currClient.Title = ""
        currClient.Fname = ""
        currClient.Sname = ""
        currClient.DOB = Nothing
        currClient.SSN = ""
        currClient.Ref = ""
        currClient.Sex = ""
        currClient.Ethnicity = ""
        currClient.Add1 = ""
        currClient.City = ""
        currClient.State = ""
        currClient.Zip = ""
        currClient.HomePhone = ""
        currClient.WorkPhone = ""
        currClient.CellPhone = ""
        currClient.Email = ""
        currClient.Template = ""
        currClient.Notes = ""

    End Sub


    Private Sub txtDOB_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDOB.Leave

        Try
            txtDOB.Text = Date.Parse(txtDOB.Text)
        Catch ex As Exception
            txtDOB.Text = ""
        End Try

    End Sub


    Private Sub btnRepUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRepUndo.Click

        'rtbDefault.Rtf = rtbDefault.Rtf.Replace("\v 0001\v0", " Steve Wallis")
        'Exit Sub


        Dim tmpStr As String = rtbDefault.Rtf

        rtbDefault.Rtf = Report.oldRtf
        Report.oldRtf = tmpStr

        rtbDefault.Select(oldReportCursorPos, 0)
        rtbDefault.ScrollToCaret()

    End Sub

    Private Sub btnRepWide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If rtbDefault.RightMargin = (rtbDefault.Width - 10) Then
            rtbDefault.RightMargin = 610
        Else
            rtbDefault.RightMargin = rtbDefault.Width - 10
        End If

        rtbDefault.Focus()

    End Sub

    Private Sub btnFontDialog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFontDialog.Click

        Dim fnt As New FontDialog

        fnt.Font = fntCurrentFont
        fnt.ShowDialog()

        txtFont.Text = fnt.Font.Name
        txtFontSize.Text = fnt.Font.Size

    End Sub

    Private Sub btnWord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWord.Click

        Dim frmWord1 As New frmWord '#sw
        Dim rtbDefault1 As New RichTextBox

        rtbDefault1.Rtf = rtbDefault.Rtf
        rtbDefault1.Select(0, rtbDefault1.TextLength)
        rtbDefault1.SelectionColor = Color.Black


        Clipboard.Clear()
        Clipboard.SetText(rtbDefault1.Rtf, TextDataFormat.Rtf)

        frmWord1.ShowDialog()

    End Sub

    Private Sub btnRepUnderline_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRepUnderline.Click

        rtbDefault.SelectionFont = New Font(rtbDefault.SelectionFont, rtbDefault.SelectionFont.Style Xor FontStyle.Underline)

        If rtbDefault.SelectionFont.Underline = False Then
            btnRepUnderline.BackColor = defContBackCol
        Else
            btnRepUnderline.BackColor = Color.LightBlue
        End If

        rtbDefault.Focus()

    End Sub

    Private Sub btnRepItalic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRepItalic.Click

        rtbDefault.SelectionFont = New Font(rtbDefault.SelectionFont, rtbDefault.SelectionFont.Style Xor FontStyle.Italic)

        If rtbDefault.SelectionFont.Italic = False Then
            btnRepItalic.BackColor = defContBackCol
        Else
            btnRepItalic.BackColor = Color.LightBlue
        End If

        rtbDefault.Focus()

    End Sub

    Private Sub btnRepBold_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRepBold.Click

        rtbDefault.SelectionFont = New Font(rtbDefault.SelectionFont, rtbDefault.SelectionFont.Style Xor FontStyle.Bold)

        If rtbDefault.SelectionFont.Bold = False Then
            btnRepBold.BackColor = defContBackCol
        Else
            btnRepBold.BackColor = Color.LightBlue
        End If

        rtbDefault.Focus()

    End Sub


    Private Sub rtbDefault_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rtbDefault.KeyPress


        If e.KeyChar = Convert.ToChar(2) Then 'ctrl-b
            rtbDefault.SelectionFont = New Font(rtbDefault.SelectionFont, rtbDefault.SelectionFont.Style Xor FontStyle.Bold)
            e.Handled = True
        End If

        If e.KeyChar = Convert.ToChar(21) Then 'ctrl-u
            rtbDefault.SelectionFont = New Font(rtbDefault.SelectionFont, rtbDefault.SelectionFont.Style Xor FontStyle.Underline)
            e.Handled = True
        End If

        If e.KeyChar = Convert.ToChar(9) Then 'ctrl-i
            rtbDefault.SelectionFont = New Font(rtbDefault.SelectionFont, rtbDefault.SelectionFont.Style Xor FontStyle.Italic)
            e.Handled = True
        End If

        setFontAttr()

    End Sub


    Public Function LoadDocument(ByVal DocID As Integer)


        Dim SQLcommand As OdbcCommand
        Dim retVal As Integer = 0
        Dim tmpClientID As Integer = -1


        SQLcommand = connStr1.CreateCommand
        SQLcommand.Connection.Open()

        If DocID > 0 Then

            SQLcommand.CommandText = "SELECT * " & _
                        " FROM   Documents " & _
                        " WHERE  DocumentID = " & DocID

            Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

            If SQLreader.Read() Then

                rtbDefault.Rtf = SQLreader("DocText").ToString.Replace("\\", "\")
                currReportID = SQLreader("DocumentID")
                tmpClientID = SQLreader("ClientID")

                Try
                    If SQLreader("Searchfield1").ToString <> "" Then currDocumentVals.Searchfield1 = SQLreader("Searchfield1").ToString
                    If SQLreader("Searchfield2").ToString <> "" Then currDocumentVals.Searchfield2 = SQLreader("Searchfield2").ToString
                    If SQLreader("Searchfield3").ToString <> "" Then currDocumentVals.Searchfield3 = SQLreader("Searchfield3").ToString
                    If SQLreader("Searchfield4").ToString <> "" Then currDocumentVals.Searchfield4 = SQLreader("Searchfield4").ToString
                    If SQLreader("Searchfield5").ToString <> "" Then currDocumentVals.Searchfield5 = SQLreader("Searchfield5").ToString
                    If SQLreader("Searchfield6").ToString <> "" Then currDocumentVals.Searchfield6 = SQLreader("Searchfield6").ToString
                Catch ex As Exception
                End Try

                oldReport = rtbDefault.Rtf

                rtbDefault.Focus()

                retVal = DocID
            End If
        End If

        SQLcommand.Connection.Close()

        If tmpClientID > -1 Then
            GetClient(tmpClientID)
            LoadTemplateData()
        End If

        Return retVal

    End Function

    Private Sub btnDemoCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDemoCancel.Click

        If currClient.ClientID > -1 And rtbDefault.TextLength > 0 Then

            If MsgBox("Save Current Report ?", MsgBoxStyle.YesNo) = _
            MsgBoxResult.Yes Then

                SaveClient()

            End If
        End If

        ClearClient()

    End Sub

    Public Sub SetPanelSize(ByVal intSize As Int16)

        PANELSIZE = intSize

        If PANELSIZE = 0 Then
            defaultPanel.Width = 361
            defaultPanel.Location = frmMainTreeView1.Location

        Else
            defaultPanel.Width = 976
            defaultPanel.Location = Panel3.Location
        End If

        frmMain_SizeChanged1()


    End Sub

    Public Sub SetApplicationLayout()

        If ApplicationName.Contains("Test") Then

            If PANELSIZE = 0 Then
                defaultPanel.Location = frmMainTreeView1.Location
            Else
                defaultPanel.Location = Panel3.Location
            End If

            PanelPreview.Visible = True
            PanelPreviewTop.Visible = True



            rtbDefault.Visible = True
            frmMainTreeView1.Visible = False

            Panel3.Width = defaultPanel.Width
            PictureBox2.Location = New Point(2.2)

        Else
            defaultPanel.Location = New Point(frmMainTreeView1.Width + 10, defaultPanel.Location.Y)
            frmMainTreeView1.Visible = True
            MenuToolStripMenuItem.Visible = False
            PrevToolStripMenuItem.Visible = False
            NextToolStripMenuItem.Visible = False

        End If

        frmMain_SizeChanged1()

    End Sub

    Private Sub frmMain_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        frmMain_SizeChanged1()
    End Sub

    Private Sub frmMain_SizeChanged1()

        Dim bar As Integer = 0

        Dim intX As Integer = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width
        Dim intY As Integer = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height

        If FormLoad = True Then Exit Sub

        Me.MaximizedBounds = Nothing
        Me.MinimumSize = Nothing

        If ApplicationName.Contains("Test") Then
            Me.MaximizedBounds = New Rectangle(0, 0, 1005, intY)
        Else
            Me.MaximizedBounds = New Rectangle(0, 0, intX, intY)
        End If

        If Me.Height > intY Then
            Me.Height = intY
        End If

        Panel1.Height = Me.Height - 45 - bar
        Panel1.Width = Me.Width - 18

        frmMainTreeView1.Height = Me.Size.Height - 215
        PanelPreview.Height = Me.Size.Height - 215

        If ApplicationName.Contains("Assess") Then
            defaultPanel.Height = Me.Size.Height - 215
            PanelPreview.Height = Me.Height - 143
            PanelPreview.Width = Me.Size.Width - 506
            PanelPreviewTop.Width = PanelPreview.Width
        Else
            If PANELSIZE = 0 Then
                defaultPanel.Height = Me.Size.Height - 215
                PanelPreview.Width = Me.Size.Width - 395
                PanelPreview.Height = Me.Height - 105 - PanelPreviewTop.Height - 5
                PanelPreview.Location = New Point(defaultPanel.Width + 10, Panel3.Location.Y + PanelPreviewTop.Height + 5)
                PanelPreviewTop.Width = PanelPreview.Width
                PanelPreviewTop.Location = New Point(PanelPreview.Location.X, PanelPreview.Location.Y - PanelPreviewTop.Height - 5)
            Else
                defaultPanel.Height = Me.Size.Height - 105
            End If
        End If


        For Each tmpP As Panel In catPanels
            tmpP.Size = defaultPanel.Size
            tmpP.Location = defaultPanel.Location
        Next

        If ApplicationName.Contains("Ass") Then '
            For Each tmpP As PictureBox In catPictureBoxes
                tmpP.Size = New Point(defaultPanel.Size.Width - 2, defaultPanel.Size.Height - 2)
            Next
        End If

    End Sub

    Private Sub SaveReportTemplateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveReportTemplateToolStripMenuItem.Click
        SaveReportTemplate()
    End Sub

    Private Sub SaveReportTemplate()

        Dim tmpFrm As frmReportName = New frmReportName

        If rtbDefault.Text.Length > 0 Then
            tmpFrm.ShowDialog()
        Else
            MsgBox("No Report Type Selected")
        End If

    End Sub

    Private Sub TemplateMaintenanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TemplateMaintenanceToolStripMenuItem.Click

        Dim tmpFrm As frmTemplateMaint = New frmTemplateMaint
        tmpFrm.ShowDialog()

    End Sub


    Private Sub LoadTemplateData()

        Dim TemplateID As Integer = -1
        Dim SQLcommand As OdbcCommand
        Dim SQLreader1 As OdbcDataReader
        Dim colWidth As Integer
        Dim tmpSQL As String
        Dim tmpSQL1 As String
        Dim tmpStr As String

        FormLoad = True

        If TotalControlCount = 0 Then Exit Sub


        If cmbTemplate.SelectedIndex > -1 Then
            TemplateID = arrTemplateID(cmbTemplate.SelectedIndex)
        End If

        SQLcommand = connStr.CreateCommand

        For count = 0 To catControls.Length - 1

            catControlAttributes(count).counter = 0
            catControlAttributes(count).PositionOrder = "" '#sw1

            currControl = count

            catControls(count).Text = Report.ProcessRepVals(catControlAttributes(count).ConText)

            If catControlAttributes(count).OnLoad.ToString.Trim.StartsWith("<vbscript>") Then RunScript(catControlAttributes(count).OnLoad)

            '#sw1

            tmpSQL = Report.ProcessRepVals(catControlAttributes(count).DataBlock)

            If catControlAttributes(count).DataBlock.Length > 0 And TemplateID > -1 Then

                Try

                    If tmpSQL.ToUpper.Contains("ORDER BY") And tmpSQL.Contains("DataBlocks") Then
                        tmpSQL1 = tmpSQL.Substring(0, tmpSQL.ToUpper.IndexOf("ORDER BY"))
                        tmpSQL1 += " AND Datablocks.Templates & " & TemplateID & " > 0 "
                        tmpSQL1 += " " & tmpSQL.Substring(tmpSQL.IndexOf("ORDER BY"))
                        tmpSQL = tmpSQL1
                    ElseIf tmpSQL.Contains("DataBlocks") Then
                        tmpSQL += " AND Datablocks.Templates & " & TemplateID & " > 0 "
                    End If


                    SQLcommand.CommandText = tmpSQL

                    SQLreader1 = SQLcommand.ExecuteReader()

                    If catControlAttributes(count).Type = 5 Or catControlAttributes(count).Type = 3 Then
                        catControls(count).Items.Clear()
                    Else
                        catControls(count).Text = ""
                    End If

                    Dim oldCol As String = ""

                    While SQLreader1.Read()

                        If SQLreader1(0).ToString.Trim <> oldCol.Trim Then 'ragged way of choosing unique

                            oldCol = SQLreader1(0)

                            If catControlAttributes(count).Type = 5 Or catControlAttributes(count).Type = 3 Then
                                catControls(count).Items.Add(Report.ProcessRepVals(SQLreader1(0)))
                                If SQLreader1(0).ToString.Trim.Length > colWidth Then
                                    colWidth = SQLreader1(0).ToString.Trim.Length
                                End If
                            ElseIf catControlAttributes(count).Type = 2 Then
                                catControls(count).Text += Report.ProcessRepVals(SQLreader1(0)) + " "
                            Else
                                catControls(count).Text += Report.ProcessRepVals(SQLreader1(0)) & vbCrLf
                            End If
                        End If
                    End While

                    If catControlAttributes(count).Type = 5 And catControlAttributes(count).ScrollBars = "Horizontal" Then 'if listbox set column width to max item width
                        catControls(count).ColumnWidth = COLUMNWIDTH ' colWidth * 6.5                    
                    End If

                    SQLreader1.Close()

                Catch ex As Exception
                    Dim a As String = ""
                End Try

            End If
        Next

        For count = 0 To catControlAttributes.Length - 1
            Try
                If catControlAttributes(count).SaveField = 1 Then catControls(count).text = currDocumentVals.Searchfield1
                If catControlAttributes(count).SaveField = 2 Then catControls(count).text = currDocumentVals.Searchfield2
                If catControlAttributes(count).SaveField = 3 Then catControls(count).text = currDocumentVals.Searchfield3
                If catControlAttributes(count).SaveField = 4 Then catControls(count).text = currDocumentVals.Searchfield4
                If catControlAttributes(count).SaveField = 5 Then catControls(count).text = currDocumentVals.Searchfield5
                If catControlAttributes(count).SaveField = 6 Then catControls(count).text = currDocumentVals.Searchfield6
            Catch ex As Exception
            End Try
        Next

        FormLoad = False

    End Sub

    Private Sub UserMaintenanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserMaintenanceToolStripMenuItem.Click

        Dim newUserMaint As New frmUserMaint

        newUserMaint.ShowDialog()

    End Sub

    Private Sub ApplyUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim fileName As String = ""
        Dim newUpdate As New clsUpdateAnt

        newUpdate.UpdateAnt(fileName)

    End Sub

    Private Sub frmMainTreeView1_BeforeSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles frmMainTreeView1.BeforeSelect

        If frmMainTreeView1.Nodes(1).IsSelected Then

            If Not e.Node.Text = "Demographics" Then

                If (cmbEthnicity.Text.Length = 0 Or cmbSex.Text.Length = 0 Or cmbTemplate.Text.Length = 0 _
                    Or txtDOB.TextLength = 0 Or txtFname.TextLength = 0 Or txtLname.TextLength = 0) _
                    And (frmFormEditor.Visible = False) Then
                    MsgBox("Please Complete Template, FirstName, LastName, DOB, Sex & Ethnicity")

                    e.Cancel = True
                Else
                    If pnlSearch.Visible = True Then
                        SetCurrClient()
                        LoadTemplateData()
                        pnlSearch.Hide()
                    End If

                    If txtLname.Text.Length > 0 Then
                        btnDemoConfirm.Text = "Save"
                        SaveClient()
                    End If

                End If
            End If
        End If

    End Sub


    Private Sub FlashInstallToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FlashInstallToolStripMenuItem.Click

        Dim frmFlash As frmFlashInstall = New frmFlashInstall

        frmFlash.ShowDialog()

    End Sub

    Private Sub tsCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsCut.Click

    End Sub

    Private Sub tsCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsCopy.Click

        Dim t As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim c As ContextMenuStrip = CType(t.Owner, ContextMenuStrip)

        contextMenuBuffer = c.SourceControl.Name

    End Sub

    Private Sub tsPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsPaste.Click

        Dim targetControl As String
        Dim fromID As Integer
        Dim toID As Integer
        Dim controlID As Integer
        Dim t As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim c As ContextMenuStrip = CType(t.Owner, ContextMenuStrip)
        Dim a As String
        Dim vals As String()

        ReDim vals(16)

        targetControl = c.SourceControl.Name
        a = targetControl.Substring(3, 5)

        If targetControl.Substring(3, 5) = "Panel" And contextMenuBuffer.Trim.Length > 7 Then

            If contextMenuBuffer.Substring(3, 5) = "Panel" Then

                fromID = contextMenuBuffer.Substring(8)
                toID = targetControl.Substring(8)

                Dim SQLcommand As OdbcCommand
                SQLcommand = connStr.CreateCommand


                SQLcommand.CommandText = "SELECT Controls.* from Controls " & _
                                        " WHERE  Controls.CatID = " & catPanels(fromID).Tag

                Dim SQLreader As OdbcDataReader = SQLcommand.ExecuteReader()

                While SQLreader.Read

                    For count2 = 0 To 18
                        vals(count2) = SQLreader(count2)
                    Next

                    vals(3) = catPanels(toID).Tag
                    controlID = LoadControl(vals)

                    catControlAttributes(controlID).Tag = "new"

                End While

                SaveControlChanges()

            ElseIf contextMenuBuffer.Substring(3, 5) = "Contr" Then

            End If

        End If


    End Sub

    Private Sub tsDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsDelete.Click

        Dim tmpControl As Control = DirectCast(catControls(currControl), Control)

        Dim SQLcommand As OdbcCommand
        SQLcommand = connStr.CreateCommand

        SQLcommand.CommandText = "DELETE from Controls WHERE ControlID = " & catControlAttributes(currControl).ControlID

        SQLcommand.ExecuteNonQuery()

        tmpControl.Dispose()

    End Sub


    Private Sub cmbTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTemplate.SelectedIndexChanged

        If ClientSearch = False Then
            LoadTemplateData()
        End If

    End Sub

    Private Sub EdidDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EdidDataToolStripMenuItem.Click

        Dim frmdDataBlockMaint As frmDataBlockMaint = New frmDataBlockMaint

        frmdDataBlockMaint.appVars = Me.appVars
        frmdDataBlockMaint.defControl = currControl

        frmdDataBlockMaint.ShowDialog()


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If currClient.ClientID > 0 And rtbDefault.Text.Length > 0 Then

            If SaveClient() = 0 Then
                MsgBox("Record Saved.", MsgBoxStyle.Information)
                oldReport = rtbDefault.Rtf
            Else
                MsgBox("User and/or Report Not Selected", MsgBoxStyle.Information)
            End If


        End If

    End Sub

    Private Sub HelpToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem1.Click

        Shell("explorer.exe http://gps-tools.net/OnlineHelp/help.htm", AppWinStyle.NormalFocus)

    End Sub

    Private Sub UndoRestoreToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoRestoreToolStripMenuItem1.Click
        Dim clsDb As New clsDataBaseFunctions
        clsDb.DB_UndoRestore()
    End Sub

    Private Sub RestoreDatabaseToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestoreDatabaseToolStripMenuItem1.Click
        Dim clsDb As New clsDataBaseFunctions
        clsDb.DB_Restore()
    End Sub

    Private Sub BackupDatabaseToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackupDatabaseToolStripMenuItem.Click
        Dim clsDb As New clsDataBaseFunctions
        clsDb.DB_Backup()
    End Sub

    Private Sub SelectDatabaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim clsDb As New clsDataBaseFunctions
        clsDb.ChangeCurrentDatabase()
    End Sub

    Private Sub FormEditModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormEditModeToolStripMenuItem.Click

        frmFormEditor.chkSnap.Checked = False
        frmFormEditor.Show()

    End Sub


    Private Sub UseLocalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseLocalToolStripMenuItem.Click
        Dim clsDb As New clsDataBaseFunctions
        clsDb.SetLocalDatabase()
    End Sub

    Private Sub btnDocSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocSearch.Click

        Dim tmpFrm As New frmDocSearch

        tmpFrm.txtClient.Text = currClientID
        tmpFrm.ShowDialog()

    End Sub


    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click



    End Sub

    Private Sub ODBCDatabaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ODBCDatabaseToolStripMenuItem.Click

        Dim tmpCls As New clsDataBaseFunctions

        tmpCls.openODBC()

    End Sub

    Private Sub SyncLocalDatabaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SyncLocalDatabaseToolStripMenuItem.Click

    End Sub

    Private Sub UserMaintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)



    End Sub

    Private Sub WebsiteLoginToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Shell("explorer.exe http://gps-tools.net/docman", AppWinStyle.NormalFocus)
    End Sub

    Private Sub UndoUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoUpdateToolStripMenuItem.Click

        If File.Exists("Client.s3db.update.bak") Then

            Try
                FileCopy("Client.s3db", "Client.tmp")
                FileCopy("Data.s3db", "Data.tmp")

                FileCopy("Client.s3db.update.bak", "Client.s3db")
                FileCopy("Data.s3db.update.bak", "Data.s3db")

                FileCopy("Client.tmp", "Client.s3db.update.bak")
                FileCopy("Data.tmp", "Data.s3db.update.bak")

                File.Delete("Client.tmp")
                File.Delete("Data.tmp")

                MsgBox("Undo Update Completed. Please Re-start ANT")

            Catch ex As Exception
            End Try
        Else
            MsgBox("No Previous Update Found. Undo Update Cancelled")
        End If


    End Sub


    Private Sub rtbDefault_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles rtbDefault.MouseClick

        setFontAttr()

    End Sub

    Private Sub setFontAttr()



        btnRepBold.BackColor = defContBackCol
        btnRepItalic.BackColor = defContBackCol
        btnRepUnderline.BackColor = defContBackCol

        If rtbDefault.SelectionFont.Bold = True Then btnRepBold.BackColor = Color.LightBlue
        If rtbDefault.SelectionFont.Italic = True Then btnRepItalic.BackColor = Color.LightBlue
        If rtbDefault.SelectionFont.Underline = True Then btnRepUnderline.BackColor = Color.LightBlue

        If rtbDefault.SelectionStart - 1 > 0 Then
            If rtbDefault.Text.Substring(rtbDefault.SelectionStart - 1, 1).Trim = "" Then
                rtbDefault.SelectionFont = fntCurrentFont
                txtFont.Text = fntCurrentFont.Name
                txtFontSize.Text = fntCurrentFont.Size
                btnRepBold.BackColor = defContBackCol
                btnRepItalic.BackColor = defContBackCol
                btnRepUnderline.BackColor = defContBackCol
            Else
                txtFont.Text = rtbDefault.SelectionFont.Name
                txtFontSize.Text = rtbDefault.SelectionFont.Size
            End If
        End If

    End Sub


    Private Sub frmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        If currClient.ClientID > 0 And rtbDefault.TextLength > 0 And oldReport <> rtbDefault.Rtf Then

            If MsgBox("Save Current Report ?", MsgBoxStyle.YesNo) = _
            MsgBoxResult.Yes Then

                SaveClient()

            End If
        End If

    End Sub

    Private Sub btnSpellCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSpellCheck.Click

        Dim objfrmSpellCheck As New frmSpellCheck
        objfrmSpellCheck.setSP(rtbDefault, SChecker)
        objfrmSpellCheck.ShowDialog(Me)

    End Sub

    Private Sub rtbDefault_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rtbDefault.TextChanged

    End Sub


    Private Sub ScriptEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScriptEditorToolStripMenuItem.Click

        Dim frmScriptEditor As New frmCodeEditor

        frmScriptEditor.SaveField = "Script"
        frmScriptEditor.Show()

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Dim count As Int16
        Dim counter As Decimal
        Dim tmpStr As String
        Dim TimeStart As Long
        Dim TimeNow As Long = DateTime.Now.Ticks '#sw1


        For count = 0 To catControlAttributes.Length - 1

            If catControlAttributes(count).Type = 7 And catControlAttributes(count).Enabled = True Then

                TimeStart = catControlAttributes(count).Data

                counter = (TimeNow - TimeStart) / 10000000

                tmpStr = Decimal.Round(counter, 1)

                catControls(count).Text = tmpStr.PadLeft(5, "0")

            End If
        Next
    End Sub

    Public Sub ItemSelectedLabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim tmpID As String = sender.Name.ToString.Substring(10)
        Dim tmpID1 As String
        Dim tmpStr As String = ""
        Dim LinkedItems() As String
        Dim tmpControl As Control


        If FormLoad = True Or frmFormEditor.Visible = True Then Exit Sub

        catControls(tmpID).Focus()

        If catControls(tmpID).Text.ToString.Trim.Length > 0 Then catControls(tmpID).BringToFront()


        If catControlAttributes(tmpID).Child > 0 And catControls(tmpID).Text.ToString.Trim.Length > 0 Then
            tmpID1 = GetControlID(catControlAttributes(tmpID).Child)
            catControls(tmpID1).Tag = catControlAttributes(tmpID).Items
        End If

        If catControlAttributes(tmpID).LinkedItems.ToString.Length > 0 And catControls(tmpID).Text.ToString.Trim.Length > 0 Then

            ReDim LinkedItems(Split(catControlAttributes(tmpID).LinkedItems, ",").Length)
            LinkedItems = Split(catControlAttributes(tmpID).LinkedItems, ",")

            For count = 0 To LinkedItems.Length - 1

                If LinkedItems(count) <> catControlAttributes(tmpID).ControlID Then

                    For count1 = 0 To catControlAttributes.Length - 1

                        If catControlAttributes(count1).ControlID.ToString.Trim = LinkedItems(count).Trim Then
                            catControls(count1).Text = ""
                        End If
                    Next
                End If
            Next
        End If

        're z-order controls from bottom to top to avoid overlap clipping control above.

        For Each tmpControl In catPictureBoxes(currPanel).Controls

            tmpID = tmpControl.Name.Substring(10)

            If catControlAttributes(tmpID).Type = 11 And tmpControl.Text.Length > 0 Then
                tmpStr += "," & tmpID
            End If
        Next

        Dim tmpArr() As String = tmpStr.Split(",")

        Array.Sort(tmpArr)

        For Each tmpID In tmpArr.Reverse
            If tmpID.Length > 0 Then
                catControls(tmpID).BringToFront()
            End If
        Next


    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click

    End Sub

    Private Sub frmMain_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        Dim bHandled As Boolean = False

        If frmFormEditor.Visible = False Then Exit Sub

        Select Case e.KeyCode

            Case Keys.Right
                frmFormEditor.MoveControl(1, 0)
                e.Handled = True

            Case Keys.Left
                frmFormEditor.MoveControl(-1, 0)
                e.Handled = True

            Case Keys.Up
                frmFormEditor.MoveControl(0, -1)
                e.Handled = True

            Case Keys.Down
                frmFormEditor.MoveControl(0, 1)
                e.Handled = True

        End Select

    End Sub

    Private Sub EditTextToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditTextToolStripMenuItem.Click

        Dim frmScriptEditor As New frmCodeEditor

        frmScriptEditor.SaveField = "Text"
        frmScriptEditor.Show()

    End Sub

    Public Sub RunScript(ByVal strScript)

        strScript = Report.ProcessRepVals(strScript.Trim.Remove(0, 10))

        Try
            ControlScript.ExecuteStatement(strScript)
        Catch ex As Exception
            MsgBox("VBScript Error : " & ex.Message.ToString)
        End Try

    End Sub

    Private Sub NextToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextToolStripMenuItem.Click

        If currPanel + 1 < catPanels.Length Then
            currPanel += 1
        Else
            currPanel = 0
        End If

        ChangePanel(currPanel)

    End Sub

    Private Sub PrevToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrevToolStripMenuItem.Click

        If currPanel > 0 Then
            currPanel -= 1
        Else
            currPanel = catPanels.Length - 1
        End If

        ChangePanel(currPanel)

    End Sub

    Private Sub DocumentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DocumentsToolStripMenuItem.Click

        Dim tmpFrm As New frmDocSearch

        tmpFrm.txtClient.Text = currClientID
        tmpFrm.Show()

    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        If currClient.ClientID > 0 And rtbDefault.Text.Length > 0 Then

            If SaveClient() = 0 Then
                MsgBox("Record Saved.", MsgBoxStyle.Information)
                oldReport = rtbDefault.Rtf
            Else
                MsgBox("User and/or Report Not Selected", MsgBoxStyle.Information)
            End If


        End If
    End Sub

 
    Private Sub AddTableTagsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddTableTagsToolStripMenuItem.Click

        Dim tmpRtf As String
        Dim tmpRtf1 As String
        Dim count As Integer
        Dim count1 As Integer
        Dim startPos As Integer = 0
        Dim endPos As Integer = 0
        Dim buf As String = ""
        Dim Prevbuf As String = ""
        Dim flag As Boolean = False
        Dim tmpStr As String


        If frmFormEditor.Visible = False Then
            MsgBox("Only allowed in Form Edit Mode.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If rtbDefault.Rtf.Trim.Length = 0 Then
            MsgBox("Please Select Report Type.", MsgBoxStyle.Information)
            Exit Sub
        Else
            tmpRtf = rtbDefault.Rtf
            tmpRtf1 = tmpRtf
        End If

        For count = tmpRtf.Length - 1 To 0 Step -1 'work backwards to preserve current pos in count

            buf += tmpRtf.Substring(count, 1)

            If tmpRtf.Substring(count, 1) = "\" Then

                If buf.Length > 1 Then

                    buf = StrReverse(buf)

                    If (buf.Contains("\intbl") Or buf = "\cell") And Prevbuf.Trim <> "\row" Then

                        count1 += 1

                        tmpStr = (10000 - count1).ToString.PadLeft(4, "0")
                        tmpRtf = tmpRtf.Substring(0, count + buf.Length) & "\v " & tmpStr & "\v0\" & tmpRtf.Substring(count + 1 + buf.Length)

                    End If

                    Prevbuf = buf
                    buf = ""

                End If

            End If
        Next


        If MsgBox(count1 & " New Tags, Save Changes ? ", MsgBoxStyle.YesNo) = MsgBoxResult.Yes And count1 > 0 Then

            rtbDefault.Rtf = tmpRtf

            SaveReportTemplate()

        End If

    End Sub



    Private Sub ShowHideTagsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowHideTagsToolStripMenuItem.Click

        Dim tmpStr As String

        If rtbDefault.Rtf.Contains("\v ") Then

            oldReport = rtbDefault.Rtf

            tmpStr = rtbDefault.Rtf.Replace("\v", "").Replace("\v0", "")
            rtbDefault.Rtf = tmpStr

        Else
            rtbDefault.Rtf = oldReport
        End If

    End Sub
End Class
