using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.CreateTeam
{
    internal class CreateTeamCommandHandler : ICommandHandler<CreateTeamCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeamCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var country = await _unitOfWork.CountryRepository.GetById(request.CountryId ?? 0, cancellationToken);
                if (country == null)
                {
                    return Result.Failure(new Error("", "Missing country with sent ID"));
                }
                await _unitOfWork.TeamRepository.Add(new Team
                {
                    CountryId = country.Id,
                    Name = request.Name,
                    ShortCode = request.ShortName
                }, cancellationToken);

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
