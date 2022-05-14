namespace App.CLI.Models;

public record GetWeatherInformationResult
{
    public IReadOnlyList<WeatherInformationResult> WeatherInformation { get; init; } = default!;
}
