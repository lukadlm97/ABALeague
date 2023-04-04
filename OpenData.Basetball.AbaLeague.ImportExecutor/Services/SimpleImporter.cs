using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Domain.Enums;
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
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                
                var player = new Player()
                {
                    PositionId = (short)PositionEnum.Center,
                    Name = "Savo Lesic",
                    DateOfBirth =DateTime.Today.AddYears(-36),
                    Height = 210,
                    Nationality = "SRB",
                    CountryId = 1
                };
                var isInserted = await unitOfWork.PlayerRepository.Add(player);
                await unitOfWork.Save();
                if (isInserted.Id!=0)
                {
                    Console.WriteLine(player.Id);
                }
                else
                {
                    Console.WriteLine("Can't insert!!!");
                }
                 player = await unitOfWork.PlayerRepository.Get(1);
                player.Height = -1;

                await unitOfWork.PlayerRepository.Update(player);
                await unitOfWork.Save();
                
                player = await unitOfWork.PlayerRepository.Get(2);
                if (player.Id != 0)
                {
                    Console.WriteLine(player.Height);
                }
                else
                {
                    Console.WriteLine("Can't insert!!!");
                }
                

            }
        }
    }
}
