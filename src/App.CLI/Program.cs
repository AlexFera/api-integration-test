using App.CLI;
using App.CLI.Api;
using App.CLI.Output;
using App.CLI.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Refit;

var configuration = BuildConfiguration();
var serviceProvider = BuildServiceProvider(configuration);
var app = serviceProvider.GetRequiredService<WeatherInformationApplication>();
await app.RunAsync(args);

static IConfigurationRoot BuildConfiguration()
{
    return new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .AddEnvironmentVariables("CLI_")
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
    services.AddSingleton<IWeatherInformationService,  WeatherInformationService>();
    services.AddValidatorsFromAssemblyContaining<Program>();
    services.AddRefitClient<IWeatherInformationApi>()
        .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10)
        }))
        .ConfigureHttpClient(httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration["WeatherInformationApi:BaseAddress"]);
        });
}
