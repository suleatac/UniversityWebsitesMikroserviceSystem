using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mikroservice.Site.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IcerikDosyaEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IcerikDosya",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IcerikId = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DosyaUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DosyaAdi = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DosyaBoyut = table.Column<long>(type: "bigint", nullable: false),
                    DosyaTuru = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    YuklemeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IcerikDosya", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IcerikDosya_Icerik_IcerikId",
                        column: x => x.IcerikId,
                        principalTable: "Icerik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IcerikDosya_IcerikId_Sira",
                table: "IcerikDosya",
                columns: new[] { "IcerikId", "Sira" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IcerikDosya");
        }
    }
}
