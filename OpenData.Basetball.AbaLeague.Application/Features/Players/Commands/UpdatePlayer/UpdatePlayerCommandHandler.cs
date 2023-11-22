using Jint.Parser;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.UpdatePlayer
{
    internal class UpdatePlayerCommandHandler : ICommandHandler<UpdatePlayerCommand, Result>
    {
        private IUnitOfWork _unitOfWork;

        public UpdatePlayerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
        {
            var existingPlayer = await _unitOfWork.PlayerRepository.Get(request.Id, cancellationToken);
            if(existingPlayer == null)
            {
                return Result.Failure(new Error("BadRequest", "Unable to found player with id"));
            }

            var country = await _unitOfWork.CountryRepository.GetById(request.CountryId ?? 0, cancellationToken);
            var position = await _unitOfWork.PositionRepository.Get(request.PositionId ?? 0, cancellationToken);
            if (country == null || position == null)
            {
                return Result.Failure(new Error("BadRequest", "Unable to found enumarions"));
            }

            existingPlayer.Name = request.Name;
            existingPlayer.CountryId = country.Id;
            existingPlayer.PositionId = position.Id;
            existingPlayer.DateOfBirth = request.DateOfBirth;
            existingPlayer.Height = request.Height;

            await _unitOfWork.PlayerRepository.Update(existingPlayer, cancellationToken);
            await _unitOfWork.Save();

            return Result.Success(existingPlayer);
        }
    }
}
