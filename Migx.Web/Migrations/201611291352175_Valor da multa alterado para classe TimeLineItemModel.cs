namespace Migx.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValordamultaalteradoparaclasseTimeLineItemModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeLineItemModels", "ValorMulta", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.EmprestimoModels", "ValorMulta");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmprestimoModels", "ValorMulta", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.TimeLineItemModels", "ValorMulta");
        }
    }
}
