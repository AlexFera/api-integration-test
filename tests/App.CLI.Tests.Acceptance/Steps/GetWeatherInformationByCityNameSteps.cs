using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using App.CLI.Tests.Acceptance.Models;
using BoDi;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace App.CLI.Tests.Acceptance.Steps;

[Binding]
public sealed class GetWeatherInformationByCityNameSteps
{
    private string _cityName;
    private int _woeid;
    private List<WeatherInformationRow> _setupWeatherInformation;
    private List<LocationInformationRow> _setupLocationInformation;
    private Process _getWeatherInformationProcess;

    private readonly IObjectContainer _objectContainer;

    public GetWeatherInformationByCityNameSteps(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [Given(@"the city name (.*)")]
    public void GivenTheCityName(string cityName)
    {
        _cityName = cityName;
    }

    [Given(@"the following locations returned by that city")]
    public void GivenTheFollowingLocationsReturnedByThatCity(Table table)
    {
        var locationInformationSet = table.CreateSet<LocationInformationRow>();
        _setupLocationInformation = locationInformationSet.ToList();

        var searchLocationApiResponse = _setupLocationInformation.Select(x => new SearchLocationResponse
        {
            LocationType = (LocationTypeResponse)x.LocationType,
            Title = x.Title,
            WhereOnEarthID = x.WhereOnEarthID
        });

        _woeid = _setupLocationInformation[0].WhereOnEarthID;

        var wireMockServer = _objectContainer.Resolve<WireMockServer>();
        wireMockServer
            .Given(Request.Create().WithPath("/location/search/").WithParam("query", _cityName).UsingGet())
            .RespondWith(Response.Create()
                .WithBody(JsonSerializer.Serialize(searchLocationApiResponse)));
    }

    [Given(@"the following current weather information for that city")]
    public void GivenTheFollowingCurrentWeatherInformationForThatCity(Table table)
    {
        var weatherInformationSet = table.CreateSet<WeatherInformationRow>();
        _setupWeatherInformation = weatherInformationSet.ToList();

        var getWeatherInformationResponse = new GetWeatherInformationResponse
        {
            WeatherInformation = _setupWeatherInformation.ConvertAll(x => new WeatherInformationResponse
            {
                AirPressure = x.AirPressure,
                Humidity = x.Humidity,
                Temperature = x.Temperature,
                WeatherStateName = x.WeatherStateName,
                WindDirection = x.WindDirection
            })
        };

        var wireMockServer = _objectContainer.Resolve<WireMockServer>();
        wireMockServer
            .Given(Request.Create().WithPath($"/location/{_woeid}").UsingGet())
            .RespondWith(Response.Create()
                .WithBody(JsonSerializer.Serialize(getWeatherInformationResponse)));
    }

    [When(@"a user gets the weather information for that city")]
    public void WhenAUserGetsTheWeatherInformationForThatCity()
    {
        var wireMockServer = _objectContainer.Resolve<WireMockServer>();
        var projectPaths = _objectContainer.Resolve<ProjectPaths>();
        var arguments = $"--cityName {_cityName}";

        _getWeatherInformationProcess = ProcessHelpers.CreateDotNetProcess(projectPaths.CliProjectPath, $"run {arguments}");
        _getWeatherInformationProcess.StartInfo.EnvironmentVariables["CLI_WeatherInformationAPI__BaseAddress"] = wireMockServer.Url;
        _getWeatherInformationProcess.Start();
    }

    [Then(@"weather information is returned")]
    public async Task ThenWeatherInformationIsReturned()
    {
        var expectedOutput = new GetWeatherInformationResponse
        {
            WeatherInformation = _setupWeatherInformation.ConvertAll(x => new WeatherInformationResponse
            {
                AirPressure = x.AirPressure,
                Humidity = x.Humidity,
                Temperature = x.Temperature,
                WeatherStateName = x.WeatherStateName,
                WindDirection = x.WindDirection
            })
        };

        var resultAsText = await _getWeatherInformationProcess.StandardOutput.ReadToEndAsync();
        await _getWeatherInformationProcess.WaitForExitAsync();

        var result = JsonSerializer.Deserialize<GetWeatherInformationResult>(resultAsText);
        result.Should().BeEquivalentTo(expectedOutput);
    }

    [Then(@"the error (.*) is returned")]
    public async Task ThenTheError(string errorMessage)
    {
        var resultText = (await _getWeatherInformationProcess.StandardOutput.ReadToEndAsync()).TrimEnd();
        await _getWeatherInformationProcess.WaitForExitAsync();
        resultText.Should().Be(errorMessage);
    }
}
