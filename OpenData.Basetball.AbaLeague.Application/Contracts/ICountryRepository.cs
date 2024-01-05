
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface ICountryRepository
    {
        Task<Country?> GetByIso3(string id,CancellationToken cancellationToken=default);
        Task<Country?> GetByAbaCode(string id,CancellationToken cancellationToken=default);
        Task<Country?> GetById(int id,CancellationToken cancellationToken=default);
        Task<IEnumerable<Country>?> Get(CancellationToken cancellationToken=default);

    }
}
