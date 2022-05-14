using App.CLI.Api.Responses;
using Refit;

namespace App.CLI.Api;

public interface IWeatherInformationApi
{
    [Get("/location/search/?query={query}")]
    Task<IReadOnlyList<SearchLocationResponse>> SearchLocationAsync(string query);

    [Get("/location/{woeid}")]
    Task<GetWeatherInformationResponse> GetWeatherInformationAsync(int woeid);
}
