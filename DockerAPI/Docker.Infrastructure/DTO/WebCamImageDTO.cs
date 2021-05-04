using Docker.Infrastructure.DataModel;
using System;

namespace Docker.Infrastructure.DTO
{
    public class WebCamImageInsertDTO
    {
        public WebCamImageModel Image { get; set; }
    }

    public class WebCamImageQueryDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ImageName { get; set; }
        public DateTime CaptureTime { get; set; }
    }

    public class UserWebCamImageQueryDTO
    {
        public Guid UserId { get; set; }
        public Guid WebCamImageId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ImageName { get; set; }
        public DateTime CaptureTime { get; set; }
    }
}
