using Migx.Web.Models;
using System.Linq;
using System.IO;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Migx.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private MigxContext ctx;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string email, string senha)
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
        public ActionResult Cadastrar(UsuarioModel user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
                return View();
            }

            using (ctx = new MigxContext())
            {
                string plainPass = user.Senha;
                var hashPass = Crypto.HashPassword(plainPass);

                user.Senha = hashPass;
                user.ConfirmaSenha = hashPass;

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
    }
}