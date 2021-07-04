using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Simple.Caching.API.Interfaces;
using Simple.Caching.API.Models;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Simple.Caching.API.Areas.Caches.V1
{
    [Route("api/[area]/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [Area("Caches")]
    [ApiController]
    [Produces("application/json")]
    public class CacheController : Controller
    {
        private readonly IDatabase _cacheDatabase = null;
        private readonly IAzureRedisCacheHandler _azureRedisCacheHandler = null;

        public CacheController(IAzureRedisCacheHandler azureRedisCacheHandler)
        {
            _azureRedisCacheHandler = azureRedisCacheHandler;
            _cacheDatabase = _azureRedisCacheHandler.GetDatabase();
        }

        [HttpGet("{cacheKey}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetItemByCacheKey([FromRoute] string cacheKey)
        {
            var result = await _cacheDatabase.StringGetAsync(cacheKey);
            var jObject = JsonConvert.DeserializeObject(result.ToString());

            return Ok(jObject);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AddCacheItem([FromBody] CacheItem cacheItem)
        {
            var result = await _cacheDatabase.StringSetAsync(cacheItem.CacheKey, (RedisValue)JsonConvert.SerializeObject(cacheItem.CacheValue));
            return Created("", result);
        }
    }
}
