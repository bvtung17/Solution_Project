using Microsoft.AspNetCore.Mvc;

namespace WebhookApi.Controllers
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

        [HttpPost()]
        public async Task<object> Get()
        {
            string rqBody;
            using (var stream = new StreamReader(Request.Body))
            {
                rqBody = await stream.ReadToEndAsync();
            }

            return rqBody;
        }
    }
}
