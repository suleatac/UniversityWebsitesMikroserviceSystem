using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fifth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Popuplar_PageTypes_PageTypeId",
                table: "Popuplar");

            migrationBuilder.DropTable(
                name: "Menuler");

            migrationBuilder.DropIndex(
                name: "IX_SikcaSorulanSorular_SeoUrl",
                table: "SikcaSorulanSorular");

            migrationBuilder.DropColumn(
                name: "SeoUrl",
                table: "SikcaSorulanSorular");

            migrationBuilder.DropColumn(
                name: "IcerikMetni",
                table: "Popuplar");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "Popuplar");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "Popuplar");

            migrationBuilder.DropColumn(
                name: "SeoUrl",
                table: "Popuplar");

            migrationBuilder.AddColumn<string>(
                name: "IconShortDescription",
                table: "ShortcutButtons",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PageTypeId",
                table: "Popuplar",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Popuplar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "MegaMenu",
                table: "Icerik",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Menu_Sira",
                table: "Icerik",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PageTypeId1",
                table: "Icerik",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Icerik",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Popuplar_DilId",
                table: "Popuplar",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_PageTypeId1",
                table: "Icerik",
                column: "PageTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Icerik_ParentId_Menu_Sira",
                table: "Icerik",
                columns: new[] { "ParentId", "Menu_Sira" });

            migrationBuilder.AddForeignKey(
                name: "FK_Icerik_Icerik_ParentId",
                table: "Icerik",
                column: "ParentId",
                principalTable: "Icerik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Icerik_PageTypes_PageTypeId1",
                table: "Icerik",
                column: "PageTypeId1",
                principalTable: "PageTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Popuplar_Diller_DilId",
                table: "Popuplar",
                column: "DilId",
                principalTable: "Diller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Popuplar_PageTypes_PageTypeId",
                table: "Popuplar",
                column: "PageTypeId",
                principalTable: "PageTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Icerik_Icerik_ParentId",
                table: "Icerik");

            migrationBuilder.DropForeignKey(
                name: "FK_Icerik_PageTypes_PageTypeId1",
                table: "Icerik");

            migrationBuilder.DropForeignKey(
                name: "FK_Popuplar_Diller_DilId",
                table: "Popuplar");

            migrationBuilder.DropForeignKey(
                name: "FK_Popuplar_PageTypes_PageTypeId",
                table: "Popuplar");

            migrationBuilder.DropIndex(
                name: "IX_Popuplar_DilId",
                table: "Popuplar");

            migrationBuilder.DropIndex(
                name: "IX_Icerik_PageTypeId1",
                table: "Icerik");

            migrationBuilder.DropIndex(
                name: "IX_Icerik_ParentId_Menu_Sira",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "IconShortDescription",
                table: "ShortcutButtons");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Popuplar");

            migrationBuilder.DropColumn(
                name: "MegaMenu",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "Menu_Sira",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "PageTypeId1",
                table: "Icerik");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Icerik");

            migrationBuilder.AddColumn<string>(
                name: "SeoUrl",
                table: "SikcaSorulanSorular",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PageTypeId",
                table: "Popuplar",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IcerikMetni",
                table: "Popuplar",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "Popuplar",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "Popuplar",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoUrl",
                table: "Popuplar",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Menuler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DilId = table.Column<int>(type: "integer", nullable: false),
                    HedefId = table.Column<int>(type: "integer", nullable: false),
                    PageTypeId = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    Ad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: true),
                    IconUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MegaMenu = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menuler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menuler_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menuler_Hedefler_HedefId",
                        column: x => x.HedefId,
                        principalTable: "Hedefler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Menuler_Menuler_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menuler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menuler_PageTypes_PageTypeId",
                        column: x => x.PageTypeId,
                        principalTable: "PageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Menuler_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SikcaSorulanSorular_SeoUrl",
                table: "SikcaSorulanSorular",
                column: "SeoUrl");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_DilId",
                table: "Menuler",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_HedefId",
                table: "Menuler",
                column: "HedefId");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_PageTypeId",
                table: "Menuler",
                column: "PageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_ParentId_Sira",
                table: "Menuler",
                columns: new[] { "ParentId", "Sira" });

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_SiteId_DilId",
                table: "Menuler",
                columns: new[] { "SiteId", "DilId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Popuplar_PageTypes_PageTypeId",
                table: "Popuplar",
                column: "PageTypeId",
                principalTable: "PageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
