using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Aktif",
                table: "YonetimDuyurular",
                newName: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "YonetimDuyurular",
                newName: "Aktif");
        }
    }
}
