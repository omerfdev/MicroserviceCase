using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OrderConsumer.Models
{
    [Table("order")]
    public class Order
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("ordered_on")]
        public DateTime OrderedOn { get; set; }
        public string Message { get; set; }
        public DateTime ReceivedAt { get; set; }


        public List<OrderDetail> OrderDetails { get; set; }
    }
}
