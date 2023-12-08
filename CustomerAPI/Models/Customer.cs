using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerAPI.Models
{
    [Table("Customer", Schema = "dbo")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("customer_id")]
        public Guid Id { get; set; }
        [Required]
        [Column("customer_name")]
        public string Name { get; set; }
        [Required]
        [Column("customer_email")]
        public string Email { get; set; }
        [Required]
        [Column("customer_address")]
        public Address Address { get; set; }
        [Column("customer_addressId")]
        public Guid AddressId { get; set; }
        [Required]
        [Column("customer_createdAt")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column("customer_updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
