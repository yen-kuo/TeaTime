using Microsoft.AspNetCore.Mvc;
using TeaTime.Api.Controllers;
using TeaTime.Api.DataAccess.DbEntity;
using TeaTime.Api.DataAccess;
using TeaTime.Api.Domain.Orders;
using TeaTime.Api.Domain.OrdersForUser;
using TeaTime.Api.DataAccess.Repository;

namespace TeaTime.Api.Service
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _repository;
        private readonly ILogger<OrdersService> _logger;
        private readonly IStoresService _storesService;

        public OrdersService(IOrdersRepository repository, ILogger<OrdersService> logger, IStoresService storesService)
        {
            _repository = repository;
            _logger = logger;
            _storesService = storesService;
        }

        public IEnumerable<Order> GetOrders(long storeId)
        {
            return _repository.GetOrders(storeId);
        }

        public Order? GetOrder(long storeId, long id)
        {
            // 先檢查商家是否存在
            if (!_storesService.IsStoreExist(storeId))
            {
                _logger.LogWarning("商家代號 {storeId} 不存在", storeId);
                return null;
            }

            // 再檢查訂單是否存在且屬於該商家
            var order = _repository.GetOrder(storeId, id);

            if (order is null)
            {
                _logger.LogWarning("訂單代號 {id} 不存在或不屬於商家", id);
                return null;
            }

            return order;
        }

        public Order AddOrderAndReturn(long storeId, OrderForUser newOrder)
        {
            // 先檢查商家是否存在
            if (!_storesService.IsStoreExist(storeId))
            {
                _logger.LogWarning("商家代號 {storeId} 不存在", storeId);
            }

            return _repository.AddOrderAndReturn(storeId, newOrder);
        }
    }
}
