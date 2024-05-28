using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

int hangTonKho = 10;
int slBanra = 0;
int soluongDataHang = 0;

app.MapGet("/buy", () =>
{
    LockExtension.ProtectAction("userid", string.Empty, 5);
    if (hangTonKho - slBanra > 0)
    {

        Console.WriteLine($"order thanh cong {soluongDataHang}, ban ra {slBanra}");

        slBanra++;
        soluongDataHang++;

        return $"order thanh cong {soluongDataHang}, ban ra {slBanra}";
    }
    else
    {
        Console.WriteLine("Het hang");
        return "het hang";
    }

})
.WithName("Test")
.WithOpenApi();

app.Run();

internal static class LockExtension
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, long>> ProtectActionData = new ConcurrentDictionary<string, ConcurrentDictionary<string, long>>();
    private static readonly ConcurrentDictionary<string, object> ActionLocks = new ConcurrentDictionary<string, object>();

    /// <summary>
    /// ProtectAction only use for single host application
    /// </summary>
    public static void ProtectAction(string action, string entity, int circleInSeconds = 60)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var circleInMillis = circleInSeconds * 1000;

        // Get or add a lock object for the action
        var actionLock = ActionLocks.GetOrAdd(action, _ => new object());

        lock (actionLock)
        {
            var entityRequests = ProtectActionData.GetOrAdd(action, _ => new ConcurrentDictionary<string, long>());

            if (entityRequests.TryGetValue(entity, out var lastRequestTime))
            {
                var timeSinceLastRequest = now - lastRequestTime;

                if (timeSinceLastRequest < circleInMillis)
                {
                    var remainSeconds = (circleInMillis - timeSinceLastRequest) / 1000;
                    throw new BadRequestException($"Sorry, too many attempts. Please try again in {remainSeconds} seconds.");
                }

                // Update the request time
                entityRequests[entity] = now;
            }
            else
            {
                entityRequests.TryAdd(entity, now);
            }
        }
    }
}
