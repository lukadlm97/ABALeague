using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerAnotherNames
{
    public class GetPlayerAnotherNamesQueryHandler : IQueryHandler<GetPlayerAnotherNamesQuery, Maybe<AnotherNameDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlayerAnotherNamesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<AnotherNameDto>> Handle(GetPlayerAnotherNamesQuery request, CancellationToken cancellationToken)
        {
            var player = await _unitOfWork.PlayerRepository.Get(request.PlayerId, cancellationToken);
            if(player == null) {
                return Maybe<AnotherNameDto>.None;
            }

            var antoherNames=  await _unitOfWork.PlayerRepository.GetAnotherNames(request.PlayerId, cancellationToken);

            return new AnotherNameDto(antoherNames.Select(x => new AnotherNameItemDto(x.Id, x.Name)), player.Name);
        }
    }
}
