namespace App.CLI.Tests.Acceptance.Models;

public class LocationInformationRow
{
    public string Title { get; init; } = default!;

    public LocationType LocationType { get; init; } = default!;

    public int WhereOnEarthID { get; init; }
}
