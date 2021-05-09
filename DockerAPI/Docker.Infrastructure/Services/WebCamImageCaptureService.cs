using Docker.Infrastructure.UnitOfWorks;
using Docker.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Docker.Infrastructure.Entities;

namespace Docker.Infrastructure.Services
{
    public class WebCamImageCaptureService : IWebCamImageCaptureService
    {
        private readonly IApiUnitOfWork _apiUnitOfWork;
        private readonly IMapper _mapper;

        public WebCamImageCaptureService(IApiUnitOfWork apiUnitOfWork, IMapper mapper)
        {
            _apiUnitOfWork = apiUnitOfWork;
            _mapper = mapper;
        }

        public IList<WebCamImageQueryDTO> GetWebCamImages()
        {
            var data = _apiUnitOfWork.WebCamImageRepository.GetAll();

            var result = _mapper.Map<IList<WebCamImage>, IList<WebCamImageQueryDTO>>(data);

            return result;
        }

        public WebCamImageQueryDTO GetWebCamImage(Guid id)
        {
            var data = _apiUnitOfWork.WebCamImageRepository.GetById(id);

            if (data == null)
                throw new InvalidOperationException($"No data found with id: {id}");

            var result = _mapper.Map<WebCamImage, WebCamImageQueryDTO>(data); ;

            return result;
        }

        public async Task<bool> SyncLocalWebCamImageData(WebCamImageInsertDTO imageData)
        {
            _apiUnitOfWork.WebCamImageRepository.SyncLocalWebCamImageData(imageData);
            await _apiUnitOfWork.SaveChangesAsync();
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

        public async Task<IList<UserWebCamImageQueryDTO>> GetUserWebCamImageData() => 
            await _apiUnitOfWork.WebCamImageRepository.GetUserWebCamImageData();

        public WebCamImageQueryDTO GetWebCamImageByName(string imageName)
        {
            var data = _apiUnitOfWork.WebCamImageRepository
                .Get(x=>x.ImageName.ToLower().Equals(imageName.ToLower()))
                .FirstOrDefault();

            if (data == null)
                throw new InvalidOperationException($"No data found with name: {imageName}");

            var result = _mapper.Map<WebCamImage, WebCamImageQueryDTO>(data); ;

            return result;
        }

        public async Task<bool> UpdateLocalWebCamImageData(WebCamImageUpdateDTO imageData)
        {
            _apiUnitOfWork.WebCamImageRepository.UpdateWebCamImageData(imageData);
            await _apiUnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
