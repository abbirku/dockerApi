using Docker.Infrastructure.DataModel;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.SettingsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Docker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminPolicy")]

    public class SettingsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = new ResultModel<EmailSettingsDTO>
                {
                    Data = new EmailSettingsDTO
                    {
                        SMTPEmail = EmailSettings.SMTPEmail,
                        SMTPHostname = EmailSettings.SMTPHostname,
                        SMTPPassword = EmailSettings.SMTPPassword,
                        SMTPPort = EmailSettings.SMTPPort
                    },
                    Message = string.Empty,
                    Success = true
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);

                var result = new ResultModel<EmailSettingsDTO>
                {
                    Data = new EmailSettingsDTO(),
                    Message = ex.Message,
                    Success = false
                };

                return BadRequest(result);
            }
        }
    }
}
