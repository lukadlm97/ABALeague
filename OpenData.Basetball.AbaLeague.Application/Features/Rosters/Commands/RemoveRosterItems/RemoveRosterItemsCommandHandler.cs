using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Commands.RemoveRosterItems
{
    public class RemoveRosterItemsCommandHandler : ICommandHandler<RemoveRosterItemsCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRosterItemsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(RemoveRosterItemsCommand request, CancellationToken cancellationToken)
        {
            foreach(var item in request.RemoveRosterItems)
            {
                try
                {
                    var rosterItem = await _unitOfWork.RosterRepository.Get(item.LeagueId, item.PlayerId, item.TeamId, cancellationToken);
                    if (rosterItem == null)
                    {
                        return Result.Failure(new Error("001", "unable to find roster item"));
                    }

                    rosterItem.EndOfActivePeriod = DateTime.UtcNow;
                    await _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    return Result.Failure(new Error("100", "unable to perform persistance changes"));
                }
            }

            return Result.Success();
        }
    }
}
