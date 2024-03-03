using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basetball.AbaLeague.Crawler.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenData.Basketball.AbaLeague.ConsoleApp.Helpers
{
    public class AddReversNameToAnotherNameBackgroundService : BackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddReversNameToAnotherNameBackgroundService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var leaguesWithKlsProcessor = (await _unitOfWork.LeagueRepository.GetAll(stoppingToken))
                                            .Where(x=>x.ProcessorTypeEnum == Domain.Enums.ProcessorType.Kls);
            var players = _unitOfWork.PlayerRepository.Get();
            var srbCountry = await _unitOfWork.CountryRepository.Get()
                                            .FirstOrDefaultAsync(x => x.CodeIso == "SRB", stoppingToken);
            if(srbCountry == null)
            {
                return;
            }
            var rosterItemsList = await Task
                .WhenAll(leaguesWithKlsProcessor
                            .Select(async x => await _unitOfWork.RosterRepository.SearchByLeagueId(x.Id)));
            var rosterItems = rosterItemsList.SelectMany(x => x).ToList();
            foreach (var rosterItem in rosterItems)
            {
                var selectedPlayer = players.FirstOrDefault(x => x.Id == rosterItem.PlayerId);
                if (selectedPlayer == null 
                    //|| selectedPlayer.CountryId != srbCountry.Id
                    )
                {
                    continue;
                }
                var name = selectedPlayer.Name.ReversNameSwap();
                var exisitingNames = await _unitOfWork.PlayerRepository.GetAnotherNames(selectedPlayer.Id, stoppingToken);
                if (exisitingNames.Select(x=>x.Name.ToLower() == name.ToLower()).Any())
                {
                    continue;
                }
                if (!await _unitOfWork.PlayerRepository
                                        .AddAnotherName(selectedPlayer.Id, name, stoppingToken))
                {
                    continue;
                }
            }
            await _unitOfWork.Save(stoppingToken);


        }
    }
}
