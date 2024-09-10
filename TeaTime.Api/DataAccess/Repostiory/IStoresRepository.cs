using TeaTime.Api.Domain.Stores;
using TeaTime.Api.Domain.StoresForUser;

namespace TeaTime.Api.DataAccess.Repostiory
{
    public interface IStoresRepository
    {
        IEnumerable<Store> GetStores();

        Store? GetStore(long id);

        Store AddStoreAndReturn(StoreForUser newStore);

        bool IsStoreExist(long id);
    }
}
