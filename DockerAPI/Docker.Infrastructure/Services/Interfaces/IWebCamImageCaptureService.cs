using Docker.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Services
{
    public interface IWebCamImageCaptureService
    {
        IEnumerable<WebCamImageQueryDTO> GetWebCamImages();
        WebCamImageQueryDTO GetWebCamImage(Guid id);
        bool SyncLocalWebCamImageData(WebCamImageInsertDTO imageData);
        bool DeleteWebCamImageRecord(Guid id);
    }
}
