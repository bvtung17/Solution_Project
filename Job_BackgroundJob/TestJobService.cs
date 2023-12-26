using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class TestJobService : BackgroundService
{
    private int executionCount = 0;
    private readonly ILogger<TestJobService> _logger;
    public IServiceProvider _services { get; }
    public TestJobService(ILogger<TestJobService> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Job execute {0}", executionCount);

        while (!stoppingToken.IsCancellationRequested)
        {
            await DoWork(executionCount, stoppingToken);
            await Task.Delay(1000);
            executionCount++;
        }
    }

    private async Task DoWork(int executionCount, CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is working.");
        await Console.Out.WriteLineAsync($"Xin chaooo user : {executionCount}");
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
