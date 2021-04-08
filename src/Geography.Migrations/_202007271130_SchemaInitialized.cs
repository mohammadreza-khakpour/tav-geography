using System.Data;
using FluentMigrator;

namespace Geography.Migrations
{
    [Migration(202007271130)]
    public class _202007271130_SchemaInitialized : Migration
    {
        public override void Up()
        {
            Create.Table("Provinces")
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Name").AsString(100).Unique().NotNullable();

            Create.Table("Cities")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("ProvinceId").AsInt32().NotNullable().ForeignKey("Provinces", "Id").OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("Cities");
            Delete.Table("Provinces");
        }
    }
}
