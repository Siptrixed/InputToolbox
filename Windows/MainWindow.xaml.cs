using System.Windows;

namespace InputToolbox;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(RecordingViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}