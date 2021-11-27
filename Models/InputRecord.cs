using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using InputToolbox.Import;

namespace InputToolbox.Models;

public class InputRecord
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

    public void StopRecording(int removeLast = 0)
    {
        WinApi.InputHook.Stop();
        MillisM.Stop();
        for (int i = 0; i < removeLast; i++) Actions.RemoveAt(Actions.Count - 1);
    }

    public void Clear()
    {
        Actions.Clear();
    }

    public static event EventHandler<EventArgs> PlayEnd = delegate { };

    public void Play()
    {
        if (MillisM.IsRunning || WriteLock) return;
        WriteLock = true;
        CancellationToken token = PlayingCTS.Token;
        Task PlayTask = new(() =>
        {
            foreach (InputAction ia in Actions)
            {
                ia.RunAction(token);
                if (token.IsCancellationRequested) break;
            }

            PlayEnd.Invoke(this, new());
            WriteLock = false;
        });
        PlayTask.Start();
    }

    public void Stop()
    {
        PlayingCTS.Cancel();
        PlayingCTS.Dispose();
        PlayingCTS = new();
    }

    public void Reverse()
    {
        Actions.Reverse();
    }

    private void MouseAction(object sender, WinApi.InputHook.MouseEventArgs e)
    {
        switch (e.MsgT)
        {
            case WinApi.MouseEventFlags.Move:
                Actions.Add(new(ActionType.MouseSet, e.X, e.Y, (int)MillisM.ElapsedMilliseconds));
                MillisM.Restart();
                break;
            default:
                Actions.Add(new(ActionType.MouseEvent, (int)e.MsgT, e.Wheel, (int)MillisM.ElapsedMilliseconds));
                MillisM.Restart();
                break;
        }
    }

    private void KeyboardAction(object sender, WinApi.InputHook.KeyBDEventArgs e)
    {
        if (e.State == WinApi.InputHook.HookMessages.KeyBD_BUTTONDOWN
            || e.State == WinApi.InputHook.HookMessages.KeyBD_BUTTONUP)
        {
            int Ydtm = 2;
            if (e.State == WinApi.InputHook.HookMessages.KeyBD_BUTTONDOWN)
                Ydtm = 0;
            Actions.Add(new(ActionType.KeyBDEvent, e.Button, Ydtm, (int)MillisM.ElapsedMilliseconds));
            MillisM.Restart();
        }
    }
}