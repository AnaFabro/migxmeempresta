using Migx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Migx.Web.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult GetConversaUsuario(int userID)
        {
            return View();
        }

        public ActionResult EnviarConversa(int userID)
        {
            return View();
        }
    }
}