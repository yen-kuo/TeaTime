using TeaTime.Api.DataAccess.DbEntity;
using TeaTime.Api.DataAccess;
using TeaTime.Api.Domain.Stores;
using TeaTime.Api.Domain.StoresForUser;
using TeaTime.Api.DataAccess.Repostiory;

namespace TeaTime.Api.Service
{
    public class StoresService : IStoresService
    {
        private readonly IStoresRepository _repository;
        private readonly ILogger<StoresService> _logger;

        public StoresService(IStoresRepository repository, ILogger<StoresService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IEnumerable<Store> GetStores()
        {
            return _repository.GetStores();
        }

        public Store? GetStoreAndReturn(long id)
        {
            var store = _repository.GetStore(id);

            if (store is null)
            {
                _logger.LogWarning("商家代號 {storeId} 不存在", id);
                return null;
            }

            return store;
        }

        public Store AddStore(StoreForUser newStore)
        {
            return _repository.AddStoreAndReturn(newStore);
        }

        public bool IsStoreExist(long id)
        {
            var isStoreExists = _repository.IsStoreExist(id);

            if (!isStoreExists)
            {
                _logger.LogWarning("商家代號 {storeId} 不存在", id);
            }

            return isStoreExists;
        }
    }
}
