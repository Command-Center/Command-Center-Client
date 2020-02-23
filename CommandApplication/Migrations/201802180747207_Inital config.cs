namespace CommandApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initalconfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TemperatureReadings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Temperature = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TemperatureReadings");
        }
    }
}
