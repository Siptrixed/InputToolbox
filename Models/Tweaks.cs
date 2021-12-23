using InputToolbox.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            SimpleClicker.Click(WinApi.Vk.LBUTTON);
            WinApi.SetCursorPos(Current.X,Current.Y);
        }
        public static string ReadResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
                resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
