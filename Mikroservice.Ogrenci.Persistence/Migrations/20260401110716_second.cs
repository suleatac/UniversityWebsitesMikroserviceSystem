using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Ogrenci.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ogrencis_ogrenciprogramid",
                table: "Ogrencis");

            migrationBuilder.CreateIndex(
                name: "IX_Ogrencis_ogrenciprogramid",
                table: "Ogrencis",
                column: "ogrenciprogramid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ogrencis_ogrenciprogramid",
                table: "Ogrencis");

            migrationBuilder.CreateIndex(
                name: "IX_Ogrencis_ogrenciprogramid",
                table: "Ogrencis",
                column: "ogrenciprogramid",
                unique: true);
        }
    }
}
