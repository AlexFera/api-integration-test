using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.CLI.Tests.Acceptance.Models;
using BoDi;
using TechTalk.SpecFlow;
using WireMock.Server;

namespace App.CLI.Tests.Acceptance.Hooks;

[Binding]
public sealed class GetWeatherInformationByCityNameHooks
{
    private ProjectPaths _projectPaths;
    private WireMockServer _wireMockServer;
    private readonly IObjectContainer _objectContainer;

    public GetWeatherInformationByCityNameHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeScenario()]
    public async Task BeforeScenario()
    {
        SetupFileLocations();
        await BuildCliProjectAsync();
        _wireMockServer = WireMockServer.Start();
        _objectContainer.RegisterInstanceAs(_wireMockServer);
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _wireMockServer.Stop();
    }

    private void SetupFileLocations()
    {
        var solutionDirectory = GetSolutionDirectoryInfo();
        _projectPaths = new ProjectPaths { CliProjectPath = $@"{solutionDirectory}\src\App.CLI\" };
        _objectContainer.RegisterInstanceAs(_projectPaths);
    }

    private async Task BuildCliProjectAsync()
    {
        var process = ProcessHelpers.CreateDotNetProcess(_projectPaths.CliProjectPath, "build --configuration Debug");

        process.Start();
        await process.WaitForExitAsync();
    }

    private static DirectoryInfo? GetSolutionDirectoryInfo()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (!(directory?.GetFiles("*.sln").Length > 0))
        {
            directory = directory?.Parent;
        }

        return directory;
    }
}
