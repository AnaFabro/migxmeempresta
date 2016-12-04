namespace Migx.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Solicitacaodeemprestimos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmprestimoModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UsuarioIDSolicitante = c.Int(nullable: false),
                        UsuarioIDSolicitadoPara = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        DataSolicitacao = c.DateTime(nullable: false),
                        DataDevolucaoPrevista = c.DateTime(),
                        DataDevolucao = c.DateTime(),
                        ValorMulta = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Situacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TimeLineItemModels", t => t.ItemID)
                .ForeignKey("dbo.UsuarioModels", t => t.UsuarioIDSolicitante)
                .Index(t => t.UsuarioIDSolicitante)
                .Index(t => t.ItemID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmprestimoModels", "UsuarioIDSolicitante", "dbo.UsuarioModels");
            DropForeignKey("dbo.EmprestimoModels", "ItemID", "dbo.TimeLineItemModels");
            DropIndex("dbo.EmprestimoModels", new[] { "ItemID" });
            DropIndex("dbo.EmprestimoModels", new[] { "UsuarioIDSolicitante" });
            DropTable("dbo.EmprestimoModels");
        }
    }
}
