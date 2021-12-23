using InputToolbox.Import;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InputToolbox.Models
{
    internal static class SimpleClicker
    {
        private static CancellationTokenSource CTS;
        private static bool isRunning = false;
        private static PeriodicTimer Timer;

        public static bool Running { get=>isRunning; }

        public static void Run(int fps, WinApi.Vk key, int wspeed = 0)
        {
            
            if (isRunning) throw new Exception("already running");
            isRunning = true;
            
            Timer = new(new(TimeSpan.TicksPerSecond / fps));
            CTS = new();

            CancellationToken token = CTS.Token;
            new Task(async () =>
            {
                try
                {
                    while (await Timer.WaitForNextTickAsync(token))
                    {
                        Click(key, wspeed);
                    }
                }
                catch (OperationCanceledException)
                {
                    CTS.Dispose();
                    Timer.Dispose();
                    isRunning = false;
                }
            }).Start();
        }

        public static void Stop()
        {
            if(!isRunning) throw new Exception("not running");
            CTS.Cancel();
        }

        public static void Click(WinApi.Vk key, int wspeed = 0)
        {
            switch (key)
            {
                case WinApi.Vk.UNKNOWN:
                    if(wspeed != 0)
                    WinApi.MouseEvent(WinApi.MouseEventFlags.Wheel, wspeed);
                    break;
                case WinApi.Vk.LBUTTON:
                    WinApi.MouseEvent(WinApi.MouseEventFlags.LeftDown);
                    WinApi.MouseEvent(WinApi.MouseEventFlags.LeftUp);
                    break;
                case WinApi.Vk.RBUTTON:
                    WinApi.MouseEvent(WinApi.MouseEventFlags.RightDown);
                    WinApi.MouseEvent(WinApi.MouseEventFlags.RightUp);
                    break;
                case WinApi.Vk.MBUTTON:
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
