using Docker.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Docker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GuessMe/{num}")]
        public ResultModel<int> Get(int num)
        {
            try
            {
                if (num <= 0)
                    throw new Exception("Provide number greater then zero");

                var rng = new Random();
                var result = rng.Next(num);

                return new ResultModel<int>
                {
                    Data = result,
                    Success = true,
                    Message = "Surprice"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResultModel<int>
                {
                    Data = 0,
                    Success = false,
                    Message = ex.GetBaseException().Message
                };
            }

        }
    }
}
