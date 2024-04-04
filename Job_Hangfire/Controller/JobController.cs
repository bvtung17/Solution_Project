using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Job_Hangfire.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public JobController(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetRecurringJob()
        {
            List<string> recurringJobs = new List<string>();

            // Lấy danh sách tất cả các công việc lặp lại
            IEnumerable<RecurringJobDto> recurringJobDtos = JobStorage.Current.GetConnection().GetRecurringJobs();

            foreach (var recurringJobDto in recurringJobDtos)
            {
                recurringJobs.Add(recurringJobDto.Id);
            }

            return Ok(recurringJobs);
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public IActionResult CreateRecurringJob(string cron)
        {
            int timeInSeconds = 30;

            // Replace "HangfireJob2" with your job method name
            // Replace "YourHangfireJobClass" with your Hangfire job class name
            _recurringJobManager.AddOrUpdate<HangfireJob2>("HangfireJob2",
                job => job.ExecuteAsync(),
                cron);

            return Ok($"Recurring job created. Next execution in {timeInSeconds} seconds!");
        }


        //POST: HangfireController
        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteJob(string jobId)
        {
            int timeInSeconds = 30;

            _recurringJobManager.RemoveIfExists(jobId);

            return Ok($"Job ID:. Discount email em {timeInSeconds} segundos!");
        }

    }
}
