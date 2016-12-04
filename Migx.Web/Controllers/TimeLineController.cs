using Migx.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Migx.Web.Controllers
{
    public class TimeLineController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            UsuarioModel user = (UsuarioModel)Session["oUser"];

            List<TimeLineItemModel> lista = null;
            using (MigxContext ctx = new MigxContext())
            {
                var amigos = ctx.Amigos.Where(am => am.IdUsuario == user.ID).Select(am => am.IdAmigo).ToArray();

                lista = ctx.Itens.Include("Fotos").Where(it => amigos.Contains(it.UsuarioID)).Where(it => it.Emprestado == false).ToList();
            }

            return View(lista);
        }


        [HttpGet]
        public ActionResult AdicionarItem() { return View(); }
        [HttpPost]
        public ActionResult AdicionarItem(TimeLineItemModel item)
        {
            if (!ModelState.IsValid)
                return View(item);

            var user = (UsuarioModel)Session["oUser"];
            var userId = user.ID;

            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/Images/"), userId.ToString())); //Cria o diretório caso ele não exista.

            item.UsuarioID = userId;
            item.Fotos = new List<FotoModel>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (Request.Files[i].ContentLength > 0)
                {
                    var file = Request.Files[i];
                    var fileName = Path.GetFileName(file.FileName);
                    var extensao = Path.GetExtension(file.FileName);

                    FotoModel fotoInfo = new FotoModel();
                    fotoInfo.Id = Guid.NewGuid();
                    fotoInfo.NomeArquivo = fileName;
                    fotoInfo.Extensao = extensao;

                    var novoNome = Path.Combine(Server.MapPath("~/Content/Images/"), userId.ToString(), fotoInfo.Id + fotoInfo.Extensao); //Cria um novo nome baseado no Id e extensão.
                    file.SaveAs(novoNome);

                    item.Fotos.Add(fotoInfo);
                }
            }

            using (MigxContext ctx = new MigxContext())
            {
                ctx.Itens.Add(item);
                ctx.SaveChanges();
            }

            return RedirectToAction("MeusDados", "Usuario");
        }

        [HttpGet]
        public ActionResult Editar(int idTimeline)
        {
            if (!ModelState.IsValid)
                return HttpNotFound();


            TimeLineItemModel item = GetDetalhesItem(idTimeline);

            return View(item);
        }

        [HttpPost]
        public ActionResult Editar(TimeLineItemModel item)
        {
            var user = (UsuarioModel)Session["oUser"];

            using (MigxContext ctx = new MigxContext())
            {                
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i].ContentLength > 0)
                    {
                        var file = Request.Files[i];
                        var fileName = Path.GetFileName(file.FileName);
                        var extensao = Path.GetExtension(file.FileName);

                        FotoModel fotoInfo = new FotoModel();
                        fotoInfo.Id = Guid.NewGuid();
                        fotoInfo.NomeArquivo = fileName;
                        fotoInfo.Extensao = extensao;

                        var novoNome = Path.Combine(Server.MapPath("~/Content/Images/"), user.ID.ToString(), fotoInfo.Id + fotoInfo.Extensao); //Cria um novo nome baseado no Id e extensão.
                        file.SaveAs(novoNome);

                        fotoInfo.TimeLineID = item.Id;
                        ctx.Entry(fotoInfo).State = System.Data.Entity.EntityState.Added;
                    }
                }

                item.UsuarioID = user.ID;
                ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }

            return RedirectToAction("MeusDados", "Usuario");
        }

        [HttpGet]
        public ActionResult Excluir(int idTimeLine)
        {
            if (!ModelState.IsValid)
                return HttpNotFound();

            using (MigxContext ctx = new MigxContext())
            {
                TimeLineItemModel item = GetDetalhesItem(idTimeLine, ctx);

                foreach (var foto in item.Fotos)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), item.UsuarioID.ToString(), foto.Id + foto.Extensao);

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                }

                ctx.Fotos.RemoveRange(item.Fotos);
                ctx.Itens.Remove(item);
                ctx.SaveChanges();
            }

            return RedirectToAction("MeusDados", "Usuario");
        }
       
        [HttpPost]
        public JsonResult ExcluirFoto(Guid idFoto)
        {
            var user = (UsuarioModel)Session["oUser"];

            using (MigxContext ctx = new MigxContext())
            {
                FotoModel foto = ctx.Fotos.SingleOrDefault(ft => ft.Id == idFoto);

                var path = Path.Combine(Server.MapPath("~/Content/Images/"), user.ID.ToString(), foto.Id + foto.Extensao);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                ctx.Fotos.Remove(foto);
                ctx.SaveChanges();
            }

            return Json(true);
        }

        [HttpPost]
        public JsonResult Devolver(int idTimeLine)
        {
            var user = (UsuarioModel)Session["oUser"];

            using (MigxContext ctx = new MigxContext())
            {
                TimeLineItemModel devolvido = ctx.Itens.SingleOrDefault(it => it.Id == idTimeLine);
                devolvido.Emprestado = false;

                EmprestimoModel empFinalizado = ctx.Emprestimos.SingleOrDefault(em => em.ItemID == idTimeLine && em.Situacao == EmprestimoEstado.Emprestado);
                empFinalizado.Situacao = EmprestimoEstado.Finalizado;

                ctx.SaveChanges();
            }

            return Json(true);
        }

        [HttpGet]
        public ActionResult Detalhes(int idTimeLine)
        {
            if (!ModelState.IsValid)
                return HttpNotFound();

            TimeLineItemModel item = null;

            var user = (UsuarioModel)Session["oUser"];
            var userId = user.ID;

            using (MigxContext ctx = new MigxContext())
            {
                var emp = ctx.Emprestimos.Where(em => em.UsuarioIDSolicitante == userId && em.ItemID == idTimeLine && em.Situacao != EmprestimoEstado.Finalizado).FirstOrDefault();

                if (emp != null)
                    ViewData["JaSolicitado"] = true;

                item = GetDetalhesItem(idTimeLine, ctx);
            }



            return View(item);
        }

        public static TimeLineItemModel GetDetalhesItem(int itemID, MigxContext ctx = null)
        {
            TimeLineItemModel item;
            if (ctx == null)
            {
                using (ctx = new MigxContext())
                {
                    item = ctx.Itens.Include("Fotos").Include("Usuario").Where(tl => tl.Id == itemID).First();
                }
            }
            else
            {
                item = ctx.Itens.Include("Fotos").Include("Usuario").Where(tl => tl.Id == itemID).First();
            }

            return item;
        }
        public static List<TimeLineItemModel> ListarItens(int userID)
        {
            List<TimeLineItemModel> itens = null;
            using (MigxContext ctx = new MigxContext())
            {
                itens = ctx.Itens.Include("Fotos").Include("Usuario").Where(tl => tl.UsuarioID == userID).ToList();
            }

            return itens;
        }
        public static List<TimeLineItemModel> ListarItems(int userID, string param)
        {
            List<TimeLineItemModel> encontrados = new List<TimeLineItemModel>();
            int[] listaIdAmigos;

            using (MigxContext ctx = new MigxContext())
            {
                listaIdAmigos = ctx.Amigos.Where(u => u.IdUsuario == userID).Select(c => c.IdAmigo).ToArray();

                string busca = String.Format("%{0}%", param.Trim().Replace(' ', '%')); //altera a busca pra usar o wildcard '%' => qualquer qtd de caracteres

                encontrados = ctx.Itens.Include("Fotos").Include("Usuario").Where(it => listaIdAmigos.Contains(it.UsuarioID)).Where(it => SqlFunctions.PatIndex(busca, it.Descricao) > 0).Where(it => it.Emprestado == false).ToList();
            }

            return encontrados;
        }
        public static List<TimeLineItemModel> ListarItens(int[] userIDs)
        {
            List<TimeLineItemModel> itens = null;
            using (MigxContext ctx = new MigxContext())
            {
                itens = ctx.Itens.Include("Fotos").Include("Usuario").Where(tl => userIDs.Contains(tl.UsuarioID) && tl.Emprestado == false).ToList();
            }

            return itens;
        }
    }
}