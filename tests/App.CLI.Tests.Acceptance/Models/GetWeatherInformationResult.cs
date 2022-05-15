using System.Collections.Generic;

namespace App.CLI.Tests.Acceptance.Models;
public record GetWeatherInformationResult
{
    public IReadOnlyList<WeatherInformationResult> WeatherInformation { get; init; } = default!;
}
