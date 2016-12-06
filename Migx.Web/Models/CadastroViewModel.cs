using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Migx.Web.Models
{
    public class CadastroViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Campo data inválido")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Data de Nascimento")]
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

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não são iguais")]
        public string ConfirmaSenha { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(100)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Compare("Email", ErrorMessage = "Os campos e-mail e Confirmar email não são iguais")]
        public string ConfirmaEmail { get; set; }
    }
}