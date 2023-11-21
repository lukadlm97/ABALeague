using OpenData.Basketball.AbaLeague.Application.DTOs.Country;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Utilities
{
    public static class StringExstentions
    {
        public static string ResolveCountryName(this int id, IEnumerable<CountryDto> countries)
        {
            return countries?.FirstOrDefault(x => x.CountryId == id)?.Name;
        }
    }
}
