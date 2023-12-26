using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.ProcessorType;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.ProcessorTypes.Queries
{
    public class GetProcessorTypeQueryHandler : IQueryHandler<GetProcessorTypeQuery, Maybe<IEnumerable<ProcessorTypeDto>>>
    {
        private IUnitOfWork _unitOfWork;

        public GetProcessorTypeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<IEnumerable<ProcessorTypeDto>>> Handle(GetProcessorTypeQuery request, CancellationToken cancellationToken)
        {
            var processorTypes = await _unitOfWork.ProcessorTypeRepository.GetAll(cancellationToken);
            if(!processorTypes.Any())
            {
                return Maybe<IEnumerable<ProcessorTypeDto>>.None;
            }

            return processorTypes
                .Select(x => new ProcessorTypeDto(x.Id, x.Name))
                .ToList();
        }
    }
}
