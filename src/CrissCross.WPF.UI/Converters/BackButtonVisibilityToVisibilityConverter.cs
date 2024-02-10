// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

////   This file has been borrowed from Wpf-UI.

//// This Source Code Form is subject to the terms of the MIT License.
//// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
//// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
//// All Rights Reserved.

using System.Windows.Data;

namespace CrissCross.WPF.UI.Converters;

internal class BackButtonVisibilityToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not NavigationViewBackButtonVisible backButtonVisibility)
        {
            return Visibility.Collapsed;
        }

        return backButtonVisibility switch
        {
            NavigationViewBackButtonVisible.Collapsed => Visibility.Collapsed,
            _ => (object)Visibility.Visible,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}
