namespace App.CLI.Models;

public record GetWeatherInformationError
{
    public GetWeatherInformationError(IReadOnlyList<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }

    public IReadOnlyList<string> ErrorMessages { get; } = default!;
}
