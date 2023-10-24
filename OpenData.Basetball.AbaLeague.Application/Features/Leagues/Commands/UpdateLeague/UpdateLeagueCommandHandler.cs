using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.UpdateLeague
{
    internal class UpdateLeagueCommandHandler : ICommandHandler<UpdateLeagueCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLeagueCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateLeagueCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeagueCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult is { IsValid: false })
            {
                return Result.Failure<string>(new Error("ValidationError",
                    string.Join(',', validationResult
                        .Errors.Select(x => x.ErrorMessage))));
            }

            var existingLeague = await _unitOfWork.LeagueRepository.Get(request.Id, cancellationToken);
            if (existingLeague == null)
            {
                return Result.Failure(new Error("NotFound",string.Format("Unable to found league with id:{0}", request.Id)));
            }

            try
            {
                existingLeague.ShortName = request.ShortName;
                existingLeague.BaseUrl = request.BaseUrl;
                existingLeague.BoxScoreUrl = request.BoxScoreUrl;
                existingLeague.CalendarUrl = request.CalendarUrl;
                existingLeague.MatchUrl = request.MatchUrl;
                existingLeague.OfficalName = request.OfficialName;
                existingLeague.RosterUrl = request.RosterUrl;
                existingLeague.Season = request.Season;
                existingLeague.StandingUrl = request.StandingUrl;

                await _unitOfWork.LeagueRepository.Update(existingLeague, cancellationToken);

                await _unitOfWork.Save();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("UnableToSaveUpdatedLeague", ex.Message));
            }
        }

    }
}
