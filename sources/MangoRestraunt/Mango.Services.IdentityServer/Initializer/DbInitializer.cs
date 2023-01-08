using IdentityModel;
using Mango.Services.IdentityServer.DbContexts;
using Mango.Services.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.IdentityServer.Initializer
{
    public interface IDbInitializer
    {
        public void Initialize();
    }

    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(SD.Admin).Result != null)
                return;
            
            // seed the database if identity roles don't exist
            _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();

            #region initialAdmin
            var adminUser = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111111",
                FirstName = "Mikhail",
                LastName = "Admin"
            };

            var adminCreateResult = _userManager.CreateAsync(adminUser, "Aa_12345").Result;
            if (!adminCreateResult.Succeeded)
            {
                FlushAdmin();
                throw new Exception(adminCreateResult?.Errors.Select(e => e.Description).Aggregate((a, b) => $"{a}; {b}"));
            }
            _userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();
            
            var initialAdmin = _userManager.AddClaimsAsync(adminUser, new List<Claim> 
                {
                    new Claim(JwtClaimTypes.Name, $"{adminUser.FirstName} {adminUser.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                    new Claim(JwtClaimTypes.Role, SD.Admin)
                }).Result;
            #endregion

            #region initialCustomer
            var customerUser = new ApplicationUser
            {
                UserName = "customer@gmail.com", 
                Email = "customer@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "222222222222",
                FirstName = "Victor",
                LastName = "Customer"
            };

            var customerCreateResult = _userManager.CreateAsync(customerUser, "Bb_12345").GetAwaiter().GetResult();
            if (!customerCreateResult.Succeeded)
            {
                FlushCustomer();
                throw new Exception(customerCreateResult?.Errors.Select(e => e.Description).Aggregate((a, b) => $"{a}; {b}"));
            }
            _userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();

            var initialCustomer = _userManager.AddClaimsAsync(customerUser, new List<Claim>
                {
                    new Claim(JwtClaimTypes.Name, $"{customerUser .FirstName} {customerUser.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                    new Claim(JwtClaimTypes.Role, SD.Customer)
                }).Result;
            #endregion

            void Flush()
            {
                _roleManager.DeleteAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.DeleteAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            void FlushAdmin() 
            {
                Flush();
                if (adminUser != null)
                    _userManager.DeleteAsync(adminUser).GetAwaiter().GetResult();
            }
            void FlushCustomer() 
            {
                Flush();
                if (customerUser != null)
                    _userManager.DeleteAsync(customerUser).GetAwaiter().GetResult();
            }
        }
    }
}
