using System.Text.Json;
using System.Text;
using TeaTime.Api.Domain.Stores;
using TeaTime.Api.DataAccess.Repository;
using TeaTime.Api.Domain.Orders;
using TeaTime.Api.Domain.OrdersForUser;
using Microsoft.EntityFrameworkCore;
using TeaTime.Api.DataAccess.DbEntity;

namespace TeaTime.Api.DataAccess.Repostiory
{
    public class ApiOrdersRepository : IOrdersRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        private static readonly JsonSerializerOptions _propertyNameCaseInsensitive = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiOrdersRepository(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"]!;

            // 增加 header
            var tokenHeader = configuration["ApiSettings:TokenHeader"]!;
            var secretToken = configuration["ApiSettings:SecretToken"]!;
            httpClient.DefaultRequestHeaders.Add(tokenHeader, secretToken);
        }
        public IEnumerable<Order> GetOrders(long storeId)
        {
            var response = _httpClient.GetAsync($"{_baseUrl}/stores/{storeId}/orders").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var orders = JsonSerializer.Deserialize<IEnumerable<Order>>(content, _propertyNameCaseInsensitive);

            if (orders is not null)
            {
                return orders;
            }

            return Enumerable.Empty<Order>();
        }

        public Order? GetOrder(long storeId, long id)
        {
            var response = _httpClient.GetAsync($"{_baseUrl}/stores/{storeId}/orders/{id}").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var order = JsonSerializer.Deserialize<Order>(content, _propertyNameCaseInsensitive);

            return order;
        }

        public Order AddOrderAndReturn(long storeId, OrderForUser newOrder)
        {
            var json = JsonSerializer.Serialize(newOrder);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync($"{_baseUrl}/stores/{storeId}/orders", content).Result;
            response.EnsureSuccessStatusCode();

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var order = JsonSerializer.Deserialize<Order>(responseContent, _propertyNameCaseInsensitive);

            return order!;
        }
    }
}
