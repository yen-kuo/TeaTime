using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeaTime.Api.DataAccess;
using TeaTime.Api.DataAccess.DbEntity;
using TeaTime.Api.Domain.Stores;
using TeaTime.Api.Domain.StoresForUser;
using TeaTime.Api.Service;

namespace TeaTime.Api.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoresService _service;

        public StoresController(IStoresService service)
        {
            _service = service;
        }

        // GET: api/Stores
        [HttpGet]
        public ActionResult<IEnumerable<StoreEntity>> GetStores()
        {
            var stores = _service.GetStores();

            return Ok(stores);
        }

        // GET: api/Stores/1
        [HttpGet("{id}")]
        public ActionResult<StoreEntity> GetStore(long id)
        {
            var store = _service.GetStoreAndReturn(id);

            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }

        // POST: api/Stores
        [HttpPost]
        public IActionResult AddStore([FromBody] StoreForUser newStore)
        {
            var storeForReturn = _service.AddStore(newStore);
            return CreatedAtAction(nameof(GetStore), new { id = storeForReturn.Id }, storeForReturn);
        }
    }
}
