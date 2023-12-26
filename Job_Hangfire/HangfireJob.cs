namespace Job_Hangfire
{
    public class HangfireJob
    {
        private int executionCount = 0;
        private readonly ILogger<HangfireJob> _logger;
        public IServiceProvider _services { get; }
        public HangfireJob(ILogger<HangfireJob> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Job execute {0}", executionCount);

            await DoWork(executionCount);
            executionCount++;
        }

        private async Task DoWork(int executionCount)
        {
            await Console.Out.WriteLineAsync($"Xin chaooo user : {executionCount}");
        }

    }
}
