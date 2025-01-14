// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Window = System.Windows.Window;

namespace CrissCross.WPF.UI.Hardware;

/// <summary>
/// Provides access to various DPI-related methods.
/// </summary>
internal static class DpiHelper
{
    /// <summary>
    /// Default DPI value.
    /// </summary>
    internal const int DefaultDpi = 96;

    [ThreadStatic]
    private static Matrix _transformToDevice;

    [ThreadStatic]
    private static Matrix _transformToDip;

    /// <summary>
    /// Gets DPI of the selected <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window that you want to get information about.</param>
    public static DisplayDpi GetWindowDpi(Window? window)
    {
        if (window is null)
        {
            return new DisplayDpi(DefaultDpi, DefaultDpi);
        }

        return GetWindowDpi(new WindowInteropHelper(window).Handle);
    }

    /// <summary>
    /// Gets DPI of the selected <see cref="Window"/> based on it's handle.
    /// </summary>
    /// <param name="windowHandle">Handle of the window that you want to get information about.</param>
    public static DisplayDpi GetWindowDpi(IntPtr windowHandle)
    {
        if (windowHandle == IntPtr.Zero || !UnsafeNativeMethods.IsValidWindow(windowHandle))
        {
            return new DisplayDpi(DefaultDpi, DefaultDpi);
        }

        var windowDpi = (int)User32.GetDpiForWindow(windowHandle);

        return new DisplayDpi(windowDpi, windowDpi);
    }

    /// <summary>
    /// Gets the DPI values from <see cref="SystemParameters"/>.
    /// </summary>
    /// <returns>The DPI values from <see cref="SystemParameters"/>. If the property cannot be accessed, the default value <see langword="96"/> is returned.</returns>
    public static DisplayDpi GetSystemDpi()
    {
        var dpiXProperty = typeof(SystemParameters).GetProperty(
            "DpiX",
            BindingFlags.NonPublic | BindingFlags.Static);

        if (dpiXProperty == null)
        {
            return new DisplayDpi(DefaultDpi, DefaultDpi);
        }

        var dpiYProperty = typeof(SystemParameters).GetProperty(
            "Dpi",
            BindingFlags.NonPublic | BindingFlags.Static);

        if (dpiYProperty == null)
        {
            return new DisplayDpi(DefaultDpi, DefaultDpi);
        }

        return new DisplayDpi(
            (int)dpiXProperty.GetValue(null, null)!,
            (int)dpiYProperty.GetValue(null, null)!);
    }

    /// <summary>
    /// Convert a point in device independent pixels (1/96") to a point in the system coordinates.
    /// </summary>
    /// <param name="logicalPoint">A point in the logical coordinate system.</param>
    /// <param name="dpiScaleX">Horizontal DPI scale.</param>
    /// <param name="dpiScaleY">Vertical DPI scale.</param>
    /// <returns>Returns the parameter converted to the system's coordinates.</returns>
    public static Point LogicalPixelsToDevice(Point logicalPoint, double dpiScaleX, double dpiScaleY)
    {
        _transformToDevice = Matrix.Identity;
        _transformToDevice.Scale(dpiScaleX, dpiScaleY);

        return _transformToDevice.Transform(logicalPoint);
    }

    /// <summary>
    /// Convert a point in system coordinates to a point in device independent pixels (1/96").
    /// </summary>
    /// <returns>Returns the parameter converted to the device independent coordinate system.</returns>
    public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
    {
        _transformToDip = Matrix.Identity;
        _transformToDip.Scale(1d / dpiScaleX, 1d / dpiScaleY);

        return _transformToDip.Transform(devicePoint);
    }

    public static Rect LogicalRectToDevice(Rect logicalRectangle, double dpiScaleX, double dpiScaleY)
    {
        var topLeft = LogicalPixelsToDevice(
            new Point(logicalRectangle.Left, logicalRectangle.Top),
            dpiScaleX,
            dpiScaleY);
        var bottomRight = LogicalPixelsToDevice(
            new Point(logicalRectangle.Right, logicalRectangle.Bottom),
            dpiScaleX,
            dpiScaleY);

        return new Rect(topLeft, bottomRight);
    }

    public static Rect DeviceRectToLogical(Rect deviceRectangle, double dpiScaleX, double dpiScaleY)
    {
        var topLeft = DevicePixelsToLogical(
            new Point(deviceRectangle.Left, deviceRectangle.Top),
            dpiScaleX,
            dpiScaleY);
        var bottomRight = DevicePixelsToLogical(
            new Point(deviceRectangle.Right, deviceRectangle.Bottom),
            dpiScaleX,
            dpiScaleY);

        return new Rect(topLeft, bottomRight);
    }

    public static Size LogicalSizeToDevice(Size logicalSize, double dpiScaleX, double dpiScaleY)
    {
        var pt = LogicalPixelsToDevice(
            new Point(logicalSize.Width, logicalSize.Height),
            dpiScaleX,
            dpiScaleY);

        return new Size { Width = pt.X, Height = pt.Y };
    }

    public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
    {
        var pt = DevicePixelsToLogical(
            new Point(deviceSize.Width, deviceSize.Height),
            dpiScaleX,
            dpiScaleY);

        return new Size(pt.X, pt.Y);
    }

    public static Thickness LogicalThicknessToDevice(
        Thickness logicalThickness,
        double dpiScaleX,
        double dpiScaleY)
    {
        var topLeft = LogicalPixelsToDevice(
            new Point(logicalThickness.Left, logicalThickness.Top),
            dpiScaleX,
            dpiScaleY);
        var bottomRight = LogicalPixelsToDevice(
            new Point(logicalThickness.Right, logicalThickness.Bottom),
            dpiScaleX,
            dpiScaleY);

        return new Thickness(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
    }
}
