using System.Text.Json;
using App.CLI.Api.Responses;
using App.CLI.Models;
using App.CLI.Output;
using App.CLI.Services;
using AutoFixture;
using NSubstitute;
using OneOf;
using Xunit;

namespace App.CLI.Tests.Unit;

public class WeatherInformationApplicationTests
{
    private readonly WeatherInformationApplication _sut;
    private readonly IConsoleWriter _consoleWriter = Substitute.For<IConsoleWriter>();
    private readonly IWeatherInformationService _weatherInformationService = Substitute.For<IWeatherInformationService>();
    private readonly IFixture _fixture = new Fixture();

    public WeatherInformationApplicationTests()
    {
        _sut = new WeatherInformationApplication(_consoleWriter, _weatherInformationService);
    }

    [Fact]
    public async void RunAsync_ShouldReturnWeatherInformation_WhenCityNameIsValid()
    {
        // Arrange
        const string cityName = "Madrid";
        var arguments = new[] { "--cityName", cityName };
        var getWeatherInformationResult = _fixture.Create<GetWeatherInformationResult>();
        OneOf<GetWeatherInformationResult, GetWeatherInformationError> result = getWeatherInformationResult;

        var getWeatherInformationRequest = new GeatWeatherInformationRequest(cityName);
        _weatherInformationService.GetWeatherInformationAsync(getWeatherInformationRequest).Returns(result);

        var expectedSerializedText = JsonSerializer.Serialize(getWeatherInformationResult, new JsonSerializerOptions { WriteIndented = true });

        // Act
        await _sut.RunAsync(arguments);

        // Assert
        _consoleWriter.Received(1).WriteLine(Arg.Is(expectedSerializedText));
    }

    [Fact]
    public async void RunAsync_ShouldReturnErrorMessage_WhenCityNameIsInValid()
    {
        // Arrange
        const string cityName = "666";
        var arguments = new[] { "--cityName", cityName };
        const string invalidCityNameError = "Please enter a valid city name";

        var errorResult = new GetWeatherInformationError(new[] { invalidCityNameError });
        OneOf<GetWeatherInformationResult, GetWeatherInformationError> result = errorResult;

        var getWeatherInformationRequest = new GeatWeatherInformationRequest(cityName);
        _weatherInformationService.GetWeatherInformationAsync(getWeatherInformationRequest).Returns(result);

        // Act
        await _sut.RunAsync(arguments);

        // Assert
        _consoleWriter.Received(1).WriteLine(Arg.Is(invalidCityNameError));
    }
}
