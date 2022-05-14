using App.CLI;
using App.CLI.Api;
using App.CLI.Output;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

var configuration = BuildConfiguration();
var serviceProvider = BuildServiceProvider(configuration);
var app = serviceProvider.GetRequiredService<WeatherInformationApplication>();
await app.RunAsync(args);

static IConfigurationRoot BuildConfiguration()
{
    return new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .Build();
}

static ServiceProvider BuildServiceProvider(IConfigurationRoot configuration)
{
    var services = new ServiceCollection();
    ConfigureServices(configuration, services);
    var serviceProvider = services.BuildServiceProvider();
    return serviceProvider;
}

static void ConfigureServices(IConfigurationRoot configuration, IServiceCollection services)
{
    services.AddSingleton<WeatherInformationApplication>();
    services.AddSingleton<IConsoleWriter, ConsoleWriter>();
    services.AddRefitClient<IWeatherApi>()
        .ConfigureHttpClient(httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration["WeatherApi:BaseAddress"]);
        });
}
