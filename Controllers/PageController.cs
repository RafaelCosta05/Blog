using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.ViewModels;

namespace Projeto.Controllers
{

    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> About1()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "about");

            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };

            return View(vm);
        }

        public async Task<IActionResult> Contact1()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "contact");

            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };

            return View(vm);
        }

        public async Task<IActionResult> PrivacyPolicy1()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "privacy");

            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };

            return View(vm);
        }
    }
}
