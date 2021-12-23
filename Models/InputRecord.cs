using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using InputToolbox.Import;

namespace InputToolbox.Models;

internal class InputRecord
{
    private static CancellationTokenSource PlayingCTS = new();
    private static readonly Stopwatch MillisM = new();
    private static bool WriteLock;

    public InputRecord()
    {
        WinApi.InputHook.KeyboardAction += KeyboardAction;
        WinApi.InputHook.MouseAction += MouseAction;
        WriteLock = false;
    }

    public List<InputAction> Actions { get; set; } = new();

    public void StartRecord()
    {
        if (MillisM.IsRunning || WriteLock) return;
        MillisM.Start();
        Actions.Clear();
        WinApi.InputHook.Start();
    }

    public void Clear()
    {
        Actions.Clear();
    }

    public static event EventHandler<EventArgs> PlayEnd = delegate { };
    public static event EventHandler<InputAction> ActionAdded = delegate { };

    public void Play()
    {
        if (MillisM.IsRunning || WriteLock) return;
        WriteLock = true;
        PlayingCTS = new();
        CancellationToken token = PlayingCTS.Token;
        new Task(() =>
        {
            foreach (InputAction ia in Actions)
            {
                ia.RunAction(token);
                if (token.IsCancellationRequested) break;
            }

            PlayEnd.Invoke(this, new());
            WriteLock = false;
        }).Start();
    }

    public void Stop()
    {
        if (MillisM.IsRunning)
        {
            WinApi.InputHook.Stop();
            MillisM.Stop();
        }
        else if (WriteLock)
        {
            PlayingCTS.Cancel();
            PlayingCTS.Dispose();
        }
    }

    private void MouseAction(object sender, WinApi.InputHook.MouseEventArgs e)
    {
        switch (e.MsgT)
        {
            case WinApi.MouseEventFlags.Move:
                if (MillisM.ElapsedMilliseconds < 20) break;//50 FPS Max
                Actions.Add(new(ActionType.MouseSet, e.X, e.Y, (int)MillisM.ElapsedMilliseconds));
                MillisM.Restart();
                ActionAdded.Invoke(this, Actions[Actions.Count - 1]);
                break;
            default:
                Actions.Add(new(ActionType.MouseSet, e.X, e.Y, 0));
                Actions.Add(new(ActionType.MouseEvent, (int)e.MsgT, e.Wheel, (int)MillisM.ElapsedMilliseconds));
                MillisM.Restart();
                ActionAdded.Invoke(this, Actions[Actions.Count - 2]);
                ActionAdded.Invoke(this, Actions[Actions.Count - 1]);
                break;
        }
        
    }

    private void KeyboardAction(object sender, WinApi.InputHook.KeyBDEventArgs e)
    {
        int Ydtm = 2;
        if (e.State == WinApi.InputHook.HookMessages.WM_KEYDOWN
            || e.State == WinApi.InputHook.HookMessages.WM_SYSKEYDOWN)
            Ydtm = 0;
        Actions.Add(new(ActionType.KeyBDEvent, e.Button, Ydtm, (int)MillisM.ElapsedMilliseconds));
        MillisM.Restart();
        ActionAdded.Invoke(this, Actions[Actions.Count - 1]);
    }
}