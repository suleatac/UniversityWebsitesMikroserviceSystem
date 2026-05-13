using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PopupToOneToOneWithSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YoneticiSiteler_YoneticiTipleri_YoneticiTipiId",
                table: "YoneticiSiteler");

            migrationBuilder.DropTable(
                name: "YoneticiTipleri");

            migrationBuilder.DropIndex(
                name: "IX_YoneticiSiteler_SiteId_PersonelId",
                table: "YoneticiSiteler");

            migrationBuilder.DropIndex(
                name: "IX_YoneticiSiteler_YoneticiTipiId",
                table: "YoneticiSiteler");

            migrationBuilder.DropColumn(
                name: "PersonelId",
                table: "YoneticiSiteler");

            migrationBuilder.DropColumn(
                name: "YoneticiTipiId",
                table: "YoneticiSiteler");

            migrationBuilder.DropColumn(
                name: "CookieIleTekrarGosterme",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "GosterimSuresiSaniye",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "TamEkranMi",
                table: "Icerik");

            migrationBuilder.CreateTable(
                name: "Popuplar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    KisaAciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IcerikMetni = table.Column<string>(type: "text", nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ResimUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    GosterimSayisi = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    YayimTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EklemeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    BaslamaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SeoUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    SeoDescription = table.Column<string>(type: "text", nullable: true),
                    TamEkranMi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    GosterimSuresiSaniye = table.Column<int>(type: "integer", nullable: false, defaultValue: 5),
                    CookieIleTekrarGosterme = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Popuplar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Popuplar_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Popuplar_SiteId",
                table: "Popuplar",
                column: "SiteId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Popuplar");

            migrationBuilder.AddColumn<int>(
                name: "PersonelId",
                table: "YoneticiSiteler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YoneticiTipiId",
                table: "YoneticiSiteler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CookieIleTekrarGosterme",
                table: "Icerik",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GosterimSuresiSaniye",
                table: "Icerik",
                type: "integer",
                nullable: true,
                defaultValue: 5);

            migrationBuilder.AddColumn<bool>(
                name: "TamEkranMi",
                table: "Icerik",
                type: "boolean",
                nullable: true,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "YoneticiTipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoneticiTipleri", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YoneticiSiteler_SiteId_PersonelId",
                table: "YoneticiSiteler",
                columns: new[] { "SiteId", "PersonelId" });

            migrationBuilder.CreateIndex(
                name: "IX_YoneticiSiteler_YoneticiTipiId",
                table: "YoneticiSiteler",
                column: "YoneticiTipiId");

            migrationBuilder.AddForeignKey(
                name: "FK_YoneticiSiteler_YoneticiTipleri_YoneticiTipiId",
                table: "YoneticiSiteler",
                column: "YoneticiTipiId",
                principalTable: "YoneticiTipleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
