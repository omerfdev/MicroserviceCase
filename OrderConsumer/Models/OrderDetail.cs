using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderConsumer.Models
{
    [Table("orderDetail")]
    public class OrderDetail
    {
        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }     
        [Column("quantity")]
        public decimal Quantity { get; set; }
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }
    }
}
