using BitocCore1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace BitocCore1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        IPairsProvider pairsProvider;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            pairsProvider = new BitcoinService(memoryCache);
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            IEnumerable<string> pairs = pairsProvider.GetPairs(page);

            var pager = new Pager((int)(pairsProvider.Count()), page);
            var viewModel = new IndexViewModel
            {
                Items = pairs,
                Pager = pager
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}