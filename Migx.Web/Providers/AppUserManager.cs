using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Migx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Migx.Web.Providers
{
    public class AppUserManager : UserManager<AppUserIdentity>
    {
        public AppUserManager(IUserStore<AppUserIdentity> store) : base(store)
        {
        }

        public Task<IdentityResult> Create(AppUserIdentity user, string password)
        {
            return base.CreateAsync(user, password);
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(
                new UserStore<AppUserIdentity>(context.Get<MigxContext>()));

            // optionally configure your manager
            // ...

            manager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 3,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            return manager;
        }
    }
}