<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmClock
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        TlpClock = New TableLayoutPanel()
        LblHour = New Label()
        Label2 = New Label()
        LblMinute = New Label()
        Label4 = New Label()
        LblSeconds = New Label()
        LblAmPm = New Label()
        TmrClock = New Timer(components)
        TlpClock.SuspendLayout()
        SuspendLayout()
        '
        ' TlpClock
        '
        TlpClock.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble
        TlpClock.ColumnCount = 6
        TlpClock.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0007954F))
        TlpClock.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 10.0004005F))
        TlpClock.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0008011F))
        TlpClock.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 9.9964F))
        TlpClock.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0008011F))
        TlpClock.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0008011F))
        TlpClock.Controls.Add(LblHour, 0, 0)
        TlpClock.Controls.Add(Label2, 1, 0)
        TlpClock.Controls.Add(LblMinute, 2, 0)
        TlpClock.Controls.Add(Label4, 3, 0)
        TlpClock.Controls.Add(LblSeconds, 4, 0)
        TlpClock.Controls.Add(LblAmPm, 5, 0)
        TlpClock.Dock = DockStyle.Fill
        TlpClock.Location = New Point(0, 0)
        TlpClock.Margin = New Padding(3, 2, 3, 2)
        TlpClock.Name = "TlpClock"
        TlpClock.RowCount = 1
        TlpClock.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TlpClock.Size = New Size(803, 147)
        TlpClock.TabIndex = 0
        '
        ' LblHour
        '
        LblHour.AutoSize = True
        LblHour.Dock = DockStyle.Fill
        LblHour.Font = New Font("Segoe UI", 48F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LblHour.ForeColor = Color.Maroon
        LblHour.Location = New Point(6, 3)
        LblHour.Name = "LblHour"
        LblHour.Size = New Size(150, 141)
        LblHour.TabIndex = 0
        LblHour.Text = "88"
        LblHour.TextAlign = ContentAlignment.MiddleCenter
        '
        ' Label2
        '
        Label2.AutoSize = True
        Label2.Dock = DockStyle.Fill
        Label2.Font = New Font("Segoe UI", 48F, FontStyle.Bold)
        Label2.ForeColor = Color.Maroon
        Label2.Location = New Point(165, 3)
        Label2.Name = "Label2"
        Label2.Size = New Size(72, 141)
        Label2.TabIndex = 1
        Label2.Text = ":"
        Label2.TextAlign = ContentAlignment.MiddleCenter
        '
        ' LblMinute
        '
        LblMinute.AutoSize = True
        LblMinute.Dock = DockStyle.Fill
        LblMinute.Font = New Font("Segoe UI", 48F, FontStyle.Bold)
        LblMinute.ForeColor = Color.Maroon
        LblMinute.Location = New Point(246, 3)
        LblMinute.Name = "LblMinute"
        LblMinute.Size = New Size(150, 141)
        LblMinute.TabIndex = 2
        LblMinute.Text = "88"
        LblMinute.TextAlign = ContentAlignment.MiddleCenter
        '
        ' Label4
        '
        Label4.AutoSize = True
        Label4.Dock = DockStyle.Fill
        Label4.Font = New Font("Segoe UI", 48F, FontStyle.Bold)
        Label4.ForeColor = Color.Maroon
        Label4.Location = New Point(405, 3)
        Label4.Name = "Label4"
        Label4.Size = New Size(72, 141)
        Label4.TabIndex = 3
        Label4.Text = ":"
        Label4.TextAlign = ContentAlignment.MiddleCenter
        '
        ' LblSeconds
        '
        LblSeconds.AutoSize = True
        LblSeconds.Dock = DockStyle.Fill
        LblSeconds.Font = New Font("Segoe UI", 48F, FontStyle.Bold)
        LblSeconds.ForeColor = Color.Maroon
        LblSeconds.Location = New Point(486, 3)
        LblSeconds.Name = "LblSeconds"
        LblSeconds.Size = New Size(150, 141)
        LblSeconds.TabIndex = 4
        LblSeconds.Text = "88"
        LblSeconds.TextAlign = ContentAlignment.MiddleCenter
        '
        ' LblAmPm
        '
        LblAmPm.AutoSize = True
        LblAmPm.Dock = DockStyle.Fill
        LblAmPm.Font = New Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LblAmPm.ForeColor = Color.Maroon
        LblAmPm.Location = New Point(645, 3)
        LblAmPm.Name = "LblAmPm"
        LblAmPm.Size = New Size(152, 141)
        LblAmPm.TabIndex = 5
        LblAmPm.Text = "AM"
        LblAmPm.TextAlign = ContentAlignment.TopCenter
        '
        ' TmrClock
        '
        TmrClock.Enabled = True
        '
        ' FrmClock
        '
        AutoScaleDimensions = New SizeF(9F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(803, 147)
        Controls.Add(TlpClock)
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Margin = New Padding(3, 2, 3, 2)
        Name = "FrmClock"
        Text = "88"
        TlpClock.ResumeLayout(False)
        TlpClock.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents TlpClock As TableLayoutPanel
    Friend WithEvents LblHour As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents LblMinute As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents LblSeconds As Label
    Friend WithEvents LblAmPm As Label
    Friend WithEvents TmrClock As Timer

End Class
