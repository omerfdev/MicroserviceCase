using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _orderDbContext;

        public OrderController(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return _orderDbContext.Orders;
        }

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<Order>> GetByIdOrder(int orderId)
        {
            return await _orderDbContext.Orders.FindAsync(orderId);
        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            _orderDbContext.Orders.AddAsync(order);
            await _orderDbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<Order>> UpdateOrder(Order order)
        {
            _orderDbContext.Orders.Update(order);
            await _orderDbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{orderId:int}")]
        public async Task<ActionResult<Order>> DeleteOrder(int orderId)
        {
            var product = await _orderDbContext.Orders.FindAsync(orderId);
            _orderDbContext.Orders.Remove(product);
            await _orderDbContext.SaveChangesAsync();
            return Ok();
        }


    }
}
