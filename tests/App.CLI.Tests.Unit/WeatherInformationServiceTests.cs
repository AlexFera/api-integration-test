using System.Collections.Generic;
using System.Threading.Tasks;
using App.CLI.Api;
using App.CLI.Api.Responses;
using App.CLI.Models;
using App.CLI.Services;
using App.CLI.Validators;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace App.CLI.Tests.Unit;

public class WeatherInformationServiceTests
{
    private readonly IWeatherInformationService _sut;
    private readonly IWeatherInformationApi _weatherInformationApi = Substitute.For<IWeatherInformationApi>();
    private readonly IValidator<GeatWeatherInformationRequest> _validator = new GeatWeatherInformationRequestValidator();
    private readonly IFixture _fixture = new Fixture();

    public WeatherInformationServiceTests()
    {
        _sut = new WeatherInformationService(_weatherInformationApi, _validator);
    }

    [Fact]
    public async Task GetWeatherInformationAsync_ShouldReturnResults_WhenCityNameIsValid()
    {
        // Arrange
        const string cityName = "Madrid";
        var request = new GeatWeatherInformationRequest(cityName);
        var searchLocationResponse = _fixture.Create<SearchLocationResponse>();
        _weatherInformationApi.SearchLocationAsync(cityName).Returns(new List<SearchLocationResponse> { searchLocationResponse });
        var expectedResult = _fixture.Create<GetWeatherInformationResponse>();
        _weatherInformationApi.GetWeatherInformationAsync(searchLocationResponse.WhereOnEarthID).Returns(expectedResult);

        // Act
        var result = await _sut.GetWeatherInformationAsync(request);

        result.AsT0.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetWeatherInformationAsync_ShouldReturnError_WhenCityNameIsInValid()
    {
        // Arrange
        const string cityName = "666";
        var request = new GeatWeatherInformationRequest(cityName);
        var errorMessages = new List<string> { "Please enter a valid city name" };
        var expectedResult = new GetWeatherInformationError(errorMessages);

        // Act
        var result = await _sut.GetWeatherInformationAsync(request);

        // Assert
        result.AsT1.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetWeatherInformationAsync_ShouldReturnError_WhenCityDoesNotExist()
    {
        // Arrange
        const string cityName = "ABCD";
        var request = new GeatWeatherInformationRequest(cityName);
        _weatherInformationApi.SearchLocationAsync(cityName).Returns(new List<SearchLocationResponse>());
        var errorMessages = new List<string> { "City does not exist" };
        var expectedResult = new GetWeatherInformationError(errorMessages);

        // Act
        var result = await _sut.GetWeatherInformationAsync(request);

        result.AsT1.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetWeatherInformationAsync_ShouldReturnError_WhenCityNameIsNotExact()
    {
        // Arrange
        const string cityName = "San";
        var request = new GeatWeatherInformationRequest(cityName);
        var searchLocationResponse1 = _fixture.Create<SearchLocationResponse>();
        var searchLocationResponse2 = _fixture.Create<SearchLocationResponse>();
        _weatherInformationApi.SearchLocationAsync(cityName).Returns(new List<SearchLocationResponse> {searchLocationResponse1, searchLocationResponse2 });
        var errorMessages = new List<string> { "Please provide the exact name of a city" };
        var expectedResult = new GetWeatherInformationError(errorMessages);

        // Act
        var result = await _sut.GetWeatherInformationAsync(request);

        result.AsT1.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetWeatherInformationAsync_ShouldReturnError_WhenCityNameIsNotACity()
    {
        // Arrange
        const string cityName = "Europe";
        var request = new GeatWeatherInformationRequest(cityName);
        var searchLocationResponse = _fixture
            .Build<SearchLocationResponse>()
            .With(x => x.LocationType, LocationTypeResponse.Continent)
            .Create();
        _weatherInformationApi.SearchLocationAsync(cityName).Returns(new List<SearchLocationResponse> { searchLocationResponse });
        var errorMessages = new List<string> { "Please make sure to provide the name of a city and not of a country, province or continent" };
        var expectedResult = new GetWeatherInformationError(errorMessages);

        // Act
        var result = await _sut.GetWeatherInformationAsync(request);

        result.AsT1.Should().BeEquivalentTo(expectedResult);
    }
}
