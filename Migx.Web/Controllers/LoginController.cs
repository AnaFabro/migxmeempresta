using Migx.Web.Models;
using System.Linq;
using System.IO;
using System.Web.Helpers;
using System.Web.Mvc;

using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Security;
using Migx.Web.Providers;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace Migx.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private MigxContext ctx;
        private AppUserManager _userManager;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Login(string email, string senha)
        {
            UsuarioModel user = null;
            using (ctx = new MigxContext())
            {
                user = ctx.Usuarios.Where(u => u.Email.Equals(email)).FirstOrDefault();
            }

            bool usuarioValido = false;
            if (user !=null)
            {
                usuarioValido = Crypto.VerifyHashedPassword(user.Senha, senha);
            }
            else
            {
                return View("index");
            }

            if (usuarioValido)
            {
                Session["oUser"] = user;

                var userIdentity = await UserManager.FindAsync(email, senha);

                await SignInAsync(userIdentity, false);

                return RedirectToAction("Index", "TimeLine");
            }
            else
            {
                return View("index");
            }
        }

        public ActionResult Signout()
        {
            Session["oUser"] = null;

            return View("index");
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Cadastrar(UsuarioModel user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
                return View();
            }

            string plainPass = user.Senha;


            var userIdentity = new AppUserIdentity() { UserName = user.Email, Email = user.Email };
            var result = await UserManager.CreateAsync(userIdentity, plainPass);

            if (result.Succeeded)
            {
                await SignInAsync(userIdentity, isPersistent: false);
                //return RedirectToAction("Index", "Home");
            }

            using (ctx = new MigxContext())
            {                
                var hashPass = Crypto.HashPassword(plainPass);
                user.Senha = hashPass;

                user = ctx.Usuarios.Add(user);

                ctx.SaveChanges();
            }
            Session["oUser"] = user;

            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/Images/"), user.ID.ToString())); //Cria o diretório caso ele não exista.
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var novoNome = Path.Combine(Server.MapPath("~/Content/Images/"), user.ID.ToString(), "profilePicture" + ".jpg");
                file.SaveAs(novoNome);
            }

            return RedirectToAction("Index", "TimeLine");
        }


        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(AppUserIdentity user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }


    }
}