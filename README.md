# My Digital Clock

A customizable desktop digital clock widget for Windows with automatic day/night theme switching.

## Version Information

- **Version:** 25.12.11.41
- **Last Updated:** May 07, 2026
- **Target Framework:** .NET 10.0

## Projects

This solution contains two separate applications:

### 1. MyDigitalClock (VB.NET WinForms)
The original VB.NET Windows Forms implementation with:
- Fixed-size borderless window
- Automatic theme switching
- Right-click to exit
- Drag to reposition

### 2. MyDigitalClock.Wpf (C# WPF)
Modern WPF implementation with enhanced features:
- Resizable window with grip
- Runtime font customization
- Runtime color customization
- Context menu for settings
- Automatic theme switching

## Features

- **12-Hour Digital Clock** - Large, easy-to-read display with AM/PM indicator
- **Automatic Day/Night Themes** - Switches between light and dark themes based on real sunrise/sunset times
- **Sunrise/Sunset API** - Fetches daily data from sunrise-sunset.org and caches locally
- **Borderless Window** - Clean, minimal desktop widget appearance
- **Always on Top** - Stays visible above other windows
- **Draggable** - Position anywhere on your screen
- **Persistent Settings** - Remembers position and customization between sessions
- **Digital Fonts** - Uses DS-Digital, Digital-7, or DSEG7 Classic fonts if available
- **Night Light Diagnostic** - Press Ctrl+D to view Windows Night Light registry data

## System Requirements

- Windows 10 version 26100.0 or later
- .NET 10.0 Runtime
- Recommended: Digital fonts (DS-Digital, Digital-7, or DSEG7 Classic)

## Installation

1. Download the appropriate version:
   - **MyDigitalClock.exe** - VB.NET WinForms version
   - **MyDigitalClock.Wpf.exe** - C# WPF version

2. Run the executable
3. Position the clock where you want it on your screen
4. (WPF only) Right-click for customization options

## Usage

### WinForms Version
- **Drag:** Left-click and drag to reposition
- **Exit:** Right-click anywhere on the clock
- **Diagnostic:** Press Ctrl+D to view Night Light diagnostic info

### WPF Version
- **Drag:** Left-click and drag to reposition
- **Resize:** Drag the resize grip in bottom-right corner
- **Context Menu:** Right-click for options:
  - Change Font
  - Change Text Color
  - Change Background Color
  - Use Auto Theme (re-enable automatic themes)
  - Reset Customization
  - Exit
- **Diagnostic:** Press Ctrl+D to view Night Light diagnostic info

## Theme System

The clock automatically switches between day and night themes:

**Day Theme (Light):**
- White background
- Maroon text

**Night Theme (Dark):**
- Dark gray background (RGB 30, 30, 30)
- Soft orange text (RGB 255, 140, 100)

**Theme Detection:**
- Fetches sunrise/sunset times for Raleigh, NC (configurable in code)
- Updates daily and caches locally
- Falls back to fixed schedule (7 AM - 5:30 PM) if API unavailable
- Checks theme every 30 seconds

## Data Files

Application data is stored in the `Data` subfolder:

- **settings.json** - Window position and customization preferences
- **sun_times.json** - Cached sunrise/sunset data

Logs are stored in the `Logs` subfolder:

- **sunrise_sunset_log.txt** - API fetch history

## Development

### Building from Source

**Prerequisites:**
- Visual Studio 2026 or later
- .NET 10.0 SDK

**Build Steps:**
1. Open `MyDigitalClock.sln`
2. Build the solution (both projects build to `C:\VB18\Release\MyDigitalClock\`)

### Project Structure

```
MyDigitalClock/
├── MyDigitalClock/                 # VB.NET WinForms project
│   ├── FrmClock.vb
│   ├── ApplicationEvents.vb
│   ├── Partials/
│   │   ├── FrmClock.MouseEvents.vb
│   │   ├── FrmClock.Settings.vb
│   │   ├── FrmClock.SunTimes.vb
│   │   ├── FrmClock.Theme.vb
│   │   └── FrmClock.UI.vb
│   └── NightLightDiagnostic.vb
│
├── MyDigitalClock.Wpf/             # C# WPF project
│   ├── App.xaml / App.xaml.cs
│   ├── MainWindow.xaml / MainWindow.xaml.cs
│   └── NightLightDiagnostic.cs
│
└── WPF_IMPLEMENTATION_SUMMARY.md   # Detailed technical comparison
```

## Customization

### Location Coordinates

To change the sunrise/sunset location, edit the latitude/longitude constants:

**WinForms (FrmClock.vb):**
```vb
Private Const Latitude As Double = 35.62556
Private Const Longitude As Double = -78.328611
```

**WPF (MainWindow.xaml.cs):**
```csharp
private const double Latitude = 35.62556;
private const double Longitude = -78.328611;
```

### Theme Colors

**WinForms (FrmClock.Theme.vb):**
```vb
Private Shared ReadOnly LightTheme As New ThemeColors With {
    .BackColor = Color.White,
    .ForeColor = Color.Maroon
}

Private Shared ReadOnly DarkTheme As New ThemeColors With {
    .BackColor = Color.FromArgb(30, 30, 30),
    .ForeColor = Color.FromArgb(255, 140, 100)
}
```

**WPF (MainWindow.xaml.cs):**
```csharp
WpfColor background = dark ? WpfColor.FromRgb(30, 30, 30) : Colors.White;
WpfColor foreground = dark ? WpfColor.FromRgb(255, 140, 100) : Colors.Maroon;
```

## Troubleshooting

### Clock Not Updating
- Ensure the application is running (check taskbar)
- Verify .NET 10.0 runtime is installed

### Theme Not Switching
- Press Ctrl+D to view Night Light diagnostic information
- Check internet connectivity (for sunrise/sunset API)
- Verify `Data\sun_times.json` exists and is current

### Font Not Digital
- Install digital fonts: DS-Digital, Digital-7, or DSEG7 Classic
- Restart the application after installing fonts
- Application falls back to Consolas if digital fonts unavailable

### WPF: Customization Not Saving
- Ensure `Data` folder is writable
- Check for `settings.json` in the Data folder
- Verify application has write permissions

## License

Copyright © 2025-2026 Dennis N Maidon  
PAROLE Software

## Credits

- Sunrise/Sunset data from [sunrise-sunset.org](https://sunrise-sunset.org/)
- Digital fonts from their respective creators
- Icon: Clock.ico

## Support

For issues, feature requests, or contributions, please refer to the project repository.

---

**Note:** This is a desktop widget application. It does not include installer or auto-start functionality. Add to Windows Startup folder if you want it to launch automatically.
