using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Interop;
using System.Windows;
using System;

namespace Design.CustomColors
{
    #region WindowSettings

    enum MonitorOptions : uint
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        public RECT rcMonitor = new RECT();
        public RECT rcWork = new RECT();
        public int dwFlags = 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }

    #endregion

    public static class Blur
    {
        #region Library

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute
        (
            IntPtr hwnd,
            ref WindowCompositionAttributeData data
        );

        #endregion
        #region Methods

        #region Enable

        public static void Enable(Window window)
        {
            var windowHelper = new WindowInteropHelper(window);
            BlurForWindow(windowHelper.Handle);
        }
        private static void BlurForWindow(IntPtr hwnd)
        {
            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(hwnd, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        public static void Enable(this UIElement element, double blurRadius, TimeSpan duration, TimeSpan beginTime)
        {
            BlurEffect blur = new BlurEffect()
            {
                Radius = 0
            };
            DoubleAnimation blurEnable = new DoubleAnimation(0, blurRadius, duration)
            {
                BeginTime = beginTime
            };
            element.Effect = blur;
            blur.BeginAnimation(BlurEffect.RadiusProperty, blurEnable);
        }

        #endregion
        #region Disable

        public static void Disable(this UIElement element, TimeSpan duration, TimeSpan beginTime)
        {
            BlurEffect blur = element.Effect as BlurEffect;
            if (blur is null || blur.Radius == 0)
            {
                return;
            }
            DoubleAnimation blurDisable = new DoubleAnimation(blur.Radius, 0, duration) { BeginTime = beginTime };
            blur.BeginAnimation(BlurEffect.RadiusProperty, blurDisable);
        }

        #endregion

        #endregion
    }
}