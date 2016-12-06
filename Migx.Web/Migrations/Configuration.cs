namespace Migx.Web.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Web.Helpers;
    using Migx.Web.Providers;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    internal sealed class Configuration : DbMigrationsConfiguration<Migx.Web.Models.MigxContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Migx.Web.Models.MigxContext context)
        {
            string plainPass = "123456";
            var harPass = Crypto.HashPassword(plainPass);

            context.Usuarios.AddOrUpdate(
                new Models.UsuarioModel()
                {
                    ID = 1,
                    Nome = "Administrador",
                    DtNascimento = DateTime.Now.Date,
                    Endereco = "Endereço de teste, 1111",
                    Complemento = "Complemento de teste",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Email = "adm@migx.com.br",
                    Senha = harPass,
                    Telefone = "11 12345-6789",
                    ConfirmaSenha = harPass, //Necessario pra não dar erro de validacao
                    ConfirmaEmail = "adm@migx.com.br" //Necessario pra não dar erro de validacao
                }
                );

            if (!context.Users.Any(u => u.Email == "adm@migx.com.br"))
            {
                var userStore = new UserStore<AppUserIdentity>(context);
                var userManager = new AppUserManager(userStore);
                var userToInsert = new AppUserIdentity { UserName = "adm@migx.com.br", Email = "adm@migx.com.br" };
                userManager.Create(userToInsert, "123456");
            }
        }
    }
}
