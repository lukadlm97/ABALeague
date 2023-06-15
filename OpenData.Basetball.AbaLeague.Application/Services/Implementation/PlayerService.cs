using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class PlayerService:IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PlayerDto>> Get(CancellationToken cancellationToken = default)
        {
            var players = await _unitOfWork.PlayerRepository.GetAll(cancellationToken);

            var outputList = new List<PlayerDto>();
            foreach (var player in players)
            {
                var outputPlayer = new PlayerDto(player.Id, player.Name, player.PositionEnum, player.Height,
                    player.DateOfBirth, player.Country);
                outputList.Add(outputPlayer);
            }

            return outputList;
        }

        public async Task<IEnumerable<PlayerDto>> Get(int teamId, CancellationToken cancellationToken = default)
        {

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PlayerDto>> Add(AddPlayerDto addPlayerDto, CancellationToken cancellationToken = default)
        {
            if (await _unitOfWork.PlayerRepository.Exist(addPlayerDto.Name, cancellationToken))
            {
                return null;
            }
            var country =
                await _unitOfWork.CountryRepository.GetById(addPlayerDto.NationalityId ?? -1, cancellationToken);
            var position =
                await _unitOfWork.PositionRepository.Get((int)addPlayerDto.Position, cancellationToken);
            var player = new Player()
            {
                Country = country,
                CountryId = country.Id,
                Name = addPlayerDto.Name,
                Height = addPlayerDto.Height,
                DateOfBirth = addPlayerDto.DateOfBirth,
                Position = position,
                PositionId = position.Id,
                Nationality = country.CodeIso
            };

            await _unitOfWork.PlayerRepository.Add(player,cancellationToken);
            await _unitOfWork.Save();

            return new List<PlayerDto>()
            {
                new PlayerDto(player.Id, player.Name, player.PositionEnum, player.Height, player.DateOfBirth,
                    player.Country)
            };
        }

        public async Task<IEnumerable<PlayerDto>> Add(IEnumerable<AddPlayerDto> addPlayerDtoList, CancellationToken cancellationToken = default)
        {
            List<PlayerDto> outputList = new List<PlayerDto>();
            foreach (var addPlayerDto in addPlayerDtoList)
            {
                var addedPlayer = await Add(addPlayerDto, cancellationToken);
                if (addedPlayer != null)
                {
                    outputList.AddRange(addedPlayer);
                }
            }

            return outputList;
        }
    }
}
