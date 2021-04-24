using Docker.Core;
using Docker.Infrastructure.Context;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.Entities;
using System;

namespace Docker.Infrastructure.Repositories
{
    public interface IWebCamImageRepository : IRepository<WebCamImage, Guid, ApiContext>
    {
        void SyncLocalWebCamImageData(WebCamImageInsertDTO imageData);
    }
}
