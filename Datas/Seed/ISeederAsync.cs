using Examen.Models;

namespace Examen.Datas.Seed
{
    public interface ISeederAsync
    {
        Task SeedAsync(ApplicationDbContext context);

    }
}
