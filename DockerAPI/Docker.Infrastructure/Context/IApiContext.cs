using Docker.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
namespace Docker.Infrastructure.Context
{
    public interface IApiContext
    {
        DbSet<WebCamImage> WebCamImages { get; set; }
    }
}
