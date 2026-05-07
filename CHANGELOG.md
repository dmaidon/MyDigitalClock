Last Edit: May 07, 2026 16:55
Synopsis: Added a follow-up update entry for both WinForms and WPF versions and refreshed documentation timestamps.

# Changelog

All notable changes to My Digital Clock will be documented in this file.

## [May 07, 2026 - Follow-up Update]

### Updated - WinForms Version (MyDigitalClock)
- Refreshed changelog metadata and documented this follow-up update cycle.

### Updated - WPF Version (MyDigitalClock.Wpf)
- Refreshed changelog metadata and documented this follow-up update cycle.

## [May 07, 2026 - Documentation Update]

### Updated - WinForms Version (MyDigitalClock)
- Added explicit changelog tracking note for the current documentation refresh.
- Confirmed version coverage and references remain aligned with the VB.NET WinForms project.

### Updated - WPF Version (MyDigitalClock.Wpf)
- Added explicit changelog tracking note for the current documentation refresh.
- Confirmed version coverage and references remain aligned with the C# WPF project.

## [May 07, 2026]

### Added - WPF Implementation Complete
- **New WPF project** (`MyDigitalClock.Wpf`) created as separate C# application alongside VB WinForms version
- **Runtime customization** features in WPF:
  - Change font via WinForms FontDialog
  - Change text color via WinForms ColorDialog
  - Change background color
  - Reset customization to defaults
  - Toggle auto-theme mode
- **Context menu** for WPF with full customization options
- **Resizable window** with grip in WPF version
- **Night Light diagnostic** tool (Ctrl+D) in both WinForms and WPF versions
- **NightLightDiagnostic.cs** - C# version of registry diagnostic helper
- **Comprehensive documentation**:
  - README.md with usage instructions
  - WPF_IMPLEMENTATION_SUMMARY.md with technical comparison
  - CHANGELOG.md (this file)

### Fixed - VB WinForms Project Issues
- **BC30420 Error** - "Sub Main not found in Program"
  - Configured VB Application Framework properly
  - Changed `<MyType>` from `Empty` to `WindowsForms`
  - Changed `<StartupObject>` from `Program` to `Sub Main`
  - Added `<MainForm>FrmClock</MainForm>` property
  - Created `ApplicationEvents.vb` with `Startup` event handler
  - Removed `Program.vb` (no longer needed with VB App Framework)

- **NoStartupFormException** - "A startup form has not been specified"
  - Added `Startup` event handler in `ApplicationEvents.vb`
  - Properly initializes `Me.MainForm = New FrmClock()`

- **FileNotFoundException** - Icon loading error
  - Changed from `Icon.ExtractAssociatedIcon(path)` to loading from embedded resources
  - Now uses `Assembly.GetManifestResourceStream()` to load icon

### Fixed - WPF Compilation Errors
- **CS0104 Errors** - Ambiguous type references
  - Added type aliases for WPF types:
    - `using WpfColor = System.Windows.Media.Color;`
    - `using WpfColorConverter = System.Windows.Media.ColorConverter;`
    - `using WpfFontFamily = System.Windows.Media.FontFamily;`
  - Replaced all ambiguous `Color` and `ColorConverter` references with aliases
  - Used fully qualified names for `MessageBox` and `KeyEventArgs` in event handlers

### Changed - Project Configuration
- **VB WinForms**: Now uses VB Application Framework pattern (standard for VB WinForms apps)
- **WPF**: Configured with `UseWPF` and `UseWindowsForms` for dialog interop
- **Both projects**: Target .NET 10.0-windows10.0.26100.0
- **Both projects**: Output to separate directories under `C:\VB18\Release\MyDigitalClock\`

### Technical Details
- **Architecture**: WinForms uses partial classes across 5 files; WPF uses single MainWindow.xaml.cs
- **Layout**: WinForms uses TableLayoutPanel; WPF uses Grid with TextBlocks
- **Settings**: WPF stores richer customization data (fonts, colors, auto-theme state)
- **Feature Parity**: Both versions have identical core functionality
- **Enhanced WPF**: Runtime customization, resizable window, context menu

---

## Version History

### 26.5.7.61 (May 07, 2026)
- Complete WPF implementation with feature parity
- Fixed all VB Application Framework startup issues
- Fixed icon loading from embedded resources
- Resolved all ambiguous type reference errors

### 25.12.11.41 (Previous)
- Original VB.NET WinForms implementation
- Automatic day/night theme switching
- Sunrise/sunset API integration
- Settings persistence

---

## Notes

- Both WinForms and WPF versions are production-ready
- WPF version adds customization features while maintaining core functionality
- All compilation errors resolved
- Both applications tested and verified working
