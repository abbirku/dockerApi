using Docker.Core;
using Docker.Infrastructure.Context;
using Docker.Infrastructure.DataModel;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Seed
{
    public class WebCamSeed : DataSeed<ApiContext>
    {
        private readonly IWebCamImageCaptureService _webCamImageCaptureService;

        public WebCamSeed(ApiContext apiContext,
            IWebCamImageCaptureService webCamImageCaptureService)
            : base(apiContext) => _webCamImageCaptureService = webCamImageCaptureService;

        public override async Task SeedAsync()
        {
            var webCamList = await BuildWebCamImageList();
            foreach (var item in webCamList)
                await _webCamImageCaptureService.SyncLocalWebCamImageData(item);
        }

        private async Task<List<WebCamImageInsertDTO>> BuildWebCamImageList()
        {
            return await Task.Run(() =>
            {
                var webCamImageList = new List<WebCamImageInsertDTO>()
                {
                    new WebCamImageInsertDTO{ Image = new WebCamImageModel { ImageName="Image1", CaptureTime = DateTime.Now } },
                    new WebCamImageInsertDTO{ Image = new WebCamImageModel { ImageName="Image2", CaptureTime = DateTime.Now } },
                    new WebCamImageInsertDTO{ Image = new WebCamImageModel { ImageName="Image3", CaptureTime = DateTime.Now } },
                    new WebCamImageInsertDTO{ Image = new WebCamImageModel { ImageName="Image4", CaptureTime = DateTime.Now } },
                    new WebCamImageInsertDTO{ Image = new WebCamImageModel { ImageName="Image5", CaptureTime = DateTime.Now } },
                };

                return webCamImageList;
            });
        }
    }
}
