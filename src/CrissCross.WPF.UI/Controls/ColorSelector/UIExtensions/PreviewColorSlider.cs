﻿// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Controls;
using System.Windows.Input;

namespace CrissCross.WPF.UI.UIExtensions;

internal abstract class PreviewColorSlider : Slider, INotifyPropertyChanged
{
    public static readonly DependencyProperty CurrentColorStateProperty =
        DependencyProperty.Register(
            nameof(CurrentColorState),
            typeof(ColorState),
            typeof(PreviewColorSlider),
            new PropertyMetadata(ColorStateChangedCallback));

    public static readonly DependencyProperty SmallChangeBindableProperty =
        DependencyProperty.Register(
            nameof(SmallChangeBindable),
            typeof(double),
            typeof(PreviewColorSlider),
            new PropertyMetadata(1.0, SmallChangeBindableChangedCallback));

    private readonly LinearGradientBrush _backgroundBrush = new();

    private SolidColorBrush _leftCapColor = new();

    private SolidColorBrush _rightCapColor = new();

    protected PreviewColorSlider()
    {
        Minimum = 0;
        Maximum = 255;
        SmallChange = 1;
        LargeChange = 10;
        MinHeight = 12;
        PreviewMouseWheel += OnPreviewMouseWheel;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public double SmallChangeBindable
    {
        get => (double)GetValue(SmallChangeBindableProperty);
        set => SetValue(SmallChangeBindableProperty, value);
    }

    public ColorState CurrentColorState
    {
        get => (ColorState)GetValue(CurrentColorStateProperty);
        set => SetValue(CurrentColorStateProperty, value);
    }

    public GradientStopCollection BackgroundGradient
    {
        get => _backgroundBrush.GradientStops;
        set => _backgroundBrush.GradientStops = value;
    }

    public SolidColorBrush LeftCapColor
    {
        get => _leftCapColor;
        set
        {
            _leftCapColor = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeftCapColor)));
        }
    }

    public SolidColorBrush RightCapColor
    {
        get => _rightCapColor;
        set
        {
            _rightCapColor = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RightCapColor)));
        }
    }

    public override void EndInit()
    {
        base.EndInit();
        Background = _backgroundBrush;
        GenerateBackground();
    }

    protected static void ColorStateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var slider = (PreviewColorSlider)d;
        slider.GenerateBackground();
    }

    protected abstract void GenerateBackground();

    private static void SmallChangeBindableChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        ((PreviewColorSlider)d).SmallChange = (double)e.NewValue;

    private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs args)
    {
        Value = MathHelper.Clamp(Value + (SmallChange * args.Delta / 120), Minimum, Maximum);
        args.Handled = true;
    }
}
