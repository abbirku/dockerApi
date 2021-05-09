using Docker.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Services
{
    public interface IWebCamImageCaptureService
    {
        IList<WebCamImageQueryDTO> GetWebCamImages();
        WebCamImageQueryDTO GetWebCamImage(Guid id);
        WebCamImageQueryDTO GetWebCamImageByName(string imageName);
        Task<bool> SyncLocalWebCamImageData(WebCamImageInsertDTO imageData);
        Task<bool> UpdateLocalWebCamImageData(WebCamImageUpdateDTO imageData);
        bool DeleteWebCamImageRecord(Guid id);
        Task<IList<UserWebCamImageQueryDTO>> GetUserWebCamImageData();
    }
}
