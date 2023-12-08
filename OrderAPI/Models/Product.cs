

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Column("product_id")]
        public Guid Id { get; set; }
        [Required]
        [Column("product_image")]
        public string ImageUrl { get; set; }
        [Required]
        [Column("product_name")]
        public string Name { get; set; }
    }
}
