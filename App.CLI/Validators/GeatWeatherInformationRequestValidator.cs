using App.CLI.Models;
using FluentValidation;

namespace App.CLI.Validators;

public class GeatWeatherInformationRequestValidator : AbstractValidator<GeatWeatherInformationRequest>
{
    public GeatWeatherInformationRequestValidator()
    {
        RuleFor(request => request.CityName)
            .Matches(@"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$")
            .WithMessage("Please enter a valid city name");
    }
}
