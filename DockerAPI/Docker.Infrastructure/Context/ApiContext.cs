using Docker.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Docker.Infrastructure.Context
{
    public class ApiContext : DbContext, IApiContext
    {
        private string _connectionString;
        private string _migrationAssemblyName;

        public ApiContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
            }

            base.OnConfiguring(dbContextOptionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<WebCamImage>()
                .HasKey(e => e.Id);
        }

        public DbSet<WebCamImage> WebCamImages { get; set; }
    }
}
