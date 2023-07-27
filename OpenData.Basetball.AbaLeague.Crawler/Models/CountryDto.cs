
using System.Text.Json.Serialization;

namespace OpenData.Basetball.AbaLeague.Crawler.Models
{
    public record CountryDto(
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name
    );
}
