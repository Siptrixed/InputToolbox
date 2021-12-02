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
        public RecordingViewModel recordingViewModel { get; set; }
        public ClickerViewModel clickerViewModel { get; set; }
        public MainWindowViewModel(RecordingViewModel vm1, ClickerViewModel vm2)
        {
            recordingViewModel = vm1;
            clickerViewModel = vm2;
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
                    clickerViewModel.StartCommand.Execute(default);
                    break;
                case 1:
                    recordingViewModel.StartCommand.Execute(default);
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
