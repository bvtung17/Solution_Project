namespace Job_Hangfire
{
    public class HangfireJob2
    {
        private int executionCount = 0;
        private readonly ILogger<HangfireJob2> _logger;
        public IServiceProvider _services { get; }
        public HangfireJob2(ILogger<HangfireJob2> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Job 2 execute {0}", executionCount);

            await DoWork(executionCount);
            executionCount++;
        }

        private async Task DoWork(int executionCount)
        {
            await Console.Out.WriteLineAsync($"Job 22222: {executionCount}");
        }

    }
}
