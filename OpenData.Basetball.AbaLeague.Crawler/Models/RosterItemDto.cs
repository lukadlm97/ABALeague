using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Crawler.Models
{
    public record RosterItemDto([property: JsonPropertyName("person")] PersonDto Person,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("typeName")]
        string TypeName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("startDate")]
        DateTime StartDate,
        [property: JsonPropertyName("endDate")]
        DateTime EndDate,
        [property: JsonPropertyName("dorsal")] string Dorsal,
        [property: JsonPropertyName("dorsalRaw")]
        string DorsalRaw,
        [property: JsonPropertyName("position")]
        int Position,
        [property: JsonPropertyName("positionName")]
        string PositionName,
        [property: JsonPropertyName("lastTeam")]
        string LastTeam,
        [property: JsonPropertyName("externalId")]
        int ExternalId
    );
}
