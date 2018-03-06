namespace BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coins",
                c => new
                    {
                        CoinId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CoinId);
            
            CreateTable(
                "dbo.CoinValues",
                c => new
                    {
                        CoinValueId = c.Double(nullable: false),
                        date = c.DateTime(nullable: false),
                        Coin_CoinId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CoinValueId)
                .ForeignKey("dbo.Coins", t => t.Coin_CoinId)
                .Index(t => t.Coin_CoinId);
            
            CreateTable(
                "dbo.CurrentCoinValues",
                c => new
                    {
                        CurrentCoinValueId = c.String(nullable: false, maxLength: 128),
                        value = c.Double(nullable: false),
                        date = c.DateTime(nullable: false),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.CurrentCoinValueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoinValues", "Coin_CoinId", "dbo.Coins");
            DropIndex("dbo.CoinValues", new[] { "Coin_CoinId" });
            DropTable("dbo.CurrentCoinValues");
            DropTable("dbo.CoinValues");
            DropTable("dbo.Coins");
        }
    }
}
