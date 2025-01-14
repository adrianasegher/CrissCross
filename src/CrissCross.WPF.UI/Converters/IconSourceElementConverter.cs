// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Data;

namespace CrissCross.WPF.UI.Converters;

/// <summary>
/// Converts an <see cref="IconSourceElement"/> to an <see cref="IconElement"/>.
/// </summary>
public class IconSourceElementConverter : IValueConverter
{
    /// <summary>
    /// Converts a value to an <see cref="IconElement" />.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <param name="baseValue">The base value to convert.</param>
    /// <returns>
    /// The converted IconElement.
    /// </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Unused")]
    public static object ConvertToIconElement(DependencyObject e, object baseValue) => ConvertToIconElement(baseValue);

    /// <summary>
    /// Converts a value to an <see cref="IconElement"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>The converted <see cref="IconElement"/>.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertToIconElement(value);

    /// <summary>
    /// Converts an <see cref="IconElement"/> back to an IconSourceElement.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>The converted IconSourceElement.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;

    private static object ConvertToIconElement(object value)
    {
        if (value is not IconSourceElement iconSourceElement)
        {
            return value;
        }

        if (iconSourceElement.IconSource is null)
        {
            throw new ArgumentException(nameof(iconSourceElement.IconSource));
        }

        return iconSourceElement.IconSource.CreateIconElement();
    }
}
