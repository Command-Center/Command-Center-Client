namespace CommandApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConnectionAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConnectionAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Comment = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConnectionAddresses");
        }
    }
}
