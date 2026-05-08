// Last Edit: May 08, 2026 06:58 | Synopsis: Ensured Locate is available from tray icon menu only and removed in-app Locate handler.
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Forms = System.Windows.Forms;
using DrawingIcon = System.Drawing.Icon;
using WpfBrush = System.Windows.Media.Brush;
using WpfColor = System.Windows.Media.Color;
using WpfColorConverter = System.Windows.Media.ColorConverter;
using WpfFontFamily = System.Windows.Media.FontFamily;

namespace MyDigitalClock.Wpf;

public partial class MainWindow : Window
{
    private readonly string _dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
    private readonly string _logDir = Path.Combine(AppContext.BaseDirectory, "Logs");
    private readonly string _settingsFile;
    private readonly string _sunTimesFile;

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private static readonly HttpClient HttpClient = new();

    private const double Latitude = 35.62556;
    private const double Longitude = -78.328611;

    private readonly DispatcherTimer _clockTimer;
    private Forms.NotifyIcon? _notifyIcon;

    private int _lastDisplayedHour = -1;
    private int _lastDisplayedMinute = -1;
    private int _lastDisplayedSecond = -1;
    private string _lastDisplayedAmPm = string.Empty;

    private DateTime _sunriseTime = DateTime.MinValue;
    private DateTime _sunsetTime = DateTime.MinValue;
    private DateTime _lastSunFetchDate = DateTime.MinValue;

    private bool _currentThemeIsDark;
    private bool _isAutoThemeEnabled = true;

    public MainWindow()
    {
        InitializeComponent();

        _settingsFile = Path.Combine(_dataDir, "settings.json");
        _sunTimesFile = Path.Combine(_dataDir, "sun_times.json");

        _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clockTimer.Tick += ClockTimer_Tick;
    }

