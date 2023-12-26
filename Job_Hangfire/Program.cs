using Hangfire;
using Hangfire.MemoryStorage;
using Job_Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});

GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
builder.Services.AddHangfireServer(options =>
{
    options.ServerName = "hangfireServiceName";
    options.WorkerCount = 4;
    options.SchedulePollingInterval = TimeSpan.FromSeconds(5);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHangfireDashboard();
app.MapHangfireDashboard();


try
{
    RecurringJob.AddOrUpdate<HangfireJob2>(job =>
                                             job.ExecuteAsync(),
                                             cronExpression: "*/10 * * * * *",
                                             TimeZoneInfo.Utc);

    RecurringJob.AddOrUpdate<HangfireJob>(job =>
                                                 job.ExecuteAsync(),
                                                 cronExpression: "* * * * * *",
                                                 TimeZoneInfo.Utc);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

app.UseHttpsRedirection();


app.Run();