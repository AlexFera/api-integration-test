using App.CLI.Models;
using OneOf;

namespace App.CLI.Services;

public interface IWeatherInformationService
{
    Task<OneOf<GetWeatherInformationResult, GetWeatherInformationError>> GetWeatherInformationAsync(GeatWeatherInformationRequest request);
}
