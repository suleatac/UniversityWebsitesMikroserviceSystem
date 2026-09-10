using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class sixth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Icerik_PageTypes_PageTypeId1",
                table: "Icerik");

            migrationBuilder.DropForeignKey(
                name: "FK_Popuplar_PageTypes_PageTypeId",
                table: "Popuplar");

            migrationBuilder.DropIndex(
                name: "IX_Popuplar_PageTypeId",
                table: "Popuplar");

            migrationBuilder.DropIndex(
                name: "IX_Icerik_PageTypeId1",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "PageTypeId",
                table: "Popuplar");

            migrationBuilder.DropColumn(
                name: "PageTypeId1",
                table: "Icerik");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PageTypeId",
                table: "Popuplar",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PageTypeId1",
                table: "Icerik",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Popuplar_PageTypeId",
                table: "Popuplar",
                column: "PageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_PageTypeId1",
                table: "Icerik",
                column: "PageTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Icerik_PageTypes_PageTypeId1",
                table: "Icerik",
                column: "PageTypeId1",
                principalTable: "PageTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Popuplar_PageTypes_PageTypeId",
                table: "Popuplar",
                column: "PageTypeId",
                principalTable: "PageTypes",
                principalColumn: "Id");
        }
    }
}
