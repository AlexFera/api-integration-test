using System.Text.Json.Serialization;

namespace App.CLI.Api.Responses;

public class WeatherInformationResponse
{
    [JsonPropertyName("weather_state_name")]
    public string WeatherStateName { get; init; } = default!;

    [JsonPropertyName("wind_direction_compass")]
    public string WindDirection { get; init; } = default!;

    [JsonPropertyName("the_temp")]
    public decimal Temperature { get; init; }

    [JsonPropertyName("air_pressure")]
    public decimal AirPressure { get; init; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; init; }
}
