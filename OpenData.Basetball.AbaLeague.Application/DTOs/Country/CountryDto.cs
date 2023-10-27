

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Country
{
    public record CountryDto(
        int CountryId,
        string Name
    )
    {
        public override string ToString()
        {
            return Name;
        }
    }
    
}
