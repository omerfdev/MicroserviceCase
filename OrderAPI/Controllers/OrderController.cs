using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderAPI.Models.DTOs;
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
            return _orderDbContext.Orders.Include(o => o.Address).Include(o => o.Product).ToList();
        }

        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<Order>> GetByIdOrder(Guid orderId)
        {
            return await _orderDbContext.Orders.Include(o => o.Product).Include(o => o.Address).FirstOrDefaultAsync(x => x.Id == orderId);

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
        [HttpPut("{orderId:guid}")]
        public async Task<ActionResult<Order>> UpdateOrder(Guid orderId,OrderUpdateDTO orderUpdate)
        {
            Order order = await _orderDbContext.Orders.Include(c => c.Address).Include(o=>o.Product).FirstOrDefaultAsync(x => x.Id == orderId);
            order.Address.AddressLine = orderUpdate.Address.AddressLine;
            order.Address.Country= orderUpdate.Address.Country;
            order.Address.City= orderUpdate.Address.City;
            order.Address.CityCode= orderUpdate.Address.CityCode;
            order.Price = orderUpdate.Price;
            order.Quantity = orderUpdate.Quantity;
            order.Status = orderUpdate.Status;
            order.Product.Name= orderUpdate.Product.Name;
            order.Product.ImageUrl= orderUpdate.Product.ImageUrl;
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

        [HttpPut("ChangeStatus/{orderId:guid}")]
        public async Task<ActionResult<bool>> ChangeStatus(Guid orderId, string orderStatus)
        {
            var order = await _orderDbContext.Orders.FindAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = orderStatus;

            try
            {
                await _orderDbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }



    }
}
