using Docker.Core;
using System;

namespace Docker.Infrastructure.Entities
{
    public class WebCamImage : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string ImageName { get; set; }
        public DateTime CaptureTime { get; set; }
    }
}
