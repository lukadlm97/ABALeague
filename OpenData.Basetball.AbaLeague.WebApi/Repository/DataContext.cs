using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.WebApi.Repository
{
    public record DataContext
    {
        public IEnumerable<Player> Players { get; init; }=new List<Player>()
        {
     /*       new Player()
            {
                ID = 1,
                PositionId = (short)PositionEnum.Center,
                Name = "Savo Lesic",
                DateOfBirth =DateTime.Today.AddYears(-36),
                Height = 210,
                Nationality = "SRB"
            },
            new Player()
            {
                ID = 2,
                PositionId = (short)PositionEnum.Guard,
                Name = "Brank lazic",
                DateOfBirth =DateTime.Today.AddYears(-34),
                Height = 198,
                Nationality = "SRB"
            },
            new Player()
            {
                ID = 1,
                PositionId = (short)PositionEnum.Center,
                Name = "Balsa koprivica",
                DateOfBirth =DateTime.Today.AddYears(-36),
                Height = 210,
                Nationality = "SRB"
            }*/
        };

    }
}
