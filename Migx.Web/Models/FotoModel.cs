using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migx.Web.Models
{
    public class FotoModel
    {
        [Index(IsUnique = true)]
        public Guid Id { get; set; } //Gera um ID para salvar a imagem. Evita de problema com imagem de mesmo nome.

        [Required]
        [MaxLength(10)]
        public string Extensao { get; set; } //Auxilia a montar um nome para o arquivo

        [Required]
        [MaxLength(255)]
        public string NomeArquivo { get; set; } // Quarda o nome original da foto
                
        [Required]
        public int TimeLineID { get; set; }
        public TimeLineItemModel Timeline { get; set; }
    }
}