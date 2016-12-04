namespace Migx.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteradoSqlExpress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AmigosModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdUsuario = c.Int(nullable: false),
                        IdAmigo = c.Int(nullable: false),
                        DtInicioAmizade = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FotoModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Extensao = c.String(nullable: false, maxLength: 10),
                        NomeArquivo = c.String(nullable: false, maxLength: 255),
                        TimeLineID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeLineItemModels", t => t.TimeLineID, cascadeDelete: true)
                .Index(t => t.Id, unique: true)
                .Index(t => t.TimeLineID);
            
            CreateTable(
                "dbo.TimeLineItemModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Descricao = c.String(nullable: false, maxLength: 500),
                        UsuarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsuarioModels", t => t.UsuarioID, cascadeDelete: true)
                .Index(t => t.UsuarioID);
            
            CreateTable(
                "dbo.UsuarioModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        DtNascimento = c.DateTime(nullable: false),
                        Telefone = c.String(maxLength: 30),
                        Endereco = c.String(maxLength: 100),
                        Complemento = c.String(maxLength: 100),
                        Estado = c.String(maxLength: 2),
                        Cidade = c.String(maxLength: 50),
                        Senha = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.SolicitacaoAmizadeModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdUsuarioSolicitadoPor = c.Int(nullable: false),
                        IdUsuarioSolicitadoPara = c.Int(nullable: false),
                        DtSolicitacao = c.DateTime(nullable: false),
                        MensagemSolicitacao = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeLineItemModels", "UsuarioID", "dbo.UsuarioModels");
            DropForeignKey("dbo.FotoModels", "TimeLineID", "dbo.TimeLineItemModels");
            DropIndex("dbo.UsuarioModels", new[] { "Email" });
            DropIndex("dbo.TimeLineItemModels", new[] { "UsuarioID" });
            DropIndex("dbo.FotoModels", new[] { "TimeLineID" });
            DropIndex("dbo.FotoModels", new[] { "Id" });
            DropTable("dbo.SolicitacaoAmizadeModels");
            DropTable("dbo.UsuarioModels");
            DropTable("dbo.TimeLineItemModels");
            DropTable("dbo.FotoModels");
            DropTable("dbo.AmigosModels");
        }
    }
}
