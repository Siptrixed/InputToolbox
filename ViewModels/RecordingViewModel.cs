using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using InputToolbox.Models;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace InputToolbox;

public class RecordingViewModel : ObservableObject, IDisposable
{
    private const string BtnStopTxt = "Stop";
    private const string BtnStartTxt = "Start";

    private bool isStarted;
    private InputRecord Recording = new();

    public RecordingViewModel()
    {
        StartCommand = new RelayCommand(Start);
        SaveCommand = new RelayCommand(Save);
        LoadCommand = new RelayCommand(Load);

        InputRecord.PlayEnd += InputRecordOnPlayEnd;
        InputRecord.ActionAdded += InputActionAdded; ;
    }

    private void InputActionAdded(object? sender, InputAction e)
    {
        Actions.Add(e);
    }

    public bool IsRunning { 
        get => isStarted;
        set { 
            ButtonText = value ? BtnStopTxt : BtnStartTxt;
            isStarted = value;
            OnPropertyChanged(nameof(ButtonText));
        }
    }
    public ObservableCollection<InputAction> Actions { get; set; } = new();
    public bool RecordChecked { get; set; } = true;
    public bool PlayChecked { get; set; }
    public string ButtonText { get; private set; } = BtnStartTxt;
    public IRelayCommand StartCommand { get; }
    public IRelayCommand SaveCommand { get; }
    public IRelayCommand LoadCommand { get; }
    private void Start()
    {
        if (RecordChecked)
        {
            if (IsRunning)
            {
                Recording.Stop();
                IsRunning = false;
            }
            else
            {
                Recording.StartRecord();
                IsRunning = true;
                Actions.Clear();
            }
        }
        else if(PlayChecked)
        {
            if (IsRunning)
            {
                Recording.Stop();
                IsRunning = false;
            }
            else
            {
                Recording.Play();
                IsRunning = true;
            }
        }
    }
    private async void Save()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog();
        dialog.FileName = "MyRecord"; 
        dialog.DefaultExt = ".inprec";
        dialog.Filter = "User input record (.inprec)|*.inprec";
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            string filename = dialog.FileName;
            await FilesSerialization.SaveObjToFile(filename, Recording.Actions);
        }
    }
    private void Load()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog();
        dialog.FileName = "";
        dialog.DefaultExt = ".inprec";
        dialog.Filter = "User input record (.inprec)|*.inprec";
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            string filename = dialog.FileName;
            if (FilesSerialization.TryReadObjFromFile(
                filename,
                out List<InputAction> acts))
            {
                Recording.Actions = acts;
            }
            Actions = new(Recording.Actions);
        }
    }
    public void Dispose()
    {
        InputRecord.PlayEnd -= InputRecordOnPlayEnd;
        GC.SuppressFinalize(this);
    }

    private async void InputRecordOnPlayEnd(object? sender, EventArgs e)
    {
        await Application.Current.Dispatcher.InvokeAsync(() => { IsRunning = false; });
    }

   
}