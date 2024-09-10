using TeaTime.Api.DataAccess.DbEntity;
using TeaTime.Api.DataAccess.Repository;
using TeaTime.Api.Domain.Orders;
using TeaTime.Api.Domain.OrdersForUser;

namespace TeaTime.Api.DataAccess.Repostiory
{
    public class InMemoryOrdersRepository : IOrdersRepository
    {
        private readonly InMemoryContext _context;

        public InMemoryOrdersRepository(InMemoryContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrders(long storeId)
        {
            var results = _context.Orders.Where(o => o.StoreId == storeId).ToList();

            var orders = new List<Order>();
            foreach (var result in results)
            {
                orders.Add(new Order
                {
                    Id = result.Id,
                    UserName = result.UserName,
                    ItemName = result.ItemName,
                    Price = 0 // TODO: 從商品資料表中取得價格
                });
            }

            return orders;
        }

        public Order? GetOrder(long storeId, long id)
        {
            var result = _context.Orders.Find(id);

            if (result is null || result.StoreId != storeId)
            {
                return null;
            }

            var order = new Order
            {
                Id = result.Id,
                UserName = result.UserName,
                ItemName = result.ItemName,
                Price = 0 // TODO: 從商品資料表中取得價格
            };

            return order;
        }

        public Order AddOrderAndReturn(long storeId, OrderForUser newOrder)
        {
            var entity = new OrderEntity
            {
                StoreId = storeId,
                UserName = newOrder.UserName,
                ItemName = newOrder.ItemName
            };

            _context.Orders.Add(entity);
            _context.SaveChanges();

            var order = new Order
            {
                Id = entity.Id,
                UserName = entity.UserName,
                ItemName = entity.ItemName,
                Price = 0 // TODO: 從商品資料表中取得價格
            };

            return order;
        }
    }
}
