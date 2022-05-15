using App.CLI.Api;
using App.CLI.Api.Responses;
using App.CLI.Mapping;
using App.CLI.Models;
using Ardalis.GuardClauses;
using FluentValidation;
using OneOf;

namespace App.CLI.Services;

public class WeatherInformationService : IWeatherInformationService
{
    private readonly IWeatherInformationApi _weatherInformationApi;
    private readonly IValidator<GeatWeatherInformationRequest> _validator;

    public WeatherInformationService(
        IWeatherInformationApi weatherInformationApi,
        IValidator<GeatWeatherInformationRequest> validator)
    {
        _weatherInformationApi = weatherInformationApi;
        _validator = validator;
    }

    public async Task<OneOf<GetWeatherInformationResult, GetWeatherInformationError>> GetWeatherInformationAsync(GeatWeatherInformationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.ConvertAll(x => x.ErrorMessage);
            return new GetWeatherInformationError(errorMessages);
        }

        var searchLocationResponses = await _weatherInformationApi.SearchLocationAsync(request.CityName);

        Guard.Against.Null(searchLocationResponses);

        if (searchLocationResponses.Count == 0)
        {
            return new GetWeatherInformationError(new List<string> { "City does not exist" });
        }
        if (searchLocationResponses.Count > 1)
        {
            return new GetWeatherInformationError(new List<string> { "Please provide the exact name of a city" });
        }
        if (searchLocationResponses[0].LocationType != LocationTypeResponse.City)
        {
            return new GetWeatherInformationError(new List<string> { "Please make sure to provide the name of a city and not of a country, province or continent" });
        }

        var getWeatherInformationResponse = await _weatherInformationApi.GetWeatherInformationAsync(searchLocationResponses[0].WhereOnEarthID);
        Guard.Against.Null(getWeatherInformationResponse);

        return getWeatherInformationResponse.ToGetWeatherInformationResult();
    }
}
