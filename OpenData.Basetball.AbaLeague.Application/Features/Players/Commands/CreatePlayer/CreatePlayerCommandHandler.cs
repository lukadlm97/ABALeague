using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.CreatePlayer
{
    internal class CreatePlayerCommandHandler : ICommandHandler<CreatePlayerCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlayerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
        {
            var country = await _unitOfWork.CountryRepository.GetById(request.CountryId??0, cancellationToken);
            var position = await _unitOfWork.PositionRepository.Get(request.PositionId??0, cancellationToken);
            if(country == null || position == null) 
            {
                return Result.Failure(new Error("BadRequest","Unable to found enumarions"));
            }

            var newPlayer = new Player
            {
                Name = request.Name,
                CountryId = country.Id,
                PositionId = position.Id,
                DateOfBirth = request.DateOfBirth,
                Height = request.Height,
                Nationality = country.Nationality
            };
            await _unitOfWork.PlayerRepository.Add(newPlayer, cancellationToken);

            await _unitOfWork.Save();

            return Result.Success(newPlayer);
        }
    }
}
