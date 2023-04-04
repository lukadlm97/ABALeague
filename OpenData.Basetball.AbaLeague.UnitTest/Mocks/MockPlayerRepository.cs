using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Application.UnitTests.Mocks
{
   
    public static class MockPlayerRepository
    {
        public static Mock<IPlayerRepository> GetPlayerRepository()
        {
            var players = new List<Player>
            {
                new Player()
                {
                    Id = 1,
                    Height = 192,
                    Name = "Test Vacation",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25)
                },
                new Player()
                {
                    Id = 1,
                    Height = 192,
                    Name = "Test Vacation",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25)
                },
                new Player()
                {
                    Id = 1,
                    Height = 192,
                    Name = "Test Vacation",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25)
                },
            };

            var mockRepo = new Mock<IPlayerRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(players);

           
            return mockRepo;

        }
    }
}
