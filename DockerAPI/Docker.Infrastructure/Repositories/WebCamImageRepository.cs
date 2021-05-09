using Docker.Core;
using Docker.Infrastructure.Context;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Repositories
{
    public class WebCamImageRepository : Repository<WebCamImage, Guid, ApiContext>, IWebCamImageRepository
    {

        public WebCamImageRepository(ApiContext dbContext)
            : base(dbContext)
        {}

        public async Task<IList<UserWebCamImageQueryDTO>> GetUserWebCamImageData()
        {
            var result = await QueryWithStoredProcedureAsync<UserWebCamImageQueryDTO>("GetUserWebCamImages", null, null);

            return result.result;
        }

        public void SyncLocalWebCamImageData(WebCamImageInsertDTO imageData)
        {
            if (imageData == null)
                throw new ArgumentNullException("Provided image is null");

            if(Get(x=>x.ImageName.Equals(imageData.ImageName)).Count == 0)
            {
                Add(new WebCamImage
                {
                    UserId = imageData.UserId,
                    CaptureTime = imageData.CaptureTime,
                    ImageName = imageData.ImageName
                });
            }
        }
    }

}
