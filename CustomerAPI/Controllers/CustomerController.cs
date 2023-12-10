
using CustomerAPI.DBContext;
using CustomerAPI.Models;
using CustomerAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDbContext _customerDbContext;      

        public CustomerController(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            var customersWithAddresses = _customerDbContext.Customers
      .Include(c => c.Address)
      .ToList();

            return customersWithAddresses;
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<Customer>> GetByIdCustomer(Guid customerId)
        {
            var customer = await _customerDbContext.Customers
                            .Include(c => c.Address)
                            .FirstOrDefaultAsync(x => x.Id == customerId);              
            if (customer == null)
            {               
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedAt = DateTime.Now;
                customer.UpdatedAt = DateTime.Now;
                await _customerDbContext.Customers.AddAsync(customer);
                await _customerDbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{customerId:guid}")]
        public async Task<ActionResult<Customer>> UpdateCustomer(Guid customerId,CustomerUpdateDTO updateCustomer)
        {
            Customer customer = await _customerDbContext.Customers.Include(c => c.Address).FirstOrDefaultAsync(x => x.Id == customerId);
            customer.UpdatedAt = DateTime.Now;
            customer.Name= updateCustomer.Name;
            customer.Email= updateCustomer.Email;
            customer.Address.AddressLine= updateCustomer.Address.AddressLine;
            customer.Address.City= updateCustomer.Address.City;
            customer.Address.CityCode= updateCustomer.Address.CityCode;
            await _customerDbContext.SaveChangesAsync();            
            return Ok();
        }

        [HttpDelete("{customerId:guid}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(Guid customerId)
        {
            var customer = await _customerDbContext.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            _customerDbContext.Customers.Remove(customer);
            await _customerDbContext.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpGet("Validate/{customerId:guid}")]
        public async Task<ActionResult<bool>> Validate(Guid customerId)
        {
            var customer = await _customerDbContext.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            return true;
        }


    }
}
