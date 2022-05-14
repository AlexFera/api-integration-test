using System.CommandLine;
using App.CLI.Output;

namespace App.CLI;

public class WeatherInformationApplication
{
    private readonly IConsoleWriter _consoleWriter;

    public WeatherInformationApplication(IConsoleWriter consoleWriter)
    {
        _consoleWriter = consoleWriter;
    }

    public async Task RunAsync(string[] args)
    {
        var cityNameOption = new Option<string>(
            name: "--cityName",
            description: "The name of the city");
        cityNameOption.IsRequired= true;

        var rootCommand = new RootCommand("Weather App Information");
        rootCommand.AddOption(cityNameOption);

        rootCommand.SetHandler((string cityName) => HandleCityNameOption(cityName) , cityNameOption);

        await rootCommand.InvokeAsync(args);
    }

    void HandleCityNameOption(string cityName)
    {
        _consoleWriter.WriteLine(cityName);
    }
}
