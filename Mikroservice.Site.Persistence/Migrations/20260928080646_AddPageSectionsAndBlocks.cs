using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPageSectionsAndBlocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageSectionlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    DilId = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BackgroundColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BackgroundImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    Yayinda = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSectionlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageSectionlar_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PageSectionlar_Siteler_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Siteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageBloklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PageSectionId = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ContentType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    VideoType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    BackgroundImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BackgroundColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ColumnSize = table.Column<int>(type: "integer", nullable: false, defaultValue: 12),
                    RowNumber = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Animation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageBloklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageBloklar_PageBloklar_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PageBloklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PageBloklar_PageSectionlar_PageSectionId",
                        column: x => x.PageSectionId,
                        principalTable: "PageSectionlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageBlockMedia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PageBlockId = table.Column<int>(type: "integer", nullable: false),
                    ResimUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    VideoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageBlockMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageBlockMedia_PageBloklar_PageBlockId",
                        column: x => x.PageBlockId,
                        principalTable: "PageBloklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageBlockMedia_PageBlockId_Sira",
                table: "PageBlockMedia",
                columns: new[] { "PageBlockId", "Sira" });

            migrationBuilder.CreateIndex(
                name: "IX_PageBloklar_PageSectionId_ParentId_RowNumber",
                table: "PageBloklar",
                columns: new[] { "PageSectionId", "ParentId", "RowNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_PageBloklar_ParentId",
                table: "PageBloklar",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PageSectionlar_DilId",
                table: "PageSectionlar",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_PageSectionlar_SiteId_DilId_Sira",
                table: "PageSectionlar",
                columns: new[] { "SiteId", "DilId", "Sira" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageBlockMedia");

            migrationBuilder.DropTable(
                name: "PageBloklar");

            migrationBuilder.DropTable(
                name: "PageSectionlar");
        }
    }
}
