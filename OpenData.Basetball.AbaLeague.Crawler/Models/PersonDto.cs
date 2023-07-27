
using System.Text.Json.Serialization;

namespace OpenData.Basetball.AbaLeague.Crawler.Models
{
    public record PersonDto([property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("alias")] string Alias,
        [property: JsonPropertyName("aliasRaw")] string AliasRaw,
        [property: JsonPropertyName("passportName")] string PassportName,
        [property: JsonPropertyName("passportSurname")] string PassportSurname,
        [property: JsonPropertyName("jerseyName")] string JerseyName,
        [property: JsonPropertyName("abbreviatedName")] string AbbreviatedName,
        [property: JsonPropertyName("country")] CountryDto Country,
        [property: JsonPropertyName("height")] int? Height,
        [property: JsonPropertyName("weight")] int? Weight,
        [property: JsonPropertyName("birthDate")] DateTime BirthDate,
        [property: JsonPropertyName("birthCountry")] CountryDto BirthCountry,
        [property: JsonPropertyName("twitterAccount")] string TwitterAccount,
        [property: JsonPropertyName("instagramAccount")] string InstagramAccount,
        [property: JsonPropertyName("facebookAccount")] string FacebookAccount,
        [property: JsonPropertyName("isReferee")] bool IsReferee
    );
}
