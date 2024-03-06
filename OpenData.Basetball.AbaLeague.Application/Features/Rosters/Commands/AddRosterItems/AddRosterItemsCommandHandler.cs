using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Commands.AddRosterItems
{
    public class AddRosterItemsCommandHandler : ICommandHandler<AddRosterItemsCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddRosterItemsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(AddRosterItemsCommand request, CancellationToken cancellationToken)
        {
            var league = await _unitOfWork.LeagueRepository.Get(request.LeagueId);
            if(league == null)
            {
                return Result.Failure(new Error("000","League not found for supplied league ID."));
            }

            if(request.RosterItems == null || !request.RosterItems.Any()) 
            {
                return Result.Failure(new Error("000", "Roster items for insertion not found."));
            }

            try
            {
                foreach(var item in request.RosterItems)
                {
                    if(await _unitOfWork.RosterRepository.Exist(item.LeagueId, 
                                                                item.PlayerId, 
                                                                item.TeamId, 
                                                                cancellationToken))
                    {
                        continue;
                    }

                    var previouseExistingRosterMarks =
                        _unitOfWork.RosterRepository
                                    .Get()
                                    .Where(x => x.PlayerId == item.PlayerId && item.End == null)
                                    .ToList();
                    foreach(var existingRosterItemWithValidContract in previouseExistingRosterMarks)
                    {
                        existingRosterItemWithValidContract.EndOfActivePeriod = DateTime.UtcNow;
                    }

                    await _unitOfWork.RosterRepository.Add(new Basetball.AbaLeague.Domain.Entities.RosterItem
                    {
                        LeagueId = item.LeagueId,
                        DateOfInsertion = DateTime.UtcNow,
                        PlayerId = item.PlayerId,
                        TeamId = item.TeamId,
                        EndOfActivePeriod = item.End
                    });
                }
                await _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                return Result.Failure(new Error("100", "Unable to save roster items."));
            }

            return Result.Success();
        }
    }
}
