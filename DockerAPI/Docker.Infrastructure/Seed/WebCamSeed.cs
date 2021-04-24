using Docker.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Seed
{
    public class WebCamSeed : DataSeed
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
