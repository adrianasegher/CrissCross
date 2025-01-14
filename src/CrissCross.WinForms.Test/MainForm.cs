// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if !DESIGN
using System.Reactive.Linq;
using System.Runtime.Versioning;
using ReactiveUI;
#endif

namespace CrissCross.WinForms.Test;

/// <summary>
/// Form1.
/// </summary>
#if DESIGN
public partial class MainForm : NavigationForm
#else
public partial class MainForm : NavigationForm<MainWindowViewModel>
#endif
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    [RequiresPreviewFeatures]
    public MainForm()
    {
        InitializeComponent();
#if !DESIGN
        this.WhenSetup().Subscribe(_ =>
        {
            NavBack.Command = ReactiveCommand.Create(() => this.NavigateBack(), CanNavigateBack);
            this.NavigateToView<MainViewModel>();
        });

#endif
    }
}
