namespace App.CLI.Models;

public record WeatherInformationResult
{
    public string WeatherStateName { get; init; } = default!;

    public string WindDirection { get; init; } = default!;

    public decimal Temperature { get; init; }

    public decimal AirPressure { get; init; }

    public int Humidity { get; init; }
}
