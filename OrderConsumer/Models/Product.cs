using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderConsumer.Models
{
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
