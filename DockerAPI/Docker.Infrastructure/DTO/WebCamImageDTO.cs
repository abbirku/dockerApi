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
        public string ImageName { get; set; }
        public DateTime CaptureTime { get; set; }
    }
}
