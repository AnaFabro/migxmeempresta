using Migx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Migx.Web.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult HomePage()
        {
            UsuarioModel user = (UsuarioModel)Session["oUser"];

            List<TimeLineItemModel> lista = null;

            using (MigxContext ctx = new MigxContext())
            {
                var amigos = ctx.Amigos.Where(am => am.IdUsuario == user.ID || am.IdAmigo == user.ID).Select(am => am.Id).ToArray();

                lista = ctx.Itens.Include("Fotos").Where(it => amigos.Contains(it.UsuarioID)).ToList();
            }            

            return View(lista);
        }
    }
}