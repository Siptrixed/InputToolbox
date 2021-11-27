using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;

namespace InputToolbox.Import;

public class WinHotKey : IDisposable
{
    public const int WmHotKey = 0x0312;
    private static Dictionary<int, WinHotKey> _dictHotKeyToCalBackProc;

    private bool _disposed;

    // ******************************************************************
    public WinHotKey(Key k, KeyModifier keyModifiers, Action<WinHotKey> action, bool register = true)
    {
        Key = k;
        KeyModifiers = keyModifiers;
        Action = action;
        if (register) Register();
    }

    public Key Key { get; }
    public KeyModifier KeyModifiers { get; }
    public Action<WinHotKey> Action { get; }
    public int Id { get; set; }

    // ******************************************************************
    // Implement IDisposable.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vlc);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    // ******************************************************************
    public bool Register()
    {
        int virtualKeyCode = KeyInterop.VirtualKeyFromKey(Key);
        Id = virtualKeyCode + (int)KeyModifiers * 0x10000;
        bool result = RegisterHotKey(IntPtr.Zero, Id, (uint)KeyModifiers, (uint)virtualKeyCode);

        if (_dictHotKeyToCalBackProc == null)
        {
            _dictHotKeyToCalBackProc = new();
            ComponentDispatcher.ThreadFilterMessage +=
                ComponentDispatcherThreadFilterMessage;
        }

        _dictHotKeyToCalBackProc.Add(Id, this);

        //Debug.Print(result.ToString() + ", " + Id + ", " + virtualKeyCode);
        return result;
    }

    // ******************************************************************
    public bool Unregister()
    {
        WinHotKey hotKey;
        if (_dictHotKeyToCalBackProc.TryGetValue(Id, out hotKey))
        {
            _dictHotKeyToCalBackProc.Remove(Id);
            return UnregisterHotKey(IntPtr.Zero, Id);
        }

        return false;
    }

    // ******************************************************************
    private static void ComponentDispatcherThreadFilterMessage(ref MSG msg, ref bool handled)
    {
        if (!handled)
            if (msg.message == WmHotKey)
            {
                WinHotKey hotKey;

                if (_dictHotKeyToCalBackProc.TryGetValue((int)msg.wParam, out hotKey))
                {
                    if (hotKey.Action != null) hotKey.Action.Invoke(hotKey);

                    handled = true;
                }
            }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) Unregister();
            _disposed = true;
        }
    }
    // ******************************************************************
}

// ******************************************************************
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
// ******************************************************************