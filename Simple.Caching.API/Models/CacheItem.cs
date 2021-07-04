using System.ComponentModel.DataAnnotations;

namespace Simple.Caching.API.Models
{
    public class CacheItem
    {
        [Required]
        public string CacheKey { get; set; }
        [Required]
        public dynamic CacheValue { get; set; }
    }
}
