using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace App.CLI.Tests.Acceptance.Models;

public record GetWeatherInformationResponse
{
    [JsonPropertyName("consolidated_weather")]
    public IReadOnlyList<WeatherInformationResponse> WeatherInformation { get; init; } = default!;
}
