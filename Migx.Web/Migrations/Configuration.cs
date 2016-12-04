namespace Migx.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Web.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<Migx.Web.Models.MigxContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Migx.Web.Models.MigxContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            string plainPass = "1234";
            var harPass = Crypto.HashPassword(plainPass);

            context.Usuarios.AddOrUpdate(
                new Models.UsuarioModel()
                {
                    ID = 1,
                    Nome = "Administrador",
                    DtNascimento = DateTime.Now,
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
        }
    }
}
