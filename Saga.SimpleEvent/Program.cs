class Program
{
    static void Main()
    {
        // Tạo một đơn hàng mới
        var order = new Order { OrderId = 1, Status = "Mới" };

        // Đăng ký sự kiện cho từng bước trong saga
        order.OrderEventOccurred += (sender, e) => Console.WriteLine($"Trạng thái đơn hàng: {order.Status}. Thông báo: {e.Message}");

        // Triển khai saga
        order.ConfirmOrder();
        order.ProcessPayment();
        order.ShipOrder();
        order.CompleteOrder();
    }
}


public class OrderEvent
{
    public string Message { get; set; }
}

// Lớp đơn hàng chứa thông tin về đơn hàng và quản lý sự kiện saga
public class Order
{
    public int OrderId { get; set; }
    public string Status { get; set; }

    public event EventHandler<OrderEvent> OrderEventOccurred;

    protected virtual void OnOrderEventOccurred(OrderEvent e)
    {
        OrderEventOccurred?.Invoke(this, e);
    }

    // Phương thức để thực hiện bước xác nhận đơn hàng
    public void ConfirmOrder()
    {
        Status = "Đã xác nhận";
        OnOrderEventOccurred(new OrderEvent { Message = "Đơn hàng đã được xác nhận." });
    }

    // Phương thức để thực hiện bước thanh toán
    public void ProcessPayment()
    {
        Status = "Đang xử lý thanh toán";
        OnOrderEventOccurred(new OrderEvent { Message = "Thanh toán đang được xử lý." });
    }

    // Phương thức để thực hiện bước vận chuyển sản phẩm
    public void ShipOrder()
    {
        Status = "Đang vận chuyển";
        OnOrderEventOccurred(new OrderEvent { Message = "Sản phẩm đang được vận chuyển." });
    }

    // Phương thức để thực hiện bước hoàn thành đơn hàng
    public void CompleteOrder()
    {
        Status = "Hoàn thành";
        OnOrderEventOccurred(new OrderEvent { Message = "Đơn hàng đã hoàn thành." });
    }
}
