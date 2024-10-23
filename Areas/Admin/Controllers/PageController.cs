using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using Projeto.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;
using Projeto.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;

namespace Projeto.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Editor")]
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PageController(ApplicationDbContext context, 
                                INotyfService notification, 
                                IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
        }
        //#-----------------------------------#
        //#            About Us               #
        //#-----------------------------------#
        [HttpGet]
        public async Task<IActionResult> About()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "about");

            if (page == null)
            {
                _notification.Error("Página não encontrada");
                return RedirectToAction("Index", "User");
            }

            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> About(PageVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "about");

            if (page == null)
            {
                _notification.Error("Página não encontrada");
                return RedirectToAction("Index", "User");
            }

            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;
            //page.ThumbnailUrl = vm.ThumbnailUrl;

            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = vm.ThumbnailUrl;
                page.ThumbnailUrl = UploadImageAbout(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();
            _notification.Success("Página About Us editada com sucesso");
            return RedirectToAction("Index", "Opening", new { area = "Admin" });
        }

        //#-----------------------------------#
        //#            Contact Us             #
        //#-----------------------------------#
        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "contact");

            if (page == null)
            {
                _notification.Error("Página não encontrada");
                return RedirectToAction("Index", "User");
            }

            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(PageVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "contact");

            if (page == null)
            {
                _notification.Error("Página não encontrada");
                return RedirectToAction("Index", "User");
            }

            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = vm.ThumbnailUrl;
                page.ThumbnailUrl = UploadImageContact(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();
            _notification.Success("Página Contact Us editada com sucesso");
            return RedirectToAction("Index", "Opening", new { area = "Admin" });
        }

        //#-----------------------------------#
        //#           Privacy Policy          #
        //#-----------------------------------#
        [HttpGet]
        public async Task<IActionResult> Privacy()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "privacy");

            if (page == null)
            {
                _notification.Error("Página não encontrada");
                return RedirectToAction("Index", "User");
            }

            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Privacy(PageVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "privacy");

            if (page == null)
            {
                _notification.Error("Página não encontrada");
                return RedirectToAction("Index", "User");
            }

            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = vm.ThumbnailUrl;
                page.ThumbnailUrl = UploadImagePrivacy(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();
            _notification.Success("Página Privacy Policy editada com sucesso");
            return RedirectToAction("Index", "Opening", new { area = "Admin" });
        }

        //#-----------------------------------#
        //#               Image               #
        //#-----------------------------------#
        private string UploadImageAbout(IFormFile file)
        {
            string uniqueFileName = "";

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Thumbnails/Page/About");

            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);

            }
            return uniqueFileName;
        }

        private string UploadImageContact(IFormFile file)
        {
            string uniqueFileName = "";

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Thumbnails/Page/Contact");

            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);

            }
            return uniqueFileName;
        }

        private string UploadImagePrivacy(IFormFile file)
        {
            string uniqueFileName = "";

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Thumbnails/Page/Privacy");

            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);

            }
            return uniqueFileName;
        }

    }
}
