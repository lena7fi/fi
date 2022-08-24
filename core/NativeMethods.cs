using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Core
{
    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    public class NativeMethods
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int X
            {
                get
                {
                    return Left;
                }
                set
                {
                    Right -= Left - value;
                    Left = value;
                }
            }

            public int Y
            {
                get
                {
                    return Top;
                }
                set
                {
                    Bottom -= Top - value;
                    Top = value;
                }
            }

            public int Width
            {
                get
                {
                    return Right - Left;
                }
                set
                {
                    Right = value + Left;
                }
            }

            public int Height
            {
                get
                {
                    return Bottom - Top;
                }
                set
                {
                    Bottom = value + Top;
                }
            }

            public Point Location
            {
                get
                {
                    return new Point(Left, Top);
                }
                set
                {
                    X = value.X;
                    Y = value.Y;
                }
            }

            public Size Size
            {
                get
                {
                    return new Size(Width, Height);
                }
                set
                {
                    Width = value.Width;
                    Height = value.Height;
                }
            }

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }

            public static implicit operator Rectangle(RECT r)
            {
                return new Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT rect)
                {
                    return Equals(rect);
                }

                if (obj is Rectangle rectangle)
                {
                    return Equals(new RECT(rectangle));
                }

                return false;
            }

            public override int GetHashCode()
            {
                return ((Rectangle)this).GetHashCode();
            }
        }

        public enum DwmWindowAttribute : uint
        {
            /// <summary>
            /// Use with DwmGetWindowAttribute. Discovers whether non-client rendering is enabled. The retrieved value is of type BOOL. TRUE if non-client rendering is enabled; otherwise, FALSE.
            /// </summary>
            DWMWA_NCRENDERING_ENABLED = 1,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Sets the non-client rendering policy. The pvAttribute parameter points to a value from the DWMNCRENDERINGPOLICY enumeration.
            /// </summary>
            DWMWA_NCRENDERING_POLICY,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Enables or forcibly disables DWM transitions. The pvAttribute parameter points to a value of TRUE to disable transitions or FALSE to enable transitions.
            /// </summary>
            DWMWA_TRANSITIONS_FORCEDISABLED,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Enables content rendered in the non-client area to be visible on the frame drawn by DWM. The pvAttribute parameter points to a value of TRUE to enable content rendered in the non-client area to be visible on the frame; otherwise, it points to FALSE.
            /// </summary>
            DWMWA_ALLOW_NCPAINT,
            /// <summary>
            /// Use with DwmGetWindowAttribute. Retrieves the bounds of the caption button area in the window-relative space. The retrieved value is of type RECT.
            /// </summary>
            DWMWA_CAPTION_BUTTON_BOUNDS,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Specifies whether non-client content is right-to-left (RTL) mirrored. The pvAttribute parameter points to a value of TRUE if the non-client content is right-to-left (RTL) mirrored; otherwise, it points to FALSE.
            /// </summary>
            DWMWA_NONCLIENT_RTL_LAYOUT,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Forces the window to display an iconic thumbnail or peek representation (a static bitmap), even if a live or snapshot representation of the window is available. This value normally is set during a window's creation and not changed throughout the window's lifetime. Some scenarios, however, might require the value to change over time. The pvAttribute parameter points to a value of TRUE to require a iconic thumbnail or peek representation; otherwise, it points to FALSE.
            /// </summary>
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Sets how Flip3D treats the window. The pvAttribute parameter points to a value from the DWMFLIP3DWINDOWPOLICY enumeration.
            /// </summary>
            DWMWA_FLIP3D_POLICY,
            /// <summary>
            /// Use with DwmGetWindowAttribute. Retrieves the extended frame bounds rectangle in screen space. The retrieved value is of type RECT.
            /// </summary>
            DWMWA_EXTENDED_FRAME_BOUNDS,
            /// <summary>
            /// Use with DwmSetWindowAttribute. The window will provide a bitmap for use by DWM as an iconic thumbnail or peek representation (a static bitmap) for the window. DWMWA_HAS_ICONIC_BITMAP can be specified with DWMWA_FORCE_ICONIC_REPRESENTATION. DWMWA_HAS_ICONIC_BITMAP normally is set during a window's creation and not changed throughout the window's lifetime. Some scenarios, however, might require the value to change over time. The pvAttribute parameter points to a value of TRUE to inform DWM that the window will provide an iconic thumbnail or peek representation; otherwise, it points to FALSE.
            /// </summary>
            DWMWA_HAS_ICONIC_BITMAP,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Do not show peek preview for the window. The peek view shows a full-sized preview of the window when the mouse hovers over the window's thumbnail in the taskbar. If this attribute is set, hovering the mouse pointer over the window's thumbnail dismisses peek (in case another window in the group has a peek preview showing). The pvAttribute parameter points to a value of TRUE to prevent peek functionality or FALSE to allow it.
            /// </summary>
            DWMWA_DISALLOW_PEEK,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Prevents a window from fading to a glass sheet when peek is invoked. The pvAttribute parameter points to a value of TRUE to prevent the window from fading during another window's peek or FALSE for normal behavior.
            /// </summary>
            DWMWA_EXCLUDED_FROM_PEEK,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Cloaks the window such that it is not visible to the user. The window is still composed by DWM.
            /// </summary>
            DWMWA_CLOAK,
            /// <summary>
            /// Use with DwmGetWindowAttribute.
            /// </summary>
            DWMWA_CLOAKED,
            /// <summary>
            /// Use with DwmSetWindowAttribute. Freeze the window's thumbnail image with its current visuals. Do no further live updates on the thumbnail image to match the window's contents.
            /// </summary>
            DWMWA_FREEZE_REPRESENTATION,
            /// <summary>
            /// The maximum recognized DWMWINDOWATTRIBUTE value, used for validation purposes.
            /// </summary>
            DWMWA_LAST,
            // Undocumented, available since October 2018 update (build 17763)
            DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19,
            // Windows 10 20H1 changed the value of the constant
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>Determines whether the specified window is minimized (iconic).</summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        /// <summary>Determines whether a window is maximized.</summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

       
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out bool pvAttribute, int cbAttribute);

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out int pvAttribute, int cbAttribute);

        public static Rectangle GetWindowRect(IntPtr handle)
        {
            GetWindowRect(handle, out RECT rect);
            return rect;
        }

        public static Rectangle GetWindowRectangle(IntPtr handle)
        {
            Rectangle rect = Rectangle.Empty;

            if (NativeMethods.IsDWMEnabled() && NativeMethods.GetExtendedFrameBounds(handle, out Rectangle tempRect))
            {
                rect = tempRect;
            }

            if (rect.IsEmpty)
            {
                rect = NativeMethods.GetWindowRect(handle);
            }

            if (IsWindows10OrGreater() && NativeMethods.IsZoomed(handle))
            {
                rect = NativeMethods.MaximizedWindowFix(handle, rect);
            }

            return rect;
        }

        public static bool IsWindows10OrGreater(int build = -1)
        {
            return OSVersion.Major >= 10 && OSVersion.Build >= build;
        }

        public static Rectangle MaximizedWindowFix(IntPtr handle, Rectangle windowRect)
        {
            if (GetBorderSize(handle, out Size size))
            {
                windowRect = new Rectangle(windowRect.X + size.Width, windowRect.Y + size.Height, windowRect.Width - (size.Width * 2), windowRect.Height - (size.Height * 2));
            }

            return windowRect;
        }

        public struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public static WINDOWINFO Create()
            {
                WINDOWINFO wi = new WINDOWINFO();
                wi.cbSize = (uint)Marshal.SizeOf(typeof(WINDOWINFO));
                return wi;
            }
        }

        public static bool GetBorderSize(IntPtr handle, out Size size)
        {
            WINDOWINFO wi = WINDOWINFO.Create();
            bool result = GetWindowInfo(handle, ref wi);

            if (result)
            {
                size = new Size((int)wi.cxWindowBorders, (int)wi.cyWindowBorders);
            }
            else
            {
                size = Size.Empty;
            }

            return result;
        }

        public static string GetWindowText(IntPtr handle)
        {
            if (handle.ToInt32() > 0)
            {
                try
                {
                    int length = GetWindowTextLength(handle);

                    if (length > 0)
                    {
                        StringBuilder sb = new StringBuilder(length + 1);

                        if (GetWindowText(handle, sb, sb.Capacity) > 0)
                        {
                            return sb.ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return null;
        }

        public static Process GetProcessByWindowHandle(IntPtr hwnd)
        {
            if (hwnd.ToInt32() > 0)
            {
                try
                {
                    GetWindowThreadProcessId(hwnd, out uint processID);
                    return Process.GetProcessById((int)processID);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return null;
        }

        public static bool IsActive(IntPtr handle)
        {
            return GetForegroundWindow() == handle;
        }

        public static bool GetExtendedFrameBounds(IntPtr handle, out Rectangle rectangle)
        {
            int result = DwmGetWindowAttribute(handle, (int)DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out RECT rect, Marshal.SizeOf(typeof(RECT)));
            rectangle = rect;
            return result == 0;
        }

        public static bool IsDWMEnabled()
        {
            return IsWindowsVistaOrGreater() && DwmIsCompositionEnabled();
        }

        public static readonly Version OSVersion = Environment.OSVersion.Version;

        public static bool IsWindowsVistaOrGreater()
        {
            return OSVersion.Major >= 6;
        }

        public static string GetClassName(IntPtr handle)
        {
            if (handle.ToInt32() > 0)
            {
                StringBuilder sb = new StringBuilder(256);

                if (GetClassName(handle, sb, sb.Capacity) > 0)
                {
                    return sb.ToString();
                }
            }

            return null;
        }

        public static bool IsWindowCloaked(IntPtr handle)
        {
            if (IsDWMEnabled())
            {
                int result = DwmGetWindowAttribute(handle, (int)DwmWindowAttribute.DWMWA_CLOAKED, out int cloaked, sizeof(int));
                return result == 0 && cloaked != 0;
            }


            return false;
        }
    }
}
