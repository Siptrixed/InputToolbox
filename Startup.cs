using Microsoft.Extensions.DependencyInjection;

namespace InputToolbox;

public static class Startup
{
    public static void Configure(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<RecordingViewModel>();
        serviceCollection.AddSingleton<ClickerViewModel>();
        serviceCollection.AddSingleton<RemoteViewModel>();
        serviceCollection.AddSingleton<MainWindowViewModel>();

        serviceCollection.AddSingleton<MainWindow>();
    }
}