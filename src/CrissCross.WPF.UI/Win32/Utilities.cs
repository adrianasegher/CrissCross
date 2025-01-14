// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace CrissCross.WPF.UI.Win32;

/// <summary>
/// Common Window utilities.
/// </summary>
internal static class Utilities
{
    private static readonly PlatformID _osPlatform = Environment.OSVersion.Platform;

    private static readonly Version _osVersion =
#if NET5_0_OR_GREATER
    Environment.OSVersion.Version;
#else
    GetOSVersionFromRegistry();
#endif

    /// <summary>
    /// Gets a value indicating whether the operating system is NT or newer.
    /// </summary>
    public static bool IsNT => _osPlatform == PlatformID.Win32NT;

    /// <summary>
    /// Gets a value indicating whether the operating system version is greater than or equal to 6.0.
    /// </summary>
    public static bool IsOSVistaOrNewer => new Version(6, 0) <= _osVersion;

    /// <summary>
    /// Gets a value indicating whether the operating system version is greater than or equal to 6.1.
    /// </summary>
    public static bool IsOSWindows7OrNewer => new Version(6, 1) <= _osVersion;

    /// <summary>
    /// Gets a value indicating whether the operating system version is greater than or equal to 6.2.
    /// </summary>
    public static bool IsOSWindows8OrNewer => new Version(6, 2) <= _osVersion;

    /// <summary>
    /// Gets a value indicating whether the operating system version is greater than or equal to 10.0* (build 10240).
    /// </summary>
    public static bool IsOSWindows10OrNewer => _osVersion.Build >= 10240;

    /// <summary>
    /// Gets a value indicating whether the operating system version is greater than or equal to 10.0* (build 22000).
    /// </summary>
    public static bool IsOSWindows11OrNewer => _osVersion.Build >= 22000;

    /// <summary>
    /// Gets a value indicating whether the operating system version is greater than or equal to 10.0* (build 22523).
    /// </summary>
    public static bool IsOSWindows11Insider1OrNewer => _osVersion.Build >= 22523;

    /// <summary>
    /// Gets a value indicating whether the operating system version is greater than or equal to 10.0* (build 22557).
    /// </summary>
    public static bool IsOSWindows11Insider2OrNewer => _osVersion.Build >= 22557;

    /// <summary>
    /// Gets a value indicating whether indicates whether Desktop Window Manager (DWM) composition is enabled.
    /// </summary>
    public static bool IsCompositionEnabled
    {
        get
        {
            if (!IsOSVistaOrNewer)
            {
                return false;
            }

            _ = Dwmapi.DwmIsCompositionEnabled(out var pfEnabled);

            return pfEnabled != 0;
        }
    }

    public static void SafeDispose<T>(ref T disposable)
        where T : IDisposable
    {
        // Dispose can safely be called on an object multiple times.
        IDisposable t = disposable;
        disposable = default!;

        if (t is null)
        {
            return;
        }

        t.Dispose();
    }

    public static void SafeRelease<T>(ref T comObject)
        where T : class
    {
        var t = comObject;
        comObject = default!;

        if (t is null)
        {
            return;
        }

        Debug.Assert(Marshal.IsComObject(t), "Safe Release");
        Marshal.ReleaseComObject(t);
    }

#if !NET5_0_OR_GREATER
    /// <summary>
    /// Tries to get the OS version from the Windows registry.
    /// </summary>
    private static Version GetOSVersionFromRegistry()
    {
        var major = 0;
        {
            // The 'CurrentMajorVersionNumber' string value in the CurrentVersion key is new for Windows 10,
            // and will most likely (hopefully) be there for some time before MS decides to change this - again...
            if (
                TryGetRegistryKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "CurrentMajorVersionNumber",
                    out var majorObj))
            {
                majorObj ??= 0;

                major = (int)majorObj;
            }

            // When the 'CurrentMajorVersionNumber' value is not present we fallback to reading the previous key used for this: 'CurrentVersion'
            else if (
                TryGetRegistryKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "CurrentVersion",
                    out var version))
            {
                version ??= string.Empty;

                var versionParts = ((string)version).Split('.');

                if (versionParts.Length >= 2)
                {
                    major = int.TryParse(versionParts[0], out var majorAsInt) ? majorAsInt : 0;
                }
            }
        }

        var minor = 0;
        {
            // The 'CurrentMinorVersionNumber' string value in the CurrentVersion key is new for Windows 10,
            // and will most likely (hopefully) be there for some time before MS decides to change this - again...
            if (
                TryGetRegistryKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "CurrentMinorVersionNumber",
                    out var minorObj))
            {
                minorObj ??= string.Empty;

                minor = (int)minorObj;
            }

            // When the 'CurrentMinorVersionNumber' value is not present we fallback to reading the previous key used for this: 'CurrentVersion'
            else if (
                TryGetRegistryKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "CurrentVersion",
                    out var version))
            {
                version ??= string.Empty;

                var versionParts = ((string)version).Split('.');

                if (versionParts.Length >= 2)
                {
                    minor = int.TryParse(versionParts[1], out var minorAsInt) ? minorAsInt : 0;
                }
            }
        }

        var build = 0;
        {
            if (
                TryGetRegistryKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "CurrentBuildNumber",
                    out var buildObj))
            {
                buildObj ??= string.Empty;

                build = int.TryParse((string)buildObj, out var buildAsInt) ? buildAsInt : 0;
            }
        }

        return new(major, minor, build);
    }

    private static bool TryGetRegistryKey(string path, string key, out object? value)
    {
        value = null;

        try
        {
            using var rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);

            if (rk == null)
            {
                return false;
            }

            value = rk.GetValue(key);

            return value != null;
        }
        catch
        {
            return false;
        }
    }

#endif
}
