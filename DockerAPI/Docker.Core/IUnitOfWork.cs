using System;
using System.Threading.Tasks;

namespace Docker.Core
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
