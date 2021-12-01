using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using InputToolbox.Models;
using MessagePack;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace InputToolbox;

public class RecordingViewModel : ObservableObject, IDisposable
{
    private static readonly MessagePackSerializerOptions Options =
        MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

    private bool _playGoing;
    private bool _recordingGoing;
    public InputRecord Recording = new();

    public RecordingViewModel()
    {
        StartRecordCommand = new RelayCommand(StartRecord, StartRecordEnable);
        StopRecordCommand = new RelayCommand(StopRecord, StopRecordEnable);
        PlayCommand = new RelayCommand(Play, PlayEnable);
        StopCommand = new RelayCommand(Stop, StopEnable);

        InputRecord.PlayEnd += InputRecordOnPlayEnd;

        //todo: Remove
        TestLoad();
        Import.WinHotKey key = new(System.Windows.Input.Key.End, Import.KeyModifier.None, (d) => {
            if (PlayGoing)
            {
                Stop();
            }
            else
            {
                Play();
            }
            });
    }

    public bool RecordingGoing
    {
        get => _recordingGoing;
        set
        {
            if (!SetProperty(ref _recordingGoing, value, nameof(RecordingGoing))) return;
            StartRecordCommand.NotifyCanExecuteChanged();
            StopRecordCommand.NotifyCanExecuteChanged();
            PlayCommand.NotifyCanExecuteChanged();
            StopCommand.NotifyCanExecuteChanged();
        }
    }

    public bool PlayGoing
    {
        get => _playGoing;
        set
        {
            if (!SetProperty(ref _playGoing, value, nameof(PlayGoing))) return;
            StartRecordCommand.NotifyCanExecuteChanged();
            StopRecordCommand.NotifyCanExecuteChanged();
            PlayCommand.NotifyCanExecuteChanged();
            StopCommand.NotifyCanExecuteChanged();
        }
    }

    public IRelayCommand StartRecordCommand { get; }
    public IRelayCommand StopRecordCommand { get; }
    public IRelayCommand PlayCommand { get; }
    public IRelayCommand StopCommand { get; }

    public void Dispose()
    {
        InputRecord.PlayEnd -= InputRecordOnPlayEnd;
        GC.SuppressFinalize(this);
    }

    private async void InputRecordOnPlayEnd(object? sender, EventArgs e)
    {
        await Application.Current.Dispatcher.InvokeAsync(() => { PlayGoing = false; });
    }

    [Obsolete]
    public void TestLoad()
    {
        //using Stream stream = File.OpenRead("TestFile.bxml");
        //Recording.Actions = MessagePackSerializer.Deserialize<List<InputAction>>(stream, Options);
    }

    private bool StartRecordEnable() => !PlayGoing && !RecordingGoing;

    private void StartRecord()
    {
        RecordingGoing = true;
        Recording.StartRecord();
    }

    private bool StopRecordEnable() => RecordingGoing && !PlayGoing;

    private async void StopRecord()
    {
        Recording.StopRecording();
        RecordingGoing = false;
        await TestSave();
    }

    private bool PlayEnable() => !PlayGoing && !RecordingGoing;

    private void Play()
    {
        PlayGoing = true;
        try
        {
            SimpleClicker.Run(10, Import.WinApi.Vk.VK_LBUTTON);
        }
        catch (Exception) { }
        //Recording.Play();
    }

    private bool StopEnable() => PlayGoing && !RecordingGoing;

    private void Stop()
    {
        //Recording.Stop();
        try
        {
            SimpleClicker.Stop();
        }
        catch(Exception) { }
        PlayGoing = false;
    }

    [Obsolete]
    private async Task TestSave()
    {
        await using Stream stream = File.Create("TestFile.bxml");
        await MessagePackSerializer.SerializeAsync(stream, Recording.Actions, Options);
    }
}