using System.Text.Json.Serialization;

namespace App.CLI.Api.Responses;

public record Location
{
    [JsonPropertyName("title")]
    public string Title { get; init; } = default!;

    [JsonPropertyName("location_type")]
    public string LocationType { get; init; } = default!;

    [JsonPropertyName("woeid")]
    public int WhereOnEarthID { get; init; }
}
