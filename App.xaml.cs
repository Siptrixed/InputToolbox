using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace InputToolbox;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ServiceCollection services = new();
        InputToolbox.Startup.Configure(services);
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        MainWindow = serviceProvider.GetRequiredService<MainWindow>();
        MainWindow.Show();
    }
}