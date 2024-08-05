namespace webapi_shopping_interview.Model
{
    public class Stock
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Navigation property
        public Product Product { get; set; } = null!;
    }
}
