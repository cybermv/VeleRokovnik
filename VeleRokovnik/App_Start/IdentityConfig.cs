using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using VeleRokovnik.Entities;
using VeleRokovnik.Models;

namespace VeleRokovnik
{
    public class RokovnikUserManager : UserManager<RokovnikUser>
    {
        public RokovnikUserManager(IUserStore<RokovnikUser> store)
            : base(store)
        {
        }

        public static RokovnikUserManager Create(IdentityFactoryOptions<RokovnikUserManager> options, IOwinContext context)
        {
            var manager = new RokovnikUserManager(new UserStore<RokovnikUser>(context.Get<RokovnikContext>()));

            manager.UserValidator = new UserValidator<RokovnikUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = false
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireDigit = false,
                RequireNonLetterOrDigit = false
            };

            manager.UserLockoutEnabledByDefault = false;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<RokovnikUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;

        }
    }

    public class RokovnikSignInManager : SignInManager<RokovnikUser, string>
    {
        public RokovnikSignInManager(RokovnikUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static RokovnikSignInManager Create(IdentityFactoryOptions<RokovnikSignInManager> options, IOwinContext context)
        {
            return new RokovnikSignInManager(context.GetUserManager<RokovnikUserManager>(), context.Authentication);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(RokovnikUser user)
        {
            return user.GenerateUserIdentityAsync((RokovnikUserManager)UserManager);
        }
    }

    public class RokovnikUser : IdentityUser
    {
        public int OsobaId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<RokovnikUser> manager)
        {
            var createdIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return createdIdentity;
        }
    }
}
