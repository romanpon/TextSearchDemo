using Microsoft.AspNetCore.Mvc;
using TextSearchDemo.Models;
using TextSearchDemo.Trie.Interfaces;

namespace TextSearchDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ITrieService _trieService;

        public SearchController(ITrieService trieService)
        {
            _trieService = trieService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Child>), 200)]
        public async Task<IActionResult> Search([FromQuery] string text)
        {
            var result = await _trieService.SearchAsync(text, CancellationToken.None);
            return Ok(result);
        }
    }
}
