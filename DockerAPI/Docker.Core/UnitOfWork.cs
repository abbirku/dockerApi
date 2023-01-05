using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Docker.Core
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContext _dbContext;

        public UnitOfWork(DbContext dBContext) => _dbContext = dBContext;

        public int SaveChanges() => (int)_dbContext?.SaveChanges();

        public async Task<int> SaveChangesAsync() => await _dbContext?.SaveChangesAsync();

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
