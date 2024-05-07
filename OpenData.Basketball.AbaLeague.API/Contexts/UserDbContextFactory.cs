using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace OpenData.Basketball.AbaLeague.API.Contexts
{
    internal class UserDbContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        UserContext IDesignTimeDbContextFactory<UserContext>.CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var builder = new DbContextOptionsBuilder<UserContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine("test:"+connectionString);
            builder.UseSqlServer(connectionString);

            return new UserContext(builder.Options);
        }
    }
}
