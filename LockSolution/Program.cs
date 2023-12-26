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
    private static readonly Dictionary<string, ConcurrentDictionary<string, long>> ProtectActionData = new();
    public static void ProtectAction(string action, string entity, int circleInSeconds = 60)
    {
        var now = DateTime.UtcNow.Millisecond;
        lock (ProtectActionData)
        {
            if (ProtectActionData.TryGetValue(action, out var requests))
            {
                if (requests.TryGetValue(entity, out var last))
                {
                    if (last > now - TimeSpan.FromSeconds(circleInSeconds).TotalMilliseconds)
                    {
                        var remainSeconds = circleInSeconds - (now - last) / 1000;
                        throw new Exception($"Sorry, too many attempts. Please try again in {remainSeconds} seconds.");
                    }

                    requests.TryRemove(entity, out long time);

                    if (requests.IsEmpty)
                        ProtectActionData.Remove(action);
                }
                else
                {
                    requests.TryAdd(entity, now);
                }
            }
            else
            {
                var newRequests = new ConcurrentDictionary<string, long>();
                newRequests.TryAdd(entity, now);
                ProtectActionData.TryAdd(action, newRequests);
            }
        }
    }
}
