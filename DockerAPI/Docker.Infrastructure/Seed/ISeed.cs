using System.Threading.Tasks;

namespace Docker.Infrastructure.Seed
{
    public interface ISeed
    {
        Task MigrateAsync();
        Task SeedAsync();
    }
}
