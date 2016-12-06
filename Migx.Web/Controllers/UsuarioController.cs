using Migx.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Data.Entity.SqlServer;
using System.IO;

namespace Migx.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private MigxContext ctx;

        // GET: Usuario
        public ActionResult MeusDados()
        {
            if (Session["oUser"] != null)
            {
                UsuarioModel user = (UsuarioModel)Session["oUser"];

                ViewData["TimeLineItens"] = TimeLineController.ListarItens(user.ID);

                return View(user);
            }
            else
                return RedirectToAction("index", "login");
        }

        [HttpGet]
        public ActionResult Editar()
        {
            if (Session["oUser"] != null)
            {
                UsuarioModel user = (UsuarioModel)Session["oUser"];

                return View(user);
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Editar(UsuarioModel user)
        {
            var userSession = (UsuarioModel)Session["oUser"];

            using (MigxContext ctx = new MigxContext())
            {
                var userAlterado = ctx.Usuarios.SingleOrDefault(us => us.ID == userSession.ID);
                userAlterado.Cidade = user.Cidade;
                userAlterado.Complemento = user.Complemento;
                userAlterado.DtNascimento = user.DtNascimento;
                userAlterado.Endereco = user.Endereco;
                userAlterado.Estado = user.Estado;
                userAlterado.Nome = user.Nome;
                userAlterado.Telefone = user.Telefone;

                ctx.SaveChanges();

                Session["oUser"] = userAlterado;
            }

            if (Request.Files != null && Request.Files.Count > 0)
            {
                if (Request.Files[0].ContentLength > 0)
                {
                    var file = Request.Files[0];
                    var novoNome = Path.Combine(Server.MapPath("~/Content/Images/"), user.ID.ToString(), "profilePicture" + ".jpg");
                    file.SaveAs(novoNome);
                }
            }

            return View(user);
        }


        public static List<UsuarioModel> ListarPessoas(string param)
        {
            List<UsuarioModel> encontrados = new List<UsuarioModel>();

            using (MigxContext ctx = new MigxContext())
            {
                string busca = String.Format("%{0}%", param.Trim().Replace(' ', '%')); //altera a busca pra usar o wildcard '%' => qualquer qtd de caracteres

                encontrados = ctx.Usuarios.Where(u => SqlFunctions.PatIndex(busca, u.Nome) > 0).ToList();
            }

            return encontrados;
        }

    }
}