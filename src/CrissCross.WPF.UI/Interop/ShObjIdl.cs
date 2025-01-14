// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

// <auto-generated>

#nullable enable

using System.Runtime.InteropServices;

namespace CrissCross.WPF.UI.Interop;

/// <summary>
/// Exposes methods that enumerate the contents of a view and receive notification from callback upon enumeration completion.
/// </summary>
internal static class ShObjIdl
{
    /// <summary>
    /// THUMBBUTTON flags.  THBF_*
    /// </summary>
    [Flags]
    public enum THUMBBUTTONFLAGS
    {
        THBF_ENABLED = 0,
        THBF_DISABLED = 0x1,
        THBF_DISMISSONCLICK = 0x2,
        THBF_NOBACKGROUND = 0x4,
        THBF_HIDDEN = 0x8,
        THBF_NONINTERACTIVE = 0x10
    }

    /// <summary>
    /// THUMBBUTTON mask.  THB_*
    /// </summary>
    [Flags]
    public enum THUMBBUTTONMASK
    {
        THB_BITMAP = 0x1,
        THB_ICON = 0x2,
        THB_TOOLTIP = 0x4,
        THB_FLAGS = 0x8
    }

    /// <summary>
    /// TBPF_*
    /// </summary>
    [Flags]
    public enum TBPFLAG
    {
        TBPF_NOPROGRESS = 0,
        TBPF_INDETERMINATE = 0x1,
        TBPF_NORMAL = 0x2,
        TBPF_ERROR = 0x4,
        TBPF_PAUSED = 0x8
    }

    /// <summary>
    /// STPF_*
    /// </summary>
    [Flags]
    public enum STPFLAG
    {
        STPF_NONE = 0,
        STPF_USEAPPTHUMBNAILALWAYS = 0x1,
        STPF_USEAPPTHUMBNAILWHENACTIVE = 0x2,
        STPF_USEAPPPEEKALWAYS = 0x4,
        STPF_USEAPPPEEKWHENACTIVE = 0x8
    }

    /// <summary>
    /// EBO_*
    /// </summary>
    public enum EXPLORER_BROWSER_OPTIONS
    {
        EBO_NONE = 0,
        EBO_NAVIGATEONCE = 0x1,
        EBO_SHOWFRAMES = 0x2,
        EBO_ALWAYSNAVIGATE = 0x4,
        EBO_NOTRAVELLOG = 0x8,
        EBO_NOWRAPPERWINDOW = 0x10,
        EBO_HTMLSHAREPOINTVIEW = 0x20,
        EBO_NOBORDER = 0x40,
        EBO_NOPERSISTVIEWSTATE = 0x80
    }

    /// <summary>
    /// EBF_*
    /// </summary>
    public enum EXPLORER_BROWSER_FILL_FLAGS
    {
        EBF_NONE = 0,
        EBF_SELECTFROMDATAOBJECT = 0x100,
        EBF_NODROPTARGET = 0x200
    }

    /// <summary>
    /// THUMBBUTTON
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
    public struct THUMBBUTTON
    {
        /// <summary>
        /// WPARAM value for a THUMBBUTTON being clicked.
        /// </summary>
        public const int THBN_CLICKED = 0x1800;

        public THUMBBUTTONMASK dwMask;
        public uint iId;
        public uint iBitmap;
        public IntPtr hIcon;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szTip;

        public THUMBBUTTONFLAGS dwFlags;
    }

    /// <summary>
    /// Class DECLSPEC_UUID("56FDF344-FD6D-11d0-958A-006097C9A090")
    /// </summary>
    [Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComImport]
    public class CTaskbarList { }

    /// <summary>
    /// Class DECLSPEC_UUID("9ac9fbe1-e0a2-4ad6-b4ee-e212013ea917")
    /// </summary>
    [Guid("9ac9fbe1-e0a2-4ad6-b4ee-e212013ea917")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComImport]
    public class ShellItem { }

