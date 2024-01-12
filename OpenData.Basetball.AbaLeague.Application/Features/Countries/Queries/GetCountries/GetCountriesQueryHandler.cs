using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Country;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountriesQueryHandler : IQueryHandler<GetCountriesQuery, Maybe<CountryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCountriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = _unitOfWork.CountryRepository.Get().ToList();
            if (countries == null)
            {
                return Maybe<CountryDto>.None;
            }

            return new CountryDto(countries
                                        .Select(x => new CountryItemDto(x.Id, x.Name))
                                        .OrderBy(x=>x.Name));
        }
    }
}
