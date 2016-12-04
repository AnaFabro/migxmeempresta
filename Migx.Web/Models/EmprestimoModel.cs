using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Migx.Web.Models
{
    public enum EmprestimoEstado
    {
        Solicitado = 1,
        PropostaEmprestimo,
        Emprestado,
        Finalizado
    }

    public class EmprestimoModel
    {
        [Key]
        public int ID { get; set; }

        public int UsuarioIDSolicitante { get; set; }

        public int UsuarioIDSolicitadoPara { get; set; }

        public int ItemID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataSolicitacao { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataDevolucaoPrevista{ get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataDevolucao{ get; set; }

        public EmprestimoEstado Situacao { get; set; }

        [ForeignKey("UsuarioIDSolicitante")]
        public virtual UsuarioModel UsuarioSolicitante { get; set; }

        [ForeignKey("ItemID")]
        public virtual TimeLineItemModel Item { get; set; }
    }
}