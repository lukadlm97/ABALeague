
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface ICountryRepository
    {
        Task<Country?> GetById(string nationalitySo3,CancellationToken cancellationToken=default);
        Task<Country?> GetById(int id,CancellationToken cancellationToken=default);

    }
}
