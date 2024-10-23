using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;
using Projeto.ViewModels;

namespace Projeto.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public INotyfService _notification { get; }

        public BlogController(ApplicationDbContext context,
                              UserManager<ApplicationUser> userManager,
                              INotyfService notification)
        {
            _context = context;
            _notification = notification;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Post(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                _notification.Error("Post não encontrado.");
                return View();
            }

            var post = await _context.Posts!
                .Include(x => x.Ratings)
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Slug == slug);

            if (post == null)
            {
                _notification.Error("Post não encontrado.");
                return View();
            }

            // Verifique se o post é privado e se o usuário não está autenticado
            var user = await _userManager.GetUserAsync(User);
            if (!post.IsPublic && user == null)
            {
                // Se o post for privado e o usuário não estiver logado, redirecionar para a página de login
                _notification.Error("Você precisa estar logado para aceder este post.");
                return RedirectToAction("Login", "User", new { area = "Admin" });
            }

            // Calcular a média de ratings do post
            double? averageRating = post.Ratings?.Any() == true ? post.Ratings.Average(r => r.Value) : null;

            int? userRating = null;

            if (user != null)
            {
                var rating = post.Ratings?.FirstOrDefault(r => r.ApplicationUserId == user.Id);
                if (rating != null)
                {
                    userRating = rating.Value;
                }
            }

            var vm = new BlogPostVM
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.ApplicationUser?.FirstName + " " + post.ApplicationUser?.LastName,
                CreatedDate = post.CreatedAt,
                ThumbnailUrl = post.ThumbnailUrl,
                Description = post.Description,
                shortDescription = post.ShortDescription,
                AverageRating = averageRating,
                UserRating = userRating
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Post(int postId, int value)
        {
            if (value < 1 || value > 5)
            {
                _notification.Error("Avaliação inválida. Deve estar entre 1 e 5.");
                return RedirectToAction("Post", new { slug = postId });
            }

            var post = await _context.Posts!.FindAsync(postId);
            var user = await _userManager.GetUserAsync(User);

            if (post == null || user == null)
            {
                _notification.Error("Erro ao avaliar o post.");
                return RedirectToAction("Post", new { slug = postId });
            }

            // Verificar se o usuário já avaliou o post
            var existingRating = await _context.Rating
                .FirstOrDefaultAsync(r => r.PostId == postId && r.ApplicationUserId == user.Id);

            if (existingRating != null)
            {
                // Atualizar avaliação existente
                existingRating.Value = value;
                existingRating.RatedAt = DateTime.Now;
                _context.Rating.Update(existingRating);
            }
            else
            {
                var rating = new Rating
                {
                    PostId = postId,
                    ApplicationUserId = user.Id,
                    Value = value,
                    RatedAt = DateTime.Now
                };
                _context.Rating.Add(rating);
            }

            await _context.SaveChangesAsync();

            _notification.Success("Avaliação registrada com sucesso.");
            return RedirectToAction("Post", new { slug = post.Slug });
        }
    }
}
