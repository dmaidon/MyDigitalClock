' Last Edit: May 07, 2026 12:52 | Synopsis: Restored WinForms drag and click behaviors for the clock form.
Partial Class FrmClock

    Private isDragging As Boolean = False
    Private dragStartPoint As Point

    Private Sub Controls_MouseClick(sender As Object, e As MouseEventArgs) _
        Handles Me.MouseClick, TlpClock.MouseClick, LblHour.MouseClick,
                Label2.MouseClick, LblMinute.MouseClick, Label4.MouseClick,
                LblSeconds.MouseClick, LblAmPm.MouseClick
        If e.Button = MouseButtons.Right Then
            Me.Close()
        End If
    End Sub

    Private Sub Controls_MouseDown(sender As Object, e As MouseEventArgs) _
        Handles Me.MouseDown, TlpClock.MouseDown, LblHour.MouseDown,
                Label2.MouseDown, LblMinute.MouseDown, Label4.MouseDown,
                LblSeconds.MouseDown, LblAmPm.MouseDown
        If e.Button = MouseButtons.Left Then
            isDragging = True
            dragStartPoint = New Point(MousePosition.X - Me.Location.X,
                                       MousePosition.Y - Me.Location.Y)
        End If
    End Sub

    Private Sub Controls_MouseMove(sender As Object, e As MouseEventArgs) _
        Handles Me.MouseMove, TlpClock.MouseMove, LblHour.MouseMove,
                Label2.MouseMove, LblMinute.MouseMove, Label4.MouseMove,
                LblSeconds.MouseMove, LblAmPm.MouseMove
        If isDragging Then
            Me.Location = New Point(MousePosition.X - dragStartPoint.X,
                                   MousePosition.Y - dragStartPoint.Y)
        End If
    End Sub

    Private Sub Controls_MouseUp(sender As Object, e As MouseEventArgs) _
        Handles Me.MouseUp, TlpClock.MouseUp, LblHour.MouseUp,
                Label2.MouseUp, LblMinute.MouseUp, Label4.MouseUp,
                LblSeconds.MouseUp, LblAmPm.MouseUp
        If e.Button = MouseButtons.Left Then
            isDragging = False
        End If
    End Sub

End Class