    private sealed class AppSettings
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string? FontFamily { get; set; }
        public double NumberFontSize { get; set; } = 82;
        public double AmPmFontSize { get; set; } = 44;
        public string? ForegroundColor { get; set; }
        public string? BackgroundColor { get; set; }
        public bool IsAutoThemeEnabled { get; set; } = true;
    }

    private sealed class SunTimesData
    {
        public string FetchDate { get; set; } = string.Empty;
        public string SunriseLocal { get; set; } = string.Empty;
        public string SunsetLocal { get; set; } = string.Empty;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        CreateFolders();
        InitializeNotifyIcon();
        SetDigitalFont();
        LoadSettings();
        LoadOrFetchSunTimes();

        if (_isAutoThemeEnabled)
        {
            CheckAndApplyTheme();
        }

        ClockTimer_Tick(this, EventArgs.Empty);
        _clockTimer.Start();
    }

    private void Window_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        _clockTimer.Stop();
        SaveSettings();
        DisposeNotifyIcon();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState != MouseButtonState.Pressed)
        {
            return;
        }

        DragMove();
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        {
            string diagInfo = NightLightDiagnostic.GetDiagnosticInfo();
            System.Windows.MessageBox.Show(diagInfo, "Night Light Diagnostic", MessageBoxButton.OK, MessageBoxImage.Information);
            e.Handled = true;
        }
    }

    private void ClockTimer_Tick(object? sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        int hour12 = now.Hour % 12 == 0 ? 12 : now.Hour % 12;
        string currentAmPm = now.Hour >= 12 ? "PM" : "AM";

        if (hour12 != _lastDisplayedHour)
        {
            HourText.Text = hour12.ToString("D2", CultureInfo.InvariantCulture);
            _lastDisplayedHour = hour12;
        }

        if (now.Minute != _lastDisplayedMinute)
        {
            MinuteText.Text = now.Minute.ToString("D2", CultureInfo.InvariantCulture);
            _lastDisplayedMinute = now.Minute;
        }

        if (now.Second != _lastDisplayedSecond)
        {
            SecondText.Text = now.Second.ToString("D2", CultureInfo.InvariantCulture);
            _lastDisplayedSecond = now.Second;
        }

        if (!string.Equals(currentAmPm, _lastDisplayedAmPm, StringComparison.Ordinal))
        {
            AmPmText.Text = currentAmPm;
            _lastDisplayedAmPm = currentAmPm;
        }

        if (_isAutoThemeEnabled && (now.Second == 0 || now.Second == 30))
        {
            CheckAndApplyTheme();
        }
    }

    private void CreateFolders()
    {
        Directory.CreateDirectory(_dataDir);
        Directory.CreateDirectory(_logDir);
    }

    private static WpfFontFamily PickFontFamily()
    {
        foreach (string name in new[] { "DS-Digital", "Digital-7", "DSEG7 Classic", "Consolas", "Courier New" })
        {
            try
            {
                return new WpfFontFamily(name);
            }
            catch
            {
                // Font not available, try next
            }
        }

        return new WpfFontFamily("Consolas");
    }

    private void SetDigitalFont()
    {
        WpfFontFamily fontFamily = PickFontFamily();

        foreach (var text in GetAllClockTextBlocks())
        {
            text.FontFamily = fontFamily;
        }
    }

    private IEnumerable<System.Windows.Controls.TextBlock> GetAllClockTextBlocks()
    {
        yield return HourText;
        yield return ColonOneText;
        yield return MinuteText;
        yield return ColonTwoText;
        yield return SecondText;
        yield return AmPmText;
    }

    private void LoadSettings()
    {
        try
        {
            if (!File.Exists(_settingsFile))
            {
                return;
            }

            AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_settingsFile), JsonOptions);
            if (settings is null)
            {
                return;
            }

            if (!double.IsNaN(settings.Left) && !double.IsNaN(settings.Top))
            {
                Left = settings.Left;
                Top = settings.Top;
            }

            if (!double.IsNaN(settings.Width) && settings.Width >= MinWidth)
            {
                Width = settings.Width;
            }

            if (!double.IsNaN(settings.Height) && settings.Height >= MinHeight)
            {
                Height = settings.Height;
            }

            _isAutoThemeEnabled = settings.IsAutoThemeEnabled;

            if (!string.IsNullOrWhiteSpace(settings.FontFamily))
            {
                WpfFontFamily customFamily = new(settings.FontFamily);
                foreach (var text in GetAllClockTextBlocks())
                {
                    text.FontFamily = customFamily;
                }
            }

            HourText.FontSize = settings.NumberFontSize;
            MinuteText.FontSize = settings.NumberFontSize;
            SecondText.FontSize = settings.NumberFontSize;
            ColonOneText.FontSize = settings.NumberFontSize;
            ColonTwoText.FontSize = settings.NumberFontSize;
            AmPmText.FontSize = settings.AmPmFontSize;

            if (!string.IsNullOrWhiteSpace(settings.ForegroundColor))
            {
                WpfColor fg = (WpfColor)WpfColorConverter.ConvertFromString(settings.ForegroundColor)!;
                ApplyTextColor(new SolidColorBrush(fg));
            }

            if (!string.IsNullOrWhiteSpace(settings.BackgroundColor))
            {
                WpfColor bg = (WpfColor)WpfColorConverter.ConvertFromString(settings.BackgroundColor)!;
                Background = new SolidColorBrush(bg);
            }
        }
        catch
        {
            // Ignore errors loading settings - use defaults
        }
    }

    private void SaveSettings()
    {
        try
        {
            var settings = new AppSettings
            {
                Left = Left,
                Top = Top,
                Width = Width,
                Height = Height,
                FontFamily = HourText.FontFamily.Source,
                NumberFontSize = HourText.FontSize,
                AmPmFontSize = AmPmText.FontSize,
                ForegroundColor = (HourText.Foreground as SolidColorBrush)?.Color.ToString(),
                BackgroundColor = (Background as SolidColorBrush)?.Color.ToString(),
                IsAutoThemeEnabled = _isAutoThemeEnabled
            };

            File.WriteAllText(_settingsFile, JsonSerializer.Serialize(settings, JsonOptions));
        }
        catch
        {
            // Ignore errors saving settings - not critical
        }
    }

    private void LoadOrFetchSunTimes()
    {
        try
        {
            if (!File.Exists(_sunTimesFile))
            {
                _ = FetchSunriseSunsetAsync();
                return;
            }

            SunTimesData? data = JsonSerializer.Deserialize<SunTimesData>(File.ReadAllText(_sunTimesFile), JsonOptions);
            DateTime today = DateTime.Today;

            if (data is null || !string.Equals(data.FetchDate, today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), StringComparison.Ordinal))
            {
                _ = FetchSunriseSunsetAsync();
                return;
            }

            bool sunriseValid = DateTime.TryParseExact(data.SunriseLocal, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _sunriseTime);
            bool sunsetValid = DateTime.TryParseExact(data.SunsetLocal, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _sunsetTime);

            if (sunriseValid && sunsetValid)
            {
                _lastSunFetchDate = today;
                return;
            }
        }
        catch
        {
            // Ignore errors loading sun times - will fetch fresh
        }

        _ = FetchSunriseSunsetAsync();
    }

    private async Task FetchSunriseSunsetAsync()
    {
        try
        {
            DateTime today = DateTime.Today;
            if (_lastSunFetchDate == today)
            {
                return;
            }

            string apiUrl = $"https://api.sunrise-sunset.org/json?lat={Latitude}&lng={Longitude}&formatted=0";
            using HttpResponseMessage response = await HttpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string payload = await response.Content.ReadAsStringAsync();
            using JsonDocument document = JsonDocument.Parse(payload);
            JsonElement results = document.RootElement.GetProperty("results");

            string sunriseRaw = results.GetProperty("sunrise").GetString() ?? string.Empty;
            string sunsetRaw = results.GetProperty("sunset").GetString() ?? string.Empty;

            bool sunriseParsed = DateTime.TryParse(sunriseRaw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime sunriseUtc);
            bool sunsetParsed = DateTime.TryParse(sunsetRaw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime sunsetUtc);

            if (!sunriseParsed || !sunsetParsed)
            {
                return;
            }

            _sunriseTime = sunriseUtc.ToLocalTime();
            _sunsetTime = sunsetUtc.ToLocalTime();
            _lastSunFetchDate = today;

            var sunData = new SunTimesData
            {
                FetchDate = today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                SunriseLocal = _sunriseTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                SunsetLocal = _sunsetTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            };

            await File.WriteAllTextAsync(_sunTimesFile, JsonSerializer.Serialize(sunData, JsonOptions));
            string logPath = Path.Combine(_logDir, "sunrise_sunset_log.txt");
            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Sunrise: {_sunriseTime:HH:mm}, Sunset: {_sunsetTime:HH:mm}{Environment.NewLine}";
            await File.AppendAllTextAsync(logPath, line);
        }
        catch
        {
            // Ignore errors fetching sun times - fallback to time-based detection
        }
    }

    private bool IsNightLightActive()
    {
        if (_lastSunFetchDate != DateTime.Today)
        {
            _ = FetchSunriseSunsetAsync();
        }

        if (_sunriseTime != DateTime.MinValue && _sunsetTime != DateTime.MinValue)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            return now >= _sunsetTime.TimeOfDay || now < _sunriseTime.TimeOfDay;
        }

        int hour = DateTime.Now.Hour;
        int minute = DateTime.Now.Minute;

        if (hour >= 18 || (hour == 17 && minute >= 30))
        {
            return true;
        }

        return hour < 7;
    }

    private void CheckAndApplyTheme()
    {
        bool shouldBeDark = IsNightLightActive();
        if (shouldBeDark == _currentThemeIsDark)
        {
            return;
        }

        _currentThemeIsDark = shouldBeDark;
        ApplyTheme(shouldBeDark);
    }

    private void ApplyTheme(bool dark)
    {
        if (!_isAutoThemeEnabled)
        {
            return;
        }

        WpfColor background = dark ? WpfColor.FromRgb(30, 30, 30) : Colors.White;
        WpfColor foreground = dark ? WpfColor.FromRgb(255, 140, 100) : Colors.Maroon;

        Background = new SolidColorBrush(background);
        ApplyTextColor(new SolidColorBrush(foreground));
    }

    private void ApplyTextColor(WpfBrush brush)
    {
        foreach (var text in GetAllClockTextBlocks())
        {
            text.Foreground = brush;
        }
    }

    private static string ColorToHex(System.Drawing.Color color)
        => $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";

    private void InitializeNotifyIcon()
    {
        DisposeNotifyIcon();

        Forms.ContextMenuStrip trayMenu = new();
        trayMenu.Items.Add("Locate", null, LocateFromTray_Click);
        trayMenu.Items.Add("Exit", null, ExitFromTray_Click);

        _notifyIcon = new Forms.NotifyIcon
        {
            Text = "My Digital Clock",
            ContextMenuStrip = trayMenu,
            Visible = true
        };

        if (!string.IsNullOrWhiteSpace(Environment.ProcessPath))
        {
            DrawingIcon? appIcon = DrawingIcon.ExtractAssociatedIcon(Environment.ProcessPath);
            if (appIcon is not null)
            {
                _notifyIcon.Icon = appIcon;
            }
        }
    }

    private void DisposeNotifyIcon()
    {
        if (_notifyIcon is null)
        {
            return;
        }

        _notifyIcon.Visible = false;
        _notifyIcon.ContextMenuStrip?.Dispose();
        _notifyIcon.Dispose();
        _notifyIcon = null;
    }

    private void LocateFromTray_Click(object? sender, EventArgs e)
    {
        LocateWindow();
    }

    private void LocateWindow()
    {
        Left = 250;
        Top = 250;

        if (WindowState == WindowState.Minimized)
        {
            WindowState = WindowState.Normal;
        }

        Activate();
        SaveSettings();
    }

    private void ExitFromTray_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void ChangeFont_Click(object sender, RoutedEventArgs e)
    {
        using Forms.FontDialog dialog = new()
        {
            Font = new System.Drawing.Font(HourText.FontFamily.Source, (float)HourText.FontSize, System.Drawing.FontStyle.Bold),
            ShowEffects = false
        };

        if (dialog.ShowDialog() != Forms.DialogResult.OK)
        {
            return;
        }

        WpfFontFamily family = new(dialog.Font.Name);
        double numberSize = Math.Max(24, dialog.Font.Size);

        foreach (var text in GetAllClockTextBlocks())
        {
            text.FontFamily = family;
            text.FontWeight = FontWeights.Bold;
        }

        HourText.FontSize = numberSize;
        MinuteText.FontSize = numberSize;
        SecondText.FontSize = numberSize;
        ColonOneText.FontSize = numberSize;
        ColonTwoText.FontSize = numberSize;
        AmPmText.FontSize = Math.Max(16, numberSize * 0.53);

        _isAutoThemeEnabled = false;
        SaveSettings();
    }

    private void ChangeTextColor_Click(object sender, RoutedEventArgs e)
    {
        using Forms.ColorDialog dialog = new();
        if (dialog.ShowDialog() != Forms.DialogResult.OK)
        {
            return;
        }

        WpfColor color = (WpfColor)WpfColorConverter.ConvertFromString(ColorToHex(dialog.Color))!;
        ApplyTextColor(new SolidColorBrush(color));
        _isAutoThemeEnabled = false;
        SaveSettings();
    }

    private void ChangeBackgroundColor_Click(object sender, RoutedEventArgs e)
    {
        using Forms.ColorDialog dialog = new();
        if (dialog.ShowDialog() != Forms.DialogResult.OK)
        {
            return;
        }

        WpfColor color = (WpfColor)WpfColorConverter.ConvertFromString(ColorToHex(dialog.Color))!;
        Background = new SolidColorBrush(color);
        _isAutoThemeEnabled = false;
        SaveSettings();
    }

    private void UseAutoTheme_Click(object sender, RoutedEventArgs e)
    {
        _isAutoThemeEnabled = true;
        _currentThemeIsDark = !_currentThemeIsDark;
        CheckAndApplyTheme();
        SaveSettings();
    }

    private void ResetCustomization_Click(object sender, RoutedEventArgs e)
    {
        _isAutoThemeEnabled = true;
        SetDigitalFont();

        HourText.FontSize = 82;
        MinuteText.FontSize = 82;
        SecondText.FontSize = 82;
        ColonOneText.FontSize = 82;
        ColonTwoText.FontSize = 82;
        AmPmText.FontSize = 44;

        _currentThemeIsDark = !_currentThemeIsDark;
        CheckAndApplyTheme();
        SaveSettings();
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}