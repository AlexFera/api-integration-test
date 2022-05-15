using System.Text.Json.Serialization;

namespace App.CLI.Tests.Acceptance.Models;

public record SearchLocationResponse
{
    [JsonPropertyName("title")]
    public string Title { get; init; } = default!;

    [JsonPropertyName("location_type")]
    public LocationTypeResponse LocationType { get; init; } = default!;

    [JsonPropertyName("woeid")]
    public int WhereOnEarthID { get; init; }
}
