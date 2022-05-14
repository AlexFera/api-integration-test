using App.CLI.Api.Responses;
using App.CLI.Models;

namespace App.CLI.Mapping;

public static class ApiToModelMapping
{
    public static GetWeatherInformationResult ToGetWeatherInformationResult(this GetWeatherInformationResponse response)
    {
        return new()
        {
            WeatherInformation = response.WeatherInformation.Select(
                x => new WeatherInformationResult
                {
                    AirPressure = x.AirPressure,
                    Humidity = x.Humidity,
                    Temperature = x.Temperature,
                    WeatherStateName = x.WeatherStateName,
                    WindDirection = x.WindDirection
                }).ToList()
        };
    }
}
