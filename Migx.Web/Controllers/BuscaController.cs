using Migx.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Migx.Web.Controllers
{
    public class BuscaController : Controller
    {

        public ActionResult Buscar(string q)
        {
            if (!ModelState.IsValid)
                return HttpNotFound();

            int IdUsuario = ((UsuarioModel)Session["oUser"]).ID;

            BuscaModel busca = new BuscaModel();

            busca.Usuarios = UsuarioController.ListarPessoas(q);
            busca.Itens = TimeLineController.ListarItems(IdUsuario, q);

            return View(busca);
        }
    }
}