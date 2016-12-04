using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Migx.Web.Models
{
    public class NotificacoesModel
    {
        public List<SolicitacaoAmizadeModel> SolicitacoesAmizade { get; set; }
  
        public List<EmprestimoModel> SolicitacoesEmprestimos { get; set; }
    }
}