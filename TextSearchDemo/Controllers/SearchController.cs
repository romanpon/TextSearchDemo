using Microsoft.AspNetCore.Mvc;
using TextSearchDemo.Interfaces;

namespace TextSearchDemo.Controllers
{
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string text, CancellationToken cancellationToken)
        {
            var result = await searchService.Search(text, cancellationToken);
            return Ok(result);
        }
    }
}
