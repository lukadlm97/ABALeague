using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OpenData.Basetball.AbaLeague.Persistence
{
    public class AbaLeagueDbContextFactory : IDesignTimeDbContextFactory<AbaLeagueDbContext>
    {
        public AbaLeagueDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AbaLeagueDbContext>();
            var connectionString = configuration.GetConnectionString("Database");

            builder.UseSqlServer(connectionString);

            return new AbaLeagueDbContext(builder.Options);
        }
    }
}
