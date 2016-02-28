namespace MvcInternetApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Films", "FilmPlus", c => c.Int(nullable: false));
            AddColumn("dbo.Films", "FilmMinus", c => c.Int(nullable: false));
            DropColumn("dbo.Films", "Plus");
            DropColumn("dbo.Films", "Minus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Films", "Minus", c => c.Int(nullable: false));
            AddColumn("dbo.Films", "Plus", c => c.Int(nullable: false));
            DropColumn("dbo.Films", "FilmMinus");
            DropColumn("dbo.Films", "FilmPlus");
        }
    }
}
