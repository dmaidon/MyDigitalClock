' Last Edit: May 07, 2026 12:52 | Synopsis: Restored WinForms settings persistence for form position.
Imports System.IO
Imports System.Text.Json

Partial Class FrmClock

    Private settingsLoaded As Boolean = False

    Protected Overrides Sub SetVisibleCore(value As Boolean)
        If Not settingsLoaded AndAlso value Then
            LoadSettingsBeforeShow()
            settingsLoaded = True
        End If

        MyBase.SetVisibleCore(value)
    End Sub

    Private Sub LoadSettingsBeforeShow()
        Try
            If File.Exists(SettingsFile) Then
                Dim jsonString As String = File.ReadAllText(SettingsFile)
                Dim settings As AppSettings = JsonSerializer.Deserialize(Of AppSettings)(jsonString, JsonOptions)

                If settings IsNot Nothing Then
                    Dim savedLocation As New Point(settings.FormX, settings.FormY)
                    If IsLocationVisible(savedLocation) Then
                        StartPosition = FormStartPosition.Manual
                        Location = savedLocation
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SaveSettings()
        Try
            Dim settings As New AppSettings With {
                .FormX = Location.X,
                .FormY = Location.Y
            }

            Dim jsonString As String = JsonSerializer.Serialize(settings, JsonOptions)
            File.WriteAllText(SettingsFile, jsonString)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadSettings()
        If Not settingsLoaded Then
            LoadSettingsBeforeShow()
            settingsLoaded = True
        End If
    End Sub

    Private Function IsLocationVisible(location As Point) As Boolean
        For Each screen As Screen In Screen.AllScreens
            If screen.WorkingArea.Contains(location) Then
                Return True
            End If
        Next

        Return False
    End Function

End Class
