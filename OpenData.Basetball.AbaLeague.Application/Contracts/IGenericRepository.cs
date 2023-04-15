using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Application.Contracts
{
    public interface IGenericRepository<T> where T:class
    {
        Task<T> Get(int id,CancellationToken cancellationToken=default);
        Task<IReadOnlyList<T>> GetAll( CancellationToken cancellationToken = default);
        Task<T> Add(T entity, CancellationToken cancellationToken = default);
        Task<bool> Exists(int id, CancellationToken cancellationToken = default);
        Task Update(T entity, CancellationToken cancellationToken = default);
        Task Delete(T entity, CancellationToken cancellationToken = default);
    }
}
