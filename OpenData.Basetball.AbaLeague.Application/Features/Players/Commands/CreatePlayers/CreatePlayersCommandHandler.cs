using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.CreatePlayers
{
    public class CreatePlayersCommandHandler : ICommandHandler<CreatePlayersCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlayersCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public async Task<Result> Handle(CreatePlayersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var newPlayerItem in request.PlayerItems)
                {
                    if(await _unitOfWork.PlayerRepository.Exist(newPlayerItem.Name, cancellationToken))
                    {
                        continue;
                    }

                    var selectedCountry = await _unitOfWork.CountryRepository.GetById(newPlayerItem.NationalityId ?? 0, cancellationToken);
                    var selectedPosition = await _unitOfWork.PositionRepository.Get((int)newPlayerItem.Position, cancellationToken);
                    if(selectedCountry == null||selectedPosition == null) 
                    {
                        continue;
                    }

                    await _unitOfWork.PlayerRepository.Add(new Basetball.AbaLeague.Domain.Entities.Player
                    {
                        Name = newPlayerItem.Name,
                        CountryId = selectedCountry.Id,
                        Height = newPlayerItem.Height,
                        PositionId = selectedPosition.Id,
                        DateOfBirth = newPlayerItem.DateOfBirth,
                        Nationality = string.Empty
                    });
                }

                await _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                return Result.Failure(new Error("100", "Error on persistance modificaiton"));
            }

            return Result.Success();
        }
    }
}
