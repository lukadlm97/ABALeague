using FluentValidation;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Application.Validation;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.UpdateLeague
{
    internal class UpdateLeagueCommandValidator : AbstractValidator<UpdateLeagueCommand>
    {
        public UpdateLeagueCommandValidator()
        {
            RuleFor(x => x.OfficialName)
                .Length(5, 120)
                .WithError(ValidationErrors.League.EmptyOrTooShortOfficialName);
            RuleFor(x => x.ShortName)
                .Length(0, 10)
                .WithError(ValidationErrors.League.EmptyOrTooBigShortName);
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithError(ValidationErrors.League.IdShouldBePositive);
        }
    }
}
