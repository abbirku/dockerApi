using Docker.Core;
using Docker.Infrastructure.Context;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Repositories
{
    public interface IWebCamImageRepository : IRepository<WebCamImage, Guid, ApiContext>
    {
        void SyncLocalWebCamImageData(WebCamImageInsertDTO imageData);
        void UpdateWebCamImageData(WebCamImageUpdateDTO imageData);
        Task<IList<UserWebCamImageQueryDTO>> GetUserWebCamImageData();
    }
}
