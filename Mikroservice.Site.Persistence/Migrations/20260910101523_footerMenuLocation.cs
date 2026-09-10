using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class footerMenuLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Icerik",
                type: "boolean",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "Location",
                table: "Icerik",
                type: "integer",
                nullable: true,
                defaultValue: 1);

            // Mevcut menu satirlari (Tip=7) Header konumuna tasinsin
            migrationBuilder.Sql(
                "UPDATE \"Icerik\" SET \"Location\" = 1 WHERE \"Location\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_SiteId_DilId_Location",
                table: "Icerik",
                columns: new[] { "SiteId", "DilId", "Location" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Icerik_SiteId_DilId_Location",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Icerik");
        }
    }
}
