using Docker.Core;
using Docker.Infrastructure.Context;
using Docker.Infrastructure.Entities;
using Docker.Infrastructure.Repositories;

namespace Docker.Infrastructure.UnitOfWorks
{
    public interface IApiUnitOfWork : IUnitOfWork
    {
        IWebCamImageRepository WebCamImageRepository { get; set; }
    }

    public class ApiUnitOfWork : UnitOfWork, IApiUnitOfWork
    {
        public IWebCamImageRepository WebCamImageRepository { get; set; }

        public ApiUnitOfWork(ApiContext dbContext,
            IWebCamImageRepository webCamImageRepository)
            : base(dbContext)
        {
            WebCamImageRepository = webCamImageRepository;
        }
  
    }
}