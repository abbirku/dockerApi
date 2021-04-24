using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Docker.Core
{
    public abstract class DataSeed<TContext> : ISeed<TContext>
        where TContext : DbContext
    {
        protected TContext _context;

        public DataSeed(TContext apiContext) => _context = apiContext;

        public async Task MigrateAsync() => await _context.Database.MigrateAsync();

        public abstract Task SeedAsync();
    }
}
