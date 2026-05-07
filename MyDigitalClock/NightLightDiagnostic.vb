' Last Edit: May 07, 2026 12:52 | Synopsis: Restored Night Light diagnostic helper for WinForms Ctrl+D shortcut.
Imports Microsoft.Win32
Imports System.Text

Public Class NightLightDiagnostic

    Public Shared Function GetDiagnosticInfo() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("=== Night Light Registry Diagnostic ===")
        sb.AppendLine()

        Dim registryPaths() As String = {
            "SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$$windows.data.bluelightreduction.settings\Current",
            "SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.settings\windows.data.bluelightreduction.settings"
        }

        For Each registryPath As String In registryPaths
            sb.AppendLine($"Trying path: {registryPath}")
            sb.AppendLine()

            Try
                Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(registryPath)
                    If key Is Nothing Then
                        sb.AppendLine("  Key not found")
                        sb.AppendLine()
                        Continue For
                    End If

                    sb.AppendLine("  ✓ Key found!")

                    sb.AppendLine("  Value names:")
                    For Each valueName As String In key.GetValueNames()
                        sb.AppendLine($"    - {valueName}")
                    Next
                    sb.AppendLine()

                    Dim data As Byte() = TryCast(key.GetValue("Data"), Byte())
                    If data IsNot Nothing Then
                        sb.AppendLine($"  Data length: {data.Length} bytes")
                        sb.AppendLine()

                        sb.AppendLine("  First 30 bytes (hex):")
                        For i As Integer = 0 To Math.Min(29, data.Length - 1)
                            sb.Append($"  [{i:D2}]: {data(i):X2}")
                            If i Mod 5 = 4 Then sb.AppendLine()
                        Next
                        sb.AppendLine()
                        sb.AppendLine()

                        If data.Length > 24 Then
                            sb.AppendLine("  Key bytes:")
                            sb.AppendLine($"    Byte 18 (enabled?): {data(18)} (0x{data(18):X2})")
                            sb.AppendLine($"    Byte 23 (schedule?): {data(23)} (0x{data(23):X2})")
                            sb.AppendLine($"    Byte 24 (active?): {data(24)} (0x{data(24):X2})")
                        End If
                    Else
                        sb.AppendLine("  Data value not found or wrong type")
                    End If
                End Using
            Catch ex As Exception
                sb.AppendLine($"  Error: {ex.Message}")
            End Try
            sb.AppendLine()
            sb.AppendLine("---")
            sb.AppendLine()
        Next

        Return sb.ToString()
    End Function

End Class
