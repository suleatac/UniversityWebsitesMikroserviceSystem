using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class thirteenth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PageTypes");

            migrationBuilder.AddColumn<string>(
                name: "Adi",
                table: "SitePersonelleri",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PageTypeId",
                table: "SitePersonelleri",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "SitePersonelleri",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "SitePersonelleri",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoUrl",
                table: "SitePersonelleri",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Soyadi",
                table: "SitePersonelleri",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "SitePersonelleri",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SitePersonelleri_PageTypeId",
                table: "SitePersonelleri",
                column: "PageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SitePersonelleri_SiteId_SeoUrl",
                table: "SitePersonelleri",
                columns: new[] { "SiteId", "SeoUrl" },
                unique: true,
                filter: "\"IsDeleted\" = FALSE");

            migrationBuilder.AddForeignKey(
                name: "FK_SitePersonelleri_PageTypes_PageTypeId",
                table: "SitePersonelleri",
                column: "PageTypeId",
                principalTable: "PageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SitePersonelleri_PageTypes_PageTypeId",
                table: "SitePersonelleri");

            migrationBuilder.DropIndex(
                name: "IX_SitePersonelleri_PageTypeId",
                table: "SitePersonelleri");

            migrationBuilder.DropIndex(
                name: "IX_SitePersonelleri_SiteId_SeoUrl",
                table: "SitePersonelleri");

            migrationBuilder.DropColumn(
                name: "Adi",
                table: "SitePersonelleri");

            migrationBuilder.DropColumn(
                name: "PageTypeId",
                table: "SitePersonelleri");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "SitePersonelleri");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "SitePersonelleri");

            migrationBuilder.DropColumn(
                name: "SeoUrl",
                table: "SitePersonelleri");

            migrationBuilder.DropColumn(
                name: "Soyadi",
                table: "SitePersonelleri");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "SitePersonelleri");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PageTypes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
