using Microsoft.AspNetCore.Mvc;
using TeaTime.Api.DataAccess;
using TeaTime.Api.DataAccess.DbEntities;
using TeaTime.Api.Domain.Orders;

namespace TeaTime.Api.Controllers
{
    [Route("api/stores/{storeId}/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly TeaTimeContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(TeaTimeContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/stores/1/orders
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders(long storeId)
        {
            var results = _context.Orders.Where(o => o.StoreId == storeId).ToList();

            var orders = new List<Order>();
            foreach (var result in results)
            {
                orders.Add(new Order
                {
                    Id = result.Id,
                    UserName = result.UserName,
                    ItemName = result.ItemName
                });
            }

            return Ok(orders);
        }

        // GET: api/stores/1/orders/1
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(long storeId, long id)
        {
            // 先檢查商家是否存在
            var store = _context.Stores.Find(storeId);
            if (store is null)
            {
                _logger.LogWarning("商家代號 {storeId} 不存在", storeId);
                return NotFound();
            }


            // 再檢查訂單是否存在且屬於該商家
            var result = _context.Orders.Find(id);

            if (result is null || result.StoreId != storeId)
            {
                _logger.LogWarning("訂單代號 {id} 不存在或不屬於商家", id);
                return NotFound();
            }

            var order = new Order
            {
                Id = result.Id,
                UserName = result.UserName,
                ItemName = result.ItemName,
                Price = 0 // TODO: 從商品資料表中取得價格
            };

            return Ok(order);
        }

        // POST: api/stores/1/orders
        [HttpPost]
        public IActionResult AddOrder(long storeId, [FromBody] OrderForCreation newOrder)
        {
            // 先檢查商家是否存在
            var store = _context.Stores.Find(storeId);
            if (store is null)
            {
                _logger.LogWarning("商家代號 {storeId} 不存在，無法新增訂單", storeId);
                return BadRequest("無法新增訂單，請與維護人員聯繫");
            }

            var maxId =
                _context.Orders.Where(o => o.StoreId == storeId).Any() ?
                    _context.Orders.Where(o => o.StoreId == storeId).Max(o => o.Id) : 0;

            var entity = new OrderEntity
            {
                Id = maxId + 1,
                StoreId = storeId,
                UserName = newOrder.UserName,
                ItemName = newOrder.ItemName
            };

            _context.Orders.Add(entity);
            _context.SaveChanges();

            var orderForReturn = new Order
            {
                Id = entity.Id,
                UserName = entity.UserName,
                ItemName = entity.ItemName,
                Price = 0 // TODO: 從商品資料表中取得價格
            };

            return CreatedAtAction(nameof(GetOrder), new { storeId, id = entity.Id }, orderForReturn);
        }
    }
}
