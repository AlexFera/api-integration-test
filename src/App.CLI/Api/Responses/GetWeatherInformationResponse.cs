using System.Text.Json.Serialization;

namespace App.CLI.Api.Responses;

public record GetWeatherInformationResponse
{
    [JsonPropertyName("consolidated_weather")]
    public IReadOnlyList<WeatherInformationResponse> WeatherInformation { get; init; } = default!;
}
