using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.EntityFramework;
using Projeto.Data;
using Projeto.Models;
using Projeto.ViewModels;
using System.Diagnostics;

namespace Projeto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,
                                ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? year, int? month)
        {
            // Instancia o ViewModel
            var vm = new HomeVM();

            //vai buscar as informações do site a base de dados
            var setting = await _context.Settings!.FirstOrDefaultAsync();
            if (setting != null)
            {
                vm.Title = setting.Title;
                vm.ShortDescription = setting.ShortDescription;
                vm.ThumbnailUrl = setting.ThumbnailUrl;
            }

            //vai buscar os posts todos a base de dados
            //ordena pela data de criação
            var allPosts = await _context.Posts!
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            //cria um dicionário de anos de todos os posts (allposts)
            //criado em cima

            //onde agrupa pelos anos (g.key)
            //e depois seleciona o mes de cada post no grupo de cada ano
            //o distinct serve para nao repetir anos, e no final cria uma lista

            //{
            //  2023: [1, 2, 3],
            //  2022: [5, 6]
            //}
            //gera algo deste género
            vm.AvailableYears = allPosts
                .GroupBy(x => x.CreatedAt.Year)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.CreatedAt.Month).Distinct().ToList()
                );

            // Aplicar os filtros de ano e mês (se houver)
            var filteredPostsQuery = _context.Posts!
                .Include(x => x.ApplicationUser)
                .Include(x => x.Ratings)
                .OrderByDescending(x => x.CreatedAt)
                .AsQueryable();

            //verifica se a funçao recebeu valores
            //nos parametros do ano e mes
            //em caso de receber, rescreve os dados da pesquisa 
            if (year.HasValue)
            {
                filteredPostsQuery = filteredPostsQuery.Where(x => x.CreatedAt.Year == year.Value);
                if (month.HasValue)
                {
                    filteredPostsQuery = filteredPostsQuery.Where(x => x.CreatedAt.Month == month.Value);
                }
            }

            //vai buscar todos os posts filtrados (sem paginação)
            var filteredPosts = await filteredPostsQuery.ToListAsync();

            // Mapeamento para PostVM
            vm.Posts = filteredPosts.Select(post => new PostVM
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.ApplicationUser?.FirstName + " " + post.ApplicationUser?.LastName,
                CreatedDate = post.CreatedAt,
                ThumbnailUrl = post.ThumbnailUrl,
                Slug = post.Slug,
                ShortDescription = post.ShortDescription,
                ApplicationUser = post.ApplicationUser,
                AverageRating = post.Ratings != null && post.Ratings.Any()
                    ? post.Ratings.Average(r => r.Value)
                    : (double?)null,
                IsPublic = post.IsPublic
            }).ToList();

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
