using Docker.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.Infrastructure.Context
{
    public interface IApiContext
    {
        DbSet<WebCamImage> WebCamImages { get; set; }
    }
}
