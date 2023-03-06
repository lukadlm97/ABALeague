using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenData.Basetball.AbaLeague.Domain.Models;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;

namespace OpenData.Basetball.AbaLeague.ImportExecutor.Services
{
    public class SimpleImporter:BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SimpleImporter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();

                var player = new Player()
                {
                    PositionId = (short)PositionEnum.Center,
                    Name = "Savo Lesic",
                    DateOfBirth =DateTime.Today.AddYears(-36),
                    Height = 210,
                    Nationality = "SRB"
                };
                var isInserted = await playerRepository.Add(player);
                if (isInserted)
                {
                    Console.WriteLine(player.ID);
                }
                else
                {
                    Console.WriteLine("Can't insert!!!");
                }
            }
        }
    }
}
