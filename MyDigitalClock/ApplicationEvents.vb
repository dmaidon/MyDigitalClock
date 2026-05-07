' Last Edit: May 07, 2026 13:00 | Synopsis: Added startup form initialization and app defaults configuration.
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My

    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            ' Set the startup form
            Me.MainForm = New FrmClock()
        End Sub

        Private Sub MyApplication_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) Handles Me.ApplyApplicationDefaults
            e.HighDpiMode = HighDpiMode.PerMonitorV2
            e.ColorMode = SystemColorMode.System
        End Sub

    End Class

End Namespace
