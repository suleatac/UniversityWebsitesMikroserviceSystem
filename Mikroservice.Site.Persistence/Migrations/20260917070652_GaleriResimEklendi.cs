using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GaleriResimEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Kategori",
                table: "Icerik",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_SiteId_DilId_Kategori",
                table: "Icerik",
                columns: new[] { "SiteId", "DilId", "Kategori" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Icerik_SiteId_DilId_Kategori",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "Kategori",
                table: "Icerik");
        }
    }
}
