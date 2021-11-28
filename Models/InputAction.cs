using System.Threading;
using InputToolbox.Import;
using MessagePack;

namespace InputToolbox.Models;

[MessagePackObject]
public class InputAction
{
    [SerializationConstructor]
    public InputAction(ActionType Type, int Xparam, int Yparam = 0, int Delay = 0)
    {
        this.Delay = Delay;
        this.Type = Type;
        this.Xparam = Xparam;
        this.Yparam = Yparam;
    }

    [Key(3)]
    public int Delay { get; set; }
    [Key(0)]
    public ActionType Type { get; set; }
    [Key(1)]
    public int Xparam { get; set; }
    [Key(2)]
    public int Yparam { get; set; }

    public void RunAction(CancellationToken ct)
    {
        if(Delay != 0) ct.WaitHandle.WaitOne(Delay);
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