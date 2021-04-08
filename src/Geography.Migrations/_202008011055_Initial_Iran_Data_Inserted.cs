using FluentMigrator;

namespace Geography.Migrations
{
    [Migration(202008011055)]
    public class _202008011055_Initial_Iran_Data_Inserted : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("IranGeoData.sql");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM Cities;");
            Execute.Sql("DELETE FROM Provinces;");
        }
    }
}
