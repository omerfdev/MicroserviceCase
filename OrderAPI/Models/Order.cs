

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models
{
    
    public class Order
    {
        [Key]
        [Column("order_id")]
        public string OrderId { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("ordered_on")]
        public DateTime OrderedOn { get; set; }
      
        public List<OrderDetail> OrderDetails{ get; set; }
    }
}
