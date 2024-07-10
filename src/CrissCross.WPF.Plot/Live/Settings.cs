﻿// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace CrissCross.WPF.Plot;

/// <summary>
/// EquationData.
/// </summary>
/// <seealso cref="Settings" />
/// <remarks>
/// Initializes a new instance of the <see cref="Settings" /> class.
/// </remarks>
/// <param name="itemName">itemName.</param>
public class Settings(string itemName) : RxObject
{
    /// <summary>
    /// Gets or sets :  0 - scaled and calibrated raw data, 1 - fft data, 2 - velocity rms data.
    /// </summary>
    [Reactive]
    public int DisplayedValue { get; set; }

    /// <summary>
    /// Gets or sets the name of data plot.
    /// </summary>
    [Reactive]
    public string? ItemName { get; set; } = itemName;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is checked.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is checked; otherwise, <c>false</c>.
    /// </value>
    public ReactiveCommand<Unit, Unit> IsChecked { get; set; } = ReactiveCommand.Create(() => { });

    /// <summary>
    /// Gets or sets the color text.
    /// </summary>
    [Reactive]
    public string ColorText { get; set; } = "#FFD3D3D3";

    /// <summary>
    /// Gets or sets the color text.
    /// </summary>
    [Reactive]
    public string? Icon { get; set; }
}
