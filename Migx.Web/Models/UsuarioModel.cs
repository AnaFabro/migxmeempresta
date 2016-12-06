using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migx.Web.Models
{
    public class UsuarioModel 
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Campo data inválido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DtNascimento { get; set; }

        [StringLength(30)]
        public string Telefone { get; set; }

        [StringLength(100)]
        public string Endereco { get; set; }

        [StringLength(100)]
        public string Complemento { get; set; }

        [StringLength(02)]
        public string Estado { get; set; }

        [StringLength(50)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [NotMapped]
        //[Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        //[Compare("Senha", ErrorMessage = "As senhas não são iguais")]
        public string ConfirmaSenha { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Email { get; set; }

        [NotMapped]
        //  [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        // [Compare("Email", ErrorMessage = "Os e-mails não são iguais")]
        public string ConfirmaEmail { get; set; }


        public virtual ICollection<SolicitacaoAmizadeModel> SolicitadosPorMim { get; set; }

        public virtual ICollection<SolicitacaoAmizadeModel> SolicitadosParaMim { get; set; }

        public virtual ICollection<AmigosModel> Amigos { get; set; }

    }
}
