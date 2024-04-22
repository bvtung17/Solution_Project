using Microsoft.AspNetCore.Mvc;
using RedisDb.Services;

namespace RedisDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRedisService redisService;

        public WeatherForecastController(IRedisService redisService)
        {
            this.redisService = redisService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            await redisService.GetValue("test");
            return Ok();
        }

        [HttpPost(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Set()
        {
            await redisService.SetValue("test", "value");

            return Ok();
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            await redisService.GetAllValue("test");
            return Ok();
        }

        [HttpPost("set-value")]
        public async Task<IActionResult> SetWithHash(string value)
        {
            await redisService.SetValue("test", Guid.NewGuid().ToString(), value);

            return Ok();
        }
    }
}
