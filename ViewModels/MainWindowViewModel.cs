using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputToolbox
{
    public class MainWindowViewModel : ObservableObject, IDisposable
    {
        internal Import.WinHotKey HotK;
        public MainWindowViewModel()
        {
            HotK = new Import.WinHotKey(
                System.Windows.Input.Key.End,
                Import.KeyModifier.None,
                (x) => Start());
        }
        public int selectedTab { get; set; }
        public void Start()
        {
            switch (selectedTab)
            {
                case 0:
                    break;
            }
        }
        public void Dispose()
        {
            HotK.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
