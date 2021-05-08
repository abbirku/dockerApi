using System;
using System.Net.Http;

namespace Docker.Infrastructure.DataModel
{
    public class WebCamImageModel
    {
        public Guid UserId { get; set; }
        public string ImageName { get; set; }
        public DateTime CaptureTime { get; set; }
        public string Image { get; set; }
    }
}
