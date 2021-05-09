using AutoMapper;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.Entities;

namespace Docker.Infrastructure.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Web Cam Image
            CreateMap<WebCamImageInsertDTO, WebCamImage>();
            CreateMap<WebCamImageUpdateDTO, WebCamImage>();
            CreateMap<WebCamImage, WebCamImageQueryDTO>();
            #endregion
        }
    }
}
