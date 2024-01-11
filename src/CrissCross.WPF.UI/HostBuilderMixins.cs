﻿// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Windows;
using System.Windows.Controls;
using CP.Extensions.Hosting.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Ui;

namespace CrissCross.WPF.UI
{
    /// <summary>
    /// HostBuilderMixins.
    /// </summary>
    public static class HostBuilderMixins
    {
        /// <summary>
        /// Configures CrissSCross for Page Navigation.
        /// </summary>
        /// <typeparam name="TWindow">The type of the window.</typeparam>
        /// <typeparam name="TPage">The type of the page.</typeparam>
        /// <param name="hostBuilder">The host builder.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder ConfigureCrissCrossForPageNavigation<TWindow, TPage>(this IHostBuilder hostBuilder)
            where TWindow : Window, INavigationWindow
            where TPage : Page => hostBuilder
            .ConfigureAppConfiguration(c => c.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory)!))
            .ConfigureServices(
            services =>
                services.AddHostedService<ApplicationHostService<TWindow, TPage>>() // App Host
                .AddSingleton<IPageService, PageService>() // Page resolver service
                .AddSingleton<IThemeService, ThemeService>() // Theme manipulation
                .AddSingleton<ITaskBarService, TaskBarService>() // TaskBar manipulation
                .AddSingleton<INavigationService, NavigationService>() // Service containing navigation, same as INavigationWindow... but without window
                .AddSingleton<INavigationWindow, TWindow>());

        /// <summary>
        /// Configures the criss cross for view model navigation.
        /// </summary>
        /// <typeparam name="TWindow">The type of the window.</typeparam>
        /// <typeparam name="TViewModel">The type of the ViewModel.</typeparam>
        /// <param name="hostBuilder">The host builder.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder ConfigureCrissCrossForViewModelNavigation<TWindow, TViewModel>(this IHostBuilder hostBuilder)
            where TWindow : NavigationWindow
            where TViewModel : class, IRxObject, new() => hostBuilder
            .ConfigureAppConfiguration(c => c.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory)!))
            .ConfigureServices(
            services =>
                services.AddHostedService<ApplicationVMHostService<TWindow, TViewModel>>() // App Host
                .AddSingleton<IThemeService, ThemeService>() // Theme manipulation
                .AddSingleton<ITaskBarService, TaskBarService>() // TaskBar manipulation
                .AddSingleton<NavigationWindow, TWindow>());
    }
}
