using Docker.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Seed
{
    public abstract class DataSeed : ISeed
    {
        private readonly ApiContext _apiContext;

        public DataSeed(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        public async Task MigrateAsync()
        {
            await _apiContext.Database.MigrateAsync();
        }

        public abstract Task SeedAsync();
    }
}
