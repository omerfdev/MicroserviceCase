using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.RabbitMQ;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly IRabbitMQProducer _rabbitMQProducer;


        public OrderController(OrderDbContext orderDbContext, IRabbitMQProducer rabbitMQProducer)
        {
            _orderDbContext = orderDbContext;
            _rabbitMQProducer = rabbitMQProducer;
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
            _rabbitMQProducer.SendMessage(order);
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
