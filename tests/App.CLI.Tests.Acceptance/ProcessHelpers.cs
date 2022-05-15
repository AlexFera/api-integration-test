using System.Collections.Generic;
using System.Diagnostics;

namespace App.CLI.Tests.Acceptance;

public static class ProcessHelpers
{
    public static Process CreateDotNetProcess(string workingDirectory, string arguments)
    {
        return new Process
        {
            StartInfo =
            {
                FileName = "dotnet",
                WorkingDirectory = workingDirectory,
                Arguments = arguments,
                RedirectStandardOutput = true,
            }
        };
    }
}
