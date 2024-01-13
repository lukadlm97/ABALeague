using System.Globalization;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using Result = OpenData.Basketball.AbaLeague.Domain.Common.Result;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.CreateLeague
{
    internal sealed class CreateLeagueCommandHandler : ICommandHandler<CreateLeagueCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeagueCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateLeagueCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeagueCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult is { IsValid: false })
            {
                return Result.Failure<string>(new Error("ValidationError", 
                    string.Join(',', validationResult
                        .Errors.Select(x => x.ErrorMessage))));
            }

            var existingLeagues = await _unitOfWork.LeagueRepository.GetAll(cancellationToken);
            if(existingLeagues.Any(x=> 
                   x.OfficalName.Trim().ToLower().Equals(request.OfficialName.Trim().ToLower()) ||
                   x.ShortName.Trim().ToLower().Equals(request.ShortName.Trim().ToLower())))
            {
                return Result.Failure(new Error("AlreadyExist","league with specified official name or short name already exist"));
            }
            var existingProcessor = (await _unitOfWork.ProcessorTypeRepository.GetAll( cancellationToken))
                                                        .FirstOrDefault(x=>x.Id == (short) request.ProcessorType);
            if(existingProcessor == null)
            {

                return Result.Failure(new Error("UnableToFoundProcessor", ""));
            }

            var existingSeason = _unitOfWork.SeasonRepository.Get().FirstOrDefault(x => x.Id == request.SeasonId);
            if (existingSeason == null)
            {

                return Result.Failure(new Error("UnableToFoundSeason", ""));
            }

            var existingCompetitionOrganization = _unitOfWork.CompetitionOrganizationRepository.Get()
                .FirstOrDefault(x => x.Id == (short) request.CompetitionOrganization);
            if (existingCompetitionOrganization == null)
            {

                return Result.Failure(new Error("UnableToFoundSeason", ""));
            }

            try
            {
                var result = await _unitOfWork.LeagueRepository.Add(new League()
                {
                    ShortName = request.ShortName,
                    BaseUrl = request.BaseUrl,
                    BoxScoreUrl = request.BoxScoreUrl,
                    CalendarUrl = request.CalendarUrl,
                    MatchUrl = request.MatchUrl,
                    OfficalName = request.OfficialName,
                    RosterUrl = request.RosterUrl,
                    StandingUrl = request.StandingUrl,
                    RoundMatches = new List<RoundMatch>(),
                    ProcessorTypeId = existingProcessor.Id,
                    SeasonId = existingSeason.Id,
                    RoundsToPlay = request.RoundsToPlay,
                    CompetitionOrganizationId = existingCompetitionOrganization.Id,
                }, cancellationToken);

                await _unitOfWork.Save();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("UnableToSaveNewLeague", ex.Message));
            }
        }
    }
}
