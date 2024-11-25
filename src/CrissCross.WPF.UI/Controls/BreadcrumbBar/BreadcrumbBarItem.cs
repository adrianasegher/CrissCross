// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using CrissCross.WPF.UI.Converters;

namespace CrissCross.WPF.UI.Controls;

/// <summary>
/// Represents an item in a <see cref="BreadcrumbBar"/> control.
/// </summary>
public class BreadcrumbBarItem : System.Windows.Controls.ContentControl
{
    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(BreadcrumbBarItem),
        new PropertyMetadata(null, null, IconSourceElementConverter.ConvertToIconElement));

    /// <summary>
    /// Property for <see cref="IconMargin"/>.
    /// </summary>
    public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register(
        nameof(IconMargin),
        typeof(Thickness),
        typeof(BreadcrumbBarItem),
        new PropertyMetadata(new Thickness(0)));

    /// <summary>
    /// Property for <see cref="IsLast"/>.
    /// </summary>
    public static readonly DependencyProperty IsLastProperty = DependencyProperty.Register(
        nameof(IsLast),
        typeof(bool),
        typeof(BreadcrumbBarItem),
        new PropertyMetadata(false));

    /// <summary>
    /// The self property.
    /// </summary>
    public static readonly DependencyProperty SelfProperty =
        DependencyProperty.Register(
            nameof(Self),
            typeof(BreadcrumbBarItem),
            typeof(BreadcrumbBarItem),
            new PropertyMetadata(null));

    /// <summary>
    /// The navigation type property.
    /// </summary>
    public static readonly DependencyProperty NavigationTypeProperty =
        DependencyProperty.Register(
            nameof(NavigationType),
            typeof(Type),
            typeof(BreadcrumbBarItem),
            new PropertyMetadata(null));

    /// <summary>
    /// Initializes a new instance of the <see cref="BreadcrumbBarItem"/> class.
    /// </summary>
    public BreadcrumbBarItem() => Self = this;

    /// <summary>
    /// Gets or sets the type of the navigation.
    /// </summary>
    /// <value>
    /// The type of the navigation.
    /// </value>
    public Type NavigationType
    {
        get => (Type)GetValue(NavigationTypeProperty);
        set => SetValue(NavigationTypeProperty, value);
    }

    /// <summary>
    /// Gets the self.
    /// </summary>
    /// <value>
    /// The self.
    /// </value>
    public BreadcrumbBarItem Self
    {
        get => (BreadcrumbBarItem)GetValue(SelfProperty);
        private set => SetValue(SelfProperty, value);
    }

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    public IconElement? Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets get or sets margin for the <see cref="Icon"/>.
    /// </summary>
    public Thickness IconMargin
    {
        get => (Thickness)GetValue(IconMarginProperty);
        set => SetValue(IconMarginProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current item is the last one.
    /// </summary>
    public bool IsLast
    {
        get => (bool)GetValue(IsLastProperty);
        set => SetValue(IsLastProperty, value);
    }
}
