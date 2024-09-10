using Dapper;
using TeaTime.Api.DataAccess.DbEntity;
using TeaTime.Api.Domain.Stores;
using TeaTime.Api.Domain.StoresForUser;

namespace TeaTime.Api.DataAccess.Repostiory
{
    public class OracleStoresRepository : IStoresRepository
    {
        private readonly OracleDbContext _context;

        public OracleStoresRepository(OracleDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Store> GetStores()
        {
            using var connection = _context.GetConnection();

            var sql = "SELECT ID, NAME, PHONENUMBER, MENUURL FROM TEATIME_STORE";
            var results = connection.Query<StoreEntity>(sql);

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
            using var connection = _context.GetConnection();

            var sql = "SELECT ID, NAME, PHONENUMBER, MENUURL FROM TEATIME_STORE WHERE ID = :Id";
            var result = connection.QueryFirstOrDefault<StoreEntity>(sql, new { Id = id });

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
            using var connection = _context.GetConnection();

            // 先取得 Id 的最大值
            var sql1 = "SELECT MAX(ID) FROM TEATIME_STORE";
            var maxId = connection.ExecuteScalar<long>(sql1);
            var newId = maxId + 1;

            var entity = new StoreEntity
            {
                Id = newId,
                Name = newStore.Name,
                PhoneNumber = newStore.PhoneNumber,
                MenuUrl = newStore.MenuUrl
            };

            var sql2 = "INSERT INTO TEATIME_STORE (ID, NAME, PHONENUMBER, MENUURL) VALUES (:Id, :Name, :PhoneNumber, :MenuUrl)";

            connection.Execute(sql2, new
            {
                entity.Id,
                entity.Name,
                entity.PhoneNumber,
                entity.MenuUrl
            });


            var storeForReturn = new Store
            {
                Id = newId,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber,
                MenuUrl = entity.MenuUrl
            };

            return storeForReturn;
        }

        public bool IsStoreExist(long id)
        {
            using var connection = _context.GetConnection();

            var sql = "SELECT COUNT(*) FROM TEATIME_STORE WHERE ID = :Id";
            var count = connection.ExecuteScalar<int>(sql, new { Id = id });

            return count > 0;
        }
    }
}
