using TeaTime.Api.Domain.Orders;
using TeaTime.Api.Domain.OrdersForUser;

namespace TeaTime.Api.Service
{
    public interface IOrdersService
    {
        IEnumerable<Order> GetOrders(long storeId);

        Order? GetOrder(long storeId, long id);

        Order AddOrderAndReturn(long storeId, OrderForUser newOrder);
    }
}
