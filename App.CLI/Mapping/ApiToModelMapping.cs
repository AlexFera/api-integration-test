using App.CLI.Api.Responses;
using App.CLI.Models;

namespace App.CLI.Mapping;

public static class ApiToModelMapping
{
    public static GetWeatherInformationResult ToWeatherInformationResult(this WeatherInformationResponse response)
    {
        return new()
        {
            AirPressure = response.AirPressure,
            Humidity = response.Humidity,
            Temperature = response.Temperature,
            WeatherStateName = response.WeatherStateName,
            WindDirection = response.WindDirection
        };
    }
}
