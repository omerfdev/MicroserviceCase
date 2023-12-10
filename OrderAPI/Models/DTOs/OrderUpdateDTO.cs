using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTOs
{
    public class OrderUpdateDTO
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public Address Address { get; set; }
        public Product Product { get; set; }
    }
}
