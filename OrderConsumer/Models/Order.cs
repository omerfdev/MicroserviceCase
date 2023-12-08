using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OrderConsumer.Models
{
    
    public class Order
    {
        [Key]
        public int OrderLogId { get; set; }
        public string Message { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}
