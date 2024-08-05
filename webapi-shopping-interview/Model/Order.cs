namespace webapi_shopping_interview.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = null!;
    }
}