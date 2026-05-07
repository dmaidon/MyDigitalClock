// Last Edit: May 07, 2026 12:52 | Synopsis: Updated WPF app startup type usage to avoid WinForms/WPF Application ambiguity.
using WpfApplication = System.Windows.Application;
using StartupEventArgs = System.Windows.StartupEventArgs;

namespace MyDigitalClock.Wpf;

public partial class App : WpfApplication
{
    /// <inheritdoc/>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
    }
}