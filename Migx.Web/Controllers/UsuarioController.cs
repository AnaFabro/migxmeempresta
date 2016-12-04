using Migx.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Data.Entity.SqlServer;

namespace Migx.Web.Controllers
{
    public class UsuarioController : Controller
    {
#pragma warning disable CS0169 // The field 'UsuarioController.ctx' is never used
        private MigxContext ctx;
#pragma warning restore CS0169 // The field 'UsuarioController.ctx' is never used

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

        public ActionResult AtualizarCadastro(UsuarioModel user)
        {
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