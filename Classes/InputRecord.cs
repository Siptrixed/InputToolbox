using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InputToolbox.Classes
{
    internal class InputRecord
    {
        static CancellationTokenSource Recording;
        List<InputAction> Actions = new List<InputAction>();
        public InputRecord()
        {
            WinApi.InputHook.KeyboardAction += KeyboardAction;
            WinApi.InputHook.MouseAction += MouseAction;
        }
        public void StartRecord()
        {
            WinApi.InputHook.Start();
        }

        private void MouseAction(object sender, WinApi.InputHook.MouseEventArgs e)
        {
            
        }
        private void KeyboardAction(object sender, WinApi.InputHook.KeyBDEventArgs e)
        {
            
        }

        public void StopRecording()
        {
            WinApi.InputHook.Stop();
        }
    }
    internal class InputAction
    {
        int Delay;
        ActionType Type;
        int Xdtm;
        int Ydtm;
        public InputAction(ActionType Type, int Xdtm, int Ydtm = 0, int Delay = 0)
        {
            this.Delay = Delay;
            this.Type = Type;
            this.Xdtm = Xdtm;
            this.Ydtm = Ydtm;
        }
        public void RunAction()
        {
            Thread.Sleep(Delay);
            switch (Type)
            {
                case ActionType.KeyBDEvent:
                    WinApi.KeyBDEvent((byte)Xdtm, Ydtm);
                    break;
                case ActionType.MouseEvent:
                    WinApi.MouseEvent(Xdtm, Ydtm);
                    break;
                case ActionType.MouseSet:
                    WinApi.SetCursorPos(Xdtm, Ydtm);
                    break;
            }
        }
    }
    enum ActionType
    {
        KeyBDEvent,
        MouseEvent,
        MouseSet
    }
}
