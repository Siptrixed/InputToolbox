using InputToolbox.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InputToolbox.Models
{
    internal class SimpleClicker
    {
        private static CancellationTokenSource CTS = new();
        private static bool isRunning = false;
        public static void RunClicks(int fps, WinApi.Vk key)
        {
            if (isRunning) return;
            isRunning = true;
            CancellationToken token = CTS.Token;
            new Task(() =>
            {
                while (true)
                {
                    token.WaitHandle.WaitOne(999 / fps);
                    ClickFrame(key);
                    if (token.IsCancellationRequested) break;
                }
            }).Start();
        }
        public static void RunScroll(int fps, int speed)
        {
            if (isRunning) return;
            isRunning = true;
            speed = (int)(speed * 6.5);
            CancellationToken token = CTS.Token;
            new Task(() =>
            {
                while (true)
                {
                    token.WaitHandle.WaitOne(999 / fps);
                    WinApi.MouseEvent(WinApi.MouseEventFlags.Wheel, speed);
                    if (token.IsCancellationRequested) break;
                }
            }).Start();
        }

        public static void Stop()
        {
            CTS.Cancel();
            CTS.Dispose();
            CTS = new();
            isRunning = false;
        }

        private static void ClickFrame(WinApi.Vk key)
        {
            switch (key)
            {
                case WinApi.Vk.VK_LBUTTON:
                    WinApi.MouseEvent(WinApi.MouseEventFlags.LeftDown);
                    WinApi.MouseEvent(WinApi.MouseEventFlags.LeftUp);
                    break;
                case WinApi.Vk.VK_RBUTTON:
                    WinApi.MouseEvent(WinApi.MouseEventFlags.RightDown);
                    WinApi.MouseEvent(WinApi.MouseEventFlags.RightUp);
                    break;
                case WinApi.Vk.VK_MBUTTON:
                    WinApi.MouseEvent(WinApi.MouseEventFlags.MiddleDown);
                    WinApi.MouseEvent(WinApi.MouseEventFlags.MiddleUp);
                    break;
                default:
                    WinApi.KeyBDEvent(key, WinApi.KeyBDdwFlags.KEYEVENTF_KEYDOWN);
                    WinApi.KeyBDEvent(key, WinApi.KeyBDdwFlags.KEYEVENTF_KEYUP);
                    break;
            }
        }
    }
}
