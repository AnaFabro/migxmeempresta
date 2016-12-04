namespace Migx.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacionamentoamigousuario : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AmigosModels", new[] { "UsuarioModel_ID" });
            DropColumn("dbo.AmigosModels", "IdAmigo");
            RenameColumn(table: "dbo.AmigosModels", name: "UsuarioModel_ID", newName: "IdAmigo");
            AlterColumn("dbo.AmigosModels", "IdAmigo", c => c.Int(nullable: false));
            CreateIndex("dbo.AmigosModels", "IdAmigo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AmigosModels", new[] { "IdAmigo" });
            AlterColumn("dbo.AmigosModels", "IdAmigo", c => c.Int());
            RenameColumn(table: "dbo.AmigosModels", name: "IdAmigo", newName: "UsuarioModel_ID");
            AddColumn("dbo.AmigosModels", "IdAmigo", c => c.Int(nullable: false));
            CreateIndex("dbo.AmigosModels", "UsuarioModel_ID");
        }
    }
}