    /// <summary>
    /// MIDL_INTERFACE("c43dc798-95d1-4bea-9030-bb99e2983a1a")
    /// ITaskbarList4 : public ITaskbarList3
    /// </summary>
    [ComImport]
    [Guid("c43dc798-95d1-4bea-9030-bb99e2983a1a")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITaskbarList4
    {
        /// <summary>
        /// Hrs the initialize.
        /// </summary>
        [PreserveSig]
        void HrInit();

        /// <summary>
        /// Adds the tab.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        [PreserveSig]
        void AddTab(IntPtr hwnd);

        /// <summary>
        /// Deletes the tab.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        [PreserveSig]
        void DeleteTab(IntPtr hwnd);

        /// <summary>
        /// Activates the tab.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        [PreserveSig]
        void ActivateTab(IntPtr hwnd);

        /// <summary>
        /// Sets the active alt.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        [PreserveSig]
        void SetActiveAlt(IntPtr hwnd);

        /// <summary>
        /// Marks the fullscreen window.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="fFullscreen">if set to <c>true</c> [f fullscreen].</param>
        [PreserveSig]
        void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        /// <summary>
        /// Sets the progress value.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="ullCompleted">The ull completed.</param>
        /// <param name="ullTotal">The ull total.</param>
        [PreserveSig]
        void SetProgressValue(IntPtr hwnd, UInt64 ullCompleted, UInt64 ullTotal);

        /// <summary>
        /// Sets the state of the progress.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="tbpFlags">The TBP flags.</param>
        [PreserveSig]
        void SetProgressState(IntPtr hwnd, TBPFLAG tbpFlags);

        /// <summary>
        /// Registers the tab.
        /// </summary>
        /// <param name="hwndTab">The HWND tab.</param>
        /// <param name="hwndMDI">The HWND MDI.</param>
        [PreserveSig]
        void RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

        /// <summary>
        /// Unregisters the tab.
        /// </summary>
        /// <param name="hwndTab">The HWND tab.</param>
        [PreserveSig]
        void UnregisterTab(IntPtr hwndTab);

        /// <summary>
        /// Sets the tab order.
        /// </summary>
        /// <param name="hwndTab">The HWND tab.</param>
        /// <param name="hwndInsertBefore">The HWND insert before.</param>
        [PreserveSig]
        void SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

        /// <summary>
        /// Sets the tab active.
        /// </summary>
        /// <param name="hwndTab">The HWND tab.</param>
        /// <param name="hwndInsertBefore">The HWND insert before.</param>
        /// <param name="dwReserved">The dw reserved.</param>
        [PreserveSig]
        void SetTabActive(IntPtr hwndTab, IntPtr hwndInsertBefore, uint dwReserved);

        /// <summary>
        /// Thumbs the bar add buttons.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="cButtons">The c buttons.</param>
        /// <param name="pButtons">The p buttons.</param>
        /// <returns>
        /// HRESULT
        /// </returns>
        [PreserveSig]
        int ThumbBarAddButtons(
            IntPtr hwnd,
            uint cButtons,
            [MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] pButtons
        );

        /// <summary>
        /// Thumbs the bar update buttons.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="cButtons">The c buttons.</param>
        /// <param name="pButtons">The p buttons.</param>
        /// <returns>
        /// HRESULT
        /// </returns>
        [PreserveSig]
        int ThumbBarUpdateButtons(
            IntPtr hwnd,
            uint cButtons,
            [MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] pButtons
        );

        /// <summary>
        /// Thumbs the bar set image list.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="himl">The himl.</param>
        [PreserveSig]
        void ThumbBarSetImageList(IntPtr hWnd, IntPtr himl);

        /// <summary>
        /// Sets the overlay icon.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="hIcon">The h icon.</param>
        /// <param name="pszDescription">The PSZ description.</param>
        [PreserveSig]
        void SetOverlayIcon(
            IntPtr hwnd,
            IntPtr hIcon,
            [MarshalAs(UnmanagedType.LPWStr)] string pszDescription
        );

        /// <summary>
        /// Sets the thumbnail tooltip.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="pszTip">The PSZ tip.</param>
        [PreserveSig]
        void SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

        /// <summary>
        /// Sets the thumbnail clip.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="prcClip">The PRC clip.</param>
        [PreserveSig]
        void SetThumbnailClip(IntPtr hwnd, IntPtr prcClip);

        /// <summary>
        /// Sets the tab properties.
        /// </summary>
        /// <param name="hwndTab">The HWND tab.</param>
        /// <param name="stpFlags">The STP flags.</param>
        void SetTabProperties(IntPtr hwndTab, STPFLAG stpFlags);
    }
}
