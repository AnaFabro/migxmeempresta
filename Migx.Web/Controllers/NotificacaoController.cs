using Migx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Migx.Web.Controllers
{
    public class NotificacaoController : Controller
    {
        // GET: Notificacao
        public ActionResult Index()
        {
            UsuarioModel user = (UsuarioModel)Session["oUser"];

            NotificacoesModel notificacoes = new NotificacoesModel();

            notificacoes.SolicitacoesEmprestimos = EmprestimoController.ListarSolicitacoes(user.ID);
            notificacoes.SolicitacoesAmizade = AmizadeController.ListarSolicitacoes(user.ID);


            return View(notificacoes);
        }

        public JsonResult TemNotificacao()
        {
            UsuarioModel user = (UsuarioModel)Session["oUser"];

            bool notificacao = false;
            using (MigxContext ctx = new MigxContext())
            {
                notificacao = ctx.Emprestimos.Any(em => em.UsuarioIDSolicitadoPara == user.ID && em.Situacao == EmprestimoEstado.Solicitado);

                if (!notificacao)
                    notificacao = ctx.SolicitoesAmizade.Any(sl => sl.UsuarioSolicitadoParaID == user.ID);
            }

            return Json(notificacao);
        }
    }
}