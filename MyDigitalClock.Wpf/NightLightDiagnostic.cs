// Last Edit: May 07, 2026 13:05 | Synopsis: Created Night Light diagnostic helper for WPF Ctrl+D shortcut.
using Microsoft.Win32;
using System.Text;

namespace MyDigitalClock.Wpf;

internal static class NightLightDiagnostic
{
    public static string GetDiagnosticInfo()
    {
        StringBuilder sb = new();
        sb.AppendLine("=== Night Light Registry Diagnostic ===");
        sb.AppendLine();

        string[] registryPaths =
        [
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$$windows.data.bluelightreduction.settings\Current",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.settings\windows.data.bluelightreduction.settings"
        ];

        foreach (string registryPath in registryPaths)
        {
            sb.AppendLine($"Trying path: {registryPath}");
            sb.AppendLine();

            try
            {
                using RegistryKey? key = Registry.CurrentUser.OpenSubKey(registryPath);
                if (key is null)
                {
                    sb.AppendLine("  Key not found");
                    sb.AppendLine();
                    continue;
                }

                sb.AppendLine("  ✓ Key found!");

                sb.AppendLine("  Value names:");
                foreach (string valueName in key.GetValueNames())
                {
                    sb.AppendLine($"    - {valueName}");
                }
                sb.AppendLine();

                byte[]? data = key.GetValue("Data") as byte[];
                if (data is not null)
                {
                    sb.AppendLine($"  Data length: {data.Length} bytes");
                    sb.AppendLine();

                    sb.AppendLine("  First 30 bytes (hex):");
                    for (int i = 0; i < Math.Min(30, data.Length); i++)
                    {
                        sb.Append($"  [{i:D2}]: {data[i]:X2}");
                        if (i % 5 == 4)
                        {
                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine();
                    sb.AppendLine();

                    if (data.Length > 24)
                    {
                        sb.AppendLine("  Key bytes:");
                        sb.AppendLine($"    Byte 18 (enabled?): {data[18]} (0x{data[18]:X2})");
                        sb.AppendLine($"    Byte 23 (schedule?): {data[23]} (0x{data[23]:X2})");
                        sb.AppendLine($"    Byte 24 (active?): {data[24]} (0x{data[24]:X2})");
                    }
                }
                else
                {
                    sb.AppendLine("  Data value not found or wrong type");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"  Error: {ex.Message}");
            }

            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
