using App.CLI.Api.Responses;
using Refit;

namespace App.CLI.Api;

public interface IWeatherApi
{
    [Get("/location/search/?query={cityName}")]
    Task<IReadOnlyList<Location>> SearchLocationByCityNameAsync(string cityName);
}
