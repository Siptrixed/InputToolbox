using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace InputToolbox.Classes
{
    [DataContract(Name = "Recording")]
    public class InputRecord
    {
        [DataMember(Name = "ActionList")]
        public List<InputAction> Actions = new List<InputAction>();

        private static CancellationTokenSource PlayingCTS = new CancellationTokenSource();
        private static Stopwatch MillisM = new Stopwatch();
        private static bool WriteLock = false;
        public InputRecord()
        {
            WinApi.InputHook.KeyboardAction += KeyboardAction;
            WinApi.InputHook.MouseAction += MouseAction;
        }
        public void StartRecord()
        {
            if (WriteLock) return;
            MillisM.Start();
            WinApi.InputHook.Start();
        }
        public void StopRecording()
        {
            WinApi.InputHook.Stop();
            MillisM.Stop();
        }
        public void Clear()
        {
            Actions.Clear();
        }

        public void Play()
        {
            if (MillisM.IsRunning) return;
            WriteLock = true;
            CancellationToken token = PlayingCTS.Token;
            Task PlayTask = new Task(() =>
            {
                foreach (InputAction ia in Actions)
                {
                    ia.RunAction(token);
                    if (token.IsCancellationRequested) break;
                }
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

        public void Save(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
            DataContractSerializer ser = new DataContractSerializer(typeof(InputRecord));
            using (var writer = XmlDictionaryWriter.CreateBinaryWriter(fs))
            {
                ser.WriteObject(writer, this);
            }
            fs.Close();
        }
        public static InputRecord Load(string filename)
        {
            if (!File.Exists(filename)) return new InputRecord();
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(InputRecord));
                    using (var reader = XmlDictionaryReader.CreateBinaryReader(fs, XmlDictionaryReaderQuotas.Max))
                    {
                        var ir = (InputRecord?)ser.ReadObject(reader);
                        if (ir == null) ir = new InputRecord();
                        return ir;
                    }
                }
            }
            catch (SerializationException)
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

    [DataContract(Name = "Action")]
    public class InputAction
    {
        [DataMember(Name = "Delay")]
        public int Delay;
        [DataMember(Name = "Type")]
        public ActionType Type;
        [DataMember(Name = "Xparam")]
        public int Xparam;
        [DataMember(Name = "Yparam")]
        public int Yparam;
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
