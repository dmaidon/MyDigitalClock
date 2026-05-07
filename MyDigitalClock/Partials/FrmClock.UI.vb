' Last Edit: May 07, 2026 12:52 | Synopsis: Restored WinForms digital font selection and assignment logic.
Partial Class FrmClock

    Private Sub SetDigitalFont()
        Dim fontNames() As String = {"DS-Digital", "Digital-7", "DSEG7 Classic", "Consolas", "Courier New"}
        Dim numberFont As Font = Nothing
        Dim amPmFont As Font = Nothing

        For Each fontName As String In fontNames
            Try
                numberFont = New Font(fontName, 54, FontStyle.Bold)
                amPmFont = New Font(fontName, 30, FontStyle.Bold)

                If numberFont.Name = fontName Then
                    Exit For
                End If
            Catch ex As Exception
                Continue For
            End Try
        Next

        If numberFont IsNot Nothing Then
            LblHour.Font = numberFont
            LblMinute.Font = numberFont
            LblSeconds.Font = numberFont
            Label2.Font = numberFont
            Label4.Font = numberFont
        End If

        If amPmFont IsNot Nothing Then
            LblAmPm.Font = amPmFont
        End If
    End Sub

End Class
