using Microsoft.AspNetCore.Mvc;
using TextSearchDemo.Interfaces;

namespace TextSearchDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery(Name = "text")] string text)
        {
            var result = await searchService.Search(text, CancellationToken.None);
            return Ok(result);
        }
    }
}
