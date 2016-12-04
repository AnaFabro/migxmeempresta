using Migx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Migx.Web.Controllers
{
    public class AmizadeController : Controller
    {
        [HttpPost]
        public JsonResult SolicitarAmizade(int IdUsuario)
        {
            if (!ModelState.IsValid)
                return Json(false);

            UsuarioModel user = (UsuarioModel)Session["oUser"];
            using (MigxContext ctx = new MigxContext())
            {
                SolicitacaoAmizadeModel solicitacao = new SolicitacaoAmizadeModel()
                {
                    UsuarioSolicitadoPorID = user.ID,
                    UsuarioSolicitadoParaID = IdUsuario,
                    DtSolicitacao = DateTime.Now
                };

                ctx.SolicitoesAmizade.Add(solicitacao);
                ctx.SaveChanges();
            }

            return Json(true);
        }

        public static List<SolicitacaoAmizadeModel> ListarSolicitacoes(int userID)
        {
            List<SolicitacaoAmizadeModel> solicitacoes = null;
            using (MigxContext ctx = new MigxContext())
            {
                solicitacoes = ctx.SolicitoesAmizade.Include("UsuarioSolicitadoPor").Where(sa => sa.UsuarioSolicitadoParaID == userID).ToList();
            }

            return solicitacoes;
        }

        [HttpPost]
        public JsonResult RecusarAmizade(int IdSolicitacao)
        {
            if (!ModelState.IsValid)
                return Json(false);

            UsuarioModel user = (UsuarioModel)Session["oUser"];
            using (MigxContext ctx = new MigxContext())
            {
                SolicitacaoAmizadeModel remover = new SolicitacaoAmizadeModel()
                {
                    Id = IdSolicitacao,
                    UsuarioSolicitadoParaID = user.ID
                };

                ctx.SolicitoesAmizade.Add(remover);
                ctx.Entry<SolicitacaoAmizadeModel>(remover).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Json(true);
        }

        [HttpPost]
        public JsonResult AceitarAmizade(int IdSolicitacao)
        {
            if (!ModelState.IsValid)
                return Json(false);

            UsuarioModel user = (UsuarioModel)Session["oUser"];
            using (MigxContext ctx = new MigxContext())
            {
                SolicitacaoAmizadeModel solicitacao = ctx.SolicitoesAmizade.SingleOrDefault(sa => sa.Id == IdSolicitacao && sa.UsuarioSolicitadoParaID == user.ID);
                DateTime dataInicioAmizade = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                AmigosModel souAmigo = new AmigosModel()
                {
                    IdUsuario = user.ID,
                    IdAmigo = solicitacao.UsuarioSolicitadoPorID,
                    DtInicioAmizade = dataInicioAmizade
                };

                AmigosModel meuAmigo = new AmigosModel()
                {
                    IdUsuario = solicitacao.UsuarioSolicitadoPorID,
                    IdAmigo = user.ID,
                    DtInicioAmizade = dataInicioAmizade
                };


                ctx.SolicitoesAmizade.Remove(solicitacao);
                ctx.Amigos.Add(souAmigo);
                ctx.Amigos.Add(meuAmigo);
                ctx.SaveChanges();
            }
            return Json(true);
        }

        [HttpGet]
        public ActionResult ListarSolicitacoes()
        {
            UsuarioModel user = (UsuarioModel)Session["oUser"];

            List<SolicitacaoAmizadeModel> solicitacoes = AmizadeController.ListarSolicitacoes(user.ID);

            return View(solicitacoes);
        }
    }
}