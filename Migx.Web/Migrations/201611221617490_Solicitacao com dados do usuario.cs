namespace Migx.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Solicitacaocomdadosdousuario : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FotoModels", "TimeLineID", "dbo.TimeLineItemModels");
            DropForeignKey("dbo.TimeLineItemModels", "UsuarioID", "dbo.UsuarioModels");
            AddColumn("dbo.AmigosModels", "UsuarioModel_ID", c => c.Int());
            AddColumn("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoPorID", c => c.Int(nullable: false));
            AddColumn("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoParaID", c => c.Int(nullable: false));
            CreateIndex("dbo.AmigosModels", "UsuarioModel_ID");
            CreateIndex("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoPorID");
            CreateIndex("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoParaID");
            AddForeignKey("dbo.AmigosModels", "UsuarioModel_ID", "dbo.UsuarioModels", "ID");
            AddForeignKey("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoParaID", "dbo.UsuarioModels", "ID");
            AddForeignKey("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoPorID", "dbo.UsuarioModels", "ID");
            AddForeignKey("dbo.FotoModels", "TimeLineID", "dbo.TimeLineItemModels", "Id");
            AddForeignKey("dbo.TimeLineItemModels", "UsuarioID", "dbo.UsuarioModels", "ID");
            DropColumn("dbo.SolicitacaoAmizadeModels", "IdUsuarioSolicitadoPor");
            DropColumn("dbo.SolicitacaoAmizadeModels", "IdUsuarioSolicitadoPara");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SolicitacaoAmizadeModels", "IdUsuarioSolicitadoPara", c => c.Int(nullable: false));
            AddColumn("dbo.SolicitacaoAmizadeModels", "IdUsuarioSolicitadoPor", c => c.Int(nullable: false));
            DropForeignKey("dbo.TimeLineItemModels", "UsuarioID", "dbo.UsuarioModels");
            DropForeignKey("dbo.FotoModels", "TimeLineID", "dbo.TimeLineItemModels");
            DropForeignKey("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoPorID", "dbo.UsuarioModels");
            DropForeignKey("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoParaID", "dbo.UsuarioModels");
            DropForeignKey("dbo.AmigosModels", "UsuarioModel_ID", "dbo.UsuarioModels");
            DropIndex("dbo.SolicitacaoAmizadeModels", new[] { "UsuarioSolicitadoParaID" });
            DropIndex("dbo.SolicitacaoAmizadeModels", new[] { "UsuarioSolicitadoPorID" });
            DropIndex("dbo.AmigosModels", new[] { "UsuarioModel_ID" });
            DropColumn("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoParaID");
            DropColumn("dbo.SolicitacaoAmizadeModels", "UsuarioSolicitadoPorID");
            DropColumn("dbo.AmigosModels", "UsuarioModel_ID");
            AddForeignKey("dbo.TimeLineItemModels", "UsuarioID", "dbo.UsuarioModels", "ID", cascadeDelete: true);
            AddForeignKey("dbo.FotoModels", "TimeLineID", "dbo.TimeLineItemModels", "Id", cascadeDelete: true);
        }
    }
}
