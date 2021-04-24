using Docker.Core;
using Docker.Infrastructure.Context;
using System;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Seed
{
    public class WebCamSeed : DataSeed<ApiContext>
    {
        public WebCamSeed(ApiContext apiContext)
            : base(apiContext)
        { }

        public override Task SeedAsync()
        {
            throw new NotImplementedException();
        }
    }
}
