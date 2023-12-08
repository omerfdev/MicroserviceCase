using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OrderConsumer.Models
{
    
    public class Order
    {

        
        [Column("order_id")]
        public Guid Id { get; set; }
        [Required]
        [Column("customer_id")]
        public Guid CustomerId { get; set; }
        [Required]
        [Column("order_quantity")]
        public int Quantity { get; set; }
        [Required]
        [Column("order_price")]
        public double Price { get; set; }
        [Required]
        [Column("order_status")]
        public string Status { get; set; }
        [Required]
        [Column("order_address")]
        public Address Address { get; set; }
        [Required]
        [Column("product")]
        public Product Product { get; set; }
        [Required]
        [Column("order_createdAt")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column("order_UpdatedAt")]
        public DateTime UpdatedAt { get; set; }
        public Guid AddressId { get; set; }
        public Guid ProductId { get; set; }
        [Column("order_message")]
        public string Message { get; set; }
        [Column("order_receivedAt")]
        public DateTime ReceivedAt { get; set; }
    }
}
