namespace RestApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supportMessageAttachment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "AttachmentName", c => c.String());
            AddColumn("dbo.Messages", "AttachmentBynary", c => c.Binary());
            DropColumn("dbo.Messages", "Attachment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "Attachment", c => c.String());
            DropColumn("dbo.Messages", "AttachmentBynary");
            DropColumn("dbo.Messages", "AttachmentName");
        }
    }
}
