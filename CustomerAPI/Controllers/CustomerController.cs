using CustomerAPI.DBContext;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc;

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
           return _customerDbContext.Customers;
        }

        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<Customer>> GetByIdCustomer(int customerId)
        {
            return await _customerDbContext.Customers.FindAsync(customerId);
        }
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
             await _customerDbContext.Customers.AddAsync(customer);
             await _customerDbContext.SaveChangesAsync();   
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<Customer>> UpdateCustomer(Customer customer)
        {
            _customerDbContext.Customers.Update(customer);
            await _customerDbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{customerId:int}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int customerId)
        {
            var customer = await _customerDbContext.Customers.FindAsync(customerId);
            _customerDbContext.Customers.Remove(customer);
            await _customerDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
