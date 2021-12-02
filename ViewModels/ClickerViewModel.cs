using InputToolbox.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputToolbox
{
    public class ClickerViewModel : ObservableObject, IDisposable
    {
        private const string BtnStopTxt = "Stop";
        private const string BtnStartTxt = "Start";
        private bool isStarted;
        private Dictionary<int, Import.WinApi.Vk> Keys = new() {
            { 0,Import.WinApi.Vk.VK_LBUTTON },
            { 1, Import.WinApi.Vk.VK_RBUTTON },
            { 2, Import.WinApi.Vk.VK_MBUTTON }
        };
        public ClickerViewModel()
        {
            StartCommand = new RelayCommand(Start);
        }
        
        public bool IsRunning
        {
            get => isStarted;
            set
            {
                ButtonText = value ? BtnStopTxt : BtnStartTxt;
                isStarted = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }
        public string ButtonText { get; private set; } = BtnStartTxt;
        public int FPS { get; set; } = 1;
        public int SelectedBTN { get; set; } = 0;
        public IRelayCommand StartCommand { get; }
        private void Start()
        {
            if (IsRunning)
            {
                SimpleClicker.Stop();
                IsRunning = false;
            }
            else
            {
                SimpleClicker.Run(FPS, Keys[SelectedBTN]);
                IsRunning = true;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
