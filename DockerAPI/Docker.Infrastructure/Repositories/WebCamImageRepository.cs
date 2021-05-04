using Docker.Core;
using Docker.Infrastructure.Context;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.Entities;
using System;

namespace Docker.Infrastructure.Repositories
{
    public class WebCamImageRepository : Repository<WebCamImage, Guid, ApiContext>, IWebCamImageRepository
    {

        public WebCamImageRepository(ApiContext dbContext)
            : base(dbContext)
        {}

        public void SyncLocalWebCamImageData(WebCamImageInsertDTO imageData)
        {
            if (imageData.Image == null)
                throw new ArgumentNullException("Provided image is null");

            if(Get(x=>x.ImageName.Equals(imageData.Image.ImageName)).Count == 0)
            {
                Add(new WebCamImage
                {
                    UserId = imageData.Image.UserId,
                    CaptureTime = imageData.Image.CaptureTime,
                    ImageName = imageData.Image.ImageName
                });
            }
        }
    }

}
