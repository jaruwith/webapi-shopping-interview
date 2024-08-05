using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webapi_shopping_interview.Model
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }
    }
}