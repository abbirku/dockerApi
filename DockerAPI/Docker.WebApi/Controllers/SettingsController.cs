using Docker.Infrastructure.DataModel;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.SettingsModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
