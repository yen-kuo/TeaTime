using Microsoft.AspNetCore.Mvc;
using TeaTime.Api.DataAccess;
using TeaTime.Api.DataAccess.DbEntity;
using TeaTime.Api.Domain.Orders;
using TeaTime.Api.Domain.OrdersForUser;
using TeaTime.Api.Domain.Stores;
using TeaTime.Api.Service;
namespace TeaTime.Api.Controllers
{
    [Route("api/stores/{storeId}/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IStoresService _storesService;

        public OrdersController(IOrdersService ordersService, IStoresService storesService)
        {
            _ordersService = ordersService;
            _storesService = storesService;
        }

        // GET: api/stores/1/orders
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders(long storeId)
        {
            var orders = _ordersService.GetOrders(storeId);

            return Ok(orders);
        }

        // GET: api/stores/1/orders/1
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(long storeId, long id)
        {
            var order = _ordersService.GetOrder(storeId, id);

            if (order is null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/stores/1/orders
        [HttpPost]
        public IActionResult AddOrder(long storeId, [FromBody] OrderForUser newOrder)
        {
            var orderForReturn = _ordersService.AddOrderAndReturn(storeId, newOrder);

            if (orderForReturn is null)
            {
                return BadRequest("無法新增訂單，請與維護人員聯繫");
            }

            return CreatedAtAction(nameof(GetOrder), new { storeId, id = orderForReturn.Id }, orderForReturn);
        }
    }
}
