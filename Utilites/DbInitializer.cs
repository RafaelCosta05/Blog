using Projeto.Data;
using Projeto.Models;
using Microsoft.AspNetCore.Identity;
using Projeto.Utilites;

namespace Projeto.Utilites
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            var roles = new[]
            {
                WebsiteRoles.WebsiteAdmin,
                WebsiteRoles.WebsiteAuthor,
                WebsiteRoles.WebsiteEditor
            };

            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role!).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(role!)).GetAwaiter().GetResult();
                }
            }

            _context.SaveChanges();
        }
    }
}
