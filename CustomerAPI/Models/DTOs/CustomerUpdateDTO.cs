using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CustomerAPI.ExtensionValidation;

namespace CustomerAPI.Models.DTOs
{
    public class CustomerUpdateDTO
    {
        [NoDigits]
        public string Name { get; set; }
        [Email]
        public string Email { get; set; }        
        public Address Address { get; set; }       
    
    }
}
