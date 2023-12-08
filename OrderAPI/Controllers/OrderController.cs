using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return _orderDbContext.Orders.Include(o=>o.Address).Include(o=>o.Product).ToList();
        }

        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<Order>> GetByIdOrder(Guid orderId)
        {
            return await _orderDbContext.Orders.Include(o=>o.Product).Include(o=>o.Address).FirstOrDefaultAsync(x=>x.Id == orderId);
      
        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            order.CreatedAt = DateTime.Now;
            _orderDbContext.Orders.AddAsync(order);           
            await _orderDbContext.SaveChangesAsync();
            _rabbitMQProducer.SendMessage(order);
            return Ok();
        }
        [HttpPut]//id ileyap
        public async Task<ActionResult<Order>> UpdateOrder(Order order)
        {
            order.UpdatedAt= DateTime.Now;  
            _orderDbContext.Orders.Update(order);
            await _orderDbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{orderId:guid}")]
        public async Task<ActionResult<Order>> DeleteOrder(Guid orderId)
        {
            var product = await _orderDbContext.Orders.FindAsync(orderId);
            _orderDbContext.Orders.Remove(product);
            await _orderDbContext.SaveChangesAsync();
            return Ok();
        }


    }
}
