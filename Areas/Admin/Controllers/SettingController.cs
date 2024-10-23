using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;
using Projeto.ViewModels;

namespace Projeto.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin, Editor")]
    public class SettingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingController(ApplicationDbContext context, 
                                    INotyfService notification,
                                    IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = _context.Settings!.ToList();

            if(settings.Count > 0)
            {
                var vm = new SettingVM()
                {
                    Id = settings[0].Id,
                    SiteName = settings[0].SiteName,
                    Title = settings[0].Title,
                    ShortDescription = settings[0].ShortDescription,
                    ThumbnailUrl = settings[0].ThumbnailUrl,
                    FacebookUrl = settings[0].FacebookUrl,
                    TwitterUrl = settings[0].TwitterUrl,
                    GithubUrl = settings[0].GithubUrl,
                };
                return View(vm);
            }

            var setting = new Setting()
            {
                SiteName = "Demo Name"
            };

            await _context.Settings!.AddAsync(setting);
            await _context.SaveChangesAsync();

            var createdSettings = _context.Settings!.ToList();
            
            var createdVm = new SettingVM()
            {
                Id = createdSettings[0].Id,
                SiteName = createdSettings[0].SiteName,
                Title = createdSettings[0].Title,
                ShortDescription = createdSettings[0].ShortDescription,
                ThumbnailUrl = createdSettings[0].ThumbnailUrl,
                FacebookUrl = createdSettings[0].FacebookUrl,
                TwitterUrl = createdSettings[0].TwitterUrl,
                GithubUrl = createdSettings[0].GithubUrl,
            };
            
            return View(createdVm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SettingVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            var setting = await _context.Settings!.FirstOrDefaultAsync(x=> x.Id == vm.Id);

            if (setting == null)
            {
                _notification.Error("Something went wrong");
                return View(vm);
            }

            setting.SiteName = vm.SiteName;
            setting.Title = vm.Title;
            setting.ShortDescription = vm.ShortDescription;
            setting.FacebookUrl = vm.FacebookUrl;
            setting.TwitterUrl = vm.TwitterUrl;
            setting.GithubUrl = vm.GithubUrl;

            if (vm.Thumbnail != null)
            {
                setting.ThumbnailUrl = vm.ThumbnailUrl;
                setting.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Settings atualizadas com sucesso");
            return RedirectToAction("Index", "Opening", new { area = "Admin" });
        }

        //#-----------------------------------#
        //#               Image               #
        //#-----------------------------------#
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Thumbnails/Page/Home");

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
