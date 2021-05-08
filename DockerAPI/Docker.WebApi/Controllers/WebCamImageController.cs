using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.Infrastructure.DataModel;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Docker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "CommonPolicy")]

    public class WebCamImageController : ControllerBase
    {
        private readonly ILogger<WebCamImageController> _logger;
        private readonly IWebCamImageCaptureService _webCamImageCaptureService;

        public WebCamImageController(IWebCamImageCaptureService webCamImageCaptureService,
            ILogger<WebCamImageController> logger)
        {
            _webCamImageCaptureService = webCamImageCaptureService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = new ResultModel<IEnumerable<WebCamImageQueryDTO>>
                {
                    Data = _webCamImageCaptureService.GetWebCamImages(),
                    Message = string.Empty,
                    Success = true
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);

                var result = new ResultModel<IEnumerable<WebCamImageQueryDTO>>
                {
                    Data = new List<WebCamImageQueryDTO>(),
                    Message = ex.Message,
                    Success = false
                };

                return BadRequest(result);
            }
        }

        [HttpGet("GetUserWebCamImages")]
        public async Task<IActionResult> GetUserWebCamImages()
        {
            try
            {
                var result = new ResultModel<IEnumerable<UserWebCamImageQueryDTO>>
                {
                    Data = await _webCamImageCaptureService.GetUserWebCamImageData(),
                    Message = string.Empty,
                    Success = true
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);

                var result = new ResultModel<IEnumerable<UserWebCamImageQueryDTO>>
                {
                    Data = new List<UserWebCamImageQueryDTO>(),
                    Message = ex.Message,
                    Success = false
                };

                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var result = new ResultModel<WebCamImageQueryDTO>
                {
                    Data = _webCamImageCaptureService.GetWebCamImage(id),
                    Message = string.Empty,
                    Success = true
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                var result = new ResultModel<WebCamImageQueryDTO>
                {
                    Data = new WebCamImageQueryDTO(),
                    Message = "Error Occurred",
                    Success = false
                };

                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(WebCamImageInsertDTO data)
        {
            try
            {
                await _webCamImageCaptureService.SyncLocalWebCamImageData(data);

                var result = new ResultModel<WebCamImageQueryDTO>
                {
                    Data = null,
                    Message = "Insert successful",
                    Success = true
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                var result = new ResultModel<WebCamImageQueryDTO>
                {
                    Data = new WebCamImageQueryDTO(),
                    Message = "Error Occurred",
                    Success = false
                };

                return BadRequest(result);
            }
        }

        //[HttpPut("{id}")]
        //public IActionResult Put(Guid id, [FromBody] WebCamImageUpdatetDTO value)
        //{
        //    return Ok();
        //}

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var result = _webCamImageCaptureService.DeleteWebCamImageRecord(id);
                var finalResult = new ResultModel<WebCamImageQueryDTO>
                {
                    Data = null,
                    Message = result ? string.Empty : "Error Occurred",
                    Success = result
                };

                return Ok(finalResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                var result = new ResultModel<WebCamImageQueryDTO>
                {
                    Data = new WebCamImageQueryDTO(),
                    Message = "Error Occurred",
                    Success = false
                };

                return BadRequest(result);
            }
        }
    }
}
