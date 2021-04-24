using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Docker.Core
{
    public interface ISeed<TContext> where TContext : DbContext
    {
        Task MigrateAsync();
        Task SeedAsync();
    }
}
