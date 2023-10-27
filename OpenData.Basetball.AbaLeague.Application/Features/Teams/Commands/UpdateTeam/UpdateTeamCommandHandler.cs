using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.CreateTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.UpdateTeam
{
    internal class UpdateTeamCommandHandler : ICommandHandler<UpdateTeamCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTeamCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingTeam = await _unitOfWork.TeamRepository.Get(request.Id, cancellationToken);
                if (existingTeam == null)
                {
                    return Result.Failure(new Error("", "Missing team with sent ID"));
                }
                var country = await _unitOfWork.CountryRepository.GetById(request.CountryId ?? 0, cancellationToken);
                if (country == null)
                {
                    return Result.Failure(new Error("", "Missing country with sent ID"));
                }

                existingTeam.Name = request.Name;
                existingTeam.ShortCode = request.ShortName;
                existingTeam.CountryId = country.Id;


                await _unitOfWork.TeamRepository.Update(existingTeam, cancellationToken);

                await _unitOfWork.Save();

                return Result.Success();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                return Result.Failure(new Error("", ex.Message));
            }
        }
    }
}


