using System.Windows;

namespace InputToolbox;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(
        MainWindowViewModel viewModel0,
        RecordingViewModel viewModel, 
        ClickerViewModel viewModel2)
    {
        DataContext = viewModel0;
        InitializeComponent();
        RecordControl.DataContext = viewModel;
        ClickControl.DataContext  = viewModel2;
        
    }
}