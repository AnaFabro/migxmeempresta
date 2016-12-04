using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Migx.Web.Models
{
    public class SolicitacaoAmizadeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioSolicitadoPorID { get; set; }

        [Required]
        public int UsuarioSolicitadoParaID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DtSolicitacao { get; set; }

        [MaxLength(100)]
        public string MensagemSolicitacao { get; set; }


        [ForeignKey("UsuarioSolicitadoPorID")]
        [InverseProperty("SolicitadosPorMim")]
        public virtual UsuarioModel UsuarioSolicitadoPor { get; set; }


        [ForeignKey("UsuarioSolicitadoParaID")]
        [InverseProperty("SolicitadosParaMim")]
        public virtual UsuarioModel UsuarioSolicitadoPara { get; set; }
    }
}