using TeaTime.Api.Domain.Stores;
using TeaTime.Api.Domain.StoresForUser;

namespace TeaTime.Api.Service
{
    public interface IStoresService
    {
        IEnumerable<Store> GetStores();

        Store? GetStoreAndReturn(long id);

        Store AddStore(StoreForUser newStore);

        bool IsStoreExist(long id);
    }
}
