namespace Migx.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Campoemprestadonoitemdatimeline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeLineItemModels", "Emprestado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeLineItemModels", "Emprestado");
        }
    }
}
