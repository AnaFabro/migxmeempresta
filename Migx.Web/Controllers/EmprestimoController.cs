using Migx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Migx.Web.Controllers
{
    public class EmprestimoController : Controller
    {
        //Itens que o usuario pegou emprestado
        public ActionResult GetMeusEmprestimos()
        {
            return View();
        }

        //Itens que o usuario emprestou para alguém
        public ActionResult GetMeusEmprestados()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ListarEmprestimosSolicitados()
        {
            UsuarioModel user = (UsuarioModel)Session["oUser"];
            List<EmprestimoModel> emprestimos = EmprestimoController.ListarSolicitacoes(user.ID);

            return View(emprestimos);
        }

        public static List<EmprestimoModel> ListarSolicitacoes(int idUser)
        {
            List<EmprestimoModel> emprestimos = null;
            using (MigxContext ctx = new MigxContext())
            {
                emprestimos = ctx.Emprestimos.Include("UsuarioSolicitante").Include("Item").Where(em => em.UsuarioIDSolicitadoPara == idUser && em.Situacao == EmprestimoEstado.Solicitado).ToList();
            }

            return emprestimos;
        }

        [HttpPost]
        public JsonResult SolicitarEmprestimo(int idTimeLine, DateTime dtDevolucao)
        {
            if (!ModelState.IsValid)
                return Json(false);

            UsuarioModel user = (UsuarioModel)Session["oUser"];
            using (MigxContext ctx = new MigxContext())
            {
                var itemEmprestado = ctx.Itens.SingleOrDefault(it => it.Id == idTimeLine);

                EmprestimoModel emp = new EmprestimoModel()
                {
                    ItemID = idTimeLine,
                    UsuarioIDSolicitante = user.ID,
                    UsuarioIDSolicitadoPara = itemEmprestado.UsuarioID,
                    DataSolicitacao = DateTime.Now.Date,
                    DataDevolucaoPrevista = dtDevolucao.Date,
                    Situacao = EmprestimoEstado.Solicitado
                };

                ctx.Emprestimos.Add(emp);
                ctx.SaveChanges();
            }

            return Json(true);
        }

        [HttpPost]
        public JsonResult AceitarEmprestimo(int IdEmprestimo)
        {
            if (!ModelState.IsValid)
                return Json(false);

            UsuarioModel user = (UsuarioModel)Session["oUser"];
            using (MigxContext ctx = new MigxContext())
            {
                EmprestimoModel aceitar = ctx.Emprestimos.SingleOrDefault(em => em.ID == IdEmprestimo && em.UsuarioIDSolicitadoPara == user.ID);
                aceitar.Situacao = EmprestimoEstado.Emprestado;                
                aceitar.Item.Emprestado = true;
               
                ctx.SaveChanges();
            }

            return Json(true);
        }
        
        [HttpPost]
        public JsonResult RecusarEmprestimo(int IdEmprestimo)
        {
            if (!ModelState.IsValid)
                return Json(false);

            UsuarioModel user = (UsuarioModel)Session["oUser"];
            using (MigxContext ctx = new MigxContext())
            {
                EmprestimoModel remover = new EmprestimoModel()
                {
                    ID = IdEmprestimo,
                    UsuarioIDSolicitadoPara = user.ID
                };

                ctx.Emprestimos.Add(remover);
                ctx.Entry<EmprestimoModel>(remover).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Json(true);
        }
    }
}