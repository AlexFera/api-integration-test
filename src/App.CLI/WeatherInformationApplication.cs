using System.CommandLine;
using System.Text.Json;
using App.CLI.Models;
using App.CLI.Output;
using App.CLI.Services;
using OneOf;

namespace App.CLI;

public class WeatherInformationApplication
{
    private readonly IConsoleWriter _consoleWriter;
    private readonly IWeatherInformationService _weatherInformationService;

    public WeatherInformationApplication(
        IConsoleWriter consoleWriter,
        IWeatherInformationService weatherInformationService)
    {
        _consoleWriter = consoleWriter;
        _weatherInformationService = weatherInformationService;
    }

    public async Task RunAsync(string[] args)
    {
        var cityNameOption = new Option<string>(
            name: "--cityName",
            description: "The name of the city");
        cityNameOption.IsRequired = true;

        var rootCommand = new RootCommand("Weather App Information");
        rootCommand.AddOption(cityNameOption);

        rootCommand.SetHandler((string cityName) => HandleCityNameOptionAsync(cityName), cityNameOption);

        await rootCommand.InvokeAsync(args);
    }

    async Task HandleCityNameOptionAsync(string cityName)
    {
        var request = new GeatWeatherInformationRequest(cityName);
        var result = await _weatherInformationService.GetWeatherInformationAsync(request);

        HandleGetWeatherInformationResult(result);
    }

    private void HandleGetWeatherInformationResult(OneOf<GetWeatherInformationResult, GetWeatherInformationError> result)
    {
        result.Switch(getWeatherInformationResult =>
        {
            var formattedTextResult = JsonSerializer.Serialize(getWeatherInformationResult, new JsonSerializerOptions { WriteIndented = true });
            _consoleWriter.WriteLine(formattedTextResult);
        },
        error =>
        {
            var formattedErrors = string.Join(",", error.ErrorMessages);
            _consoleWriter.WriteLine(formattedErrors);
        });
    }
}
