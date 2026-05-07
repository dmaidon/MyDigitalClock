' Last Edit: May 07, 2026 12:52 | Synopsis: Restored WinForms day and night theme switching behavior.
Partial Class FrmClock

    Private Structure ThemeColors
        Public BackColor As Color
        Public ForeColor As Color
    End Structure

    Private Shared ReadOnly LightTheme As New ThemeColors With {
        .BackColor = Color.White,
        .ForeColor = Color.Maroon
    }

    Private Shared ReadOnly DarkTheme As New ThemeColors With {
        .BackColor = Color.FromArgb(30, 30, 30),
        .ForeColor = Color.FromArgb(255, 140, 100)
    }

    Private currentThemeIsDark As Boolean = False

    Private Sub CheckAndApplyTheme()
        Dim shouldBeDark As Boolean = IsNightLightActive()

        If shouldBeDark <> currentThemeIsDark Then
            currentThemeIsDark = shouldBeDark
            ApplyTheme(If(shouldBeDark, DarkTheme, LightTheme))
        End If
    End Sub

    Private Function IsNightLightActive() As Boolean
        Dim today As Date = Date.Today
        If lastSunFetchDate <> today Then
            Task.Run(AddressOf FetchSunriseSunsetAsync)
        End If

        If sunriseTime <> DateTime.MinValue AndAlso sunsetTime <> DateTime.MinValue Then
            Dim currentTime As TimeSpan = DateTime.Now.TimeOfDay
            Return currentTime >= sunsetTime.TimeOfDay OrElse currentTime < sunriseTime.TimeOfDay
        End If

        Return IsNightTimeSimpleFallback()
    End Function

    Private Function IsNightTimeSimpleFallback() As Boolean
        Dim now As DateTime = DateTime.Now
        Dim currentHour As Integer = now.Hour
        Dim currentMinute As Integer = now.Minute

        If currentHour >= 18 OrElse (currentHour = 17 AndAlso currentMinute >= 30) Then
            Return True
        End If

        Return currentHour < 7
    End Function

    Private Sub ApplyTheme(theme As ThemeColors)
        SuspendLayout()
        TlpClock.SuspendLayout()

        Try
            BackColor = theme.BackColor
            TlpClock.BackColor = theme.BackColor

            Dim foreColor As Color = theme.ForeColor
            LblHour.ForeColor = foreColor
            LblMinute.ForeColor = foreColor
            LblSeconds.ForeColor = foreColor
            Label2.ForeColor = foreColor
            Label4.ForeColor = foreColor
            LblAmPm.ForeColor = foreColor

            TlpClock.CellBorderStyle = If(theme.BackColor.ToArgb() = DarkTheme.BackColor.ToArgb(),
                                         TableLayoutPanelCellBorderStyle.Single,
                                         TableLayoutPanelCellBorderStyle.OutsetDouble)
        Finally
            TlpClock.ResumeLayout()
            ResumeLayout()
        End Try
    End Sub

End Class
