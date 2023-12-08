using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerAPI.Models
{
    public class Address
    {
        [Key]
        [Column("address_id")]
        public Guid AddressId { get; set; }
        [Required]
        [Column("address_line")]
        public string AddressLine { get; set; }
        [Required]
        [Column("address_city")]
        public string City { get; set; }
        [Required]
        [Column("address_country")]
        public string Country { get; set; }
        [Required]
        [Column("address_cityCode")]
        public int CityCode { get; set; }
        
    
    }
}
