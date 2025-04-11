using Examen.Datas.Seed;
using Examen.Models;
using Microsoft.EntityFrameworkCore;

namespace Examen.Datas.Seed
{
    public class DbSeeder
    {
        private readonly ILogger<DbSeeder> _logger; 
        private readonly IEnumerable<ISeeder> _seeders;
        private readonly IEnumerable<ISeederAsync> _seedersAsync;

        public DbSeeder(IEnumerable<ISeeder> seeders, ILogger<DbSeeder> logger, IEnumerable<ISeederAsync> seederAsyncs) 
        {
            _seeders = seeders;
            _logger = logger;
            _seedersAsync = seederAsyncs;
        }

        public void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            foreach (var seeder in _seeders)
            {
                _logger.LogInformation($"Running seeder: {seeder.GetType().Name}");
                seeder.Seed(context);
            }
        }
        
        public async Task SeedAsync(ApplicationDbContext context)
        {
            context.Database.Migrate();

            foreach (var seeder in _seedersAsync)
            {
                await seeder.SeedAsync(context);
            }
        }
    }
}
