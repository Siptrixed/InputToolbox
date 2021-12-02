using InputToolbox.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputToolbox.Models
{
    internal static class Tweaks
    {
        public static void BulletClick(int x, int y)
        {
            WinApi.MousePoint Current = WinApi.GetCursorPosition();
            WinApi.SetCursorPos(x, y);
            SimpleClicker.Click(WinApi.Vk.VK_LBUTTON);
            WinApi.SetCursorPos(Current.X,Current.Y);
        }
    }
}
