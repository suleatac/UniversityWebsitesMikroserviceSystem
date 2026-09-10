using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seventh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Icerik_ParentId_Menu_Sira",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "Menu_Sira",
                table: "Icerik");

            migrationBuilder.AlterColumn<int>(
                name: "Sira",
                table: "Icerik",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_ParentId_Sira",
                table: "Icerik",
                columns: new[] { "ParentId", "Sira" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Icerik_ParentId_Sira",
                table: "Icerik");

            migrationBuilder.AlterColumn<int>(
                name: "Sira",
                table: "Icerik",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Menu_Sira",
                table: "Icerik",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_ParentId_Menu_Sira",
                table: "Icerik",
                columns: new[] { "ParentId", "Menu_Sira" });
        }
    }
}
