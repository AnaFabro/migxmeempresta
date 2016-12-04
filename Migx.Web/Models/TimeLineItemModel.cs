using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Migx.Web.Models
{
    public class TimeLineItemModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        public bool Emprestado { get; set; }

        [DisplayName("Valor da Multa")]
        public decimal ValorMulta { get; set; }

        public int UsuarioID { get; set; }
        public virtual UsuarioModel Usuario { get; set; }

        public virtual IList<FotoModel> Fotos { get; set; }
    }
}