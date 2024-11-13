using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Projeto.Data;
using Projeto.ViewModels;
using Projeto.Models;
using Projeto.Utilites;

namespace Projeto.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notification { get; }

        public UserController(UserManager<ApplicationUser> userManager,
                                ApplicationDbContext context,
                                IWebHostEnvironment webHostEnvironment,
                                SignInManager<ApplicationUser> signInManager,
                                INotyfService notification)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _notification = notification;
        }

        //#-----------------------------------#
        //#               Index               #
        //#-----------------------------------#
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var vm = users.Select(x => new UserVM()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
            }).ToList();

            //assinging role
            foreach(var user in vm)
            {
                var singleUser = await _userManager.FindByIdAsync(user.Id);
                var role = await _userManager.GetRolesAsync(singleUser);
                user.Role = role.FirstOrDefault();
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin)
            {
                return View(vm);
            }
            else
            {
                return RedirectToAction("Index", "Opening", new { area = "Admin" });
            }
        }

        //#-----------------------------------#
        //#             User-Info             #
        //#-----------------------------------#
        [HttpGet]
        public async Task<IActionResult> UserInfo(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
            {
                _notification.Error("Utilizador não existe");
                return View();
            }

            var role = await _userManager.GetRolesAsync(existingUser);
            var vm = new UserInfoVM
            {
                Id = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                UserName = existingUser.UserName,
                Email = existingUser.Email,
                PhoneNumber = existingUser.PhoneNumber,
                Role = role.FirstOrDefault(),
                ProfilePicturelUrl = existingUser.ProfilePictureUrl,
            };

            return View(vm);
        }




        //#-----------------------------------#
        //#           Reset Password          #
        //#-----------------------------------#
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
            {
                _notification.Error("Utilizador não existe");
                return View();             
            }

            var vm = new ResetPasswordVM()
            {
                Id = existingUser.Id,
                Username = existingUser.UserName
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                _notification.Error("Password não corresponde aos requisitos");
                return RedirectToAction("ResetPassword", "User");
            }

            var existingUser = await _userManager.FindByIdAsync(vm.Id);

            if (existingUser == null)
            {
                _notification.Error("Utilizador não existe");
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

            var result = await _userManager.ResetPasswordAsync(existingUser, token, vm.NewPassword);

            if (result.Succeeded)
            {
                _notification.Success("Password trocada com sucesso");
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        //#-----------------------------------#
        //#             Register              #
        //#-----------------------------------#
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var checkUserByEmail = await _userManager.FindByEmailAsync(vm.Email);

            if (checkUserByEmail != null)
            {
                _notification.Error("Email já em uso");
                return View(vm);
            }

            var checkUserByUsername = await _userManager.FindByNameAsync(vm.UserName);

            if (checkUserByUsername != null)
            {
                _notification.Error("Username já em uso");
                return View(vm);
            }

            var applicationUser = new ApplicationUser()
            {
                Email = vm.Email,
                UserName = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(applicationUser, vm.Password);

            if (result.Succeeded)
            {
                if (vm.Role == "Admin")
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAdmin);
                }
                else if (vm.Role == "Editor")
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteEditor);
                }
                else
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAuthor);
                }

                if (vm.ProfilePicture != null)
                {
                    applicationUser.ProfilePictureUrl = UploadImage(vm.ProfilePicture);
                    await _userManager.UpdateAsync(applicationUser);
                }

                _notification.Success("Utilizador registado com sucesso");
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }


            return View();
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "ProfilePictures");

            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);

            }
            return uniqueFileName;
        }
        //#-----------------------------------#
        //#                Edit               #
        //#-----------------------------------#

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
            {
                _notification.Error("Utilizador não existe");
                return View();
            }

            var vm = new EditVM()
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                PhoneNumber = existingUser.PhoneNumber,
                ProfilePictureUrl = existingUser.ProfilePictureUrl,
                Email = existingUser.Email
                
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var existingUser = await _userManager.FindByIdAsync(vm.Id);

            if (existingUser == null)
            {
                _notification.Error("Utilizador não existe");
                return View(vm);
            }

            var checkUserByEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (checkUserByEmail != null && checkUserByEmail.Id != existingUser.Id)
            {
                _notification.Error("Email já em uso");
                return View(vm);
            }

            var checkUserByUsername = await _userManager.FindByNameAsync(vm.UserName);
            if (checkUserByUsername != null && checkUserByUsername.Id != existingUser.Id)
            {
                _notification.Error("Username já em uso");
                return View(vm);
            }

            existingUser.UserName = vm.UserName;
            existingUser.FirstName = vm.FirstName;
            existingUser.LastName = vm.LastName;
            existingUser.PhoneNumber = vm.PhoneNumber;
            existingUser.Email = vm.Email;

            if (vm.ProfilePicture != null)
            {
                existingUser.ProfilePictureUrl = UploadImage(vm.ProfilePicture);
            }

            await _userManager.UpdateAsync(existingUser);
            _notification.Success("Perfil editado com sucesso");
            return RedirectToAction("Index", "Post", new { area = "Admin" });
        }


        //#-----------------------------------#
        //#          Login & Logout           #
        //#-----------------------------------#
        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new LoginVM());
            }
            return RedirectToAction("Index", "Opening", new { area = "Admin" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == vm.Username);

            if (existingUser == null)
            {
                _notification.Error("Username não existe");
                return View(vm);
            }

            var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);

            if (!verifyPassword)
            {
                _notification.Error("Password incorreta");
                return View(vm);
            }

            await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);

            _notification.Success("Login feito com sucesso");
            return RedirectToAction("Index", "Opening", new { area = "Admin" });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            _notification.Success("Logout feito com sucesso");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        //#-----------------------------------#
        //#          Forgot Password          #
        //#-----------------------------------#
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                _notification.Error("Password não corresponde aos requisitos");
                return View(vm);
            }

            var existingUser = await _userManager.FindByNameAsync(vm.Username);
            if (existingUser == null)
            {
                _notification.Error("Utilizador não existe");
                return View(vm);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

            var result = await _userManager.ResetPasswordAsync(existingUser, token, vm.NewPassword);

            if (result.Succeeded)
            {
                _notification.Success("Password trocada com sucesso");
                return RedirectToAction("Login", "User");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(vm);
        }

        //#-----------------------------------#
        //#           Create Account          #
        //#-----------------------------------#
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var checkUserByEmail = await _userManager.FindByEmailAsync(vm.Email);

            if (checkUserByEmail != null)
            {
                _notification.Error("Email já em uso");
                return View(vm);
            }

            var checkUserByUsername = await _userManager.FindByNameAsync(vm.UserName);

            if (checkUserByUsername != null)
            {
                _notification.Error("Username já em uso");
                return View(vm);
            }

            var applicationUser = new ApplicationUser()
            {
                Email = vm.Email,
                UserName = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(applicationUser, vm.Password);

            if (result.Succeeded)
            {
                if (vm.Role == "Admin")
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAdmin);
                }
                else if (vm.Role == "Editor")
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteEditor);
                }
                else
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAuthor);
                }

                if (vm.ProfilePicture != null)
                {
                    applicationUser.ProfilePictureUrl = UploadImage(vm.ProfilePicture);
                    await _userManager.UpdateAsync(applicationUser);
                }
            }
                _notification.Success("Utilizador registado com sucesso");
                return RedirectToAction("Login", "User");
        }


        //#-----------------------------------#
        //#           AccessDenied            #
        //#-----------------------------------#
        [HttpGet("AccessDenied")]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //#-----------------------------------#
        //#        Count Number of User       #
        //#-----------------------------------#
    }
}
