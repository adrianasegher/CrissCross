// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace CrissCross.WPF.UI.Appearance;

/// <summary>
/// Allows to manage the application theme by swapping resource dictionaries containing dynamic resources with color information.
/// </summary>
/// <example>
/// <code lang="csharp">
/// ApplicationThemeManager.Apply(
///     ApplicationTheme.Light
/// );
/// </code>
/// <code lang="csharp">
/// if (ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark)
/// {
///     ApplicationThemeManager.Apply(
///         ApplicationTheme.Light
///     );
/// }
/// </code>
/// <code>
/// ApplicationThemeManager.Changed += (theme, accent) =>
/// {
///     Debug.WriteLine($"Application theme changed to {theme.ToString()}");
/// };
/// </code>
/// </example>
public static class ApplicationThemeManager
{
    internal const string LibraryNamespace = "crisscross.wpf.ui;";

    internal const string ThemesDictionaryPath = "pack://application:,,,/CrissCross.WPF.UI;component/Resources/Theme/";
    private static ApplicationTheme _cachedApplicationTheme = ApplicationTheme.Unknown;

    /// <summary>
    /// Event triggered when the application's theme is changed.
    /// </summary>
    public static event ThemeChangedEvent? Changed;

    /// <summary>
    /// Gets a value that indicates whether the application is currently using the high contrast theme.
    /// </summary>
    /// <returns><see langword="true"/> if application uses high contrast theme.</returns>
    public static bool IsHighContrast() => _cachedApplicationTheme == ApplicationTheme.HighContrast;

    /// <summary>
    /// Gets a value that indicates whether the Windows is currently using the high contrast theme.
    /// </summary>
    /// <returns><see langword="true"/> if system uses high contrast theme.</returns>
    public static bool IsSystemHighContrast() => SystemThemeManager.HighContrast;

    /// <summary>
    /// Changes the current application theme.
    /// </summary>
    /// <param name="applicationTheme">Theme to set.</param>
    /// <param name="backgroundEffect">Whether the custom background effect should be applied.</param>
    /// <param name="updateAccent">Whether the color accents should be changed.</param>
    public static void Apply(
        ApplicationTheme applicationTheme,
        WindowBackdropType backgroundEffect = WindowBackdropType.Mica,
        bool updateAccent = true)
    {
        if (updateAccent)
        {
            ApplicationAccentColorManager.Apply(
                ApplicationAccentColorManager.GetColorizationColor(),
                applicationTheme,
                false);
        }

        if (applicationTheme == ApplicationTheme.Unknown)
        {
            return;
        }

        var appDictionaries = new ResourceDictionaryManager(LibraryNamespace);

        var themeDictionaryName = "Light";

        switch (applicationTheme)
        {
            case ApplicationTheme.Dark:
                themeDictionaryName = "Dark";
                break;
            case ApplicationTheme.HighContrast:
                themeDictionaryName = GetSystemTheme() switch
                {
                    SystemTheme.HC1 => "HC1",
                    SystemTheme.HC2 => "HC2",
                    SystemTheme.HCBlack => "HCBlack",
                    SystemTheme.HCWhite => "HCWhite",
                    _ => "HCWhite",
                };
                break;
        }

        var isUpdated = appDictionaries.UpdateDictionary(
            "theme",
            new Uri(ThemesDictionaryPath + themeDictionaryName + ".xaml", UriKind.Absolute));

#if DEBUG
        System.Diagnostics.Debug.WriteLine(
            $"INFO | {typeof(ApplicationThemeManager)} tries to update theme to {themeDictionaryName} ({applicationTheme}): {isUpdated}",
            nameof(ApplicationThemeManager));
#endif
        if (!isUpdated)
        {
            return;
        }

        SystemThemeManager.UpdateSystemThemeCache();

        _cachedApplicationTheme = applicationTheme;

        Changed?.Invoke(applicationTheme, ApplicationAccentColorManager.SystemAccent);

        if (UiApplication.Current.MainWindow is System.Windows.Window mainWindow)
        {
            WindowBackgroundManager.UpdateBackground(
                mainWindow,
                applicationTheme,
                backgroundEffect);
        }
    }

