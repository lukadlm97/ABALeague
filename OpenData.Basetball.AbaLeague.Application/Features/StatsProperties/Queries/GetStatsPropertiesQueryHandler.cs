using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.StatsProperties;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.StatsProperties.Queries
{
    public class GetStatsPropertiesQueryHandler : 
        IQueryHandler<GetStatsPropertiesQuery, Maybe<StatsPropertiesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStatsPropertiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<StatsPropertiesDto>> 
            Handle(GetStatsPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = _unitOfWork.StatsPropertyRepository.Get().ToList();

            return new StatsPropertiesDto(properties
                                            .Select(x => new StatsPropertyItemDto(x.Id, x.Name)));
        }
    }
}
