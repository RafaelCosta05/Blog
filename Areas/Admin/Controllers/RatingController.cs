using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projeto.Data;
using Projeto.Models;
using Microsoft.EntityFrameworkCore;
using Projeto.Utilites;
using Microsoft.AspNetCore.Authorization;
using Projeto.ViewModels;

namespace Projeto.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingController(ApplicationDbContext context,
                                INotyfService notification,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            //cria uma lista do tipo rating
            List<Rating> ratings;

            // Verifica se o usuário logado é Admin ou Editor
            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUserRole[0] == WebsiteRoles.WebsiteEditor)
            {
                // Admins e editores veem todos os ratings
                ratings = await _context.Rating!
                    .Include(r => r.Post)
                    .Include(r => r.ApplicationUser)
                    .ToListAsync();
            }
            else
            {
                // Autores veem apenas os ratings dos posts deles
                ratings = await _context.Rating!
                    .Include(r => r.Post)
                    .Include(r => r.ApplicationUser)
                    .Where(r => r.Post!.ApplicationUserId == loggedInUser!.Id)
                    .ToListAsync();
            }

            //manda os dados das avaliaçoes para o viewModel
            //isto e um array, onde depois na view usase um foreach
            //para imprimir todos os rating
            var ratingsVM = ratings.Select(r => new RatingVM
            {
                PostId = r.PostId,
                UserName = r.ApplicationUser != null ? r.ApplicationUser.FirstName + " " + r.ApplicationUser.LastName : "Usuário desconhecido",
                Value = r.Value,
                RatedAt = r.RatedAt
            }).ToList();

            //retorna a view
            return View(ratingsVM);
        }

    }
}
