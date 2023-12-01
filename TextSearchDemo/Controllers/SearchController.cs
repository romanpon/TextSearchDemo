using Microsoft.AspNetCore.Mvc;

namespace TextSearchDemo.Controllers
{
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;

        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string text, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