    /// <summary>
    /// Applies Resources in the <paramref name="frameworkElement" />.
    /// </summary>
    /// <param name="frameworkElement">The framework element.</param>
    public static void Apply(FrameworkElement frameworkElement)
    {
        if (frameworkElement is null)
        {
            return;
        }

        var resourcesRemove = frameworkElement
            .Resources.MergedDictionaries.Where(e => e.Source?.ToString().ToLower().Contains(LibraryNamespace) == true)
            .ToArray();

        foreach (var resource in UiApplication.Current.Resources.MergedDictionaries)
        {
            System.Diagnostics.Debug.WriteLine(
                $"INFO | {typeof(ApplicationThemeManager)} Add {resource.Source}",
                "CrissCross.WPF.UI.Appearance");
            frameworkElement.Resources.MergedDictionaries.Add(resource);
        }

        foreach (var resource in resourcesRemove)
        {
            System.Diagnostics.Debug.WriteLine(
                $"INFO | {typeof(ApplicationThemeManager)} Remove {resource.Source}",
                "CrissCross.WPF.UI.Appearance");
            frameworkElement.Resources.MergedDictionaries.Remove(resource);
        }

        foreach (System.Collections.DictionaryEntry resource in UiApplication.Current.Resources)
        {
            System.Diagnostics.Debug.WriteLine(
                $"INFO | {typeof(ApplicationThemeManager)} Copy Resource {resource.Key} - {resource.Value}",
                "CrissCross.WPF.UI.Appearance");
            frameworkElement.Resources[resource.Key] = resource.Value;
        }
    }

    /// <summary>
    /// Applies the system theme.
    /// </summary>
    public static void ApplySystemTheme() => ApplySystemTheme(true);

    /// <summary>
    /// Applies the system theme.
    /// </summary>
    /// <param name="updateAccent">if set to <c>true</c> [update accent].</param>
    public static void ApplySystemTheme(bool updateAccent)
    {
        SystemThemeManager.UpdateSystemThemeCache();

        var systemTheme = GetSystemTheme();

        var themeToSet = ApplicationTheme.Light;

        if (systemTheme is SystemTheme.Dark or SystemTheme.CapturedMotion or SystemTheme.Glow)
        {
            themeToSet = ApplicationTheme.Dark;
        }
        else if (
            systemTheme is SystemTheme.HC1 or SystemTheme.HC2 or SystemTheme.HCBlack or SystemTheme.HCWhite)
        {
            themeToSet = ApplicationTheme.HighContrast;
        }

        Apply(themeToSet, updateAccent: updateAccent);
    }

    /// <summary>
    /// Gets currently set application theme.
    /// </summary>
    /// <returns><see cref="ApplicationTheme.Unknown"/> if something goes wrong.</returns>
    public static ApplicationTheme GetAppTheme()
    {
        if (_cachedApplicationTheme == ApplicationTheme.Unknown)
        {
            FetchApplicationTheme();
        }

        return _cachedApplicationTheme;
    }

    /// <summary>
    /// Gets currently set system theme.
    /// </summary>
    /// <returns><see cref="SystemTheme.Unknown"/> if something goes wrong.</returns>
    public static SystemTheme GetSystemTheme() => SystemThemeManager.GetCachedSystemTheme();

    /// <summary>
    /// Gets a value that indicates whether the application is matching the system theme.
    /// </summary>
    /// <returns><see langword="true"/> if the application has the same theme as the system.</returns>
    public static bool IsAppMatchesSystem()
    {
        var appApplicationTheme = GetAppTheme();
        var sysTheme = GetSystemTheme();

        return appApplicationTheme switch
        {
            ApplicationTheme.Dark
                => sysTheme is SystemTheme.Dark or SystemTheme.CapturedMotion or SystemTheme.Glow,
            ApplicationTheme.Light
                => sysTheme is SystemTheme.Light or SystemTheme.Flow or SystemTheme.Sunrise,
            _ => appApplicationTheme == ApplicationTheme.HighContrast && SystemThemeManager.HighContrast
        };
    }

    /// <summary>
    /// Checks if the application and the operating system are currently working in a dark theme.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is matched dark]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMatchedDark()
    {
        var appApplicationTheme = GetAppTheme();
        var sysTheme = GetSystemTheme();

        if (appApplicationTheme != ApplicationTheme.Dark)
        {
            return false;
        }

        return sysTheme is SystemTheme.Dark or SystemTheme.CapturedMotion or SystemTheme.Glow;
    }

    /// <summary>
    /// Checks if the application and the operating system are currently working in a light theme.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is matched light]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMatchedLight()
    {
        var appApplicationTheme = GetAppTheme();
        var sysTheme = GetSystemTheme();

        if (appApplicationTheme != ApplicationTheme.Light)
        {
            return false;
        }

        return sysTheme is SystemTheme.Light or SystemTheme.Flow or SystemTheme.Sunrise;
    }

    /// <summary>
    /// Tries to guess the currently set application theme.
    /// </summary>
    private static void FetchApplicationTheme()
    {
        ResourceDictionaryManager appDictionaries = new(LibraryNamespace);
        var themeDictionary = appDictionaries.GetDictionary("theme");

        if (themeDictionary == null)
        {
            return;
        }

        var themeUri = themeDictionary.Source.ToString().Trim().ToLower();

        if (themeUri.Contains("light"))
        {
            _cachedApplicationTheme = ApplicationTheme.Light;
        }

        if (themeUri.Contains("dark"))
        {
            _cachedApplicationTheme = ApplicationTheme.Dark;
        }

        if (themeUri.Contains("highcontrast"))
        {
            _cachedApplicationTheme = ApplicationTheme.HighContrast;
        }
    }
}
