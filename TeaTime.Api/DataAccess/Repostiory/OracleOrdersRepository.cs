using TeaTime.Api.DataAccess.DbEntity;
using TeaTime.Api.Domain.Orders;
using TeaTime.Api.Domain.OrdersForUser;
using Dapper;
using TeaTime.Api.DataAccess.Repository;

namespace TeaTime.Api.DataAccess.Repostiory
{
    public class OracleOrdersRepository : IOrdersRepository
    {
        private readonly OracleDbContext _context;

        public OracleOrdersRepository(OracleDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrders(long storeId)
        {
            using var connection = _context.GetConnection();

            var sql = "SELECT ID, STOREID, USERNAME, ITEMNAME FROM TEATIME_ORDER WHERE STOREID = :StoreId";
            var results = connection.Query<OrderEntity>(sql, new { StoreId = storeId });

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
            using var connection = _context.GetConnection();

            var sql = "SELECT ID, STOREID, USERNAME, ITEMNAME FROM TEATIME_ORDER WHERE ID = :Id AND STOREID = :StoreId";
            var result = connection.QueryFirstOrDefault<OrderEntity>(sql, new { Id = id, StoreId = storeId });

            if (result is null)
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
            using var connection = _context.GetConnection();

            // 先取得 Id 的最大值
            var sql1 = "SELECT MAX(ID) FROM TEATIME_ORDER WHERE STOREID = :StoreId";
            var maxId = connection.QueryFirstOrDefault<long?>(sql1, new { StoreId = storeId });
            var newId = maxId ?? 0 + 1;

            var entity = new OrderEntity
            {
                Id = newId,
                StoreId = storeId,
                UserName = newOrder.UserName,
                ItemName = newOrder.ItemName
            };

            var sql2 = "INSERT INTO TEATIME_ORDER (ID, STOREID, USERNAME, ITEMNAME) VALUES (:Id, :StoreId, :UserName, :ItemName)";
            connection.Execute(sql2, entity);

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
