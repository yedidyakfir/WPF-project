namespace BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CurrentCoinValues", "Url");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CurrentCoinValues", "Url", c => c.String());
        }
    }
}
