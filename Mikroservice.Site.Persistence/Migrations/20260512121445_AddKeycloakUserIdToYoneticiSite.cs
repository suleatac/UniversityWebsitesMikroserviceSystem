using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddKeycloakUserIdToYoneticiSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KeycloakUserId",
                table: "YoneticiSiteler",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "PersonelId",
                table: "YoneticiSiteler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Unvanlar",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Unvanlar",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_YoneticiSiteler_SiteId_PersonelId",
                table: "YoneticiSiteler",
                columns: new[] { "SiteId", "PersonelId" });

            migrationBuilder.CreateIndex(
                name: "IX_Unvanlar_Ad",
                table: "Unvanlar",
                column: "Ad");

            migrationBuilder.CreateIndex(
                name: "IX_Unvanlar_KisaAd",
                table: "Unvanlar",
                column: "KisaAd");

            migrationBuilder.CreateIndex(
                name: "IX_Unvanlar_ParentId",
                table: "Unvanlar",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Unvanlar_Sira",
                table: "Unvanlar",
                column: "Sira");

            migrationBuilder.AddForeignKey(
                name: "FK_Unvanlar_Unvanlar_ParentId",
                table: "Unvanlar",
                column: "ParentId",
                principalTable: "Unvanlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Unvanlar_Unvanlar_ParentId",
                table: "Unvanlar");

            migrationBuilder.DropIndex(
                name: "IX_YoneticiSiteler_SiteId_PersonelId",
                table: "YoneticiSiteler");

            migrationBuilder.DropIndex(
                name: "IX_Unvanlar_Ad",
                table: "Unvanlar");

            migrationBuilder.DropIndex(
                name: "IX_Unvanlar_KisaAd",
                table: "Unvanlar");

            migrationBuilder.DropIndex(
                name: "IX_Unvanlar_ParentId",
                table: "Unvanlar");

            migrationBuilder.DropIndex(
                name: "IX_Unvanlar_Sira",
                table: "Unvanlar");

            migrationBuilder.DropColumn(
                name: "PersonelId",
                table: "YoneticiSiteler");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Unvanlar");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Unvanlar");

            migrationBuilder.AlterColumn<string>(
                name: "KeycloakUserId",
                table: "YoneticiSiteler",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
