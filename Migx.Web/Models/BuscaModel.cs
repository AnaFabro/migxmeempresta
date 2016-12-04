using Migx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Migx.Web.Models
{
    public class BuscaModel
    {
        public List<UsuarioModel> Usuarios { get; set; }

        public List<TimeLineItemModel> Itens { get; set; }
    }
}