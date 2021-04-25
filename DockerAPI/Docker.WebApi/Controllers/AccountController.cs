using Docker.Infrastructure.DataModel;
using Docker.Membership.DTO;
using Docker.Membership.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService,
            ILogger<AccountController> logger)
        {
            _logger = logger;
            _accountService = accountService;
        }
        //Dev fix
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.RegisterUserAsync(model);
                    var data = new ResultModel<bool>
                    {
                        Message = result ? "Registration was successful" : "Registration was unsuccessful",
                        Success = result
                    };

                    if (result)
                        return Ok(data);

                    return BadRequest(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.StackTrace);

                    var result = new ResultModel<bool>
                    {
                        Message = ex.Message,
                        Success = false
                    }; ;

                    return BadRequest(result);
                }
            }

            return BadRequest("Some properties are not valid");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.LoginUserAsync(model);
                    var data = new ResultModel<AuthenticationResponseDTO>
                    {
                        Data = result,
                        Message = "Login Successful",
                        Success = true
                    };

                    return Ok(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.StackTrace);

                    var result = new ResultModel<bool>
                    {
                        Message = ex.Message,
                        Success = false
                    }; ;

                    return BadRequest(result);
                }
            }

            return BadRequest("Some properties are not valid");
        }
    }
}
