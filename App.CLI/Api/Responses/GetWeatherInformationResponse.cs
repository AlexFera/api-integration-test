using System.Text.Json.Serialization;

namespace App.CLI.Api.Responses;

public class GetWeatherInformationResponse
{
    [JsonPropertyName("consolidated_weather")]
    public IReadOnlyList<WeatherInformationResponse> WeatherInformation { get; set; } = default!;
}
