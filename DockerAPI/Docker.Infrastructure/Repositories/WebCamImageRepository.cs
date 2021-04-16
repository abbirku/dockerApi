using Docker.Core;
using Docker.Infrastructure.Context;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.Entities;
using Docker.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Repositories
{
    public class WebCamImageRepository : Repository<WebCamImage, Guid, ApiContext>, IWebCamImageRepository
    {

        public WebCamImageRepository(ApiContext dbContext)
            : base(dbContext)
        {}

        public void SyncLocalWebCamImageData(string rootPath, WebCamImageInsertDTO imageData)
        {
            if (imageData.Image == null)
                throw new ArgumentNullException("Provided image is null");

            Add(new WebCamImage
            {
                CaptureTime = imageData.Image.CaptureTime,
                ImageName = imageData.Image.ImageName
            });
        }
    }

}
