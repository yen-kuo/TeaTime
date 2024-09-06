using Microsoft.AspNetCore.Mvc;
using TeaTime.Api.DataAccess;
using TeaTime.Api.DataAccess.DbEntities;
using TeaTime.Api.Domain.Stores;

namespace TeaTime.Api.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly TeaTimeContext _context;

        public StoresController(TeaTimeContext context)
        {
            _context = context;
        }

        // GET: api/stores
        [HttpGet]
        public ActionResult<IEnumerable<Store>> GetStores()
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

            return Ok(stores);
        }

        // GET: api/stores/1
        [HttpGet("{id}")]
        public ActionResult<Store> GetStore(long id)
        {
            var result = _context.Stores.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            var store = new Store
            {
                Id = result.Id,
                Name = result.Name,
                PhoneNumber = result.PhoneNumber,
                MenuUrl = result.MenuUrl
            };

            return Ok(store);
        }

        // POST: api/stores
        [HttpPost]
        public IActionResult AddStore(StoreForCreation newStore)
        {
            var maxId = _context.Stores.Any() ? _context.Stores.Max(s => s.Id) : 0;
            var entity = new StoreEntity
            {
                Id = maxId + 1,
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

            return CreatedAtAction(nameof(GetStore), new { id = entity.Id }, storeForReturn);
        }
    }
}
