using CustomerAPI.DBContext;
using CustomerAPI.Models;
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
                // Handle the case when the customer with the given Id is not found
                return NotFound();
            }

            // Your logic to handle the found customer
            // For example, you can return the customer as part of the response
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

        [HttpPut]
        public async Task<ActionResult<Customer>> UpdateCustomer(Customer updatedCustomer)
        {
            updatedCustomer.UpdatedAt= DateTime.Now;
            _customerDbContext.Customers.Update(updatedCustomer);
            await _customerDbContext.SaveChangesAsync();
            
            return Ok();
        }



        [HttpDelete("{customerId}")]
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

    }
}
