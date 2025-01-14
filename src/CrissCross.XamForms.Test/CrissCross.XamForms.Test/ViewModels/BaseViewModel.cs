﻿// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using CrissCross.XamForms.Test.Models;
using CrissCross.XamForms.Test.Services;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace CrissCross.XamForms.Test.ViewModels;

/// <summary>
/// BaseViewModel.
/// </summary>
/// <seealso cref="RxObject" />
public class BaseViewModel : RxObject
{
    /// <summary>
    /// Gets the data store.
    /// </summary>
    /// <value>
    /// The data store.
    /// </value>
    public static IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

    /// <summary>
    /// Gets or sets a value indicating whether this instance is busy.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
    /// </value>
    [Reactive]
    public bool IsBusy { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    [Reactive]
    public string? Title { get; set; }
}
