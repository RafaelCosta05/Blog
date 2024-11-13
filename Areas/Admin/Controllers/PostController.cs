using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto.Data;
using Projeto.ViewModels;
using Projeto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projeto.Utilites;
using Microsoft.Extensions.Hosting;

namespace Projeto.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApplicationDbContext context,
                                INotyfService notification,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        //#-----------------------------------#
        //#               Index               #
        //#-----------------------------------#
        public async Task<IActionResult> Index()
        {
            //criação da lista
            var listOfPosts = new List<Post>();
            //Usuário logado
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            //Role do usuário logado (lista de roles)
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            //se o primeira linha da lista for admin ou editor deixa ver todos os posts
            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUserRole[0] == WebsiteRoles.WebsiteEditor)
            {
                listOfPosts = await _context.Posts!
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Ratings)
                    .ToListAsync();
            }
            //se ao for nem admin nem editor vai ser author entao
            //so deixa ver os posts criados pelo utilizador
            else
            {
                listOfPosts = await _context.Posts!
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Ratings)
                    //este where faz uma comparação com o criador do post
                    //se for igual mostra o post 
                    .Where(x => x.ApplicationUser!.Id == loggedInUser!.Id)
                    .ToListAsync();
            }

            var listofPostsVM = listOfPosts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedAt,
                ThumbnailUrl = x.ThumbnailUrl,
                AuthorName = x.ApplicationUser!.FirstName + " " + x.ApplicationUser!.LastName,
                //calcula media das avaliações do post
                //senao houver avaliações retorna null
                AverageRating = x.Ratings != null && x.Ratings.Any()
                    ? x.Ratings.Average(r => r.Value)
                    : (double?)null,
                IsPublic = x.IsPublic,
                Slug = x.Slug
                
            }).ToList();

            return View(listofPostsVM);
        }


        //#-----------------------------------#
        //#               Create              #
        //#-----------------------------------#
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            //verificação se os dados estao dentro dos requisitos
            //senao envia erros e devolve a pagina com os dados que ja estao corretos
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _notification.Error("Erro");
                    }
                }
                return View(vm);
            }

            //buscar user logado ou seja quem esta a criar o post
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            //cria um novo objeto do tipo post
            var post = new Post
            {
                Title = vm.Title,
                Description = vm.Description,
                ShortDescription = vm.ShortDescription,
                ApplicationUserId = loggedInUser!.Id,
                IsPublic = vm.IsPublic // Definindo a privacidade
            };


            if (post.Title != null)
            {
                string slug = vm.Title!.Trim().Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();
            _notification.Success("Post criado com sucesso");

            return RedirectToAction("Index");
        }


        //#-----------------------------------#
        //#               Delete              #
        //#-----------------------------------#
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == id);

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUserRole[0] == WebsiteRoles.WebsiteEditor 
                ||  loggedInUser?.Id == post?.ApplicationUserId)
            {
                _context.Posts!.Remove(post!);
                await _context.SaveChangesAsync();
                _notification.Success("Post eliminado com sucesso");
                return RedirectToAction("Index", "Post", new { area = "Admin" });
            }

            return View();
        }

        //#-----------------------------------#
        //#                Edit               #
        //#-----------------------------------#
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var post = await _context.Posts!
                .FirstOrDefaultAsync(x => x.Id == id);

            // Se o post não for encontrado, exibe um erro e retorna a view
            if (post == null)
            {
                _notification.Error("Não foi encontrado esse Post");
                return View();
            }

            // Obtém o usuário logado
            var loggedInUser = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            // Obtém o papel (role) do usuário logado
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            // Se o usuário não for admin ou editor, verifica se ele é o autor do post
            // Se não for, redireciona para a página de acesso negado
            if (!(loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUserRole[0] == WebsiteRoles.WebsiteEditor)
                && loggedInUser?.Id != post?.ApplicationUserId)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            // Cria um ViewModel com as informações do post para editar
            var vm = new CreatePostVM()
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Description = post.Description,
                ThumbnailUrl = post.ThumbnailUrl,
                IsPublic = post.IsPublic,
                Slug = post.Slug,  // Passa o Slug para o ViewModel
            };

            // Retorna a view com o ViewModel
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CreatePostVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == vm.Id);

            if (post == null)
            {
                _notification.Error("Post não encontrado");
                return View();
            }

            //vai rescrever os dados que recebeu do viewModel no Model
            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.Description = vm.Description;
            post.IsPublic = vm.IsPublic;

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = vm.ThumbnailUrl;
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Post editado com sucesso");
            return RedirectToAction("Index", "Post", new { area = "Admin"} );
        }

        //#-----------------------------------#
        //#               Image               #
        //#-----------------------------------#
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";

            //combina o caminho do root mais das thumbnails
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Thumbnails/Posts");

            //da o nome ao ficheiro com um GUID mais o nome do ficheiro
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            //o caminho da imagem vai ser a folder
            //mais o nome
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            //retorna o uniqueFileName pois e o que vai salvar na base de dados
            return uniqueFileName;
        }
    }
}
