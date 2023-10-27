using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Country;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountriesQueryHandler : IQueryHandler<GetCountriesQuery, Maybe<CountryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCountriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<CountryResponse>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await _unitOfWork.CountryRepository.Get(cancellationToken);
            if (countries == null)
            {
                return Maybe<CountryResponse>.None;
            }


            var dto =  countries.Select(x => new CountryDto(x.Id, x.Name));

            return new CountryResponse(dto);
        }
    }
}
