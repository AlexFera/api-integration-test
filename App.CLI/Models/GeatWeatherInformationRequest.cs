namespace App.CLI.Models;

public record GeatWeatherInformationRequest
{
    public GeatWeatherInformationRequest(string cityName)
    {
        CityName = cityName;
    }

    public string CityName { get; }
}
