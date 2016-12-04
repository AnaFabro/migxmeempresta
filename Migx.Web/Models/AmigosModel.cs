using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Migx.Web.Models
{
    public class AmigosModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int IdAmigo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required]
        public DateTime DtInicioAmizade { get; set; }

        [ForeignKey("IdAmigo")]
        [InverseProperty("Amigos")]
        public virtual UsuarioModel UsuarioAmigo { get; set; }
    }
}