using TeaTime.Api.DataAccess.DbEntity;
using TeaTime.Api.Domain.Stores;
using TeaTime.Api.Domain.StoresForUser;

namespace TeaTime.Api.DataAccess.Repostiory
{
    public class InMemoryStoresRepository : IStoresRepository
    {
        private readonly InMemoryContext _context;

        public InMemoryStoresRepository(InMemoryContext context)
        {
            _context = context;
        }

        public IEnumerable<Store> GetStores()
        {
            var results = _context.Stores.ToList();

            var stores = new List<Store>();
            foreach (var result in results)
            {
                stores.Add(new Store
                {
                    Id = result.Id,
                    Name = result.Name,
                    PhoneNumber = result.PhoneNumber,
                    MenuUrl = result.MenuUrl
                });
            }

            return stores;
        }

        public Store? GetStore(long id)
        {
            var result = _context.Stores.Find(id);

            if (result is null)
            {
                return null;
            }

            var store = new Store
            {
                Id = result.Id,
                Name = result.Name,
                PhoneNumber = result.PhoneNumber,
                MenuUrl = result.MenuUrl
            };

            return store;
        }

        public Store AddStoreAndReturn(StoreForUser newStore)
        {
            var entity = new StoreEntity
            {
                Name = newStore.Name,
                PhoneNumber = newStore.PhoneNumber,
                MenuUrl = newStore.MenuUrl
            };

            _context.Stores.Add(entity);
            _context.SaveChanges();

            var storeForReturn = new Store
            {
                Id = entity.Id,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber,
                MenuUrl = entity.MenuUrl
            };

            return storeForReturn;
        }

        public bool IsStoreExist(long id)
        {
            return _context.Stores.Find(id) != null;
        }
    }
}
