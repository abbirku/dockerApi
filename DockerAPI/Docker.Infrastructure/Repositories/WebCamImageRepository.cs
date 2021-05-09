using AutoMapper;
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
        private readonly IMapper _mapper;

        public WebCamImageRepository(ApiContext dbContext, IMapper mapper)
            : base(dbContext) => _mapper = mapper;

        public async Task<IList<UserWebCamImageQueryDTO>> GetUserWebCamImageData()
        {
            var result = await QueryWithStoredProcedureAsync<UserWebCamImageQueryDTO>("GetUserWebCamImages", null, null);

            return result.result;
        }

        public void SyncLocalWebCamImageData(WebCamImageInsertDTO imageData)
        {
            if (imageData == null)
                throw new ArgumentNullException("Provided image is null");

            if (Get(x => x.ImageName.ToLower().Equals(imageData.ImageName.ToLower())).Count == 0)
            {
                var webCamImageData = _mapper.Map<WebCamImageInsertDTO, WebCamImage>(imageData);
                Add(webCamImageData);
            }
            else
                throw new InvalidOperationException("Can not insert duplicate image name");
        }

        public void UpdateWebCamImageData(WebCamImageUpdateDTO imageData)
        {
            if (imageData == null)
                throw new ArgumentNullException("Provided image is null");

            if (GetById(imageData.Id) != null)
            {
                var webCamImageData = _mapper.Map<WebCamImageUpdateDTO, WebCamImage>(imageData);
                Edit(webCamImageData);
            }
            else
                throw new InvalidOperationException($"No data exists with Id: {imageData.Id}");
        }
    }

}
