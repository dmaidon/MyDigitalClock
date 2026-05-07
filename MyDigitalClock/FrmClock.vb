' Last Edit: May 07, 2026 16:27 | Synopsis: Suppressed S3011 reflection warning for EnableDoubleBuffering performance optimization.
Imports System.IO
Imports System.Net.Http
Imports System.Text.Json

Partial Public Class FrmClock

    Private ReadOnly DataDir As String = Path.Combine(Application.StartupPath, "Data")
    Private ReadOnly LogDir As String = Path.Combine(Application.StartupPath, "Logs")
    Private ReadOnly SettingsFile As String = Path.Combine(DataDir, "settings.json")
    Private ReadOnly SunTimesFile As String = Path.Combine(DataDir, "sun_times.json")

    Private Shared ReadOnly JsonOptions As New JsonSerializerOptions With {.WriteIndented = True}
    Private Shared ReadOnly HttpClient As New HttpClient()

    Private Const Latitude As Double = 35.62556
    Private Const Longitude As Double = -78.328611

    Private Class AppSettings
        Public Property FormX As Integer
        Public Property FormY As Integer
    End Class

    Private Class SunTimesData
        Public Property FetchDate As String
        Public Property SunriseLocal As String
        Public Property SunsetLocal As String
    End Class

    Private lastDisplayedHour As Integer = -1
    Private lastDisplayedMinute As Integer = -1
    Private lastDisplayedSecond As Integer = -1
    Private lastDisplayedAmPm As String = ""

    Private Sub TmrClock_Tick(sender As Object, e As EventArgs) Handles TmrClock.Tick
        Dim nowTime As DateTime = DateTime.Now
        Dim hour12 As Integer = If(nowTime.Hour Mod 12 = 0, 12, nowTime.Hour Mod 12)
        Dim currentAmPm As String = If(nowTime.Hour >= 12, "PM", "AM")

        If hour12 <> lastDisplayedHour Then
            LblHour.Text = hour12.ToString("D2")
            lastDisplayedHour = hour12
        End If

        If nowTime.Minute <> lastDisplayedMinute Then
            LblMinute.Text = nowTime.Minute.ToString("D2")
            lastDisplayedMinute = nowTime.Minute
        End If

        If nowTime.Second <> lastDisplayedSecond Then
            LblSeconds.Text = nowTime.Second.ToString("D2")
            lastDisplayedSecond = nowTime.Second
        End If

        If currentAmPm <> lastDisplayedAmPm Then
            LblAmPm.Text = currentAmPm
            lastDisplayedAmPm = currentAmPm
        End If

        If nowTime.Second = 0 OrElse nowTime.Second = 30 Then
            CheckAndApplyTheme()
        End If
    End Sub

    Private Sub CreateFolders()
        For Each dirPath In {DataDir, LogDir}
            If Not Directory.Exists(dirPath) Then
                Directory.CreateDirectory(dirPath)
            End If
        Next
    End Sub



    Private Sub FrmClock_Load(sender As Object, e As EventArgs) Handles Me.Load
        CreateFolders()

        TmrClock.Interval = 1000
        FormBorderStyle = FormBorderStyle.None
        KeyPreview = True

        ' Load icon from embedded resources
        Dim assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Using stream = assembly.GetManifestResourceStream("MyDigitalClock.Clock.ico")
            If stream IsNot Nothing Then
                Icon = New Icon(stream)
            End If
        End Using

        DoubleBuffered = True
        EnableDoubleBuffering(TlpClock)

        SetStyle(ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.AllPaintingInWmPaint Or
                 ControlStyles.UserPaint, True)
        UpdateStyles()

        For Each lbl As Label In {LblHour, LblMinute, LblSeconds, Label2, Label4, LblAmPm}
            EnableDoubleBuffering(lbl)
        Next

        SetDigitalFont()

        LoadSettings()
        Show()
        LoadOrFetchSunTimes()
        CheckAndApplyTheme()
    End Sub

    Private Sub EnableDoubleBuffering(control As Control)
        ' S3011: Reflection is required here to access WinForms internal DoubleBuffered property for performance
#Disable Warning S3011
        Dim prop As Reflection.PropertyInfo = GetType(Control).GetProperty("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
        prop.SetValue(control, True, Nothing)
#Enable Warning S3011
    End Sub

    Private Sub FrmClock_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveSettings()
    End Sub

    Private Sub FrmClock_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.D Then
            Dim diagInfo As String = NightLightDiagnostic.GetDiagnosticInfo()
            MessageBox.Show(diagInfo, "Night Light Diagnostic", MessageBoxButtons.OK, MessageBoxIcon.Information)
            e.Handled = True
        End If
    End Sub

End Class
