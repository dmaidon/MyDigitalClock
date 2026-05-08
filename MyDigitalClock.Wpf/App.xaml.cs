// Last Edit: May 08, 2026 07:03 | Synopsis: Added single-instance startup guard for WPF app using a named mutex.
using System.Threading;
using WpfApplication = System.Windows.Application;
using StartupEventArgs = System.Windows.StartupEventArgs;

namespace MyDigitalClock.Wpf;

public partial class App : WpfApplication
{
    private Mutex? _singleInstanceMutex;

    /// <inheritdoc/>
    protected override void OnStartup(StartupEventArgs e)
    {
        const string mutexName = "MyDigitalClock.Wpf.SingleInstance";
        _singleInstanceMutex = new Mutex(true, mutexName, out bool createdNew);

        if (!createdNew)
        {
            Shutdown();
            return;
        }

        base.OnStartup(e);
        ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
    }

    /// <inheritdoc/>
    protected override void OnExit(System.Windows.ExitEventArgs e)
    {
        _singleInstanceMutex?.ReleaseMutex();
        _singleInstanceMutex?.Dispose();
        _singleInstanceMutex = null;

        base.OnExit(e);
    }
}