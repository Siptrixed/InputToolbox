using InputToolbox.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InputToolbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly InputRecord Recording;
        public MainWindow()
        {
            Recording = InputRecord.Load("TestFile.bxml");
            InputRecord.PlayEnd += InputRecord_PlayEnd;
            InitializeComponent();
            RecordButton.Content = "StartRecord";
            PlayButton.Content = "Play";
        }

        private void InputRecord_PlayEnd(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(new Action(() => PlayButton.Content = "Play"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button Sender = sender as Button;
            if (Sender.Content == "StartRecord")
            {
                Sender.Content = "StopRecord";
                Recording.StartRecord();
            }
            else
            {
                Sender.Content = "StartRecord";
                Recording.StopRecording(2);
                Recording.Save("TestFile.bxml");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button Sender = sender as Button;
            if (Sender.Content == "Play")
            {
                Sender.Content = "Stop";
                Recording.Play();
            }
            else
            {
                Recording.Stop();
            }
        }
    }
}
