using StackExchange.Redis;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Nhap vao gia tri");
            var input = Console.ReadLine();
            CheckLock(input);
            if (string.IsNullOrEmpty(input))
                break;
        }
    }

    static void CheckLock(string key)
    {
        // Kết nối đến Redis Server
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

        var productKey = $"product:{key}";
        var orderKey = $"{productKey}:orders";
        var lockKey = $"{productKey}:lock";

        try
        {
            // Kết nối với Redis
            var db = redis.GetDatabase();

            // Chờ và cố gắng khóa
            using (var lockWait = new ManualResetEventSlim())
            {
                var lockTaken = false;

                do
                {
                    lockTaken = db.LockTake(lockKey, Environment.MachineName, TimeSpan.FromSeconds(10));
                    if (!lockTaken)
                    {
                        // Nếu không thể lấy được khóa, chờ một khoảng thời gian rồi thử lại
                        lockWait.Wait(TimeSpan.FromMilliseconds(50));
                    }

                } while (!lockTaken);

                // Kiểm tra xem có đơn hàng nào trong khoảng thời gian ngắn không
                var existingOrder = db.StringGet(orderKey);
                if (!string.IsNullOrEmpty(existingOrder))
                {
                    Console.WriteLine("Someone else placed an order during this time. Please try again.");
                    return;
                }

                // Lưu thông tin đặt hàng của người dùng
                var orderTimestamp = DateTime.Now.ToString();
                db.StringSet(orderKey, orderTimestamp);

                // Giải phóng khóa
                db.LockRelease(lockKey, Environment.MachineName);
            }

            // Xác nhận đơn hàng
            Console.WriteLine("Order placed successfully!");
        }
        catch (Exception ex)
        {
            // Xử lý lỗi
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
