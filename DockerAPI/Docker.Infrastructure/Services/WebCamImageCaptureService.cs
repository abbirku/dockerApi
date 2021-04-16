using Docker.Infrastructure.UnitOfWorks;
using Docker.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Services
{
    public class WebCamImageCaptureService : IWebCamImageCaptureService
    {
        private readonly IApiUnitOfWork _apiUnitOfWork;

        public WebCamImageCaptureService(IApiUnitOfWork apiUnitOfWork)
        {
            _apiUnitOfWork = apiUnitOfWork;
        }

        public IEnumerable<WebCamImageQueryDTO> GetWebCamImages()
        {
            var data = _apiUnitOfWork.WebCamImageRepository.GetAll();

            var result = data.Select(x => new WebCamImageQueryDTO
            {
                Id = x.Id,
                CaptureTime = x.CaptureTime,
                ImageName = x.ImageName
            });

            return result;
        }

        public WebCamImageQueryDTO GetWebCamImage(Guid id)
        {
            var data = _apiUnitOfWork.WebCamImageRepository.GetById(id);

            if (data == null)
                throw new InvalidOperationException($"No data found with id: {id}");

            var result = new WebCamImageQueryDTO
            {
                Id = data.Id,
                CaptureTime = data.CaptureTime,
                ImageName = data.ImageName
            };

            return result;
        }

        public bool SyncLocalWebCamImageData(string rootPath, WebCamImageInsertDTO imageData)
        {
            _apiUnitOfWork.WebCamImageRepository.SyncLocalWebCamImageData(rootPath, imageData);
            _apiUnitOfWork.SaveChanges();
            return true;
        }

        public bool DeleteWebCamImageRecord(Guid id)
        {
            var data = _apiUnitOfWork.WebCamImageRepository.GetById(id);

            if (data == null)
                throw new InvalidOperationException($"No data found with id: {id}");

            _apiUnitOfWork.WebCamImageRepository.Remove(data);
            _apiUnitOfWork.SaveChanges();
            return true;
        }
    }
}
