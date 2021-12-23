using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace InputToolbox.Import;

internal static class WinApi
{
    public enum GetWindow_Cmd : uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6,
        WM_GETTEXT = 0x000D,
    }

    public enum KeyBDdwFlags
    {
        KEYEVENTF_KEYDOWN = 0x0000,
        KEYEVENTF_EXTENDEDKEY = 0x0001,
        KEYEVENTF_KEYUP = 0x0002,
    }

    [Flags]
    public enum KeyModifier
    {
        None = 0x0000,
        Alt = 0x0001,
        Ctrl = 0x0002,
        NoRepeat = 0x4000,
        Shift = 0x0004,
        Win = 0x0008,
    }

    public enum MouseEventFlags
    {
        LeftDown = 0x00000002,
        LeftUp = 0x00000004,
        MiddleDown = 0x00000020,
        MiddleUp = 0x00000040,
        Move = 0x00000001,
        Wheel = 0x00000800,
        Absolute = 0x00008000,
        RightDown = 0x00000008,
        RightUp = 0x00000010,
    }

    public enum Vk : byte
    {
        UNKNOWN = 0x00, //Неизвестно
        LBUTTON = 0x01, //Левая кнопка мыши
        RBUTTON = 0x02, //Правая кнопка мыши
        CANCEL = 0x03, //Обработка Control-break
        MBUTTON = 0x04, //Средняя кнопка мыши
        XBUTTON1 = 0x05, //кнопка мыши X1
        XBUTTON2 = 0x06, //кнопка мыши X2
        BACK = 0x08, //BACKSPACE key

        //0x07	Не определено
        TAB = 0x09, //TAB key

        //0x0A-0x0B Зарезервировано
        CLEAR = 0x0C, //CLEAR key
        RETURN = 0x0D, //ENTER key

        //0x0E-0x0F Не определено
        SHIFT = 0x10, //SHIFT key
        CONTROL = 0x11, //CTRL key
        MENU = 0x12, //ALT key
        PAUSE = 0x13, //PAUSE key
        CAPITAL = 0x14, //CAPS LOCK key

        //0x15 IME modes key
        //0x16 Не определено
        //0x17-0x19 IME modes key
        //0x1A Не определено
        ESCAPE = 0x1B, //ESC key

        //0x1C-0x1F IME Keys
        SPACE = 0x20, //Пробел
        PRIOR = 0x21, //PAGE UP key
        NEXT = 0x22, //PAGE DOWN key
        END = 0x23, //END key
        HOME = 0x24, //HOME key
        LEFT = 0x25, //LEFT ARROW key
        UP = 0x26, //UP ARROW key
        RIGHT = 0x27, //RIGHT ARROW key
        DOWN = 0x28, //DOWN ARROW key
        SELECT = 0x29, //SELECT key
        PRINT = 0x2A, //PRINT key
        EXECUTE = 0x2B, //EXECUTE key
        SNAPSHOT = 0x2C, //PRINT SCREEN key for Windows 3.0 and later
        INSERT = 0x2D, //INS key
        DELETE = 0x2E, //DEL key
        HELP = 0x2F, //HELP key
        KEY0 = 0x30, //0 key
        KEY1 = 0x31, //1 key
        KEY2 = 0x32, //2 key
        KEY3 = 0x33, //3 key
        KEY4 = 0x34, //4 key
        KEY5 = 0x35, //5 key
        KEY6 = 0x36, //6 key
        KEY7 = 0x37, //7 key
        KEY8 = 0x38, //8 key
        KEY9 = 0x39, //9 key

        //0x3A-0x40 Не определено
        A = 0x41, //A key
        B = 0x42, //B key
        C = 0x43, //C key
        D = 0x44, //D key
        E = 0x45, //E key
        F = 0x46, //F key
        G = 0x47, //G key
        H = 0x48, //H key
        I = 0x49, //I key
        J = 0x4A, //J key
        K = 0x4B, //K key
        L = 0x4C, //L key
        M = 0x4D, //M key
        N = 0x4E, //N key
        O = 0x4F, //O key
        P = 0x50, //P key
        Q = 0x51, //Q key
        R = 0x52, //R key
        S = 0x53, //S key
        T = 0x54, //T key
        U = 0x55, //U key
        V = 0x56, //V key
        W = 0x57, //W key
        X = 0x58, //X key
        Y = 0x59, //Y key
        Z = 0x5A, //Z key
        LWIN = 0x5B, //Left Windows key(Microsoft Natural Keyboard)
        RWIN = 0x5C, //Right Windows key(Microsoft Natural Keyboard)
        APPS = 0x5D, //Applications key(Microsoft Natural Keyboard)

        //0x5E Зарезервировано
        SLEEP = 0x5F, //Computer Sleep key
        NUMPAD0 = 0x60, //Numeric keypad 0 key
        NUMPAD1 = 0x61, //Numeric keypad 1 key
        NUMPAD2 = 0x62, //Numeric keypad 2 key
        NUMPAD3 = 0x63, //Numeric keypad 3 key
        NUMPAD4 = 0x64, //Numeric keypad 4 key
        NUMPAD5 = 0x65, //Numeric keypad 5 key
        NUMPAD6 = 0x66, //Numeric keypad 6 key
        NUMPAD7 = 0x67, //Numeric keypad 7 key
        NUMPAD8 = 0x68, //Numeric keypad 8 key
        NUMPAD9 = 0x69, //Numeric keypad 9 key
        MULTIPLY = 0x6A, //Multiply key(*)
        ADD = 0x6B, //Add key(+)
        SEPARATOR = 0x6C, //Separator key
        SUBTRACT = 0x6D, //Subtract key(-)
        DECIMAL = 0x6E, //Decimal key
        DIVIDE = 0x6F, //Divide key(/)
        F1 = 0x70, //F1 key
        F2 = 0x71, //F2 key
        F3 = 0x72, //F3 key
        F4 = 0x73, //F4 key
        F5 = 0x74, //F5 key
        F6 = 0x75, //F6 key
        F7 = 0x76, //F7 key
        F8 = 0x77, //F8 key
        F9 = 0x78, //F9 key
        F10 = 0x79, //F10 key
        F11 = 0x7A, //F11 key
        F12 = 0x7B, //F12 key
        F13 = 0x7C, //F13 key
        F14 = 0x7D, //F14 key
        F15 = 0x7E, //F15 key
        F16 = 0x7F, //F16 key
        F17 = 0x80, //F17 key
        F18 = 0x81, //F18 key
        F19 = 0x82, //F19 key
        F20 = 0x83, //F20 key
        F21 = 0x84, //F21 key
        F22 = 0x85, //F22 key
        F23 = 0x86, //F23 key
        F24 = 0x87, //F24 key

        //0x88-0x8F Не используются
        NUMLOCK = 0x90, //NUM LOCK key
        SCROLL = 0x91, //SCROLL LOCK key

        //0x92-0x96 OEM Keys
        //0x97-0x9F Не используются
        LSHIFT = 0xA0, //Left SHIFT key
        RSHIFT = 0xA1, //Right SHIFT key
        LCONTROL = 0xA2, //Left CONTROL key
        RCONTROL = 0xA3, //Right CONTROL key
        LMENU = 0xA4, //Left MENU key
        RMENU = 0xA5, //Right MENU key

        BROWSER_BACK = 0xA6, //Browser Back key
        BROWSER_FORWARD = 0xA7, //Browser Forward key
        BROWSER_REFRESH = 0xA8, //Browser Refresh key
        BROWSER_STOP = 0xA9, //Browser Stop key
        BROWSER_SEARCH = 0xAA, //Browser Search key
        BROWSER_FAVORITES = 0xAB, //Browser Favorites key
        BROWSER_HOME = 0xAC, //Browser Start and Home key
        VOLUME_MUTE = 0xAD, //Volume Mute key
        VOLUME_DOWN = 0xAE, //Volume Down key
        VOLUME_UP = 0xAF, //Volume Up key
        MEDIA_NEXT_TRACK = 0xB0, //Next Track key
        MEDIA_PREV_TRACK = 0xB1, //Previous Track key
        MEDIA_STOP = 0xB2, //Stop Media key
        MEDIA_PLAY_PAUSE = 0xB3, //Play/Pause Media key
        LAUNCH_MAIL = 0xB4, //Start Mail key
        LAUNCH_MEDIA_SELECT = 0xB5, //Select Media key
        LAUNCH_APP1 = 0xB6, //Start Application 1 key
        LAUNCH_APP2 = 0xB7, //Start Application 2 key

        //B8-B9 Зарезервировано
        OEM_1 = 0xBA, //For the US standard keyboard, the ';:' key
        OEM_PLUS = 0xBB, //For any country/region, the '+' key
        OEM_COMMA = 0xBC, //For any country/region, the ',' key
        OEM_MINUS = 0xBD, //For any country/region, the '-' key
        OEM_PERIOD = 0xBE, //For any country/region, the '.' key
        OEM_2 = 0xBF, //For the US standard keyboard, the '/?' key
        OEM_3 = 0xC0, //For the US standard keyboard, the '`~' key

        //0xC1-0xD7 Зарезервировано
        //0xD8-0xDA Не используются
        OEM_4 = 0xDB, //For the US standard keyboard, the '[{' key
        OEM_5 = 0xDC, //For the US standard keyboard, the '\|' key
        OEM_6 = 0xDD, //For the US standard keyboard, the ']}' key
        OEM_7 = 0xDE, //For the US standard keyboard, the 'single-quote/double-quote' key
        OEM_8 = 0xDF, //Used for miscellaneous characters; it can vary by keyboard.

        //0xE0 Зарезервировано
        //0xE1 OEM specific
        OEM_102 = 0xE2, //Either the angle bracket key or the backslash key on the RT 102-key keyboard

        //0xE3-0xE4 OEM specific
        IME = 0xE5,// IME Key
        //0xE6 OEM specific
        PACKET = 0xE7, //Used to pass Unicode characters as if they were keystrokes.The PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods.For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
        //0xE8 Не используется

        OEM_RESET = 0xE9, //Only used by Nokia.
        OEM_JUMP = 0xEA, //Only used by Nokia.
        OEM_PA1 = 0xEB, //Only used by Nokia.
        OEM_PA2 = 0xEC, //Only used by Nokia.
        OEM_PA3 = 0xED, //Only used by Nokia.
        OEM_WSCTRL = 0xEE, //Only used by Nokia.
        OEM_CUSEL = 0xEF, //Only used by Nokia.
        OEM_ATTN = 0xF0, //Only used by Nokia.
        OEM_FINNISH = 0xF1, //Only used by Nokia.
        OEM_COPY = 0xF2, //Only used by Nokia.
        OEM_AUTO = 0xF3, //Only used by Nokia.
        OEM_ENLW = 0xF4, //Only used by Nokia.
        OEM_BACKTAB = 0xF5, //Only used by Nokia.

        ATTN = 0xF6, //Attn key
        CRSEL = 0xF7, //CrSel key
        EXSEL = 0xF8, //ExSel key
        EREOF = 0xF9, //Erase EOF key
        PLAY = 0xFA, //Play key
        ZOOM = 0xFB, //Zoom key
        NONAME = 0xFC, //Reserved for future use.
        PA1 = 0xFD, //PA1 key
        OEM_CLEAR = 0xFE, //Clear key
        //0xFF Мультимедийные клавиши.См.ScanCode клавиши.
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetWindow(IntPtr HWnd, GetWindow_Cmd cmd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out MousePoint lpMousePoint);

    public static MousePoint GetCursorPosition()
    {
        MousePoint currentMousePoint;
        bool gotPoint = GetCursorPos(out currentMousePoint);
        if (!gotPoint) currentMousePoint = new(0, 0);
        return currentMousePoint;
    }

    [DllImport("user32.dll")]
    private static extern int GetAsyncKeyState(int i);

    public static bool GetKeyState(int i, out int state)
    {
        state = GetAsyncKeyState(i);
        return state != 0;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowThreadProcessId([In] IntPtr hWnd, [Out] [Optional] IntPtr lpdwProcessId);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern ushort GetKeyboardLayout([In] int idThread);

    public static ushort GetKeyboardLayout() =>
        GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero));

    [DllImport("user32.dll")]
    public static extern int PeekMessage(out NativeMessage lpMsg, IntPtr window, uint wMsgFilterMin, uint wMsgFilterMax,
        uint wRemoveMsg);

    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    public static void KeyBDEvent(Vk button, KeyBDdwFlags value = 0)
    {
        keybd_event((byte)button, 0, (int)value, 0);
    }

    public static void KeyBDEvent(byte button, int value = 0)
    {
        keybd_event(button, 0, value, 0);
    }

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    public static void MouseEvent(MouseEventFlags value, int dwData = 0)
    {
        mouse_event((int)value, 0,0, dwData, 0);
    }
    public static void MouseEvent(MouseEventFlags value, MousePoint position, int dwData = 0)
    {
        mouse_event((int)value, position.X, position.Y, dwData, 0);
    }

    public static void MouseEvent(int value, int dwData = 0)
    {
        mouse_event(value, 0, 0, dwData, 0);
    }

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vlc);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc,
        int ySrc, int dwRop);

    public static void SetEnableScreen(bool enable)
    {
        if (enable)
        {
            SendMessage(0xffff, 0x0112, 0xF170, -1);
            MouseEvent(MouseEventFlags.Move);
        }
        else
        {
            SendMessage(0xffff, 0x0112, 0xF170, 2);
        }
    }

    public static class InputHook

    {
        public enum HookMessages
        {
            WM_KEYDOWN = 256,
            WM_KEYUP = 257, 
            WM_SYSKEYDOWN = 260, 
            WM_SYSKEYUP = 261,

            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
        }

        private const int WH_MOUSE_LL = 14;
        private const int WH_KEYBOARD_LL = 13;

        private static readonly LowLevelMouseProc _Mproc = MouseHookCallback;
        private static IntPtr _MhookID = IntPtr.Zero;
        private static readonly LowLevelKeyboardProc _Kproc = KeyHookCallback;
        private static IntPtr _KhookID = IntPtr.Zero;

        private static readonly Dictionary<HookMessages, MouseEventFlags> MMT = new()
        {
            {HookMessages.WM_LBUTTONDOWN, MouseEventFlags.LeftDown},
            {HookMessages.WM_LBUTTONUP, MouseEventFlags.LeftUp},
            {HookMessages.WM_MOUSEMOVE, MouseEventFlags.Move},
            {HookMessages.WM_MOUSEWHEEL, MouseEventFlags.Wheel},
            {HookMessages.WM_RBUTTONDOWN, MouseEventFlags.RightDown},
            {HookMessages.WM_RBUTTONUP, MouseEventFlags.RightUp},
            {HookMessages.WM_MBUTTONDOWN, MouseEventFlags.MiddleDown},
            {HookMessages.WM_MBUTTONUP, MouseEventFlags.MiddleUp},
        };

        public static event EventHandler<MouseEventArgs> MouseAction = delegate { };
        public static event EventHandler<KeyBDEventArgs> KeyboardAction = delegate { };

        public static void Start()
        {
            if (_MhookID == IntPtr.Zero)
                _MhookID = SetHook(_Mproc);
            if (_KhookID == IntPtr.Zero)
                _KhookID = SetHook(_Kproc);
        }

        public static void Stop()
        {
            UnhookWindowsHookEx(_MhookID);
            _MhookID = IntPtr.Zero;
            UnhookWindowsHookEx(_KhookID);
            _KhookID = IntPtr.Zero;
        }

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL,
                    proc,
                    GetModuleHandle(curModule.ModuleName),
                    0);
            }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL,
                    proc,
                    GetModuleHandle(curModule.ModuleName),
                    0);
            }
        }

        private static IntPtr MouseHookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            HookMessages MSGTP = (HookMessages)wParam;
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseAction(null,
                    new(NativeMethods.GET_WHEEL_DELTA_WPARAM(hookStruct.mouseData),
                        hookStruct.pt.x,
                        hookStruct.pt.y,
                        MSGTP));
            }

            return CallNextHookEx(_MhookID, nCode, wParam, lParam);
        }

        private static IntPtr KeyHookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            HookMessages MSGTP = (HookMessages)wParam;
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                KeyboardAction(null, 
                    new((byte)hookStruct.vkCode, 
                        MSGTP));
            }

            return CallNextHookEx(_KhookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public readonly int x;
            public readonly int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public readonly POINT pt;
            public readonly uint mouseData;
            public readonly uint flags;
            public readonly uint time;
            public readonly IntPtr dwExtraInfo;
        }

        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public ulong dwExtraInfo;
        }

        internal static class NativeMethods
        {
            internal static ushort HIWORD(IntPtr dwValue) => (ushort)(((long)dwValue >> 0x10) & 0xffff);

            internal static ushort HIWORD(uint dwValue) => (ushort)(dwValue >> 0x10);

            internal static int GET_WHEEL_DELTA_WPARAM(IntPtr wParam) => (short)HIWORD(wParam);

            internal static int GET_WHEEL_DELTA_WPARAM(uint wParam) => (short)HIWORD(wParam);
        }

        public class MouseEventArgs : EventArgs
        {
            public MouseEventFlags MsgT;
            public int Wheel, X, Y;

            public MouseEventArgs(int wheel, int x, int y, HookMessages flag)
            {
                Wheel = wheel;
                X = x;
                Y = y;
                MsgT = MMT[flag];
            }
        }

        public class KeyBDEventArgs : EventArgs
        {
            public byte Button;
            public HookMessages State;

            public KeyBDEventArgs(byte button, HookMessages state)
            {
                Button = button;
                State = state;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(MousePoint a, MousePoint b) => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(MousePoint a, MousePoint b) => a.X != b.X || a.Y != b.Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeMessage
    {
        public IntPtr handle;
        public uint msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point p;

        public override string ToString() =>
            handle + ", " + msg + ", " + wParam + ", " + lParam + ", " + time + ", " + p;
    }
}