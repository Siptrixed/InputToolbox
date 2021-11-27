using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using MessagePack;

namespace InputToolbox.Classes
{
    public class InputRecord
    {
        public List<InputAction> Actions = new List<InputAction>();

        private static CancellationTokenSource PlayingCTS = new CancellationTokenSource();
        private static Stopwatch MillisM = new Stopwatch();
        private static bool WriteLock = false;
        public InputRecord()
        {
            WinApi.InputHook.KeyboardAction += KeyboardAction;
            WinApi.InputHook.MouseAction += MouseAction;
            WriteLock = false;
        }
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
            for (int i = 0;i < removeLast;i++)
            {
                Actions.RemoveAt(Actions.Count - 1);
            }
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
            Task PlayTask = new Task(() =>
            {
                foreach (InputAction ia in Actions)
                {
                    ia.RunAction(token);
                    if (token.IsCancellationRequested) break;
                }
                PlayEnd.Invoke(this,new EventArgs());
                WriteLock = false;
            });
            PlayTask.Start();
        }
        public void Stop()
        {
            PlayingCTS.Cancel();
            PlayingCTS.Dispose();
            PlayingCTS = new CancellationTokenSource();
        }
        public void Reverse()
        {
            Actions.Reverse();
        }

        public async void Save(string filename)
        {
            await using FileStream fs = new(filename, FileMode.OpenOrCreate);
            await MessagePackSerializer.SerializeAsync(fs, Actions, MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray));
        }
        public static async Task<InputRecord> Load(string filename)
        {
            if (!File.Exists(filename)) return new InputRecord();
            try
            {
                await using FileStream fs = new FileStream(filename, FileMode.Open);
                return new InputRecord()
                {
                    Actions = await MessagePackSerializer.DeserializeAsync<List<InputAction>>(fs, MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray))
                };
            }
            catch (MessagePackSerializationException)
            {
                return new InputRecord();
            }
        }


        private void MouseAction(object sender, WinApi.InputHook.MouseEventArgs e)
        {
            switch (e.MsgT)
            {
                case WinApi.MouseEventFlags.Move:
                    Actions.Add(new InputAction(ActionType.MouseSet, e.X, e.Y, (int)MillisM.ElapsedMilliseconds));
                    MillisM.Restart(); 
                    break;
                default:
                    Actions.Add(new InputAction(ActionType.MouseEvent, (int)e.MsgT, e.Wheel, (int)MillisM.ElapsedMilliseconds));
                    MillisM.Restart();
                    break;

            }
        }
        private void KeyboardAction(object sender, WinApi.InputHook.KeyBDEventArgs e)
        {
            if (e.State == WinApi.InputHook.HookMessages.KeyBD_BUTTONDOWN ||
                e.State == WinApi.InputHook.HookMessages.KeyBD_BUTTONUP)
            {
                int Ydtm = 2;
                if (e.State == WinApi.InputHook.HookMessages.KeyBD_BUTTONDOWN)
                    Ydtm = 0;
                Actions.Add(new InputAction(ActionType.KeyBDEvent, e.Button, Ydtm, (int)MillisM.ElapsedMilliseconds));
                MillisM.Restart();
            }
        }
    }

    [MessagePackObject]
    public class InputAction
    {
        [Key(3)]
        public int Delay { get; set; }
        [Key(0)]
        public ActionType Type { get; set; }
        [Key(1)]
        public int Xparam { get; set; }
        [Key(2)]
        public int Yparam { get; set; }
        [MessagePack.SerializationConstructor]
        public InputAction(ActionType Type, int Xparam, int Yparam = 0, int Delay = 0)
        {
            this.Delay = Delay;
            this.Type = Type;
            this.Xparam = Xparam;
            this.Yparam = Yparam;
        }
        public void RunAction(CancellationToken ct)
        {
            ct.WaitHandle.WaitOne(Delay);
            if (ct.IsCancellationRequested) return;
            switch (Type)
            {
                case ActionType.KeyBDEvent:
                    WinApi.KeyBDEvent((byte)Xparam, Yparam);
                    break;
                case ActionType.MouseEvent:
                    WinApi.MouseEvent(Xparam, Yparam);
                    break;
                case ActionType.MouseSet:
                    WinApi.SetCursorPos(Xparam, Yparam);
                    break;
            }
        }
    }
    public enum ActionType
    {
        KeyBDEvent,
        MouseEvent,
        MouseSet
    }
}
