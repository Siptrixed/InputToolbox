using Microsoft.Extensions.DependencyInjection;

namespace InputToolbox;

public static class Startup
{
    public static void Configure(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<RecordingViewModel>();

        serviceCollection.AddSingleton<MainWindow>();
    }
}